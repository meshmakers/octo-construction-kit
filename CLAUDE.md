# Octo Construction Kit - Development Guide

## Project Overview

**Octo Construction Kit** is a .NET-based project that implements a modular data mesh SDK framework. The project provides multiple industry-specific "Construction Kits" (CKs) that define domain models using YAML-based schema definitions.

**Repository**: https://github.com/meshmakers/octo-construction-kit  
**License**: MIT (Copyright 2025 meshmakers.io)

---

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

---

## Project Structure

```
octo-construction-kit/
├── src/
│   ├── ConstructionKits/                                    # Main CK implementations
│   │   ├── Octo.Sdk.Packages.Basic/                        # Base construction kit
│   │   ├── Octo.Sdk.Packages.Industry.Basic/               # Industry-specific basic
│   │   ├── Octo.Sdk.Packages.Industry.Energy/              # Energy sector CK
│   │   ├── Octo.Sdk.Packages.Industry.Fluid/               # Fluid systems CK
│   │   ├── Octo.Sdk.Packages.Industry.Maintenance/         # Maintenance operations CK
│   │   ├── Octo.Sdk.Packages.Industry.Manufactoring/       # Manufacturing operations CK
│   │   ├── Octo.Sdk.Packages.Environment/                  # Environmental data CK
│   │   ├── Octo.Sdk.Packages.EnergyCommunity/              # Energy community CK
│   │   ├── Octo.Sdk.Demo/                                  # Demo/example CK
│   │   └── Octo.Energy.Demo/                               # Energy-specific demo
│   └── Octo.Sdk.Packages.Industry.Basic.old/               # Legacy basic CK
├── tests/
│   ├── Octo.Sdk.Packages.Industry.Basic.old.SystemTests/   # System tests
│   └── Samples/                                             # Sample projects
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

### Construction Kit Structure (per-project)

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
└── bin/Release/net9.0/
    └── octo-ck-libraries/XXX/
        └── out/ck-XXX.yaml            # Compiled CK library
```

---

## Build & Development Commands

### Build

```bash
# Build the entire solution
dotnet build Octo.ConstructionKit.sln --configuration Release

# Build with custom NuGet server
dotnet build Octo.ConstructionKit.sln \
  --configuration Release \
  /p:OctoNugetPrivateServer=https://your-nuget-server
```

### Test

```bash
# Run all tests (excluding system tests)
dotnet test Octo.ConstructionKit.sln \
  --configuration Release \
  --filter "FullyQualifiedName!~SystemTests"

# Run specific test project
dotnet test tests/Octo.Sdk.Packages.Industry.Basic.old.SystemTests \
  --configuration Release
```

### Package

```bash
# Build and generate NuGet packages automatically
# (GeneratePackageOnBuild is set to true in csproj files)
dotnet build Octo.ConstructionKit.sln --configuration Release

# Packages are output to: bin/Release/net9.0/*.nupkg
```

### Build Configurations

The solution supports three build configurations:

- **Debug** - Development build with optimizations disabled
- **Release** - Production build with optimizations
- **DebugL** - Local debug build (uses 999.0.0 version, includes local nuget folder)

```bash
# Build with DebugL configuration
dotnet build Octo.ConstructionKit.sln --configuration DebugL
```

### Version Management

Version is controlled via:

1. **OctoVersion** property in `Directory.Build.props`
   - Default: `3.2.*` (Release), `0.1.*` (NuGet private server), `999.0.0` (DebugL)
   
2. **Configuration-based override**:
   - DebugL: Forces `999.0.0`
   
3. **Azure Pipelines**: Dynamically sets version during CI/CD

---

## Construction Kit (CK) Models

### What is a Construction Kit?

A Construction Kit is a domain-specific data model package containing:

