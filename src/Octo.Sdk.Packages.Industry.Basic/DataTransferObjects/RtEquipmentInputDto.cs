using Meshmakers.Octo.Common.Shared.DataTransferObjects;
using Newtonsoft.Json;

namespace Meshmakers.Octo.Sdk.Packages.Industry.Basic.DataTransferObjects;

public class RtEquipmentInputDto
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string? Designation { get; set; }
    
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string? Description { get; set; }
    
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public RtAssociationInputDto[]? Parent { get; set; }
    
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public RtAssociationInputDto[]? Children { get; set; }

}