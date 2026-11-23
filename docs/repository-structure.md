---
description: Technology stack, repository layout and the per-project Construction Kit folder structure.
applies_to: Octo.ConstructionKit.sln, Directory.Build.props, src/ConstructionKits/*/*.csproj
---

# Repository structure and technology

## Technology Stack

- **Language**: C# on `net10.0` (`Directory.Build.props`)
- **Build System**: MSBuild / dotnet CLI
- **Configuration Format**: YAML for Construction Kit definitions
- **CI/CD**: Azure Pipelines (`azure-pipelines.yml`)
- **Package Management**: NuGet (private server option available)

**Key Dependencies** (referenced by every CK project, version `$(OctoCoreLibVersion)`):
- `Meshmakers.Octo.Runtime.Contracts` - Runtime contracts for the SDK
- `Meshmakers.Octo.ConstructionKit.SourceGeneration` - Code generation analyzer
- `Meshmakers.Octo.ConstructionKit.MsBuildTasks` - Build-time task execution

## Project Structure

```
octo-construction-kit/
├── src/
│   ├── ConstructionKits/                       # One project per Construction Kit
│   │   ├── Octo.Sdk.Packages.Basic/
│   │   ├── Octo.Sdk.Packages.Basic.Accounting/
│   │   ├── Octo.Sdk.Packages.Basic.Energy/
│   │   ├── Octo.Sdk.Packages.Industry.Basic/
│   │   ├── Octo.Sdk.Packages.Industry.Energy/
│   │   ├── Octo.Sdk.Packages.Industry.Fluid/
│   │   ├── Octo.Sdk.Packages.Industry.Logistics/
│   │   ├── Octo.Sdk.Packages.Industry.Maintenance/
│   │   ├── Octo.Sdk.Packages.Industry.Manufacturing/
│   │   ├── Octo.Sdk.Packages.Environment/
│   │   ├── Octo.Sdk.Packages.EnergyCommunity/
│   │   ├── Octo.Sdk.Demo/                      # Demo/example CK
│   │   └── Octo.Energy.Demo/                   # Energy-specific demo
│   ├── Blueprints/                             # Installable blueprints (Locations.Austria, Samples.*)
│   ├── Samples/                                # GraphQL sample
│   └── grafana/                                # Monitoring/visualization
├── assets/                                     # Brand assets (icons, etc.)
├── azure-pipelines.yml                         # CI pipeline (templates from octo-pipeline-templates)
├── Directory.Build.props                       # Global MSBuild properties
└── Octo.ConstructionKit.sln                    # Solution
```

The repository has no test projects. The checks are the build itself (schema validation, source generation, the
ownership marker lint) and, in the pipeline, the version gate of the CK publish step.

### Construction Kit Structure (per-project)

Each Construction Kit follows this internal structure:

```
Octo.Sdk.Packages.XXX/
├── ConstructionKit/                    # Model definitions
│   ├── ckModel.yaml                   # CK metadata and dependencies
│   ├── CHANGELOG.md                   # Release notes per model version
│   ├── types/                         # Type definitions
│   ├── records/                       # Record/struct definitions
│   ├── attributes/                    # Attribute specifications
│   ├── enums/                         # Enumeration definitions
│   └── associations/                  # Relationship definitions
├── Octo.Sdk.Packages.XXX.csproj       # Project file
└── bin/<Configuration>/net10.0/
    └── octo-ck-libraries/XXX/
        └── out/ck-<model>-<major>.yaml    # Compiled CK library
```
