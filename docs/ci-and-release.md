---
description: The Azure pipeline stages, the CK v2 publish guard, which models are published and the build artifacts.
applies_to: azure-pipelines.yml
---

# Continuous integration and publishing

## Continuous Integration / CD

## Azure Pipelines Configuration

**File**: `azure-pipelines.yml` (repo root; templates from octo-pipeline-templates `tpl-v1.3.1`)

**Triggers**:
- Branches: `dev/*`, `test/*`, `main`
- Tags: Excluded (tags starting with 'r' are releases)

**Stages**:

1. **Build Stage**: Executes on `meshmakers-ci-agents` pool
   - Updates build number
   - Sets version information
   - CK v2 publish guard (AB#6277, operating rule O1, `ck-o1-guard.yml`): before the build
     on main / r-tags (source mode; the publish itself follows after the tests) it refuses `ckLanguage: 2` in any
     `ckModel.yaml` and range retention (`OctoCkRangeRetention=true`); before the blueprint
     publish step it refuses a blueprint whose dependency floor is a CK v2 model. Lifted only
     by the variable `OctoCkO1Lifted` (group `OctoDefault`) through AB#6279. Exception: publishes to the dev catalog `octo-catalog-dev`
     (test/* lane) may carry CK v2 models (tpl-v1.2.1)
   - Builds solution: `dotnet build --configuration Release` with `/p:OctoPublishCatalog=LocalFileSystemCatalog`
     (the CKs depend on each other and resolve through the run-isolated local catalog; no build publishes
     to a shared catalog since `tpl-v1.0.0`)
   - Runs tests: `dotnet test` (excluding SystemTests)
   - `validate-and-publish-ck-versions` (12 CKs, dependency order): SemVer gate, compile, publish to the
     catalogs of the channel (main → private, r* tags → private and public; never replaces a version)
     The 12 published models are listed identically in `constructionKitPaths` (this step, dependency order) and in
     `constructionKitLibraryPaths` (handle-artifacts): Basic, Basic.Accounting, Basic.Energy, EnergyCommunity,
     Environment, Industry.{Basic,Logistics,Energy,Fluid,Maintenance,Manufacturing}, OctoSdkDemo. Basic.Accounting was
     missing from both lists until AB#6721 (1.10.0 was never published). `Monitoring` is deliberately not published
     (dropped in r3.4.137); `Octo.Energy.Demo` is a sample. A new publishable CK must be added to both lists.
   - `validate-and-publish-blueprints`: blueprint version gate and publish to the catalogs of the channel
     (an already published version is a no-op; the lane blueprint step below stays for `test/*`)
   - Processes artifacts
   - Pushes NuGet packages

**Build Artifacts**:

CK libraries are published from:
- `src/ConstructionKits/Octo.Sdk.Demo/bin/Release/net10.0/octo-ck-libraries/Octo.Sdk.Demo`
- `src/ConstructionKits/Octo.Sdk.Packages.Basic/bin/Release/net10.0/octo-ck-libraries/Octo.Sdk.Packages.Basic`
- (And similar for all other CKs)

**Version Naming**: `$(MajorVersion).$(MinorVersion).$(date:yyMM).$(DayOfMonth)$(rev:rrr)-$(Build.SourceBranchName)`
