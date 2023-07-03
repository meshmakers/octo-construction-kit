using GraphQL;
using Meshmakers.Common.Shared;
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
}