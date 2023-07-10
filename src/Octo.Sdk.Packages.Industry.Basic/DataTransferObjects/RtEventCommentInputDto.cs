using Meshmakers.Octo.Common.Shared.DataTransferObjects;
using Newtonsoft.Json;

namespace Meshmakers.Octo.Sdk.Packages.Industry.Basic.DataTransferObjects;

public class RtEventCommentInputDto
{
    public DateTime? ReceivedDateTime { get; set; }
    public string? Comment { get; set; }
    
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public RtAssociationInputDto[]? Parent { get; set; }
}