using Meshmakers.Octo.Sdk.ServiceClient.AssetRepositoryServices.Tenants;

namespace Meshmakers.Octo.Sdk.Packages.Industry.Basic.DataTransferObjects;

public class RtEventDto : QlRtEntityDtoWithAssociations
{
    public DateTime? ReceivedDateTime { get; set; }
    public string? Message { get; set; }
}