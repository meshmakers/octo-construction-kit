# Samples

Everything that used to live here is now a blueprint under
[`../Blueprints`](../Blueprints) — see that README for the list and the
rationale. A blueprint replaces the `octo-cli -c ImportRt` scripts that used to
encode the import order by hand, is versioned, and can be updated, rolled back
and uninstalled.

What is left is the one piece of sample material that is **not** runtime
entities and therefore cannot be a blueprint:

| File | What it is |
|---|---|
| `graphql-create-trees.graphql` | A GraphQL mutation that creates three `Basic/Tree` roots (business area, functional location, workspace). Paste it into the Refinery Studio's GraphQL console. |

## Removed: the cross-adapter demo

`rt-dataflow-cross-adapter.yaml` and its master data (`rt-simulator-industry.yaml`)
demonstrated a three-pipeline flow hopping **mesh → edge → mesh**. They were
deleted rather than ported, because the edge adapter they targeted no longer
exists:

- the "Simulation" adapter they executed on was removed — `Simulation@1` is part
  of the Mesh Adapter SDK (`AddSimulationNodes()`), so no separate simulation
  plug adapter is needed, and its rtIds collided entity-for-entity with the
  (also removed) zenon sample, so the two could never coexist on one tenant;
- rebuilding a second adapter against today's workload model (an Edge pool with
  `Environment: Edge` and a plug someone actually runs) is a modelling decision,
  not a mechanical port.

`Samples.PipelineBasics` still demonstrates pipeline chaining over the event
hub — the same mechanism, on a single adapter. Chaining **across** adapters is
the identical `ToPipelineDataEvent@1` / `FromPipelineDataEvent@1` pair with a
different `Executes` association on the consuming pipeline.