- **Types**: Core domain entities (Asset, Employee, Document, etc.)
- **Records**: Value objects and structures (Address, Contact, PhoneNumber)
- **Attributes**: Property specifications for types
- **Enums**: Enumerated values (e.g., ProductionOrderItemState)
- **Associations**: Relationships between types
- **Dependencies**: References to other CKs

### Key CK Model Files

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

### Build-Time Processing

During `dotnet build`, MSBuild tasks:

1. Scan `ConstructionKit/` folder for YAML definitions
2. Validate against JSON schemas
3. Generate C# code via source generators
4. Create compiled CK library output (`octo-ck-XXX.yaml`)
5. Optionally generate documentation

### Project Dependencies

Construction Kit dependencies are defined in the solution file:

- **Octo.Sdk.Packages.Industry.Basic** → depends on Octo.Sdk.Packages.Basic
- **Octo.Sdk.Packages.Industry.Energy** → depends on Octo.Sdk.Packages.Industry.Basic
- **Octo.Sdk.Packages.Industry.Fluid** → depends on Octo.Sdk.Packages.Industry.Basic
- **Octo.Sdk.Packages.Environment** → depends on Octo.Sdk.Packages.Basic
- **Octo.Sdk.Packages.EnergyCommunity** → depends on Octo.Sdk.Packages.Basic

---

## Continuous Integration / CD

### Azure Pipelines Configuration

**File**: `devops-build/azure-pipelines.yml`

**Triggers**:
- Branches: `dev/*`, `test/*`, `main`
- Tags: Excluded (tags starting with 'r' are releases)

**Stages**:

1. **Build Stage**: Executes on `meshmakers-ci-agents` pool
   - Updates build number
   - Sets version information
   - Builds solution: `dotnet build --configuration Release`
   - Runs tests: `dotnet test` (excluding SystemTests)
   - Processes artifacts
   - Pushes NuGet packages

**Build Artifacts**:

CK libraries are published from:
- `src/ConstructionKits/Octo.Sdk.Demo/bin/Release/net9.0/octo-ck-libraries/Octo.Sdk.Demo`
- `src/ConstructionKits/Octo.Sdk.Packages.Basic/bin/Release/net9.0/octo-ck-libraries/Octo.Sdk.Packages.Basic`
- (And similar for all other CKs)

**Version Naming**: `$(MajorVersion).$(MinorVersion).$(date:yyMM).$(DayOfMonth)$(rev:rrr)-$(Build.SourceBranchName)`

---

## Development Workflow

### Adding a New Construction Kit

1. Create new project folder: `src/ConstructionKits/Octo.Sdk.Packages.XXX/`

2. Create `.csproj` file with:
   ```xml
   <Project Sdk="Microsoft.NET.Sdk">
       <PropertyGroup>
           <AssemblyName>Meshmakers.Octo.Sdk.Packages.XXX</AssemblyName>
           <RootNamespace>Meshmakers.Octo.Sdk.Packages.XXX</RootNamespace>
           <GeneratePackageOnBuild>true</GeneratePackageOnBuild>
           <TargetFramework>net9.0</TargetFramework>
           <OctoPublishCkModel>true</OctoPublishCkModel>
           <OctoGenerateCkDocumentation>true</OctoGenerateCkDocumentation>
       </PropertyGroup>
       <ItemGroup>
           <PackageReference Include="Meshmakers.Octo.Runtime.Contracts" Version="$(OctoVersion)" />
           <PackageReference Include="Meshmakers.Octo.ConstructionKit.SourceGeneration" OutputItemType="Analyzer" ReferenceOutputAssembly="false" Version="$(OctoVersion)" />
           <PackageReference Include="Meshmakers.Octo.ConstructionKit.MsBuildTasks" Version="$(OctoVersion)" PrivateAssets="all" />
       </ItemGroup>
       <ItemGroup>
           <ConstructionKitFolder Visible="false" Include="$(MSBuildProjectDirectory)\ConstructionKit"/>
       </ItemGroup>
   </Project>
   ```

