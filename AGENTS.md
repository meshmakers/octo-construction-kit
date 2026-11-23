# AGENTS.md

Instructions for coding agents in this repository. Detail lives in `docs/`; read the matching file before changing the code it covers.

## What this repo is

**Octo Construction Kit** is a .NET-based project that implements a modular data mesh SDK framework. The project provides multiple industry-specific "Construction Kits" (CKs) that define domain models using YAML-based schema definitions.

**Repository**: https://github.com/meshmakers/octo-construction-kit  
**License**: MIT (Copyright 2025 meshmakers.io)

## Read before you change

<!-- >>> generated: routing -->
| When you change | Read first |
|---|---|
| `azure-pipelines.yml` | `docs/ci-and-release.md` |
| `src/ConstructionKits/*/*.csproj`, `src/ConstructionKits/**/ConstructionKit/ckModel.yaml` | `docs/ck-dependencies.md` |
| `src/ConstructionKits/**/ConstructionKit/ckModel.yaml`, `src/ConstructionKits/**/ConstructionKit/types/**` | `docs/ck-language-2.md` |
| `src/ConstructionKits/**/ConstructionKit/**` | `docs/ck-models.md` |
| `src/ConstructionKits/**/ConstructionKit/attributes/*.yaml` | `docs/ck-ownership-markers.md` |
| `Directory.Build.props`, `src/ConstructionKits/*/*.csproj` | `docs/nuget-packages.md` |
| `Octo.ConstructionKit.sln`, `Directory.Build.props`, `src/ConstructionKits/*/*.csproj` | `docs/repository-structure.md` |
<!-- <<< end generated: routing -->

## Build & test

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

# Packages are output to: bin/Release/net10.0/*.nupkg
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
   - `999.0.0` (DebugL), `0.1.*` (NuGet private server); otherwise it comes only from the pipeline
     (AB#6297) — a Release build without `-p:OctoVersion=X.Y.Z` fails fast with `OCTO0001`
   
2. **Configuration-based override**:
   - DebugL: Forces `999.0.0`
   
3. **Azure Pipelines**: Dynamically sets version during CI/CD

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

## Before you commit

## Rules

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
