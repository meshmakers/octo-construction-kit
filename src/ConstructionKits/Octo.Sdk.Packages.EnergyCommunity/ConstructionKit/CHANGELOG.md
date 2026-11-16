# Changelog

## 5.1.0 - 2026-11-16

_Required bump level: minor._

**CK language 2 (AB#8870, Feature F4.4):** this package moves to `ckLanguage: 2` on top of Basic v2. In ckLanguage 2 an omitted `derivable` means `Model`, so every type and every record declares `derivable: Any` explicitly: raising the language without it would be a Major per element (row T6) and would break every subtype in another model with message 113 (`CkElementNotDerivable`). With the explicit modifier the compiled elements are unchanged; the only change is the language. The compiled model carries `minEngineVersion` 3.5.1 and is published under `ck-models/v3/` only. All dependency ranges and floors stay as they were (raising a floor beyond the version the previous release resolved is a Major, row D1). Types derived from `Basic/NamedEntity` or `Basic/TreeNode` inherit the System interfaces of Basic (`Named-1`, `Describable-1`, `Hierarchical-1`); nothing is redeclared. Because the Basic floor is unchanged, the inherited System interfaces appear once the tenant runs Basic ≥ 2.7.0 (a tenant on an older Basic shows none, as intended).

Existing EnergyCommunity tenants upgrade without a data migration: the elements, their ids and the stored data are unchanged.

**Known gap: no `System/Describable-1`.** `CommunitySettings` and `SimulationSettings` carry `System/Description` under the name `Comment`, and `SimulationResetRequest` under the name `Message`. `Describable-1` requires the member `System/Description` with the name `Description` (members match on attribute id and name, messages 96-99 and 121), so these types cannot implement it without renaming the attribute, which is a breaking change (message 98). They are deliberately left without the interface; no rename is made.

### Changed

- Model 'EnergyCommunity': ckLanguage changed '1' → '2' _(minor — CK language version raised; older engines reject the model with a clear error)_
- Dependency 'Basic': version changed '2.5.0' → '2.7.0' _(none — resolved dependency Basic moved 2.5.0 -> 2.7.0 inside its declared range [2.4,3.0) of a range-retaining model; the closure is compiler-set metadata and the model itself is unchanged, no bump needed (row D8))_
- Dependency 'Basic.Energy': version changed '2.0.0' → '2.1.0' _(none — resolved dependency Basic.Energy moved 2.0.0 -> 2.1.0 inside its declared range [2.0,3.0) of a range-retaining model; the closure is compiler-set metadata and the model itself is unchanged, no bump needed (row D8))_
- Dependency 'System': version changed '2.7.0' → '2.8.0' _(none — resolved dependency System moved 2.7.0 -> 2.8.0 inside its declared range [2.5,3.0) of a range-retaining model; the closure is compiler-set metadata and the model itself is unchanged, no bump needed (row D8))_

## 5.0.0 - 2026-10-27

_Required bump level: major._

### Breaking

- Dependency 'Basic.Energy': version changed '1.10.0' → '2.0.0' _(major — dependency switched to a new major version — transitively breaking)_
- Dependency range 'Basic.Energy': range changed '[1.9,2.0)' → '[2.0,3.0)' _(major — dependency range excludes the version the previous release resolved (1.10.0) — a tenant that installed the previous release with it cannot import this release without changing the dependency first (rows D1/D3))_
- Dependency range 'Basic.Energy': floor changed '1.9.0' → '2.0.0' _(major — dependency floor excludes the version the previous release resolved (1.10.0) — a tenant that installed the previous release with it cannot import this release without changing the dependency first (rows D1/D3))_
- Type association 'BillingDocumentLineItem-1/EnergyCommunity/BilledMeteringPoint-1 -> Basic.Energy@1/MeteringPoint-1' removed _(major — consumers use the association navigation)_
- Type 'Consumer-1': derivedFromCkTypeId changed 'Basic.Energy@1/Consumer-1' → 'Basic.Energy@2/Consumer-1' _(major — inheritance hierarchy breaks (GraphQL schema, queries))_
- Type attribute 'Consumer-1/PartitionFactor': id changed 'Basic.Energy@1/PartitionFactor-1' → 'Basic.Energy@2/PartitionFactor-1' _(major — attribute assignment references a different attribute definition (defensive))_
- Type attribute 'Customer-1/State': id changed 'Basic.Energy@1/State-1' → 'Basic.Energy@2/State-1' _(major — attribute assignment references a different attribute definition (defensive))_
- Type association 'Customer-1/EnergyCommunity/AssociatedFacilities-1 -> Basic.Energy@1/OperatingFacility-1' removed _(major — consumers use the association navigation)_
- Type association 'ParticipationPeriod-1/EnergyCommunity/ParticipationPeriod-1 -> Basic.Energy@2/MeteringPoint-1' added _(major — new mandatory association (multiplicity One) — existing instances become invalid)_
- Type association 'ParticipationPeriod-1/EnergyCommunity/ParticipationPeriod-1 -> Basic.Energy@1/MeteringPoint-1' removed _(major — consumers use the association navigation)_
- Type 'Producer-1': derivedFromCkTypeId changed 'Basic.Energy@1/Producer-1' → 'Basic.Energy@2/Producer-1' _(major — inheritance hierarchy breaks (GraphQL schema, queries))_
- Type attribute 'Producer-1/PartitionFactor': id changed 'Basic.Energy@1/PartitionFactor-1' → 'Basic.Energy@2/PartitionFactor-1' _(major — attribute assignment references a different attribute definition (defensive))_

### Added

- Type association 'BillingDocumentLineItem-1/EnergyCommunity/BilledMeteringPoint-1 -> Basic.Energy@2/MeteringPoint-1' added _(minor — purely additive association)_
- Type association 'Customer-1/EnergyCommunity/AssociatedFacilities-1 -> Basic.Energy@2/OperatingFacility-1' added _(minor — purely additive association)_

### Changed

- Dependency 'System': version changed '2.5.0' → '2.7.0' _(minor — compatible dependency version change)_
