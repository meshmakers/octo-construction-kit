---
description: The build lints attribute ownership markers: the OCTO-CK001 rule and the review question for every new attribute.
applies_to: src/ConstructionKits/**/ConstructionKit/attributes/*.yaml
---

# Attribute ownership markers

## Ownership markers are enforced by the build (AB#6327)

`Directory.Build.props` sets `OctoEnforceRuntimeStateMarkers=true` for every project of this repository; the
`CkLintRuntimeStateMarkers` task (CK MsBuildTasks package) runs in each project with a `ConstructionKitFolder` and fails the
build with `OCTO-CK001` when an attribute in `ConstructionKit/attributes/*.yaml` declares no `ownership` (or the deprecated
`isRuntimeState`). A new CK project in `src/ConstructionKits/` is covered automatically.

AB#6327 restated the existing behaviour: all 394 attributes of the 12 models with attributes got `ownership: SeedOwned`
(OctoEnergyDemo has none). Same resolved ownership, so every model keeps its version (the build's compatibility check and
`octo-ckc ValidateVersion` report "no structural changes"). No preserving marker was added: the seed ownership audit
(AB#6317) reviewed the sample-blueprint attributes of these models as blueprint-owned (R7).

**Review question for every new attribute:** could an operator or user type this value in the product? Yes → `ownership:
TenantOwned` (or `Secret` for a credential); a service, pipeline or adapter writes it → `RuntimeState`; the product ships and
corrects it → `SeedOwned`. Changing an existing attribute to a preserving marker is a behaviour change and needs a minor bump
of the model (and a rebuild of its dependents, see below).
