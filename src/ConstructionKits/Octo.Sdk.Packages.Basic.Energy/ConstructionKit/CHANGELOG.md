# Changelog

## 2.1.0 - 2026-11-16

_Required bump level: minor._

**CK language 2 (AB#8870, Feature F4.4):** this package moves to `ckLanguage: 2` on top of Basic v2. In ckLanguage 2 an omitted `derivable` means `Model`, so every type and every record declares `derivable: Any` explicitly: raising the language without it would be a Major per element (row T6) and would break every subtype in another model with message 113 (`CkElementNotDerivable`). With the explicit modifier the compiled elements are unchanged; the only change is the language. The compiled model carries `minEngineVersion` 3.5.1 and is published under `ck-models/v3/` only. All dependency ranges and floors stay as they were (raising a floor beyond the version the previous release resolved is a Major, row D1). Types derived from `Basic/NamedEntity` or `Basic/TreeNode` inherit the System interfaces of Basic (`Named-1`, `Describable-1`, `Hierarchical-1`); nothing is redeclared. Because the Basic floor is unchanged, the inherited System interfaces appear once the tenant runs Basic ≥ 2.7.0 (a tenant on an older Basic shows none, as intended).

`Consumer` and `Producer` are cross-model bases (EnergyCommunity derives from them) and stay `derivable: Any` on purpose.

### Changed

- Model 'Basic.Energy': ckLanguage changed '1' → '2' _(minor — CK language version raised; older engines reject the model with a clear error)_
- Dependency 'Basic': version changed '2.5.0' → '2.7.0' _(none — resolved dependency Basic moved 2.5.0 -> 2.7.0 inside its declared range [2.4,3.0) of a range-retaining model; the closure is compiler-set metadata and the model itself is unchanged, no bump needed (row D8))_
- Dependency 'System': version changed '2.7.0' → '2.8.0' _(none — resolved dependency System moved 2.7.0 -> 2.8.0 inside its declared range [2.5,3.0) of a range-retaining model; the closure is compiler-set metadata and the model itself is unchanged, no bump needed (row D8))_

## 2.0.0 - 2026-10-27

_Required bump level: major._

**Breaking (AB#5356, decided by Reimar Klammer 2026-10-27):** DataQuality knows only the real qualities L1, L2 and L3. `Unknown` (0) and `Manual` (4) are removed: whether a value was entered by hand is recorded as the origin of the archive row (`$Origin`), not as a quality. Release gate: before 2.0.0 reaches a tenant, verify that the tenant stores no DataQuality 0 or 4; fix such rows first: rows with Manual (4) get origin manual plus a real quality; rows with Unknown (0) came from automated deliveries, so they keep origin automated and get a real quality (or are requested again). There is deliberately no migration that maps 0/4 to a quality.

### Breaking

- Enum value 'DataQuality-1/Unknown' removed _(major — stored values become unreadable)_
- Enum value 'DataQuality-1/Manual' removed _(major — stored values become unreadable)_

### Changed

- Dependency 'System': version changed '2.5.0' → '2.7.0' _(none — resolved dependency System moved 2.5.0 -> 2.7.0 inside its declared range [2.5,3.0) of a range-retaining model; the closure is compiler-set metadata and the model itself is unchanged, no bump needed (row D8))_
- Enum 'DataQuality-1': description changed 'Defines the quality of the metering data' → 'Defines the quality of the metering data. Only real qualities: whether a value was entered manually is recorded as the origin of the archive row, not as a quality.' _(patch — purely documentational change)_
