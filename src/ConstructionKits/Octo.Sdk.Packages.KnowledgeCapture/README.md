# Octo.Sdk.Packages.KnowledgeCapture

GenAI-based knowledge capture for SME business processes: turns weakly structured
business artifacts (work items, commit histories, chats, transcripts, support
reports) into structured, human-verified wiki entries. Operationalizes the design
of *System Designs for GenAI-Based Knowledge Capture in SME Business Processes*
(Schwaab, 2026) on OctoMesh, using the `LlmQuery@1` / `McpToolCall@1` pipeline
nodes (branch `dev/philipp/llmquery-node-v2`).

## Design

### Medallion layering inside a mesh domain

OctoMesh is the data-mesh substrate (tenants and CK models as domain boundaries,
blueprints as self-serve provisioning, verified entries as the data product).
The medallion pattern describes how data quality progresses *inside* this domain
model; nothing about it leaks into other models.

| Layer | Type | Content |
|---|---|---|
| Bronze | `SourceArtifact` | Raw artifact text, append-only, preserved for reprocessing and audit. `SourceUri` links to the system of record. |
| Silver | `WikiEntry` | Audience-neutral structured sections + citations, verified **once** by a human (`VerificationStatus`). |
| Gold | `WikiRendition` | Audience-specific markdown (development / marketing / architect schemas), cheap and repeatable. |

Downstream consumers only touch **Verified** entries and their renditions — that
is the data-product quality gate. Provenance (`GeneratedFrom`, `RenderOf`, prompt
name/version, model id) is the product contract.

### Two architectures (thesis §2.2/§4.2)

- **Structured pipeline** (`wiki-capture-structured`): deterministic control
  flow — fetch artifact → one generation LLM call with a fixed JSON schema →
  one verification LLM call (evaluator pass; unsupported claims land in
  `HallucinationFlags`) → write `WikiEntry` as Draft.
- **Agentic workflow** (`wiki-capture-agentic`): one agentic `LlmQuery@1` with
  read-only MCP retrieval tools and an EXTRACT→RETRIEVE→DRAFT→REVIEW→REVISE
  self-correction loop, bounded by `maxToolRounds`. The entity write happens
  deterministically *outside* the agent loop, so the MCP service account only
  needs `OctoApiReadOnly`.

Human-in-the-loop verification happens in Studio: review the Draft entry
(sections, citations, hallucination flags), edit, then set `VerificationStatus`
to `Verified`. Render renditions only from Verified entries.

### PII redaction is optional

Off by default — in production, names and companies carry attribution value.
If a source requires redaction, redact before/during ingestion (e.g. Microsoft
Presidio in a relay) and set `isPiiRedacted: true` in the ingest payload; the
prompts then keep anonymized identifiers as-is.

## Setup

0. Build and publish to the local catalogs (the blueprint is NOT visible in
   Studio until published — unlike `System.*` blueprints, no service registers
   it via DI):
   - `invoke-buildall -configuration DebugL -excludeFrontend $true` — compiles
     the CK model and publishes it to the local CK catalog
     (`OctoPublishCkModel`).
   - Publish with the BlueprintManager tool (`octo-bpm`). Install it once from
     the local feed:
     `dotnet tool install --global Meshmakers.Octo.BlueprintManager --version 999.0.0 --add-source <repo-root>/nuget`
     (or run from source:
     `dotnet run --project octo-construction-kit-engine/src/BlueprintManager -- ...`).
     Then:
     `octo-bpm -c publish -p .../Octo.Sdk.Packages.KnowledgeCapture/Blueprints/KnowledgeCapture.MainLatest`
     (`-f` to replace, `-c validate` for a pre-check, `-c catalogs` /
     `-c config` to inspect the catalog root). Publishes to the
     LocalFileSystemBlueprintCatalog — make sure its root matches what the
     asset services read (`$ROOTPATH/.octo/*` in the octo developer shell).
   - Refresh the blueprint catalog in Studio if it doesn't appear immediately.
1. Install the blueprint `KnowledgeCapture.MainLatest-1.0.0` (Studio or MCP
   `install_blueprint`). Requires `System.Communication` ≥ 3.25.1.
