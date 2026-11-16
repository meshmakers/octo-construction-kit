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
└── bin/Release/net10.0/
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
6. Lint the attribute ownership markers (`CkLintRuntimeStateMarkers`, see below)

### Ownership markers are enforced by the build (AB#6327)

`Directory.Build.props` sets `OctoEnforceRuntimeStateMarkers=true` for every project of this repository; the
`CkLintRuntimeStateMarkers` task (CK MsBuildTasks package) runs in each project with a `ConstructionKitFolder` and fails the
build with `OCTO-CK001` when an attribute in `ConstructionKit/attributes/*.yaml` declares no `ownership` (or the deprecated
`isRuntimeState`). A new CK project in `src/ConstructionKits/` is covered automatically.

AB#6327 restated the existing behaviour: all 394 attributes of the 12 models with attributes got `ownership: SeedOwned`
(OctoEnergyDemo has none). Same resolved ownership, so every model keeps its version (the build's compatibility check and
`octo-ckc ValidateVersion` report "no structural changes"). No preserving marker was added: the seed ownership audit
(AB#6317) reviewed the sample-blueprint attributes of these models as blueprint-owned (R7).

**Review question for every new attribute:** could an operator or user type this value in the product? Yes → `ownership:
TenantOwned` (or `Secret` for a credential); a service, pipeline or adapter writes it → `RuntimeState`; the product ships and
corrects it → `SeedOwned`. Changing an existing attribute to a preserving marker is a behaviour change and needs a minor bump
of the model (and a rebuild of its dependents, see below).

### CK language 2: Basic and `derivable: Any` (AB#8868)

Basic (since 2.6.0) is the first package of this repository on `ckLanguage: 2` (Epic AB#5584, Feature F4.4). Every
other domain package followed (AB#8869: Industry.Basic 2.7.0, Industry.Energy 3.7.0, Industry.Fluid 2.6.0,
Industry.Logistics 3.5.0, Industry.Maintenance 2.6.0 (2.7.0: AB#9146, below), Industry.Manufacturing 2.7.0, Environment 3.7.0; AB#8870:
Basic.Energy 2.1.0, Basic.Accounting 1.14.0, EnergyCommunity 5.1.0), each with `derivable: Any` on every type and record
and with all dependency ranges and floors unchanged (gate: MINOR per package, the only bump-relevant row is the language change).
The demo packages (`OctoSdkDemo`, `OctoEnergyDemo`) stay on ckLanguage 1. Every compiled ckLanguage 2 model carries
`minEngineVersion` 3.5.1 and is published under `ck-models/v3/` only.

**Rule for every model that moves to ckLanguage 2:** in ckLanguage 2 an omitted `derivable` means `Model` (only the
declaring model may derive), in ckLanguage 1 it means `Any`. Raising the language without `derivable: Any` on every
type and record is therefore a **Major** per element (semver row T6), and every subtype in another model fails with
message 113 (`CkElementNotDerivable`). Basic declares `derivable: Any` on all its types and records. The four hot bases
(`Basic/NamedEntity`, `Basic/TreeNode`, `Basic/Asset`, and `Industry.Basic/Machine`, ckLanguage 2 since Industry.Basic 2.7.0) stay
`derivable: Any` on purpose: each has subtypes in other packages (33 cross-package subtypes in 11 packages of this
repository, counted 2026-11-16, including `Basic/Document` for EnergyCommunity, plus models in other repositories), so
`Model` would break them. The same reason made the System model declare `derivable: Any` everywhere.

**Interfaces came in a second release (Basic 2.7.0).** Letting `NamedEntity` implement `System/Named-1` and `System/Describable-1` and
`TreeNode` implement `System/Hierarchical-1` needs the System floor `[2.7,3.0)` (the interfaces exist from System 2.7.0;
the compiler refuses an `implements` that the range floor cannot resolve). Raising the floor beyond the System version
the previously published Basic resolved is a Major (row D1): Basic 2.5.0 resolved System 2.5.0. Basic 2.6.0 resolves
System 2.8.0, so the floor raise was a Minor in the release after it, Basic 2.7.0 (AB#9097).

**Basic 2.7.0 implements System interfaces (AB#9097).** `NamedEntity` implements `System/Named-1` and
`System/Describable-1`, `TreeNode` implements `System/Hierarchical-1`; System floor `[2.7,3.0)`. Rules for every
package that implements an interface of another CK model:

- The project needs a `<PackageReference>` to the C# package of the model that declares the interface
  (for System: `Meshmakers.Octo.ConstructionKit.Models.System`, `Version="$(OctoCoreLibVersion)"`). Without it the
  source generator reports `OM1006` ("no referenced assembly declares the generated interface"); with two
  assemblies that declare it, `OM1006` asks to keep only one reference. It needs the engine generator fix of AB#9096.
- The generated class lists foreign interfaces fully qualified
  (`global::Meshmakers.Octo.ConstructionKit.Models.System.Generated.System.v2.IRtNamed`) and implements their
  members explicitly; interfaces of the own model keep the short name `IRt<Name>`.
- Dependents need no change: they inherit the interfaces through their base types and get the System package
  transitively through Basic's package (Industry.Maintenance 2.7.0 additionally implements one itself, see below).
  Their Basic floor stays 2.4 (Industry.Energy, Fluid, Maintenance and Manufacturing depend on Industry.Basic with
  floor 2.5). The releases published before this migration all resolved
  Basic 2.5.0, so raising their floor to 2.7 in this migration would have been a Major (row D1); the ckLanguage 2
  releases of AB#8869/AB#8870 resolve Basic 2.7.0, so after they are published a Basic floor of 2.7 no longer exceeds
  the newest published resolution and is a Minor. Because the floors stay unchanged in this migration, the inherited
  interfaces appear on a tenant only once it runs Basic 2.7.0 or later.
- **Floors of the dependents (AB#8869, AB#8870).** Before a package raises its System or Basic floor (for example to
  implement an interface itself), check which versions its newest *published* release resolved
  (`dependencies` in the catalog JSON): a floor above that is a Major (row D1). The way out is two releases: first the
  language change with the floors unchanged, then the floor raise with the `implements` (Minor). Precondition: the
  first release resolves only the dependency versions its build can see (the catalog, or a sibling built in the same
  run), so the dependency release (e.g. Basic 2.7.0) must be published, or built in the same run, before the first
  release; otherwise it resolves the older version and the later floor raise is a Major again.
  `Industry.Maintenance/EnergyBalance` -> `System/Describable-1` followed this pattern: Industry.Maintenance 2.6.0
  (language change, resolved System 2.8.0), then 2.7.0 (AB#9146: System floor `[2.7,3.0)` plus `implements`,
  Minor). A dependent that implements a System interface gets the System C# package transitively through Basic's
  package and needs no own `<PackageReference>` (Industry.Maintenance has none; its build reports no `OM1006`).
  EnergyCommunity's `CommunitySettings`, `SimulationSettings` (`System/Description` named `Comment`) and
  `SimulationResetRequest` (named `Message`) cannot implement `Describable-1` without a breaking rename (message 98)
  and stay without it.

Gate command (local; `-lcr` points at a new, empty directory, so no stale local catalog entry takes part in the
resolution and nothing is written into the shared local catalog):

```bash
octo-ckc -c ValidateVersion -p src/ConstructionKits/Octo.Sdk.Packages.Basic/ConstructionKit \
  -cn PrivateGitHubCatalog -rf -lcr "$(mktemp -d)"
```

### Project Dependencies

CK model dependencies are declared in each `ckModel.yaml` **and** mirrored as a `<ProjectReference>` to the
sibling model project in the `.csproj`. The ProjectReference is mandatory: without it a parallel build can
compile a model before its sibling dependency has been published to the local catalog
(`../.octo/local-catalog`), so the dependency resolves against a stale catalog entry (AB#5661 race class).
A transitive reference is sufficient (e.g. Industry.Energy → Industry.Basic → Basic).

- **Octo.Sdk.Packages.Basic.Accounting** → depends on Octo.Sdk.Packages.Basic
- **Octo.Sdk.Packages.Basic.Energy** → depends on Octo.Sdk.Packages.Basic
- **Octo.Sdk.Packages.Industry.Basic** → depends on Octo.Sdk.Packages.Basic
- **Octo.Sdk.Packages.Industry.Energy** → depends on Octo.Sdk.Packages.Industry.Basic
- **Octo.Sdk.Packages.Industry.Fluid** → depends on Octo.Sdk.Packages.Industry.Basic
- **Octo.Sdk.Packages.Industry.Logistics** → depends on Octo.Sdk.Packages.Basic
- **Octo.Sdk.Packages.Environment** → depends on Octo.Sdk.Packages.Basic
- **Octo.Sdk.Packages.EnergyCommunity** → depends on Octo.Sdk.Packages.Basic and Octo.Sdk.Packages.Basic.Energy (since EnergyCommunity 4.0.0: community/billing types subtype the Basic.Energy metering-point types; metering data is stream data, no EnergyQuantity entity anymore)
- **Octo.Sdk.Packages.Monitoring** → depends only on the System model (no other CK package). Config entities for operational watchdog pipelines; consumed by the `src/Blueprints/Monitoring.CkHealth` blueprint (CK model health monitoring with Teams alerting).

---

## Continuous Integration / CD

### Azure Pipelines Configuration

**File**: `azure-pipelines.yml` (repo root; templates from octo-pipeline-templates `tpl-v1.3.1`)

**Triggers**:
- Branches: `dev/*`, `test/*`, `main`
- Tags: Excluded (tags starting with 'r' are releases)

**Stages**:

1. **Build Stage**: Executes on `meshmakers-ci-agents` pool
   - Updates build number
   - Sets version information
   - CK v2 publish guard (AB#6277, operating rule O1, `ck-o1-guard.yml`): before the build
     on main / r-tags (source mode; the publish itself follows after the tests) it refuses `ckLanguage: 2` in any
     `ckModel.yaml` and range retention (`OctoCkRangeRetention=true`); before the blueprint
     publish step it refuses a blueprint whose dependency floor is a CK v2 model. Lifted only
     by the variable `OctoCkO1Lifted` (group `OctoDefault`) through AB#6279. Exception: publishes to the dev catalog `octo-catalog-dev`
     (test/* lane) may carry CK v2 models (tpl-v1.2.1)
   - Builds solution: `dotnet build --configuration Release` with `/p:OctoPublishCatalog=LocalFileSystemCatalog`
     (the CKs depend on each other and resolve through the run-isolated local catalog; no build publishes
     to a shared catalog since `tpl-v1.0.0`)
   - Runs tests: `dotnet test` (excluding SystemTests)
   - `validate-and-publish-ck-versions` (12 CKs, dependency order): SemVer gate, compile, publish to the
     catalogs of the channel (main → private, r* tags → private and public; never replaces a version)
     The 12 published models are listed identically in `constructionKitPaths` (this step, dependency order) and in
     `constructionKitLibraryPaths` (handle-artifacts): Basic, Basic.Accounting, Basic.Energy, EnergyCommunity,
     Environment, Industry.{Basic,Logistics,Energy,Fluid,Maintenance,Manufacturing}, OctoSdkDemo. Basic.Accounting was
     missing from both lists until AB#6721 (1.10.0 was never published). `Monitoring` is deliberately not published
     (dropped in r3.4.137); `Octo.Energy.Demo` is a sample. A new publishable CK must be added to both lists.
   - `validate-and-publish-blueprints`: blueprint version gate and publish to the catalogs of the channel
     (an already published version is a no-op; the lane blueprint step below stays for `test/*`)
   - Processes artifacts
   - Pushes NuGet packages

**Build Artifacts**:

CK libraries are published from:
- `src/ConstructionKits/Octo.Sdk.Demo/bin/Release/net10.0/octo-ck-libraries/Octo.Sdk.Demo`
- `src/ConstructionKits/Octo.Sdk.Packages.Basic/bin/Release/net10.0/octo-ck-libraries/Octo.Sdk.Packages.Basic`
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
           <TargetFramework>net10.0</TargetFramework>
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

- **Location**: `bin/{Configuration}/net10.0/*.nupkg`
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

- Compiled CK Library: `bin/{Configuration}/net10.0/octo-ck-libraries/ProjectName/out/ck-{name}.yaml`
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
