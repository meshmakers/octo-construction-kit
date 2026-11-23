---
description: Common build and validation failures of the Construction Kit projects with their fixes.
background: true
---

# Troubleshooting

## Common Issues

**Build fails with "ConstructionKit folder not found"**
- Ensure the `ConstructionKit/` directory exists in the project root.
- Verify the path in the `.csproj`: `<ConstructionKitFolder Include="$(MSBuildProjectDirectory)\ConstructionKit"/>`.

**Build fails with `OCTO0001`**
- The version comes only from the pipeline. Build with `--configuration DebugL`, pass `-p:OctoVersion=X.Y.Z`, or pass a
  private feed with `/p:OctoNugetPrivateServer=<url>`.

**Version conflicts in dependencies**
- Check the dependency versions in `ckModel.yaml`.
- Run: `dotnet clean --configuration DebugL && dotnet build --configuration DebugL`.

**YAML validation errors**
- Validate the schema URL in the YAML files (correct `$schema` directive).
- Check for typos in field names.
- Refer to schema examples from other working CK definitions.

## Build Artifacts Not Generated

- Verify `GeneratePackageOnBuild=true` in the `.csproj`.
- Check that `OctoPublishCkModel=true` is set.
- Ensure the MSBuild tasks package is referenced: `Meshmakers.Octo.ConstructionKit.MsBuildTasks`.
- Review the build output for source generation errors.
