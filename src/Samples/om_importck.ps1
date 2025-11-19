param (
    [string]$configuration = "Release"
)
$framework = "net10.0"
if (!(Test-Path -Path $PSScriptRoot/../ConstructionKits/Octo.Sdk.Packages.Basic/bin/$configuration/$framework/octo-ck-libraries/Octo.Sdk.Packages.Basic/out/ck-basic.yaml)) {
    Write-Host "Octo.Sdk.Packages.Basic Construction Kit not found at $PSScriptRoot/../ConstructionKits/Octo.Sdk.Packages.Basic/bin/$configuration/$framework/octo-ck-libraries/Octo.Sdk.Packages.Basic/out/ck-basic.yaml"
    exit 1
}


octo-cli -c importck -f ../ConstructionKits/Octo.Sdk.Packages.Basic/bin/$configuration/$framework/octo-ck-libraries/Octo.Sdk.Packages.Basic/out/ck-basic.yaml -w
octo-cli -c importck -f ../ConstructionKits/Octo.Sdk.Packages.EnergyCommunity/bin/$configuration/$framework/octo-ck-libraries/Octo.Sdk.Packages.EnergyCommunity/out/ck-energycommunity.yaml -w
octo-cli -c importck -f ../ConstructionKits/Octo.Sdk.Packages.Industry.Basic/bin/$configuration/$framework/octo-ck-libraries/Octo.Sdk.Packages.Industry.Basic/out/ck-industry.basic.yaml -w
octo-cli -c importck -f ../ConstructionKits/Octo.Sdk.Packages.Industry.Energy/bin/$configuration/$framework/octo-ck-libraries/Octo.Sdk.Packages.Industry.Energy/out/ck-industry.energy.yaml -w
octo-cli -c importck -f ../ConstructionKits/Octo.Sdk.Packages.Industry.Fluid/bin/$configuration/$framework/octo-ck-libraries/Octo.Sdk.Packages.Industry.Fluid/out/ck-industry.fluid.yaml -w
octo-cli -c importck -f ../ConstructionKits/Octo.Sdk.Packages.Industry.Maintenance/bin/$configuration/$framework/octo-ck-libraries/Octo.Sdk.Packages.Industry.Maintenance/out/ck-industry.maintenance.yaml -w
octo-cli -c importck -f ../ConstructionKits/Octo.Sdk.Packages.Environment/bin/$configuration/$framework/octo-ck-libraries/Octo.Sdk.Packages.Environment/out/ck-environment.yaml -w
octo-cli -c importck -f ../ConstructionKits/Octo.Sdk.Demo/bin/$configuration/$framework/octo-ck-libraries/Octo.Sdk.Demo/out/ck-octosdkdemo.yaml -w