namespace Meshmakers.Octo.Sdk.Packages.Industry.Basic.DataTransferObjects;

public class RtAlarmDto : RtEventDto
{
    public AlarmStates? State { get; set; }
    public DateTime? ClearedDateTime { get; set; }
    public DateTime? AcknowledgedDateTime { get; set; }
    public DateTime? ReactivatedDateTime { get; set; }
    public string? AlarmCause { get; set; }
    public int? ReactivatedCount { get; set; }
}