using Meshmakers.Octo.Common.Shared;
using Meshmakers.Octo.Common.Shared.DataTransferObjects;
using Meshmakers.Octo.Sdk.Packages.Industry.Basic.DataTransferObjects;

namespace Meshmakers.Octo.Sdk.Packages.Industry.Basic.Repositories;

public interface IAssetRepository
{
    Task<PagedResult<RtEquipmentModelDto>> GetEquipmentModelAsync(string equipmentModelName);
    Task<RtEquipmentGroupDto?> GetEquipmentByGroupRtIdAsync(OctoObjectId equipmentGroupRtId);
    Task<PagedResult<RtAlarmDto>?> GetAlarmsByMachineRtIdAndStateAsync(OctoObjectId machineRtId, AlarmStates alarmState);
    Task<PagedResult<RtAlarmDto>> GetAlarmByRtIdQueryAsync(OctoObjectId alarmRtId);
}