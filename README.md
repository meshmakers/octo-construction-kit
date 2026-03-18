# Octo Construction Kit

Domain-specific data model packages for the [OctoMesh](https://www.meshmakers.io) platform. Each **Construction Kit** (CK) defines a bounded context with types, records, attributes, enumerations and associations — all described in YAML and compiled into NuGet packages at build time.

## Available Construction Kits

| Package | Model ID | Depends On | Description |
|---------|----------|------------|-------------|
| **Basic** | `Basic-2.0.2` | System | Core domain model (Asset, Employee, Document, TreeNode, ...) |
| **Basic.Accounting** | `Basic.Accounting-1.3.0` | Basic | Accounting and billing domain |
| **Basic.Energy** | `Basic.Energy-1.0.1` | Basic | Base energy domain types |
| **Industry.Basic** | `Industry.Basic-2.1.0` | Basic | Extended industrial domain model |
| **Industry.Energy** | `Industry.Energy-2.0.0` | Industry.Basic | Energy sector specific types |
| **Industry.Fluid** | `Industry.Fluid-2.0.0` | Industry.Basic | Fluid system management |
| **Industry.Maintenance** | `Industry.Maintenance-2.0.0` | Industry.Basic | Maintenance operations and scheduling |
| **Industry.Manufacturing** | `Industry.Manufacturing-2.0.0` | Industry.Basic | Production orders and manufacturing |
| **Environment** | `Environment-2.0.0` | Basic | Environmental data and monitoring |
| **EnergyCommunity** | `EnergyCommunity-3.0.3` | Basic | Energy community management |

### Demo Packages

| Package | Model ID | Description |
|---------|----------|-------------|
| **Octo.Sdk.Demo** | `OctoSdkDemo-1.0.1` | General SDK demo |
| **Octo.Energy.Demo** | `OctoEnergyDemo` | Energy domain demo |

### Dependency Graph

```
System
  └── Basic
        ├── Basic.Accounting
        ├── Basic.Energy
        ├── Environment
        ├── EnergyCommunity
        ├── OctoSdkDemo
        └── Industry.Basic
              ├── Industry.Energy
              ├── Industry.Fluid
              ├── Industry.Maintenance
              └── Industry.Manufacturing
```

## Project Structure

```
octo-construction-kit/
├── src/
│   ├── ConstructionKits/       # All CK implementations
│   ├── Samples/                # Import scripts and sample data
│   └── grafana/                # Grafana dashboard definitions
├── tests/                      # System tests
├── devops-build/               # Azure Pipelines CI/CD
├── assets/                     # Brand assets (icons)
├── Directory.Build.props       # Global MSBuild properties
└── Octo.ConstructionKit.sln    # Solution file
```

### Construction Kit Layout

Each CK project follows the same structure:

```
Octo.Sdk.Packages.XXX/
├── ConstructionKit/
│   ├── ckModel.yaml            # Metadata: modelId, dependencies
│   ├── types/                  # Entity definitions (e.g. Asset.yaml)
│   ├── records/                # Value objects (e.g. Address.yaml)
│   ├── attributes/             # Attribute specifications
│   ├── enums/                  # Enumerations
│   └── associations/           # Relationship definitions
└── Octo.Sdk.Packages.XXX.csproj
```

## Technology Stack

- **.NET 10.0** / C# (latest major)
- **YAML** for model definitions (validated against [JSON schemas](https://schemas.meshmakers.cloud/construction-kit-meta.schema.json))
- **MSBuild Tasks** compile YAML into C# source code and CK library artifacts
- **NuGet** for package distribution
- **xUnit** + FakeItEasy for testing
- **Azure Pipelines** for CI/CD

## How It Works

1. Domain models are authored as YAML files inside `ConstructionKit/` directories
2. At build time, `Meshmakers.Octo.ConstructionKit.MsBuildTasks` validates the YAML against schemas
3. `Meshmakers.Octo.ConstructionKit.SourceGeneration` generates C# classes (entities, attributes, enums, associations)
4. A compiled CK library (`ck-*.yaml`) is produced as a build artifact
5. NuGet packages are generated automatically (`GeneratePackageOnBuild`)

## Getting Started

### Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download)

### Build

```bash
# Restore and build
dotnet build Octo.ConstructionKit.sln --configuration Release

# Local development build (uses version 999.0.0, includes local nuget folder)
dotnet build Octo.ConstructionKit.sln --configuration DebugL

# With private NuGet server
dotnet build Octo.ConstructionKit.sln --configuration Release /p:OctoNugetPrivateServer=https://your-nuget-server
```

### Test

```bash
# Run all tests (excluding system tests)
dotnet test Octo.ConstructionKit.sln --configuration Release --filter "FullyQualifiedName!~SystemTests"
```

## Creating a New Construction Kit

1. Create a project folder under `src/ConstructionKits/`
2. Add a `.csproj` referencing:
   - `Meshmakers.Octo.Runtime.Contracts`
   - `Meshmakers.Octo.ConstructionKit.SourceGeneration` (analyzer)
   - `Meshmakers.Octo.ConstructionKit.MsBuildTasks`
3. Create a `ConstructionKit/` directory with a `ckModel.yaml`:
   ```yaml
   $schema: https://schemas.meshmakers.cloud/construction-kit-meta.schema.json
   modelId: YourModelId
   dependencies:
     - Basic-[2.0,3.0)
   ```
4. Add YAML definitions in `types/`, `records/`, `attributes/`, `enums/`, `associations/` subdirectories
5. Add the project to `Octo.ConstructionKit.sln`
6. Update `devops-build/azure-pipelines.yml` for CI artifact publishing

## Sample Data

The `src/Samples/` directory contains PowerShell scripts for importing sample data into an OctoMesh instance:

- `om_importck.ps1` -- Import construction kits
- `om_create_tenants.ps1` / `om_delete_tenants.ps1` -- Tenant management
- `om_importrt_sample_*.ps1` -- Import runtime sample data (general, maintenance, simulation, PV)

Sample data sets cover maintenance, simulator, PV (photovoltaic), and zenon integration scenarios.

## Build Configurations

| Configuration | Version | NuGet Sources | Purpose |
|---------------|---------|---------------|---------|
| **Debug** | Default | nuget.org | Development |
| **Release** | `3.3.*` | nuget.org | Production builds |
| **DebugL** | `999.0.0` | nuget.org + local `../nuget/` | Local cross-repo development |

## License

[MIT](LICENSE) -- Copyright (c) 2026 [meshmakers.io](https://www.meshmakers.io)