3. Create `ConstructionKit/` directory with subfolders:
   - `types/` - Core entities
   - `records/` - Value objects
   - `attributes/` - Property specs
   - `enums/` - Enumeration values
   - `associations/` - Relationships

4. Add `ckModel.yaml`:
   ```yaml
   $schema: https://schemas.meshmakers.cloud/construction-kit-meta.schema.json
   modelId: YourModelId
   dependencies:
     - DependentCK-[version,)
   ```

5. Define model elements in YAML files

6. Add project to `Octo.ConstructionKit.sln`

7. Update `devops-build/azure-pipelines.yml` with artifact paths

### Modifying Existing CKs

When modifying Construction Kits (like Manufacturoring):

1. Edit YAML files in `ConstructionKit/` subfolders
2. Build locally: `dotnet build Octo.ConstructionKit.sln --configuration Release`
3. Check for validation errors in build output
4. Run tests: `dotnet test`
5. Validate generated documentation (check build output)

**Current Branch (dev/reimar/add-manufactoring-ck)** - Modified files:
- `associations/associations.yaml` - Relationship definitions
- `attributes/order.yaml` - Order attribute specs
- `enums/productionOrderItemState.yaml` - Production order states
- `types/productionOrder.yaml` - Production order entity
- `types/productionOrderItem.yaml` - Production order item entity

---

## Code Style & Standards

### C# Code Style

From `Directory.Build.props`:

