using Meshmakers.Octo.Sdk.ServiceClient.AssetRepositoryServices.Tenants;

// ReSharper disable ClassNeverInstantiated.Global

namespace Meshmakers.Octo.Common.Shared.DataTransferObjects;

public class RtEquipmentDto : QlRtEntityDtoWithAssociations
{
    [QlConnection("children", "meshmakersAssetsMeterConnection")]
    public QlItemsContainer<RtEventDto> EventChildren { get; set; } = null!;
}