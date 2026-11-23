---
description: Common build and validation failures with their fixes, plus a list of everyday dotnet commands.
background: true
---

# Troubleshooting and quick reference commands

## Troubleshooting

## Common Issues

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

## Build Artifacts Not Generated

- Verify `GeneratePackageOnBuild=true` in `.csproj`
- Check `OctoPublishCkModel=true` is set
- Ensure MSBuild tasks package is referenced: `Meshmakers.Octo.ConstructionKit.MsBuildTasks`
- Review build output for source generation errors
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
