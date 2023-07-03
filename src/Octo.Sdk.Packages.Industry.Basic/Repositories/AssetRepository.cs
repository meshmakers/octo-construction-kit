using Meshmakers.Octo.Sdk.ServiceClient.AssetRepositoryServices.Tenants;

namespace Meshmakers.Octo.Sdk.Packages.Industry.Basic.Repositories;

public class AssetRepository : IAssetRepository
{
    private readonly ITenantClient _tenantClient;

    public AssetRepository(ITenantClient tenantClient)
    {
        _tenantClient = tenantClient;
    }
    
    // public async Task<PagedResult<RtEquipmentDto>> GetFirstPhotovoltaic(string tenantId)
    // {
    //     ArgumentValidation.ValidateString(nameof(tenantId), tenantId);
    //
    //     var getQuery = new GraphQLRequest
    //     {
    //         Query = GraphQl.GetPhotovoltaics,
    //     };
    //
    //     var result = await _tenantClient.SendQueryAsync<RtEquipmentDto>(getQuery);
    //     return new PagedResult<RtEquipmentDto>(result?.Items ?? new List<RtEquipmentDto>());
    // }
}