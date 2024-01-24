using Meshmakers.Octo.Communication.Contracts.DataTransferObjects;
using Meshmakers.Octo.ConstructionKit.Contracts;
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
    Task<PagedResult<RtEquipmentMachineDto>> CreateEquipmentMachinesAsync(IEnumerable<RtEquipmentMachineInputDto> machineEntities);

    Task<PagedResult<RtEquipmentMachineDto>>
        UpdateEquipmentMachinesAsync(IEnumerable<MutationDto<RtEquipmentMachineInputDto>> machineEntities);

    Task<PagedResult<RtEventCommentDto>> CreateAlarmCommentAsync(IEnumerable<RtEventCommentInputDto> alarmComments);
    Task<RtAlarmWithCommentsDto?> GetCommentsForAlarmByRtIdAsync(string alarmRtId);
    
    Task<PagedResult<RtEquipmentGroupDto>> GetMachinesAndAlarmsByGroupRtIdAsync(OctoObjectId groupRtId, DateTime fromDateTime, DateTime toDateTime,
        string groupBy);

    Task<PagedResult<RtEquipmentMachineDto>> GetAlarmsByMachineRtIdAsync(OctoObjectId machineRtId, DateTime fromDateTime,
        DateTime toDateTime, string groupBy);
}