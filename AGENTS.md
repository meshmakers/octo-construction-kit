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
| `src/ConstructionKits/**/ConstructionKit/ckModel.yaml`, `src/ConstructionKits/**/ConstructionKit/{types,records}/**` | `docs/ck-language-2.md` |
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
- A change to a CK model (types, records, attributes, enums, associations) bumps the version as the **Versions** rule
  below says, adds the entry to the `CHANGELOG.md` next to it and updates the version in the table of `README.md` (the
  two demo models have no changelog, and `Octo.Energy.Demo` carries no version). Models that depend on it are rebuilt
  ([docs/ck-dependencies.md](docs/ck-dependencies.md)).
- A new publishable model, and a new blueprint, is added to the path lists of `azure-pipelines.yml`
  ([docs/ci-and-release.md](docs/ci-and-release.md)).
- Run `Test-OctoAgentDocs` (octo-tools) after changing `AGENTS.md` or `docs/`; `Test-OctoAgentDocs -Fix` regenerates
  the routing table. The shim file for Claude Code is generated: edit this file, not the shim.

## Rules

These rules bind new and changed models.

- **This repository is public.** No internal hostnames, customer or tenant names, or secrets in the documentation, CK
  YAML, blueprints and in new or changed code comments and test names; no work item ids there either (work item ids
  stay in `CHANGELOG.md` entries). Remove an existing one when you touch its line.
- **A new or changed attribute declares its `ownership`; the build fails with `OCTO-CK001`.** The lint also accepts
  the deprecated `isRuntimeState` and skips `Octo.Energy.Demo`, so declare `ownership` yourself in both cases. Ask:
  could an operator or user type this value in the product? Yes: `TenantOwned` (`Secret` for a credential). A service,
  pipeline or adapter writes it: `RuntimeState`. The product ships and corrects it: `SeedOwned`. Changing an existing
  attribute to a preserving marker is a behaviour change and needs a minor bump
  ([docs/ck-ownership-markers.md](docs/ck-ownership-markers.md)).
- **Versions are honest.** A change to a CK model bumps the version in its `ConstructionKit/ckModel.yaml`
  (`modelId: <Name>-<version>`; `Octo.Energy.Demo` carries no version) by the semver rules that
  `octo-ckc ValidateVersion` checks; raising a dependency floor above the newest published resolution is a Major. A
  merge to `main` publishes to the private catalog and a version is never replaced, so a published version is never
  edited ([docs/ck-language-2.md](docs/ck-language-2.md), [docs/ci-and-release.md](docs/ci-and-release.md)).
- **CK language 2 models declare `derivable`.** A new type or record in a `ckLanguage: 2` model states `derivable`
  explicitly, `Any` where subtypes in other models are expected ([docs/ck-language-2.md](docs/ck-language-2.md)).
- **Dependencies are declared twice.** A new dependency on a model of this repository in `ckModel.yaml` is mirrored as a
  `<ProjectReference>` to that project, directly or transitively ([docs/ck-dependencies.md](docs/ck-dependencies.md)).
