using Meshmakers.Octo.Common.Shared;
using Newtonsoft.Json;

namespace Meshmakers.Octo.Sdk.Packages.Industry.Basic.DataTransferObjects;

public class RtEquipmentMachineInputDto : RtEquipmentInputDto
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string? Manufacturer { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string? ModelNumber { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string? SerialNumber { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public int? OperatingHoursCounter { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public int? StandStillCounter { get; set; }
    
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    [JsonConverter(typeof(NewtonEnumValueConverter))]
    public MachineState? State { get; set; }
}