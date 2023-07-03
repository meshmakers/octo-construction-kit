namespace Meshmakers.Octo.Common.Shared.DataTransferObjects;

public class RtAlarmDto : RtEventDto
{
    public AlarmStates? State { get; set; }
    public DateTime? ClearedDateTime { get; set; }
    public DateTime? AcknowledgedDateTime { get; set; }
}