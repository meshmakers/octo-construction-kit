---
description: Step-by-step workflow for a new Construction Kit project and for changing an existing one.
background: true
---

# Adding and modifying Construction Kits

## Development Workflow

## Adding a New Construction Kit

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

## Modifying Existing CKs

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
