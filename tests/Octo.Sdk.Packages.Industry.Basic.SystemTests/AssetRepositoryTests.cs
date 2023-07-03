using Meshmakers.Octo.Common.Shared;
using Meshmakers.Octo.Sdk.Packages.Industry.Basic.DataTransferObjects;
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
        Assert.Equal(3, equipmentGroup.MachinesChildren.Items.Count());
    }
    
        
    [Fact]
    public async void TestGetAlarmsByMachineRtIdAsync()
    {
        var tenantClient = _tenantFixture.GetTenantClient();

        var assetRepository = new AssetRepository(tenantClient);

        var result = await assetRepository.GetAlarmsByMachineRtIdAndStateAsync(new OctoObjectId("64a2b64c3da56d342f1c3880"), AlarmStates.Received);
        Assert.Equal(1, result.List.Count);
        Assert.Equal(AlarmStates.Received, result.List.First().State);
    }
    
    [Fact]
    public async void TestGetAlarmByRtIdQueryAsync()
    {
        var tenantClient = _tenantFixture.GetTenantClient();

        var assetRepository = new AssetRepository(tenantClient);

        var result = await assetRepository.GetAlarmByRtIdQueryAsync(new OctoObjectId("64a2c3ee53e42e7e7eaa25e2"));
        Assert.Equal(1, result.List.Count);
        Assert.Equal(new OctoObjectId("64a2c3ee53e42e7e7eaa25e2"), result.List.First().RtId);
    }
}