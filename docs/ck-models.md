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
modelId: Basic-2.7.0
ckLanguage: 2
dependencies:
  - System-[2.7,3.0)
```

**Type Definition Example** (types/Asset.yaml of Basic, shortened):
```yaml
$schema: https://schemas.meshmakers.cloud/construction-kit-elements.schema.json
types:
- typeId: Asset
  derivable: Any
  derivedFromCkTypeId: ${this}/TreeNode
```

## YAML conventions

Every CK YAML file starts with the `$schema` directive of its kind (`construction-kit-meta`, `construction-kit-elements`,
and so on), as the examples above show. Types of the same model are referenced as `${this}/<Name>`. Exception:
the `migrations/*.yaml` files use full ids instead of `${this}/`.

## Build-Time Processing

During `dotnet build`, MSBuild tasks:

1. Scan `ConstructionKit/` folder for YAML definitions
2. Validate against JSON schemas
3. Generate C# code via source generators
4. Create compiled CK library output (`ck-<model>-<major>.yaml`)
5. Optionally generate documentation
6. Lint the attribute ownership markers (`CkLintRuntimeStateMarkers`, see [ck-ownership-markers.md](ck-ownership-markers.md))
