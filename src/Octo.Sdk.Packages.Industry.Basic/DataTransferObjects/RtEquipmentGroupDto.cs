using Meshmakers.Octo.Sdk.ServiceClient.AssetRepositoryServices.Tenants;

namespace Meshmakers.Octo.Sdk.Packages.Industry.Basic.DataTransferObjects;

public class RtEquipmentGroupDto: QlRtEntityDtoWithAssociations
{
    public string? Designation { get; set; }
    public string? Description { get; set; }
    
    [QlConnection("children", "meshmakersEquipmentMillingMachineConnection")]
    public QlItemsContainer<RtEquipmentMillingMachineDto> MillingMachineChildren { get; set; } = null!;
    
    [QlConnection("children", "meshmakersEquipmentInjectionMouldingConnection")]
    public QlItemsContainer<RtInjectionMouldingMachineDto> InjectionMouldingMachineChildren { get; set; } = null!;
    
    [QlConnection("children", "meshmakersEquipmentGroupConnection")]
    public QlItemsContainer<RtEquipmentGroupDto> GroupChildren { get; set; } = null!;
}

