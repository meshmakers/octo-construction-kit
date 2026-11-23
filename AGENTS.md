# AGENTS.md

Instructions for coding agents in this repository. Detail lives in `docs/`; read the matching file before changing the
code it covers.

## What this repo is

The Octo Construction Kit repository holds the public Construction Kit (CK) libraries of the OctoMesh platform: domain
models written as YAML (types, records, attributes, enums, associations), one project per model under
`src/ConstructionKits`. Each project builds a NuGet package, and the pipeline validates and publishes the compiled model
to the CK catalogs. Installable blueprints that seed tenants with these models live in `src/Blueprints`. The repository
is MIT licensed.

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

Background, no path trigger: [docs/adding-and-modifying-cks.md](docs/adding-and-modifying-cks.md) (new CK project,
changing an existing one) and [docs/troubleshooting.md](docs/troubleshooting.md) (common build and validation failures).

## Build & test

```bash
# Local build against the DebugL package chain (version 999.0.0, packages from ../nuget)
dotnet build Octo.ConstructionKit.sln --configuration DebugL

# Build against a private feed (version range 0.1.*)
dotnet build Octo.ConstructionKit.sln --configuration Release /p:OctoNugetPrivateServer=<feed-url>

# Build against a published release (version must be passed)
dotnet build Octo.ConstructionKit.sln --configuration Release -p:OctoVersion=X.Y.Z
```

- The version comes only from the pipeline: a Debug or Release build with neither `-p:OctoVersion` nor a private feed
  fails fast with MSBuild error `OCTO0001`. `GeneratePackageOnBuild` is on, so a build also packs the NuGet packages.
- There are no test projects in this repository. The build is the check: schema validation, source generation and the
  ownership marker lint (`OCTO-CK001`) run in it. Check a version bump with the gate command in
  [docs/ck-language-2.md](docs/ck-language-2.md).

## Before you commit

- The solution builds in `DebugL` without errors.
- A change to a CK model (types, records, attributes, enums, associations) bumps the version in that model's
  `ConstructionKit/ckModel.yaml` (`modelId: <Name>-<version>`) honestly, adds the entry to the
  `CHANGELOG.md` next to it and updates the version in the table of `README.md`; the gate is
  `octo-ckc ValidateVersion` ([docs/ck-language-2.md](docs/ck-language-2.md)). Models that depend on it are rebuilt
  ([docs/ck-dependencies.md](docs/ck-dependencies.md)).
- A new publishable model is added to both path lists of `azure-pipelines.yml`
  ([docs/ci-and-release.md](docs/ci-and-release.md)).
- Run `Test-OctoAgentDocs` (octo-tools) after changing `AGENTS.md` or `docs/`; `Test-OctoAgentDocs -Fix` regenerates
  the routing table. The shim file for Claude Code is generated: edit this file, not the shim.
- This repository is public: no internal hostnames, customer or tenant names, or secrets in `AGENTS.md`, `docs/`, CK
  YAML or blueprints.

## Rules

These rules bind new and changed models.

- **Every new attribute declares its ownership.** An attribute in a model's `attributes/*.yaml` without `ownership`
  fails the build (`OCTO-CK001`). Ask: could an operator or user type this value in the product? Yes: `TenantOwned`
  (`Secret` for a credential). A service, pipeline or adapter writes it: `RuntimeState`. The product ships and corrects
  it: `SeedOwned`. Changing an existing attribute to a preserving marker is a behaviour change and needs a minor bump
  ([docs/ck-ownership-markers.md](docs/ck-ownership-markers.md)).
- **Versions are honest.** The model version in `ckModel.yaml` follows the semver rules that `octo-ckc ValidateVersion`
  checks; raising a dependency floor above what the newest published release resolved is a Major
  ([docs/ck-language-2.md](docs/ck-language-2.md)).
- **CK language 2 models declare `derivable`.** A new type or record in a `ckLanguage: 2` model states `derivable`
  explicitly, `Any` where subtypes in other models are expected ([docs/ck-language-2.md](docs/ck-language-2.md)).
- **Dependencies are declared twice.** A model dependency in `ckModel.yaml` is mirrored as a `<ProjectReference>` to the
  sibling project ([docs/ck-dependencies.md](docs/ck-dependencies.md)).
- **A model that implements an interface of another model references its C# package.** Add the `<PackageReference>` of
  the declaring model unless it comes in transitively ([docs/ck-language-2.md](docs/ck-language-2.md)).
- **YAML files carry the `$schema` directive** and use `${this}/` for types of the same model
  ([docs/ck-models.md](docs/ck-models.md)).
