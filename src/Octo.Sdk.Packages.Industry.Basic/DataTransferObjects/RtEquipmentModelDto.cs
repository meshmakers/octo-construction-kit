using Meshmakers.Octo.Common.Shared.DataTransferObjects;
using Meshmakers.Octo.Sdk.ServiceClient.AssetRepositoryServices.Tenants;

namespace Octo.Sdk.Packages.Industry.Basic.DataTransferObjects;

public class RtEquipmentModelDto: QlRtEntityDtoWithAssociations
{
    public string? Designation { get; set; }
    public string? Description { get; set; }
    
    [QlConnection("children", "meshmakersAssetsMeterConnection")]
    public QlItemsContainer<RtEquipmentGroupDto> GroupChildren { get; set; } = null!;
}