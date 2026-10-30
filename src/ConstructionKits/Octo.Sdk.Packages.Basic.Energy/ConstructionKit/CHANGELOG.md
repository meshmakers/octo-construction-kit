# Changelog

## 2.0.0 - 2026-10-27

_Required bump level: major._

**Breaking (AB#5356, decided by Reimar Klammer 2026-10-27):** DataQuality knows only the real qualities L1, L2 and L3. `Unknown` (0) and `Manual` (4) are removed: whether a value was entered by hand is recorded as the origin of the archive row (`$Origin`), not as a quality. Release gate: before 2.0.0 reaches a tenant, verify that the tenant stores no DataQuality 0 or 4; fix such rows first: rows with Manual (4) get origin manual plus a real quality; rows with Unknown (0) came from automated deliveries, so they keep origin automated and get a real quality (or are requested again). There is deliberately no migration that maps 0/4 to a quality.

### Breaking

- Enum value 'DataQuality-1/Unknown' removed _(major — stored values become unreadable)_
- Enum value 'DataQuality-1/Manual' removed _(major — stored values become unreadable)_

### Changed

- Dependency 'System': version changed '2.5.0' → '2.7.0' _(none — resolved dependency System moved 2.5.0 -> 2.7.0 inside its declared range [2.5,3.0) of a range-retaining model; the closure is compiler-set metadata and the model itself is unchanged, no bump needed (row D8))_
- Enum 'DataQuality-1': description changed 'Defines the quality of the metering data' → 'Defines the quality of the metering data. Only real qualities: whether a value was entered manually is recorded as the origin of the archive row, not as a quality.' _(patch — purely documentational change)_
