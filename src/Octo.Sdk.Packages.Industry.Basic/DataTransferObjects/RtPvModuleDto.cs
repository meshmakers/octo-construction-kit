using Meshmakers.Octo.Common.Shared.DataTransferObjects;

namespace Meshmakers.Octo.Sdk.Packages.Industry.Basic.DataTransferObjects;

public class RtPvModuleDto : RtEntityDto
{
    public string? Designation { get; set; }
    public string? AmountOfPvModules { get; set; }
    public int? ProductionPower { get; set; }
}