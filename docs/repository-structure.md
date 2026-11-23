---
description: Technology stack, repository layout, the per-project Construction Kit folder structure and the data mesh design notes.
applies_to: Octo.ConstructionKit.sln, Directory.Build.props, src/ConstructionKits/*/*.csproj
---

# Repository structure and technology

## Technology Stack

- **Language**: C# (.NET 9.0)
- **Build System**: MSBuild / dotnet CLI
- **Configuration Format**: YAML for Construction Kit definitions
- **Testing Framework**: xUnit with FakeItEasy for mocking
- **CI/CD**: Azure Pipelines
- **IDE Support**: Visual Studio, Rider (JetBrains), VS Code
- **Package Management**: NuGet (private server option available)

**Key Dependencies**:
- `Meshmakers.Octo.Runtime.Contracts` - Runtime contracts for the SDK
- `Meshmakers.Octo.ConstructionKit.SourceGeneration` - Code generation analyzer
- `Meshmakers.Octo.ConstructionKit.MsBuildTasks` - Build-time task execution
- `Meshmakers.Octo.Sdk.ServiceClient` - Service client for system tests
## Project Structure

```
octo-construction-kit/
├── src/
│   ├── ConstructionKits/                                    # Main CK implementations
│   │   ├── Octo.Sdk.Packages.Basic/                        # Base construction kit
│   │   ├── Octo.Sdk.Packages.Industry.Basic/               # Industry-specific basic
│   │   ├── Octo.Sdk.Packages.Industry.Energy/              # Energy sector CK
│   │   ├── Octo.Sdk.Packages.Industry.Fluid/               # Fluid systems CK
│   │   ├── Octo.Sdk.Packages.Industry.Logistics/           # Logistics/fulfillment CK
│   │   ├── Octo.Sdk.Packages.Industry.Maintenance/         # Maintenance operations CK
│   │   ├── Octo.Sdk.Packages.Industry.Manufactoring/       # Manufacturing operations CK
│   │   ├── Octo.Sdk.Packages.Environment/                  # Environmental data CK
│   │   ├── Octo.Sdk.Packages.EnergyCommunity/              # Energy community CK
│   │   ├── Octo.Sdk.Packages.Monitoring/                   # Platform monitoring CK (CK health watchdog config)
│   │   ├── Octo.Sdk.Demo/                                  # Demo/example CK
│   │   └── Octo.Energy.Demo/                               # Energy-specific demo
│   └── Octo.Sdk.Packages.Industry.Basic.old/               # Legacy basic CK
├── tests/
│   ├── Octo.Sdk.Packages.Industry.Basic.old.SystemTests/   # System tests
│   └── Samples/                                             # Sample projects
├── src/Blueprints/                                          # Installable blueprints (Locations.Austria, Samples.*)
├── devops-build/                                            # CI/CD configuration
│   ├── azure-pipelines.yml                                 # Main Azure Pipeline
│   ├── set-version.yml                                     # Version setting
│   ├── update-build-number.yml                             # Build number updates
│   ├── handle-artifacts.yml                                # Artifact handling
│   └── push-nuget-package.yml                              # NuGet publishing
├── assets/                                                  # Brand assets (icons, etc.)
├── grafana/                                                 # Monitoring/visualization
├── Directory.Build.props                                    # Global MSBuild properties
├── Octo.ConstructionKit.sln                                # Visual Studio solution
└── LICENSE                                                 # MIT License
```

## Construction Kit Structure (per-project)

Each Construction Kit follows this internal structure:

```
Octo.Sdk.Packages.XXX/
├── ConstructionKit/                    # Model definitions
│   ├── ckModel.yaml                   # CK metadata and dependencies
│   ├── types/                         # Type definitions
│   │   ├── Asset.yaml
│   │   ├── Employee.yaml
│   │   ├── Document.yaml
│   │   └── ...
│   ├── records/                       # Record/struct definitions
│   │   ├── Address.yaml
│   │   ├── Contact.yaml
│   │   ├── PhoneNumber.yaml
│   │   └── ...
│   ├── attributes/                    # Attribute specifications
│   │   ├── order.yaml
│   │   ├── machine.yaml
│   │   └── ...
│   ├── enums/                         # Enumeration definitions
│   │   ├── productionOrderItemState.yaml
│   │   └── ...
│   └── associations/                  # Relationship definitions
│       ├── Asset.yaml
│       ├── associations.yaml
│       └── ...
├── Octo.Sdk.Packages.XXX.csproj       # Project file
└── bin/Release/net10.0/
    └── octo-ck-libraries/XXX/
        └── out/ck-XXX.yaml            # Compiled CK library
```
## Additional Notes

## Data Mesh Architecture

This project implements part of the **OCTO Data Mesh** framework, providing modular, domain-specific SDK packages through Construction Kits. Each CK represents a well-defined bounded context with its own data model and contracts.

## Extensibility

New Construction Kits can be added independently and composed with existing ones via dependency declarations in `ckModel.yaml`. This allows incremental system growth without modifying existing packages.

## Code Generation

The source generation pipeline automatically creates:
- C# entity classes from YAML type definitions
- Attribute metadata classes
- Enum implementations
- Association definitions
- Documentation

This keeps the C# code DRY and model-driven.
