# Blueprints

Reusable, cross-domain OctoMesh blueprints that belong to the Construction Kit
repository because they only depend on the shared CK models defined here (no
application-specific services or pipelines).

| Blueprint | Purpose |
|---|---|
| `Locations.Austria` | Seeds the Austrian location tree (`Basic/Tree` → Country → State → District → City) with 2237 cities and their postal codes. Data-only. |
| `Samples.PipelineBasics` | Pipeline chaining over the event hub and a cron-triggered pipeline. No domain data. |
| `Samples.Maintenance` | Maintenance master data: one line, three machines with name plates. Data-only. |
| `Samples.Photovoltaics` | PV plant master data plus the Modbus edge adapter and the two-adapter DataFlow that meters it. |
| `Samples.Simulator.Energy` | Time-range archive + three chained rollups and the pipeline that generates a synthetic load profile. The reference workload for stream-data work. |
| `Samples.Simulator.EnergyCommunity` | Energy-community tree on the `OctoSdkDemo` model, signal simulation and an on-demand customer generator. |
| `Samples.EnergyEnvironment` | Full energy-management demo: assets, load profile, alerts, three saved queries and a Meshboard dashboard. Data-only. |

The `Samples.*` blueprints replace the `ImportRt` scripts that used to live in
[`../Samples`](../Samples). They are for demo and training tenants.

## What a sample blueprint does NOT seed

- **Pool and Mesh Adapter.** `System.Communication.MainLatest` / `.Release` seed
  the Cloud pool (`670000000000000000000001`) and the Mesh Adapter
  (`670000000000000000000002`, `rtWellKnownName: MeshAdapter`), and the
  Communication Controller applies them automatically when Communication is
  enabled. Every sample pipeline points its `Executes` association at that rtId;
  both environment variants share it. An edge adapter is different — it runs
  next to the hardware and no platform blueprint provides it, which is why
  `Samples.Photovoltaics` seeds its own.
- **Pipeline service accounts.** The deploy guard requires a resolvable service
  account per adapter (AB#5027/5112), but the Communication Controller
  provisions one automatically after every blueprint apply (AB#5111,
  `BlueprintAppliedConsumer` → `EnsureTenantProvisionedAsync`).
- **Lifecycle steps.** A blueprint seeds entities, it does not drive lifecycle.
  `EnableCommunication` / `EnableStreamData` are preconditions,
  `DeployDataFlow` and `ActivateArchive` are follow-up steps — a seeded pipeline
  is not registered on the adapter until its DataFlow is deployed, and a seeded
  archive has no CrateDB table until it is activated.

## CK dependency floors

`ckModelDependencies` floors are pinned to the **published** version and listed
in **dependency order**. Two reasons, both easy to get wrong:

- The engine installs the range's lower bound as an exact version, so
  `Industry.Energy-[3.0,4.0)` asks for a `3.0.0` that is not in the catalog.
- It does **not** resolve a CK model's own dependencies, so `Basic` has to be
  named before `Industry.Basic` before `Industry.Energy`.

`System.*` models are the exception: they are service-managed, and
`EnsureCkModelInstalledAsync` redirects the install to the
`IServiceManagedCkModelDescriptor` version (AB#4294), so their floor is a pure
satisfiability floor.

## Working with a blueprint

```bash
# Validate
octo-bpm -c validate -p src/Blueprints/Locations.Austria

# Publish to the local catalog for testing
octo-bpm -c publish -p src/Blueprints/Locations.Austria --catalog LocalFileSystemBlueprintCatalog -f

# Install on a tenant
octo-cli -c InstallBlueprint -b Locations.Austria-1.1.0
```

`validate` lists every seed file a blueprint declares — use it to confirm a
`seedDataPaths` list is complete after adding or renaming a file.

Shared distribution happens via the GitHub blueprint catalog
(`meshmakers/blueprint-libraries-build`), published with `octo-bpm -c publish`.

## Seed data layout

A seed is split across several files via `seedDataPaths` (AB#4758) instead of
one `entities.yaml`. All listed files are merged into **one** runtime model
before the import runs, so the list order is presentation only and associations
may cross files freely. `Locations.Austria` splits by level, then by state:

```
Locations.Austria/
├── blueprint.yaml            # seedDataPaths: 11 files
└── seed-data/
    ├── tree.yaml             # Tree "Locations" + Country AT + 9 states
    ├── districts.yaml        # 116 districts, each parented to a state
    └── cities/
        ├── burgenland.yaml   # 148 cities
        ├── kaernten.yaml     # 195
        ├── ...
        └── wien.yaml         # 24
```

> **Engine floor.** An engine without AB#4758 deserialises `blueprint.yaml`
> with `IgnoreUnmatchedProperties`: `seedDataPaths` is dropped, no
> `seedDataPath` remains, and the blueprint installs with an **empty seed,
> silently, reporting success**. Neither `octo-bpm validate` nor the publish
> step catches that. Roll the engine out before publishing a split blueprint,
> and keep the last single-file version installable for older tenants
> (`Locations.Austria-1.0.0`).

## Locations.Austria data source

The seed was generated from the Austrian place directory (2237 places with
postal codes) that energy-community deployments previously imported manually
from `energy-community-deployment/data/excel/plz.xlsx` via the OctoMesh Office
Add-In. The rtIds are stable synthetic ids (`10ca…` prefix, type digit +
sequence); regenerating the seed from an updated source must keep the ids of
unchanged entities stable so blueprint re-applies stay idempotent upserts.
