# Changelog

## 1.14.0 - 2026-11-16

_Required bump level: minor._

**CK language 2 (AB#8870, Feature F4.4):** this package moves to `ckLanguage: 2` on top of Basic v2. In ckLanguage 2 an omitted `derivable` means `Model`, so every type and every record declares `derivable: Any` explicitly: raising the language without it would be a Major per element (row T6) and would break every subtype in another model with message 113 (`CkElementNotDerivable`). With the explicit modifier the compiled elements are unchanged; the only change is the language. The compiled model carries `minEngineVersion` 3.5.1 and is published under `ck-models/v3/` only. All dependency ranges and floors stay as they were (raising a floor beyond the version the previous release resolved is a Major, row D1). Types derived from `Basic/NamedEntity` or `Basic/TreeNode` inherit the System interfaces of Basic (`Named-1`, `Describable-1`, `Hierarchical-1`); nothing is redeclared. Because the Basic floor is unchanged, the inherited System interfaces appear once the tenant runs Basic ≥ 2.7.0 (a tenant on Basic 2.5 or 2.6 shows none, as intended).

### Changed

- Model 'Basic.Accounting': ckLanguage changed '1' → '2' _(minor — CK language version raised; older engines reject the model with a clear error)_
- Dependency 'Basic': version changed '2.5.0' → '2.6.0' _(none — resolved dependency Basic moved 2.5.0 -> 2.6.0 inside its declared range [2.4,3.0) of a range-retaining model; the closure is compiler-set metadata and the model itself is unchanged, no bump needed (row D8))_
