# Changelog

## 2.7.0 - 2026-11-16

_Required bump level: minor._

**Basic implements the System interfaces (AB#9097, Feature F4.4):** `NamedEntity` implements `System/Named-1` and `System/Describable-1`, and `TreeNode` implements `System/Hierarchical-1`. Every subtype inherits them (`Asset`, `Tree`, `Employee`, `Document`, ... and the subtypes in other packages), so queries such as `runtime.byInterface(System/Hierarchical-1)` address Basic entities by interface. The members match exactly (`System/Name`, `System/Description`; the `System/ParentChild` association of `TreeNode` to `TreeNode` meets the required member of `Hierarchical-1`). The System range floor rises to `[2.7,3.0)`, because the interfaces exist from System 2.7.0. Against Basic 2.6.0 (which resolved System 2.8.0) the floor raise is a Minor (row D1); nothing is renamed or retargeted. Every dependent model of this repository compiles unchanged (required level none) and keeps ckLanguage 1.

The generated C# of `RtNamedEntity` and `RtTreeNode` implements `IRtNamed`, `IRtDescribable` and `IRtHierarchical` of the System model's package (fully qualified, `global::Meshmakers.Octo.ConstructionKit.Models.System.Generated.System.v2.IRt...`). The project therefore references `Meshmakers.Octo.ConstructionKit.Models.System`; without that reference the source generator reports `OM1006`. The C# package of Basic now depends on it (engine 3.5.1 with the AB#9096 generator fix or later).

### Changed

- Dependency range 'System': floor changed '2.5.0' → '2.7.0', range '[2.5,3.0)' → '[2.7,3.0)' _(minor — dependency floor raised, tenants need System 2.7.0 or later (row D1))_
- Type 'NamedEntity': implements 'System@2/Named-1' and 'System@2/Describable-1' _(minor — type implements an additional interface)_
- Type 'TreeNode': implements 'System@2/Hierarchical-1' _(minor — type implements an additional interface)_

## 2.6.0 - 2026-11-16

_Required bump level: minor._

**CK language 2 (AB#8868, Feature F4.4):** Basic is the first domain package on `ckLanguage: 2`. In ckLanguage 2 an omitted `derivable` means `Model`, so every type and every record declares `derivable: Any` explicitly: almost every domain package derives from Basic types across model boundaries (the hot bases `NamedEntity`, `TreeNode`, `Asset` and, through Industry.Basic, `Machine`, plus `Document` for EnergyCommunity: 33 cross-package subtypes in 11 packages of this repository, counted 2026-11-16, plus models in other repositories), and `Model` would be a Major per element (row T6) and break them with message 113. With the explicit modifier the compiled elements are unchanged; the only change is the language. The compiled model carries `minEngineVersion` 3.5.1 and is published under `ck-models/v3/` only. Every dependent model compiles unchanged against it (no bump needed). The System range stays `[2.5,3.0)`.

**Next step (not in this release):** `NamedEntity` implements `System/Named-1` and `System/Describable-1`, `TreeNode` implements `System/Hierarchical-1`. That needs the System floor 2.7, and raising the floor is only a Minor once a published Basic resolved System 2.7 or later (row D1) - this 2.6.0 does.

### Changed

- Model 'Basic': ckLanguage changed '1' → '2' _(minor — CK language version raised; older engines reject the model with a clear error)_
- Dependency 'System': version changed '2.5.0' → '2.8.0' _(none — resolved dependency System moved 2.5.0 -> 2.8.0 inside its declared range [2.5,3.0) of a range-retaining model; the closure is compiler-set metadata and the model itself is unchanged, no bump needed (row D8))_
