using Meshmakers.Octo.Sdk.ServiceClient.AssetRepositoryServices.Tenants;

// ReSharper disable ClassNeverInstantiated.Global

namespace Octo.Sdk.Packages.Industry.Basic.DataTransferObjects;

public class RtEquipmentDto : QlRtEntityDtoWithAssociations
{
    [QlConnection("children", "meshmakersAssetsMeterConnection")]
    public QlItemsContainer<RtEventDto> EventChildren { get; set; } = null!;
}