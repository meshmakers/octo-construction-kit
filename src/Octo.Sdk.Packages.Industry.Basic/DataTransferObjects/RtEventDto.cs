using Meshmakers.Octo.Common.Shared.DataTransferObjects;

namespace Meshmakers.Octo.Sdk.Packages.Industry.Basic.DataTransferObjects;

public class RtEventDto : RtEntityDto
{
    public DateTime? ReceivedDateTime { get; set; }
    public string? Message { get; set; }
    
    public EventGroups? Group { get; set; }
    public EventClassification? Classification { get; set; }
}