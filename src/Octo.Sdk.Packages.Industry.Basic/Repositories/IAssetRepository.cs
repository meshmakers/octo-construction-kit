using Meshmakers.Octo.Common.Shared;
using Meshmakers.Octo.Common.Shared.DataTransferObjects;
using Meshmakers.Octo.Sdk.Packages.Industry.Basic.DataTransferObjects;

namespace Meshmakers.Octo.Sdk.Packages.Industry.Basic.Repositories;

public interface IAssetRepository
{
    Task<PagedResult<RtEquipmentModelDto>> GetEquipmentModelAsync(string equipmentModelName);
    Task<RtEquipmentGroupDto?> GetGroupsByGroupRtIdAsync(OctoObjectId equipmentGroupRtId);
    Task<RtEquipmentGroupDto?> GetEquipmentByGroupRtIdAsync(OctoObjectId equipmentGroupRtId);
    Task<PagedResult<RtAlarmDto>?> GetAlarmsByMachineRtIdAndStateAsync(OctoObjectId machineRtId, AlarmStates alarmState);
    Task<PagedResult<RtAlarmDto>> GetAlarmByRtIdQueryAsync(OctoObjectId alarmRtId);

    Task<PagedResult<RtAlarmDto>> GetAlarmByWellKnownName(IEnumerable<string> foreignKeyNameList);

    Task<PagedResult<RtAlarmDto>> CreateAlarmsAsync(IEnumerable<RtAlarmInputDto> alarmEntities);

    Task<PagedResult<RtAlarmDto>> UpdateAlarmsAsync(IEnumerable<MutationDto<RtAlarmInputDto>> alarmEntities);

    Task<PagedResult<RtEquipmentModelDto>> CreateEquipmentModelsAsync(IEnumerable<RtEquipmentModelInputDto> modelEntities);
    Task<PagedResult<RtEquipmentModelDto>> UpdateEquipmentModelsAsync(IEnumerable<MutationDto<RtEquipmentModelInputDto>> modelEntities);
    Task<PagedResult<RtEquipmentGroupDto>> CreateEquipmentGroupsAsync(IEnumerable<RtEquipmentGroupInputDto> groupEntities);
    Task<PagedResult<RtEquipmentGroupDto>> UpdateEquipmentGroupsAsync(IEnumerable<MutationDto<RtEquipmentGroupInputDto>> groupEntities);
    Task<PagedResult<RtEquipmentMachine>> CreateEquipmentMachinesAsync(IEnumerable<RtEquipmentMachineInputDto> machineEntities);

    Task<PagedResult<RtEquipmentMachine>>
        UpdateEquipmentMachinesAsync(IEnumerable<MutationDto<RtEquipmentMachineInputDto>> machineEntities);

    Task<PagedResult<RtEventCommentDto>> CreateAlarmCommentAsync(IEnumerable<RtEventCommentInputDto> alarmComments);
    Task<RtAlarmWithCommentsDto?> GetCommentsForAlarmAsync(string alarmId);
}