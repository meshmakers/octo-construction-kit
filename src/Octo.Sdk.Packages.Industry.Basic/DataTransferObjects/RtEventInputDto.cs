using Meshmakers.Octo.Common.Shared;
using Newtonsoft.Json;

namespace Meshmakers.Octo.Sdk.Packages.Industry.Basic.DataTransferObjects;

public class RtEventInputDto 
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string? RtWellKnownName { get; set; }
    
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public DateTime? ReceivedDateTime { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string? Message { get; set; }
    
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    [JsonConverter(typeof(NewtonEnumValueConverter))]
    public EventGroups? Group { get; set; }
    
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    [JsonConverter(typeof(NewtonEnumValueConverter))]
    public EventClassification? Classification { get; set; }
}