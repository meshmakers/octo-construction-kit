---
description: What a Construction Kit contains, the key model files and what the build does with the CK YAML.
applies_to: src/ConstructionKits/**/ConstructionKit/**
---

# Construction Kit models and the build

## What is a Construction Kit?

A Construction Kit is a domain-specific data model package containing:

- **Types**: Core domain entities (Asset, Employee, Document, etc.)
- **Records**: Value objects and structures (Address, Contact, PhoneNumber)
- **Attributes**: Property specifications for types
- **Enums**: Enumerated values (e.g., ProductionOrderItemState)
- **Associations**: Relationships between types
- **Dependencies**: References to other CKs

## Key CK Model Files

**ckModel.yaml** - Package metadata:
```yaml
$schema: https://schemas.meshmakers.cloud/construction-kit-meta.schema.json
modelId: Basic
dependencies:
  - System-[1.0,)
```

**Type Definition Example** (types/State.yaml):
```yaml
$schema: https://schemas.meshmakers.cloud/construction-kit-elements.schema.json
types:
  - typeId: State
    derivedFromCkTypeId: ${this}/TreeNode
```

## Build-Time Processing

During `dotnet build`, MSBuild tasks:

1. Scan `ConstructionKit/` folder for YAML definitions
2. Validate against JSON schemas
3. Generate C# code via source generators
4. Create compiled CK library output (`octo-ck-XXX.yaml`)
5. Optionally generate documentation
6. Lint the attribute ownership markers (`CkLintRuntimeStateMarkers`, see below)
