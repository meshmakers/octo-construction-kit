---
description: Step-by-step workflow for a new Construction Kit project and for changing an existing one.
background: true
---

# Adding and modifying Construction Kits

## Adding a New Construction Kit

1. Create the project folder `src/ConstructionKits/Octo.Sdk.Packages.XXX/`.

2. Create the `.csproj` file (copy the one of an existing CK such as `Octo.Sdk.Packages.Industry.Energy` and adapt names):
   ```xml
   <Project Sdk="Microsoft.NET.Sdk">
       <PropertyGroup>
           <AssemblyName>Meshmakers.Octo.Sdk.Packages.XXX</AssemblyName>
           <RootNamespace>Meshmakers.Octo.Sdk.Packages.XXX</RootNamespace>
           <GeneratePackageOnBuild>true</GeneratePackageOnBuild>
           <Configurations>Debug;Release;DebugL</Configurations>
           <Platforms>AnyCPU</Platforms>
           <OctoPublishCkModel Condition="'$(TargetFramework)' == 'net10.0'">true</OctoPublishCkModel>
           <OctoGenerateCkDocumentation Condition="'$(TargetFramework)' == 'net10.0'">true</OctoGenerateCkDocumentation>
       </PropertyGroup>
       <ItemGroup>
           <PackageReference Include="Meshmakers.Octo.Runtime.Contracts" Version="$(OctoCoreLibVersion)" />
           <PackageReference Include="Meshmakers.Octo.ConstructionKit.SourceGeneration" OutputItemType="Analyzer" ReferenceOutputAssembly="false" Version="$(OctoCoreLibVersion)" />
           <PackageReference Include="Meshmakers.Octo.ConstructionKit.MsBuildTasks" Version="$(OctoCoreLibVersion)" PrivateAssets="all" />
       </ItemGroup>
       <ItemGroup>
           <ConstructionKitFolder Visible="false" Include="$(MSBuildProjectDirectory)\ConstructionKit"/>
       </ItemGroup>
   </Project>
   ```
   A `<ProjectReference>` to every sibling model project it depends on is added as well
   ([ck-dependencies.md](ck-dependencies.md)).

3. Create the `ConstructionKit/` directory with the subfolders `types/`, `records/`, `attributes/`, `enums/` and
   `associations/`.

4. Add `ckModel.yaml`:
   ```yaml
   $schema: https://schemas.meshmakers.cloud/construction-kit-meta.schema.json
   modelId: YourModelId-1.0.0
   ckLanguage: 2
   dependencies:
     - DependentCK-[version,)
   ```

5. Define the model elements in YAML files.

6. Add the project to `Octo.ConstructionKit.sln`.

7. Add the model to `constructionKitPaths` (dependency order) and `constructionKitLibraryPaths` in
   `azure-pipelines.yml` ([ci-and-release.md](ci-and-release.md)) and to the table in `README.md`.

## Modifying Existing CKs

1. Edit the YAML files in the `ConstructionKit/` subfolders.
2. Build locally: `dotnet build Octo.ConstructionKit.sln --configuration DebugL`.
3. Check the build output for validation errors.
4. Bump the model version honestly and add the `CHANGELOG.md` entry of the model ([ck-language-2.md](ck-language-2.md)).
5. Check the generated documentation in the build output.
