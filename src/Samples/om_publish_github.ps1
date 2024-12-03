param (
    [string]$configuration = "Release"
)

if (!(Test-Path -Path $PSScriptRoot/../ConstructionKits/Octo.Sdk.Packages.Basic/bin/$configuration/net8.0/octo-ck-libraries/Octo.Sdk.Packages.Basic/out/ck-basic.yaml)) {
    Write-Host "Octo.Sdk.Packages.Basic Construction Kit not found at $PSScriptRoot/../ConstructionKits/Octo.Sdk.Packages.Basic/bin/$configuration/net8.0/octo-ck-libraries/Octo.Sdk.Packages.Basic/out/ck-basic.yaml"
    exit 1
}


octo-ckc -c Publish -rep "PublicGitHubRepository" -r -f ../ConstructionKits/Octo.Sdk.Packages.Basic/bin/$configuration/net8.0/octo-ck-libraries/Octo.Sdk.Packages.Basic/out/ck-basic.yaml
octo-ckc -c Publish -rep "PublicGitHubRepository" -r -f ../ConstructionKits/Octo.Sdk.Packages.EnergyCommunity/bin/$configuration/net8.0/octo-ck-libraries/Octo.Sdk.Packages.EnergyCommunity/out/ck-energycommunity.yaml
octo-ckc -c Publish -rep "PublicGitHubRepository" -r -f ../ConstructionKits/Octo.Sdk.Packages.Industry.Basic/bin/$configuration/net8.0/octo-ck-libraries/Octo.Sdk.Packages.Industry.Basic/out/ck-industry.basic.yaml 
octo-ckc -c Publish -rep "PublicGitHubRepository" -r -f ../ConstructionKits/Octo.Sdk.Packages.Industry.Energy/bin/$configuration/net8.0/octo-ck-libraries/Octo.Sdk.Packages.Industry.Energy/out/ck-industry.energy.yaml 
octo-ckc -c Publish -rep "PublicGitHubRepository" -r -f ../ConstructionKits/Octo.Sdk.Packages.Industry.Fluid/bin/$configuration/net8.0/octo-ck-libraries/Octo.Sdk.Packages.Industry.Fluid/out/ck-industry.fluid.yaml 
octo-ckc -c Publish -rep "PublicGitHubRepository" -r -f ../ConstructionKits/Octo.Sdk.Packages.Environment/bin/$configuration/net8.0/octo-ck-libraries/Octo.Sdk.Packages.Environment/out/ck-environment.yaml 
octo-ckc -c Publish -rep "PublicGitHubRepository" -r -f ../ConstructionKits/Octo.Sdk.Demo/bin/$configuration/net8.0/octo-ck-libraries/Octo.Sdk.Demo/out/ck-octosdkdemo.yaml 