- **Language Version**: Latest major (C# 12+)
- **Nullable Reference Types**: Enabled (`<Nullable>enable</Nullable>`)
- **Implicit Usings**: Enabled (global imports)
- **Warnings as Errors**: Enabled
- **Target Framework**: .NET 9.0

### YAML Model Definitions

- Use JSON schema validation: `$schema` directive required
- Follow naming conventions from existing models
- Use relative references with `${this}/` for same-package types
- Document complex types with descriptions

### Project Organization

- Separate concerns: types, records, attributes, enums, associations
- Clear naming: `typeName.yaml`, `enumName.yaml`, etc.
- One definition per file (generally)
- Use consistent YAML formatting

---

## Testing

### Test Framework

- **Framework**: xUnit
- **Mocking**: FakeItEasy
- **Code Coverage**: coverlet.collector
- **Configuration**: `appsettings.test.json` per test project

### Running Tests

```bash
# All unit tests
dotnet test Octo.ConstructionKit.sln --configuration Release

# Specific test project
dotnet test tests/Octo.Sdk.Packages.Industry.Basic.old.SystemTests --configuration Release

# With coverage
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura
```

### Test Project Structure

System tests are in `tests/Octo.Sdk.Packages.Industry.Basic.old.SystemTests/`:
- Uses `appsettings.test.json` for test configuration
- References corresponding SDK package
- Tests actual SDK client integration

---

## IDE & Editor Setup

### Visual Studio
- Solution file: `Octo.ConstructionKit.sln`
- Settings: `Octo.ConstructionKit.sln.DotSettings`
- User overrides: `Octo.ConstructionKit.sln.DotSettings.user`

### Rider / IntelliJ IDEA
- Project structure: `.idea.Octo.ConstructionKit/`
- Directory structure: `.idea.octo-construction-kit.dir/`

### VS Code / Other Editors
- C# support: Use OmniSharp or Roslyn extension
- YAML validation: Use YAML extension with schema support
- .editorconfig: Standard rules inherited from root

### Git Configuration

- **.gitignore**: Standard Visual Studio ignores
- Notable exclusions:
  - Copilot XML files (`**/copilot.*.xml`)
  - Debug/Release/obj/bin directories
  - User-specific IDE files

---

## NuGet Package Output

### Generated Packages

Each Construction Kit generates a NuGet package on build:

- **Location**: `bin/{Configuration}/net9.0/*.nupkg`
- **Assembly Name**: `Meshmakers.Octo.Sdk.Packages.XXX.nupkg`
- **Root Namespace**: `Meshmakers.Octo.Sdk.Packages.XXX`
- **Metadata**:
  - Icon: `meshmakers64.png`
  - License: MIT License file included
  - Project URL: https://www.meshmakers.io
  - Repository: https://github.com/meshmakers/octo-construction-kit
  - Tags: "Octo data mesh iot"

### NuGet Server Configuration

Private NuGet server is optional:

- Set via: `/p:OctoNugetPrivateServer=<url>`
- Fallback sources: Default NuGet.org + local nuget folder (DebugL)
- Configured in: `Directory.Build.props`

---

## Documentation & References

### External Resources

- **Construction Kit Schema**: https://schemas.meshmakers.cloud/construction-kit-meta.schema.json
- **Elements Schema**: https://schemas.meshmakers.cloud/construction-kit-elements.schema.json
- **Company**: https://www.meshmakers.io
- **Repository**: https://github.com/meshmakers/octo-construction-kit

### Key Generated Artifacts

- Compiled CK Library: `bin/{Configuration}/net9.0/octo-ck-libraries/ProjectName/out/ck-{name}.yaml`
- Documentation: Generated alongside CK libraries (controlled by `OctoGenerateCkDocumentation`)
- Source-generated Code: Produces C# classes from YAML definitions

---

## Troubleshooting

### Common Issues

**Build fails with "ConstructionKit folder not found"**
- Ensure `ConstructionKit/` directory exists in project root
- Verify path in `.csproj`: `<ConstructionKitFolder Include="$(MSBuildProjectDirectory)\ConstructionKit"/>`

**Version conflicts in dependencies**
- Check dependency versions in `ckModel.yaml`
- Verify `OctoVersion` in `Directory.Build.props`
- Run: `dotnet clean && dotnet build --configuration Release`

**Tests not running**
- Exclude SystemTests from regular test runs: Use filter `FullyQualifiedName!~SystemTests`
- Verify test configuration in `appsettings.test.json`

**YAML validation errors**
- Validate schema URL in YAML files (correct `$schema` directive)
- Check for typos in field names
- Refer to schema examples from other working CK definitions

### Build Artifacts Not Generated

- Verify `GeneratePackageOnBuild=true` in `.csproj`
- Check `OctoPublishCkModel=true` is set
- Ensure MSBuild tasks package is referenced: `Meshmakers.Octo.ConstructionKit.MsBuildTasks`
- Review build output for source generation errors

---

## Quick Reference Commands

```bash
# Clone and setup
git clone https://github.com/meshmakers/octo-construction-kit.git
cd octo-construction-kit

# Restore dependencies
dotnet restore

# Build entire solution
dotnet build Octo.ConstructionKit.sln --configuration Release

# Run all tests (skip system tests)
dotnet test Octo.ConstructionKit.sln --configuration Release

# Clean build artifacts
dotnet clean

# View detailed build output
dotnet build Octo.ConstructionKit.sln --configuration Release --verbosity:detailed

# Pack NuGet packages manually
dotnet pack Octo.ConstructionKit.sln --configuration Release

# Check for build issues only (don't run)
dotnet build Octo.ConstructionKit.sln --configuration Release --no-restore
```

---

## Additional Notes

### Data Mesh Architecture

This project implements part of the **OCTO Data Mesh** framework, providing modular, domain-specific SDK packages through Construction Kits. Each CK represents a well-defined bounded context with its own data model and contracts.

### Extensibility

New Construction Kits can be added independently and composed with existing ones via dependency declarations in `ckModel.yaml`. This allows incremental system growth without modifying existing packages.

### Code Generation

The source generation pipeline automatically creates:
- C# entity classes from YAML type definitions
- Attribute metadata classes
- Enum implementations
- Association definitions
- Documentation

This keeps the C# code DRY and model-driven.

---

Last Updated: October 2024  
For questions or contributions, visit: https://github.com/meshmakers/octo-construction-kit
