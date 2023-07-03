namespace Meshmakers.Octo.Sdk.Packages.Industry.Basic.DataTransferObjects;

public class RtEquipmentMachine : RtEquipmentDto
{
    public string Manufacturer { get; set; } = null!;
    public string ModelNumber { get; set; } = null!;
    public string SerialNumber { get; set; } = null!;
}