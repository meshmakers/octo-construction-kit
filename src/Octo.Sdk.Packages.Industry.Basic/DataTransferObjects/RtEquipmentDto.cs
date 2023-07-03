using Meshmakers.Octo.Sdk.ServiceClient.AssetRepositoryServices.Tenants;

// ReSharper disable ClassNeverInstantiated.Global

namespace Meshmakers.Octo.Sdk.Packages.Industry.Basic.DataTransferObjects;

public class RtEquipmentDto : QlRtEntityDtoWithAssociations
{
    public string Manufacturer { get; set; } = null!;
    public string ModelNumber { get; set; } = null!;
    public string SerialNumber { get; set; } = null!;
    
    [QlConnection("children", "meshmakersAlarmConnection")]
    public QlItemsContainer<RtAlarmDto>? AlarmChildren { get; set; } = null!;
    
    [QlConnection("children", "meshmakersEventConnection")]
    public QlItemsContainer<RtEventDto>? EventChildren { get; set; } = null!;

}