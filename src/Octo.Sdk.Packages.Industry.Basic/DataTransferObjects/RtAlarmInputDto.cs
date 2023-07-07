using Meshmakers.Octo.Common.Shared;
using Meshmakers.Octo.Common.Shared.DataTransferObjects;
using Newtonsoft.Json;

namespace Meshmakers.Octo.Sdk.Packages.Industry.Basic.DataTransferObjects;

public class RtAlarmInputDto : RtEventInputDto
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    [JsonConverter(typeof(NewtonEnumValueConverter))]
    public AlarmStates? State { get; set; }
    
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public DateTime? ClearedDateTime { get; set; }
    
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public DateTime? AcknowledgedDateTime { get; set; }
    
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public RtAssociationInputDto[]? Parent { get; set; }
    
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    [JsonConverter(typeof(NewtonEnumValueConverter))]
    public AlarmGroups? Group { get; set; }
    
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    [JsonConverter(typeof(NewtonEnumValueConverter))]
    public AlarmClassification? Classification { get; set; }
}