2. Register an OIDC client `wiki-capture-mcp` in octo-identity-services:
   client-credentials grant, enabled, require client secret, and allowed scope
   **`octo_api`** — that is the literal scope string (the value of the
   `CommonConstants.OctoApiFullAccess` constant) which `ServiceAccountTokenService`
   requests hardcoded; read-only scoping awaits the configurable-scope
   enhancement from mcp-auth-plan. Ensure the tenant is covered by
   `allowed_tenants`, then set the `ClientSecret` on the `wiki-capture-sa`
   ServiceAccountConfiguration (same value, two places).
   CLI: `octo-cli -c AddClientCredentialsClient -id wiki-capture-mcp -n "Wiki Capture MCP" -s <secret>`
   Verify independently of the adapter before deploying:
   `curl -k -X POST https://localhost:5003/connect/token -H "Content-Type: application/x-www-form-urlencoded" -d "grant_type=client_credentials&client_id=wiki-capture-mcp&client_secret=<secret>&scope=octo_api&acr_values=tenant:meshtest"`
   must return an access_token (`invalid_client` = id/secret problem,
   `invalid_scope` = the client lacks `octo_api`).
3. Set the `ApiKey` on the `wiki-capture-llm` AiConfiguration.
4. Check the `wiki-capture-mcp` McpConfiguration `Url`
   (dev default: `https://localhost:5017/meshtest/mcp`).
5. Deploy the pipelines (Studio or MCP `deploy_pipeline`).

Smoke test (adapter dev ports 5020/5021, tenant `meshtest`):

```
POST https://localhost:5020/meshtest/knowledge-capture/ingest
  { "title": "...", "artifactType": 2, "sourceSystem": "AzureDevOps",
    "sourceUri": "...", "content": "...", "contentPreview": "...",
    "isPiiRedacted": false, "capturedAt": "2026-07-20T12:00:00Z" }

POST https://localhost:5020/meshtest/knowledge-capture/generate
  { "artifactRtIds": ["<rtId from ingest>"] }

POST https://localhost:5020/meshtest/knowledge-capture/render
  { "wikiEntryRtIds": ["<rtId>"], "audience": "development",
    "audienceKey": 0, "title": "My entry (dev)" }
```

## Ingestion options

`wiki-capture-ingest` is a generic normalized endpoint — any webhook/relay/script
can POST to it (Azure DevOps service hooks via a small relay, git provider
webhooks, export scripts). For direct source connectors, build additional
pipelines with the existing trigger nodes: `FromMicrosoftGraph@1` (Teams
channels), `FromTeamsBot@1`, `FromMicrosoftGraphEmail@1` / `FromEmail@1`
(mail/support reports), `FromPolling@1` + `MakeHttpRequest@1` (REST polling of
ADO/git APIs), each ending in the same `CreateUpdateInfo@1` → `ApplyChanges@2`
write. Note: octo-mcp-service exposes only the OctoMesh API surface — external
systems are reached via adapter trigger nodes, not MCP.

**Batch processing** (backlog / cron): create a pipeline with
`FromPipelineTriggerEvent@1` → `GetRtEntitiesByType@1` (Skip/Take paging) →
`ForEach@1` around the generation nodes, and a `PipelineTrigger` entity with a
`CronExpression` associated via `Triggers`. Deliberately not seeded in v1.

## Storage design and evolution

- v1 stores `SourceArtifact.Content` **inline as String**. MongoDB's 16 MB
  document limit is far away at SME artifact sizes (~10–200 KB), and WiredTiger
  block compression (snappy default, zstd configurable) compresses prose ~3–5×.
- **Revisit triggers**: corpus reaching tens of GB, or a bulk analytics/export
  path. Then move `Content` to `BinaryLinked` (GridFS, out-of-document via
  `ILinkedBinaryDataSource`) or an object store, and export analytical record
  sets as Parquet for lakehouse engines. Model change is additive
  (KnowledgeCapture 1.1.0); `SourceUri` always keeps the authoritative copy in
  the system of record.
- Stream data / CrateDB is time-series only — not used for text.

## Known v1 limitations

