---
description: The build lints attribute ownership markers: the OCTO-CK001 rule and the review question for every new attribute.
applies_to: src/ConstructionKits/**/ConstructionKit/attributes/*.yaml
---

# Attribute ownership markers

## Ownership markers are enforced by the build

`Directory.Build.props` sets `OctoEnforceRuntimeStateMarkers=true` for every project of this repository; the
`CkLintRuntimeStateMarkers` task (CK MsBuildTasks package) runs in each project with a `ConstructionKitFolder` and fails the
build with `OCTO-CK001` when an attribute in `ConstructionKit/attributes/*.yaml` declares no `ownership` (or the deprecated
`isRuntimeState`). A new CK project in `src/ConstructionKits/` is covered automatically. `Octo.Energy.Demo` is not in the solution and is not linted.

**Review question for every new attribute:** could an operator or user type this value in the product? Yes → `ownership:
TenantOwned` (or `Secret` for a credential); a service, pipeline or adapter writes it → `RuntimeState`; the product ships and
corrects it → `SeedOwned`. Changing an existing attribute to a preserving marker is a behaviour change and needs a minor bump
of the model (and a rebuild of its dependents, see [ck-dependencies.md](ck-dependencies.md)).
