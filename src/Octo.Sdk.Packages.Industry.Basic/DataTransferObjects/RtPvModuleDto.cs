namespace Meshmakers.Octo.Common.Shared.DataTransferObjects;

public class RtPvModuleDto : RtEntityDto
{
    public string? Designation { get; set; }
    public string? AmountOfPvModules { get; set; }
    public int? ProductionPower { get; set; }
}