# Changelog

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