- **Prompt duplication**: `LlmQuery@1` takes prompt text inline, so pipeline
  definitions embed a copy of the `PromptTemplate` text. Templates are the
  governed source of truth; bump `TemplateVersion` and update the pipeline
  definition together. (Future node enhancement: prompt-by-reference.)
- `GeneratedFrom` association is written for the **first** artifact rtId only;
  multi-artifact consolidation needs a `ForEach@1` over the association step.
- Run metadata (`LatencySeconds`, tokens, `CostUsd`) is not auto-populated by
  the pipelines; `LlmQuery@1` emits OpenTelemetry metrics instead. Attributes
  are optional and can be filled when the node exposes usage in the document.
- `ContentHash` is not computed by the ingest pipeline yet (provide it in the
  payload if you need idempotency).
- Few-shot examples (thesis CL-02) are intentionally not seeded; add 3–5
  tenant-specific synthetic exemplars to the `FewShotExamples` attribute and
  the pipeline prompts once real verified entries exist.

## Pinned follow-ups (from the first smoke test, 2026-07-21)

- **LlmQuery@1 `metadataTargetPath`**: emit run metadata (model *as resolved at
  request time*, prompt/completion tokens, latency, cost) into the data
  context so `ModelId`/`LatencySeconds`/`CostUsd` on WikiEntry are mapped via
  `valuePath` instead of hardcoded literals (currently the seeded `ModelId`
  is a static stamp and goes stale when the node's model is overridden).
- **Render pipeline quality gate**: add a `fieldFilters` clause
  (`VerificationStatus == 2`) to the `GetRtEntitiesById@1` node in
  `wiki-render-audience` so only Verified entries can be rendered — today the
  pipeline trusts the caller.
- **Citation prompt tuning (template v2)**: instruct "quote only from the
  Content attribute; keep quotes minimal" — v1 outputs cited serialized
  entity JSON for the Sources section and whole-content quotes for
  Technical detail.
- **Studio pipeline-editor bug**: the LlmQuery provider dropdown shows the
  enum value twice (proper spelling + SCREAMING_SNAKE variant) — schema/form
  generation issue, cosmetic.
- **CreateUpdateInfo@1 schema-inferred record coercion** (implemented
  2026-07-21 on `dev/philipp/llmquery-node-v2`): plain JSON objects coerce to
  the record type declared by the CK schema; the `{CkRecordId, Attributes}`
  envelope remains required for polymorphic records and dotted attribute
  paths. This package's generation pipelines depend on that adapter change.
- **MCP tool allowlist**: the agentic pipeline currently offers the entire
  octo-mcp-service surface (~198 tools ≈ 20k+ prompt tokens per call) —
  expensive and poor agent-computer-interface design. Add a tool
  allowlist/filter to `McpConfiguration` or `LlmQuery@1` and scope this
  pipeline to the few read tools it needs (get_entity_by_id, query_entities,
  get_type_schema).
- **Small-model tool use**: gpt-oss-120b made zero tool calls despite 198
  offered (replicates the thesis Run 1 small-model finding). Judge the
  agentic architecture on a stronger model (claude-sonnet) before drawing
  conclusions; note also the node's documented fallback that
  `responseFormat: json` is incompatible with tools on most providers (JSON
  is then enforced by the system prompt alone).

## Sources

Anthropic, [Building effective agents](https://www.anthropic.com/engineering/building-effective-agents) ·
Anthropic, [Prompting best practices](https://platform.claude.com/docs/en/build-with-claude/prompt-engineering/claude-prompting-best-practices) ·
Anthropic, [Reduce hallucinations](https://platform.claude.com/docs/en/test-and-evaluate/strengthen-guardrails/reduce-hallucinations) ·
Microsoft, [Medallion lakehouse architecture](https://learn.microsoft.com/en-us/azure/databricks/lakehouse/medallion) ·
Microsoft, [Unified data platform (data domains & products)](https://learn.microsoft.com/en-us/azure/cloud-adoption-framework/data/executive-strategy-unify-data-platform) ·
Schwaab, *System Designs for GenAI-Based Knowledge Capture in SME Business Processes* (2026).
