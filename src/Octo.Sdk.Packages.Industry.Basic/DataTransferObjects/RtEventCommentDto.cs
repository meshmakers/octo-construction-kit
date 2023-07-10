using Meshmakers.Octo.Common.Shared.DataTransferObjects;

namespace Meshmakers.Octo.Sdk.Packages.Industry.Basic.DataTransferObjects;

public class RtEventCommentDto : RtEntityDto
{
    public DateTime? ReceivedDateTime { get; set; }
    public string? Comment { get; set; }
}