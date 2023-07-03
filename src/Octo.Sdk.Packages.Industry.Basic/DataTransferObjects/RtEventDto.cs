using Meshmakers.Octo.Common.Shared.DataTransferObjects;

namespace Meshmakers.Octo.Sdk.Packages.Industry.Basic.DataTransferObjects;

public class RtEventDto : RtEntityDto
{
    public DateTime? ReceivedDateTime { get; set; }
    public string? Message { get; set; }
}