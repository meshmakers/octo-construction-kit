# End-to-end setup for the time-range-archive energy simulation.
#
# Pre-requisites: ../ConstructionKits/* must have been built. Pass the same configuration the
# CKs were built with via -configuration (default Release; pass DebugL for local development).
#
# Steps:
#   1) Import the Basic + Basic.Energy CK models (idempotent — skipped if already up to date).
#   2) Enable communication + stream data on the tenant.
#   3) Import the meshtest Adapter (idempotent — needed for the pipeline's Executes association).
#   4) Import base RT entities: OperatingFacility + 5 MeteringPoints.
#   5) Import archive RT entities: 1 TimeRangeArchive + 3 RollupArchives (all Status=Created).
#   6) Activate each archive in source-to-rollup order — Activate provisions the per-archive
#      CrateDB table; chained rollups need their source Activated first.
#   7) Import the pipeline DataFlow + Pipeline. Trigger the simulation with:
#        octo-cli -c ExecutePipeline --identifier cc00000000000000000000e2

param (
    [string]$configuration = "Release"
)
$framework = "net10.0"

$basicCk = "$PSScriptRoot/../ConstructionKits/Octo.Sdk.Packages.Basic/bin/$configuration/$framework/octo-ck-libraries/Octo.Sdk.Packages.Basic/out/ck-basic-2.yaml"
$basicEnergyCk = "$PSScriptRoot/../ConstructionKits/Octo.Sdk.Packages.Basic.Energy/bin/$configuration/$framework/octo-ck-libraries/Octo.Sdk.Packages.Basic.Energy/out/ck-basic.energy.yaml"

if (!(Test-Path -Path $basicCk)) {
    Write-Host "Basic CK not found at $basicCk. Run 'dotnet build -c $configuration' first."
    exit 1
}
if (!(Test-Path -Path $basicEnergyCk)) {
    Write-Host "Basic.Energy CK not found at $basicEnergyCk. Run 'dotnet build -c $configuration' first."
    exit 1
}

octo-cli -c ImportCk -f $basicCk -w
octo-cli -c ImportCk -f $basicEnergyCk -w

octo-cli -c EnableCommunication
octo-cli -c EnableStreamData

octo-cli -c ImportRt -f ./_general/rt-adapters-mesh.yaml -w -r

octo-cli -c ImportRt -f ./simulator/rt-simulator-energy-base.yaml -w -r
octo-cli -c ImportRt -f ./simulator/rt-simulator-energy-archives.yaml -w -r

octo-cli -c ActivateArchive -id ab0000000000000000000015     # TimeRangeArchive (PT15M)
octo-cli -c ActivateArchive -id ab00000000000000000001a0     # Hourly Rollup
octo-cli -c ActivateArchive -id ab00000000000000000001b0     # Daily Rollup
octo-cli -c ActivateArchive -id ab00000000000000000001c0     # Monthly Rollup (CalendarMonth)

octo-cli -c ImportRt -f ./simulator/rt-dataflow-energy-sim.yaml -w -r
