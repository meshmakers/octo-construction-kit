using Meshmakers.Octo.Sdk.ServiceClient.AssetRepositoryServices.Tenants;

namespace Meshmakers.Octo.Sdk.Packages.Industry.Basic.DataTransferObjects;

public class RtEquipmentGroupDto: QlRtEntityDtoWithAssociations
{
    public string? Designation { get; set; }
    public string? Description { get; set; }
    
    [QlConnection("children", "meshmakersEquipmentMachineConnection")]
    public QlItemsContainer<RtEquipmentMachine>? MachinesChildren { get; set; }
    
    
    [QlConnection("children", "meshmakersEquipmentGroupConnection")]
    public QlItemsContainer<RtEquipmentGroupDto>? GroupChildren { get; set; } = null!;
}

