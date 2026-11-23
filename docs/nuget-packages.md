---
description: What each Construction Kit package contains, NuGet server configuration, external schema links and generated artifacts.
applies_to: Directory.Build.props, src/ConstructionKits/*/*.csproj
---

# NuGet package output and references

## NuGet Package Output

## Generated Packages

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

## NuGet Server Configuration

Private NuGet server is optional:

- Set via: `/p:OctoNugetPrivateServer=<url>`
- Fallback sources: Default NuGet.org + local nuget folder (DebugL)
- Configured in: `Directory.Build.props`

---

## Documentation & References

## External Resources

- **Construction Kit Schema**: https://schemas.meshmakers.cloud/construction-kit-meta.schema.json
- **Elements Schema**: https://schemas.meshmakers.cloud/construction-kit-elements.schema.json
- **Company**: https://www.meshmakers.io
- **Repository**: https://github.com/meshmakers/octo-construction-kit

## Key Generated Artifacts

- Compiled CK Library: `bin/{Configuration}/net10.0/octo-ck-libraries/ProjectName/out/ck-{name}.yaml`
- Documentation: Generated alongside CK libraries (controlled by `OctoGenerateCkDocumentation`)
- Source-generated Code: Produces C# classes from YAML definitions
