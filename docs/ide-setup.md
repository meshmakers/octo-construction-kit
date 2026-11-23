---
description: Solution files, IDE settings and git configuration notes for Visual Studio, Rider and VS Code.
background: true
---

# IDE and editor setup

## IDE & Editor Setup

## Visual Studio
- Solution file: `Octo.ConstructionKit.sln`
- Settings: `Octo.ConstructionKit.sln.DotSettings`
- User overrides: `Octo.ConstructionKit.sln.DotSettings.user`

## Rider / IntelliJ IDEA
- Project structure: `.idea.Octo.ConstructionKit/`
- Directory structure: `.idea.octo-construction-kit.dir/`

## VS Code / Other Editors
- C# support: Use OmniSharp or Roslyn extension
- YAML validation: Use YAML extension with schema support
- .editorconfig: Standard rules inherited from root

## Git Configuration

- **.gitignore**: Standard Visual Studio ignores
- Notable exclusions:
  - Copilot XML files (`**/copilot.*.xml`)
  - Debug/Release/obj/bin directories
  - User-specific IDE files
