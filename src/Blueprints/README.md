# Blueprints

Reusable, cross-domain OctoMesh blueprints that belong to the Construction Kit
repository because they only depend on the shared CK models defined here (no
application-specific services or pipelines).

| Blueprint | Purpose |
|---|---|
| `Locations.Austria` | Seeds the Austrian location tree (`Basic/Tree` → Country → State → District → City) with 2237 cities and their postal codes. Data-only. |

## Working with a blueprint

```bash
# Validate
octo-bpm -c validate -p src/Blueprints/Locations.Austria

# Publish to the local catalog for testing
octo-bpm -c publish -p src/Blueprints/Locations.Austria --catalog LocalFileSystemBlueprintCatalog -f

# Install on a tenant
octo-cli -c InstallBlueprint -b Locations.Austria-1.0.0
```

Shared distribution happens via the GitHub blueprint catalog
(`meshmakers/blueprint-libraries-build`), published with `octo-bpm -c publish`.

## Locations.Austria data source

The seed was generated from the Austrian place directory (2237 places with
postal codes) that energy-community deployments previously imported manually
from `energy-community-deployment/data/excel/plz.xlsx` via the OctoMesh Office
Add-In. The rtIds are stable synthetic ids (`10ca…` prefix, type digit +
sequence); regenerating the seed from an updated source must keep the ids of
unchanged entities stable so blueprint re-applies stay idempotent upserts.
