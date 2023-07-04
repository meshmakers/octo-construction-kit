using GraphQL;
using Meshmakers.Octo.Common.Shared;
using Meshmakers.Octo.Common.Shared.DataTransferObjects;
using Meshmakers.Octo.Sdk.Packages.Industry.Basic.DataTransferObjects;
using Meshmakers.Octo.Sdk.ServiceClient.AssetRepositoryServices.Tenants;

namespace Meshmakers.Octo.Sdk.Packages.Industry.Basic.Repositories;

public class AssetRepository : IAssetRepository
{
    private readonly ITenantClient _tenantClient;

    public AssetRepository(ITenantClient tenantClient)
    {
        _tenantClient = tenantClient;
    }
    
    public async Task<PagedResult<RtEquipmentModelDto>> GetEquipmentModelAsync(string equipmentModelName)
    {
        var getQuery = new GraphQLRequest
        {
            Query = GraphQl.GetEquipmentModelQuery,
            Variables = new
            {
                equipmentModelName
            }
        };
    
        var result = await _tenantClient.SendQueryAsync<RtEquipmentModelDto>(getQuery);
        return new PagedResult<RtEquipmentModelDto>(result?.Items ?? new List<RtEquipmentModelDto>());
    }
    
    public async Task<RtEquipmentGroupDto?> GetEquipmentByGroupRtIdAsync(OctoObjectId equipmentGroupRtId)
    {
        var getQuery = new GraphQLRequest
        {
            Query = GraphQl.GetEquipmentByGroupRtIdQuery,
            Variables = new
            {
                equipmentGroupRtId
            }
        };
    
        var result = await _tenantClient.SendQueryAsync<RtEquipmentGroupDto>(getQuery);
        return result?.Items.FirstOrDefault();
    }
    
    public async Task<PagedResult<RtAlarmDto>?> GetAlarmsByMachineRtIdAndStateAsync(OctoObjectId machineRtId, AlarmStates alarmState)
    {
        var getQuery = new GraphQLRequest
        {
            Query = GraphQl.AlarmsByMachineRtIdAndStateQuery,
            Variables = new
            {
                machineRtId,
                alarmState = (int)alarmState
            }
        };
    
        var result = await _tenantClient.SendQueryAsync<RtEquipmentMachine>(getQuery);
        var equipmentMachine = result?.Items.SingleOrDefault();
        if (equipmentMachine == null)
        {
            return null;
        }
        
        return new PagedResult<RtAlarmDto>(equipmentMachine.AlarmChildren?.Items ?? new List<RtAlarmDto>());
    }
    
    public async Task<PagedResult<RtAlarmDto>> GetAlarmByRtIdQueryAsync(OctoObjectId alarmRtId)
    {
        var getQuery = new GraphQLRequest
        {
            Query = GraphQl.GetAlarmByRtIdQuery,
            Variables = new
            {
                alarmRtId
            }
        };
    
        var result = await _tenantClient.SendQueryAsync<RtAlarmDto>(getQuery);
        return new PagedResult<RtAlarmDto>(result?.Items ?? new List<RtAlarmDto>());
    }
    
    public async Task<PagedResult<RtAlarmDto>> CreateAlarmsAsync(IEnumerable<RtAlarmInputDto> alarmEntities)
    {
        var getQuery = new GraphQLRequest
        {
            Query = GraphQl.CreateAlarmsMutation,
            Variables = new
            {
                alarmEntities
            }
        };
    
        var result = await _tenantClient.SendMutationAsync<IEnumerable<RtAlarmDto>>(getQuery);
        return new PagedResult<RtAlarmDto>(result);
    }
    
    public async Task<PagedResult<RtAlarmDto>> UpdateAlarmsAsync(IEnumerable<MutationDto<RtAlarmInputDto>> alarmEntities)
    {
        var getQuery = new GraphQLRequest
        {
            Query = GraphQl.UpdateAlarmsMutation,
            Variables = new
            {
                alarmEntities
            }
        };
    
        var result = await _tenantClient.SendMutationAsync<IEnumerable<RtAlarmDto>>(getQuery);
        return new PagedResult<RtAlarmDto>(result);
    }
    
    public async Task<PagedResult<RtEquipmentModelDto>> CreateEquipmentModelsAsync(IEnumerable<RtEquipmentModelInputDto> modelEntities)
    {
        var getQuery = new GraphQLRequest
        {
            Query = GraphQl.CreateEquipmentModel,
            Variables = new
            {
                modelEntities
            }
        };
    
        var result = await _tenantClient.SendMutationAsync<IEnumerable<RtEquipmentModelDto>>(getQuery);
        return new PagedResult<RtEquipmentModelDto>(result);
    }
    
    public async Task<PagedResult<RtEquipmentModelDto>> UpdateEquipmentModelsAsync(IEnumerable<MutationDto<RtEquipmentModelInputDto>> modelEntities)
    {
        var getQuery = new GraphQLRequest
        {
            Query = GraphQl.UpdateEquipmentModel,
            Variables = new
            {
                modelEntities
            }
        };
    
        var result = await _tenantClient.SendMutationAsync<IEnumerable<RtEquipmentModelDto>>(getQuery);
        return new PagedResult<RtEquipmentModelDto>(result);
    }
    
    public async Task<PagedResult<RtEquipmentGroupDto>> CreateEquipmentGroupsAsync(IEnumerable<RtEquipmentGroupInputDto> groupEntities)
    {
        var getQuery = new GraphQLRequest
        {
            Query = GraphQl.CreateEquipmentGroup,
            Variables = new
            {
                groupEntities
            }
        };
    
        var result = await _tenantClient.SendMutationAsync<IEnumerable<RtEquipmentGroupDto>>(getQuery);
        return new PagedResult<RtEquipmentGroupDto>(result);
    }
    
    public async Task<PagedResult<RtEquipmentGroupDto>> UpdateEquipmentGroupsAsync(IEnumerable<MutationDto<RtEquipmentGroupInputDto>> groupEntities)
    {
        var getQuery = new GraphQLRequest
        {
            Query = GraphQl.UpdateEquipmentGroup,
            Variables = new
            {
                groupEntities
            }
        };
    
        var result = await _tenantClient.SendMutationAsync<IEnumerable<RtEquipmentGroupDto>>(getQuery);
        return new PagedResult<RtEquipmentGroupDto>(result);
    }
    
    public async Task<PagedResult<RtEquipmentMachine>> CreateEquipmentMachinesAsync(IEnumerable<RtEquipmentMachineInputDto> machineEntities)
    {
        var getQuery = new GraphQLRequest
        {
            Query = GraphQl.CreateEquipmentMachine,
            Variables = new
            {
                machineEntities
            }
        };
    
        var result = await _tenantClient.SendMutationAsync<IEnumerable<RtEquipmentMachine>>(getQuery);
        return new PagedResult<RtEquipmentMachine>(result);
    }
    
    public async Task<PagedResult<RtEquipmentMachine>> UpdateEquipmentMachinesAsync(IEnumerable<MutationDto<RtEquipmentMachineInputDto>> machineEntities)
    {
        var getQuery = new GraphQLRequest
        {
            Query = GraphQl.UpdateEquipmentMachine,
            Variables = new
            {
                machineEntities
            }
        };
    
        var result = await _tenantClient.SendMutationAsync<IEnumerable<RtEquipmentMachine>>(getQuery);
        return new PagedResult<RtEquipmentMachine>(result);
    }
}
