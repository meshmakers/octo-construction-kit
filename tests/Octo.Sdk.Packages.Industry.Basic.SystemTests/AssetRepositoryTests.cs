using Meshmakers.Octo.Common.Shared;
using Meshmakers.Octo.Sdk.Packages.Industry.Basic.Repositories;
using Xunit;

namespace Octo.Sdk.Packages.Industry.Basic.SystemTests;

public class AssetRepositoryTests : IClassFixture<TenantFixture>
{
    private readonly TenantFixture _tenantFixture;

    public AssetRepositoryTests(TenantFixture tenantFixture)
    {
        _tenantFixture = tenantFixture;
    }
    
    
    [Fact]
    public async void TestGetEquipmentModelAsync()
    {
        var tenantClient = _tenantFixture.GetTenantClient();

        var assetRepository = new AssetRepository(tenantClient);

        var result = await assetRepository.GetEquipmentModelAsync("Maintenance");
        Assert.Equal(1, result.List.Count);
        Assert.Equal("Maintenance", result.List.First().Designation);
    }
    
    [Fact]
    public async void TestGetEquipmentByGroupRtIdAsync()
    {
        var tenantClient = _tenantFixture.GetTenantClient();

        var assetRepository = new AssetRepository(tenantClient);

        var equipmentGroup = await assetRepository.GetEquipmentByGroupRtIdAsync(new OctoObjectId("64a2b55a84c7869c60270d1a"));
        Assert.NotNull(equipmentGroup);
        Assert.Single(equipmentGroup.InjectionMouldingMachineChildren.Items);
        Assert.Equal(2, equipmentGroup.MillingMachineChildren.Items.Count());
    }
}