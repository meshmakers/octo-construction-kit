using Newtonsoft.Json;

namespace Meshmakers.Octo.Sdk.Packages.Industry.Basic.DataTransferObjects;

public class RtEquipmentModelInputDto
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string? Designation { get; set; }
    
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string? Description { get; set; }
}