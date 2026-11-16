# Changelog

## 2.6.0 - 2026-11-16

_Required bump level: minor._

**CK language 2 (AB#8868, Feature F4.4):** Basic is the first domain package on `ckLanguage: 2`. In ckLanguage 2 an omitted `derivable` means `Model`, so every type and every record declares `derivable: Any` explicitly: almost every domain package derives from Basic types across model boundaries (the hot bases `NamedEntity`, `TreeNode`, `Asset` and, through Industry.Basic, `Machine`, plus `Document` for EnergyCommunity: 33 cross-package subtypes in 11 packages of this repository, counted 2026-11-16, plus models in other repositories), and `Model` would be a Major per element (row T6) and break them with message 113. With the explicit modifier the compiled elements are unchanged; the only change is the language. The compiled model carries `minEngineVersion` 3.5.1 and is published under `ck-models/v3/` only. Every dependent model compiles unchanged against it (no bump needed). The System range stays `[2.5,3.0)`.

**Next step (not in this release):** `NamedEntity` implements `System/Named-1` and `System/Describable-1`, `TreeNode` implements `System/Hierarchical-1`. That needs the System floor 2.7, and raising the floor is only a Minor once a published Basic resolved System 2.7 or later (row D1) - this 2.6.0 does. It also waits for an engine fix: the source generator does not resolve interfaces of another model yet.

### Changed

- Model 'Basic': ckLanguage changed '1' → '2' _(minor — CK language version raised; older engines reject the model with a clear error)_
- Dependency 'System': version changed '2.5.0' → '2.8.0' _(none — resolved dependency System moved 2.5.0 -> 2.8.0 inside its declared range [2.5,3.0) of a range-retaining model; the closure is compiler-set metadata and the model itself is unchanged, no bump needed (row D8))_
