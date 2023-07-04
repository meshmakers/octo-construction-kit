using Meshmakers.Octo.Sdk.ServiceClient.AssetRepositoryServices.Tenants;

// ReSharper disable ClassNeverInstantiated.Global

namespace Meshmakers.Octo.Sdk.Packages.Industry.Basic.DataTransferObjects;

public class RtEquipmentDto : QlRtEntityDtoWithAssociations
{
    public string? Designation { get; set; }
    public string? Description { get; set; }
    
    [QlConnection("children", "meshmakersAlarmConnection")]
    public QlItemsContainer<RtAlarmDto>? AlarmChildren { get; set; } = null!;
    
    [QlConnection("children", "meshmakersEventConnection")]
    public QlItemsContainer<RtEventDto>? EventChildren { get; set; } = null!;

}