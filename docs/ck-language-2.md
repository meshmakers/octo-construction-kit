---
description: Rules for models on ckLanguage 2: derivable Any, System interface implementation, dependency floors and the ValidateVersion gate.
applies_to: src/ConstructionKits/**/ConstructionKit/ckModel.yaml, src/ConstructionKits/**/ConstructionKit/{types,records}/**
---

# CK language 2, derivable and interface implementation

## CK language 2: Basic and `derivable: Any`

Basic (since 2.6.0) is the first package of this repository on `ckLanguage: 2`. Every
other domain package followed (Industry.Basic 2.7.0, Industry.Energy 3.7.0, Industry.Fluid 2.6.0,
Industry.Logistics 3.5.0, Industry.Maintenance 2.6.0 (2.7.0, below), Industry.Manufacturing 2.7.0, Environment 3.7.0,
Basic.Energy 2.1.0, Basic.Accounting 1.14.0, EnergyCommunity 5.1.0), each with `derivable: Any` on every type and record
and with all dependency ranges and floors unchanged (gate: MINOR per package, the only bump-relevant row is the language change).
The demo packages (`OctoSdkDemo`, `OctoEnergyDemo`) stay on ckLanguage 1. Every compiled ckLanguage 2 model carries
`minEngineVersion` 3.5.1 and is published under `ck-models/v3/` only.

**Rule for every model that moves to ckLanguage 2:** in ckLanguage 2 an omitted `derivable` means `Model` (only the
declaring model may derive), in ckLanguage 1 it means `Any`. Raising the language without `derivable: Any` on every
type and record is therefore a **Major** per element (semver row T6), and every subtype in another model fails with
message 113 (`CkElementNotDerivable`). Basic declares `derivable: Any` on all its types and records. The four hot bases
(`Basic/NamedEntity`, `Basic/TreeNode`, `Basic/Asset`, and `Industry.Basic/Machine`, ckLanguage 2 since Industry.Basic 2.7.0) stay
`derivable: Any` on purpose: each has subtypes in other packages (33 cross-package subtypes in 11 packages of this
repository, counted 2026-11-16, including `Basic/Document` for EnergyCommunity, plus models in other repositories), so
`Model` would break them. The same reason made the System model declare `derivable: Any` everywhere.

**Interfaces came in a second release (Basic 2.7.0).** Letting `NamedEntity` implement `System/Named-1` and `System/Describable-1` and
`TreeNode` implement `System/Hierarchical-1` needs the System floor `[2.7,3.0)` (the interfaces exist from System 2.7.0;
the compiler refuses an `implements` that the range floor cannot resolve). Raising the floor beyond the System version
the previously published Basic resolved is a Major (row D1): Basic 2.5.0 resolved System 2.5.0. Basic 2.6.0 resolves
System 2.8.0, so the floor raise was a Minor in the release after it, Basic 2.7.0.

**Basic 2.7.0 implements System interfaces.** `NamedEntity` implements `System/Named-1` and
`System/Describable-1`, `TreeNode` implements `System/Hierarchical-1`; System floor `[2.7,3.0)`. Rules for every
package that implements an interface of another CK model:

- The project needs a `<PackageReference>` to the C# package of the model that declares the interface
  (for System: `Meshmakers.Octo.ConstructionKit.Models.System`, `Version="$(OctoCoreLibVersion)"`). Without it the
  source generator reports `OM1006` ("no referenced assembly declares the generated interface"); with two
  assemblies that declare it, `OM1006` asks to keep only one reference.
- The generated class lists foreign interfaces fully qualified
  (`global::Meshmakers.Octo.ConstructionKit.Models.System.Generated.System.v2.IRtNamed`) and implements their
  members explicitly; interfaces of the own model keep the short name `IRt<Name>`.
- Dependents need no change: they inherit the interfaces through their base types and get the System package
  transitively through Basic's package (Industry.Maintenance 2.7.0 additionally implements one itself, see below).
  Their Basic floor stays 2.4 (Industry.Energy, Fluid, Maintenance and Manufacturing depend on Industry.Basic with
  floor 2.5). The releases published before the ckLanguage 2 move all resolved
  Basic 2.5.0, so raising their floor to 2.7 then would have been a Major (row D1); the ckLanguage 2
  releases resolve Basic 2.7.0, so a Basic floor of 2.7 no longer exceeds
  the newest published resolution and is a Minor. Because the floors stayed unchanged in those releases, the inherited
  interfaces appear on a tenant only once it runs Basic 2.7.0 or later.
- **Floors of the dependents.** Before a package raises its System or Basic floor (for example to
  implement an interface itself), check which versions its newest *published* release resolved
  (`dependencies` in the catalog JSON): a floor above that is a Major (row D1). The way out is two releases: first the
  language change with the floors unchanged, then the floor raise with the `implements` (Minor). Precondition: the
  first release resolves only the dependency versions its build can see (the catalog, or a sibling built in the same
  run), so the dependency release (e.g. Basic 2.7.0) must be published, or built in the same run, before the first
  release; otherwise it resolves the older version and the later floor raise is a Major again.
  `Industry.Maintenance/EnergyBalance` -> `System/Describable-1` followed this pattern: Industry.Maintenance 2.6.0
  (language change, resolved System 2.8.0), then 2.7.0 (System floor `[2.7,3.0)` plus `implements`,
  Minor). A dependent that implements a System interface gets the System C# package transitively through Basic's
  package and needs no own `<PackageReference>` (Industry.Maintenance has none; its build reports no `OM1006`).
  EnergyCommunity's `CommunitySettings`, `SimulationSettings` (`System/Description` named `Comment`) and
  `SimulationResetRequest` (named `Message`) cannot implement `Describable-1` without a breaking rename (message 98)
  and stay without it.

Gate command (local; replace `<ChangedModelProject>` with the project of the changed model; `-lcr` points at a new, empty directory, so no stale local catalog entry takes part in the
resolution and nothing is written into the shared local catalog):

```bash
octo-ckc -c ValidateVersion -p src/ConstructionKits/<ChangedModelProject>/ConstructionKit \
  -cn PrivateGitHubCatalog -rf -lcr "$(mktemp -d)"
```
