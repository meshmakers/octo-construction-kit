# Changelog

## 2.7.0 - 2026-11-16

_Required bump level: minor._

**EnergyBalance implements System/Describable-1 (AB#9146, Feature F4.4):** `EnergyBalance` implements `System/Describable-1`, so `runtime.byInterface(System/Describable-1)` also returns energy balances. The member matches exactly: the optional `System/Description` attribute is already assigned under the name `Description`. The System range floor rises to `[2.7,3.0)`, because the interface exists from System 2.7.0. Against Industry.Maintenance 2.6.0 (which resolved System 2.8.0) the floor raise is a Minor (row D1); the Industry.Basic range is unchanged and nothing is renamed or retargeted. No other package of this repository depends on Industry.Maintenance.

The generated `RtEnergyBalance` implements `global::Meshmakers.Octo.ConstructionKit.Models.System.Generated.System.v2.IRtDescribable`. The project needs no own reference to `Meshmakers.Octo.ConstructionKit.Models.System`: it receives that package transitively through Industry.Basic and Basic (Basic references it since 2.7.0), so the source generator finds the interface (no `OM1006`).

### Changed

- Dependency range 'System': floor changed '2.5.0' → '2.7.0', range '[2.5,3.0)' → '[2.7,3.0)' _(minor — dependency floor raised, tenants need System 2.7.0 or later (row D1))_
- Type 'EnergyBalance': implements 'System@2/Describable-1' _(minor — type implements an additional interface)_

## 2.6.0 - 2026-11-16

_Required bump level: minor._

**CK language 2 (AB#8869, Feature F4.4):** this package moves to `ckLanguage: 2` on top of Basic v2. In ckLanguage 2 an omitted `derivable` means `Model`, so every type and every record declares `derivable: Any` explicitly: raising the language without it would be a Major per element (row T6) and would break every subtype in another model with message 113 (`CkElementNotDerivable`). With the explicit modifier the compiled elements are unchanged; the only change is the language. The compiled model carries `minEngineVersion` 3.5.1 and is published under `ck-models/v3/` only. All dependency ranges and floors stay as they were (raising a floor beyond the version the previous release resolved is a Major, row D1). Types derived from `Basic/NamedEntity` or `Basic/TreeNode` inherit the System interfaces of Basic (`Named-1`, `Describable-1`, `Hierarchical-1`); nothing is redeclared. Because the Basic floor is unchanged, the inherited System interfaces appear once the tenant runs Basic ≥ 2.7.0 (a tenant on an older Basic shows none, as intended).

**Next step (not in this release):** `EnergyBalance` implements `System/Describable-1` (its optional `System/Description` attribute is named `Description`, an exact match). That needs the System floor `[2.7,3.0)`, and raising the floor beyond the System version the previous release resolved (Industry.Maintenance 2.5.0 resolved System 2.5.0) is a Major (row D1). This 2.6.0 resolves System 2.8.0, so the floor raise and the `implements` become a Minor in the next release (the two-step pattern of Basic 2.6.0 / 2.7.0).

### Changed

- Model 'Industry.Maintenance': ckLanguage changed '1' → '2' _(minor — CK language version raised; older engines reject the model with a clear error)_
- Dependency 'Industry.Basic': version changed '2.6.0' → '2.7.0' _(none — resolved dependency Industry.Basic moved 2.6.0 -> 2.7.0 inside its declared range [2.5,3.0) of a range-retaining model; the closure is compiler-set metadata and the model itself is unchanged, no bump needed (row D8))_
- Dependency 'System': version changed '2.5.0' → '2.8.0' _(none — resolved dependency System moved 2.5.0 -> 2.8.0 inside its declared range [2.5,3.0) of a range-retaining model; the closure is compiler-set metadata and the model itself is unchanged, no bump needed (row D8))_
- Dependency 'Basic': version changed '2.5.0' → '2.7.0' _(none — resolved dependency Basic moved 2.5.0 -> 2.7.0 inside its major of a range-retaining model; the closure is compiler-set metadata and the model itself is unchanged, no bump needed (row D8))_
