---
description: What each Construction Kit package contains, NuGet server configuration and the generated artifacts.
applies_to: Directory.Build.props, src/ConstructionKits/*/*.csproj
---

# NuGet package output

## Generated Packages

Each Construction Kit generates a NuGet package on build:

- **Location**: `bin/<configuration>/net10.0/*.nupkg`
- **Assembly Name**: `Meshmakers.Octo.Sdk.Packages.XXX`
- **Metadata** (`Directory.Build.props`): icon `meshmakers64.png`, the `LICENSE` file, project URL
  https://www.meshmakers.io, the repository URL and the tags "Octo data mesh iot"

## NuGet Server Configuration

- A private feed is optional: `/p:OctoNugetPrivateServer=<url>`.
- Restore sources: NuGet.org by default, the private feed when given, and the local `../nuget` folder first in `DebugL`.
- Configured in `Directory.Build.props`.

## Schemas and generated artifacts

- **Construction Kit schema**: https://schemas.meshmakers.cloud/construction-kit-meta.schema.json
- **Elements schema**: https://schemas.meshmakers.cloud/construction-kit-elements.schema.json
- Compiled CK library: `bin/<configuration>/net10.0/octo-ck-libraries/<project>/out/ck-<model>-<major>.yaml`
- Documentation is generated alongside the CK libraries (`OctoGenerateCkDocumentation`).
- The source generator produces the C# classes of the model from the YAML definitions.
