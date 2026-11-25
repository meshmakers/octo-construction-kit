---
description: Why every ckModel.yaml dependency is mirrored as a ProjectReference, and which package depends on which.
applies_to: src/ConstructionKits/*/*.csproj, src/ConstructionKits/**/ConstructionKit/ckModel.yaml
---

# CK project dependencies

## Project Dependencies

CK model dependencies are declared in each `ckModel.yaml` **and** mirrored as a `<ProjectReference>` to the
sibling model project in the `.csproj`. The ProjectReference is mandatory: without it a parallel build can
compile a model before its sibling dependency has been published to the local catalog
(`../.octo/local-catalog`), so the dependency resolves against a stale catalog entry (a build race).
A transitive reference is sufficient (e.g. Industry.Energy → Industry.Basic → Basic).

- **Octo.Sdk.Packages.Basic.Accounting** → depends on Octo.Sdk.Packages.Basic
- **Octo.Sdk.Packages.Basic.Energy** → depends on Octo.Sdk.Packages.Basic
- **Octo.Sdk.Packages.Industry.Basic** → depends on Octo.Sdk.Packages.Basic
- **Octo.Sdk.Packages.Industry.Energy** → depends on Octo.Sdk.Packages.Industry.Basic
- **Octo.Sdk.Packages.Industry.Fluid** → depends on Octo.Sdk.Packages.Industry.Basic
- **Octo.Sdk.Packages.Industry.Logistics** → depends on Octo.Sdk.Packages.Basic
- **Octo.Sdk.Packages.Industry.Maintenance** → depends on Octo.Sdk.Packages.Industry.Basic
- **Octo.Sdk.Packages.Industry.Manufacturing** → depends on Octo.Sdk.Packages.Industry.Basic
- **Octo.Sdk.Packages.Environment** → depends on Octo.Sdk.Packages.Basic (the project references Industry.Basic, which reaches Basic)
- **Octo.Sdk.Demo** → depends on Octo.Sdk.Packages.Basic
- **Octo.Sdk.Packages.EnergyCommunity** → depends on Octo.Sdk.Packages.Basic and Octo.Sdk.Packages.Basic.Energy (since EnergyCommunity 4.0.0: community/billing types subtype the Basic.Energy metering-point types; metering data is stream data, no EnergyQuantity entity anymore)
