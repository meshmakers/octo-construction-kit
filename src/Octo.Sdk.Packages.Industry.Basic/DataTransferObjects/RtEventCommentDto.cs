using Meshmakers.Octo.Sdk.ServiceClient.AssetRepositoryServices.Tenants;

namespace Meshmakers.Octo.Sdk.Packages.Industry.Basic.DataTransferObjects;

public class RtEventCommentDto
{
    public DateTime? ReceivedDateTime { get; set; }
    public string? Message { get; set; }
    
    [QlConnection("children", "meshmakersAssetsMeterConnection")]
    public QlItemsContainer<RtEventCommentDto> CommentChildren { get; set; } = null!;
}