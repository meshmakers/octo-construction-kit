using System.Globalization;
using Meshmakers.Octo.Common.Shared;
using Meshmakers.Octo.Common.Shared.DataTransferObjects;
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

        var result = await assetRepository.GetAlarmsByMachineRtIdAndStateAsync(new OctoObjectId("64a2b64c3da56d342f1c3880"),
            AlarmStates.Received);
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

    [Fact]
    public async void TestCreateAlarm()
    {
        var tenantClient = _tenantFixture.GetTenantClient();

        var assetRepository = new AssetRepository(tenantClient);

        var alarmList = new List<RtAlarmInputDto>
        {
            new()
            {
                ReceivedDateTime = DateTime.UtcNow,
                Message = "Hi test",
                Parent = new[]
                {
                    new RtAssociationInputDto
                    {
                        ModOption = AssociationModOptionsDto.Create, Target = new RtEntityId("Meshmakers.Equipment.Machine",
                            new OctoObjectId("64a2b6c3bbf4aa537f812b62"))
                    }
                }
            }
        };


        var result = await assetRepository.CreateAlarmsAsync(alarmList);
        Assert.Equal(1, result.List.Count);
        Assert.NotNull(result.List.First().RtId);
        Assert.Equal("Hi test", result.List.First().Message);
    }

    [Fact]
    public async void TestUpdateAlarm()
    {
        var tenantClient = _tenantFixture.GetTenantClient();
        var message = Guid.NewGuid().ToString();

        var assetRepository = new AssetRepository(tenantClient);

        var alarmList = new List<MutationDto<RtAlarmInputDto>>
        {
            new()
            {
                RtId = new OctoObjectId("64a2c3ee53e42e7e7eaa25e2"),
                Item = new RtAlarmInputDto
                {
                    ClearedDateTime = DateTime.UtcNow,
                    Message = message
                }
            }
        };


        var result = await assetRepository.UpdateAlarmsAsync(alarmList);
        Assert.Equal(1, result.List.Count);
        Assert.NotNull(result.List.First().RtId);
        Assert.Equal(message, result.List.First().Message);
    }


    [Fact]
    public async void TestCreateModel()
    {
        var tenantClient = _tenantFixture.GetTenantClient();

        var assetRepository = new AssetRepository(tenantClient);

        var modelEntities = new List<RtEquipmentModelInputDto>
        {
            new()
            {
                Description = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture),
                Designation = "Hi test",
            }
        };


        var result = await assetRepository.CreateEquipmentModelsAsync(modelEntities);
        Assert.Equal(1, result.List.Count);
        Assert.NotNull(result.List.First().RtId);
        Assert.Equal("Hi test", result.List.First().Designation);
    }

    [Fact]
    public async void TestUpdateModel()
    {
        var tenantClient = _tenantFixture.GetTenantClient();
        var message = Guid.NewGuid().ToString();

        var assetRepository = new AssetRepository(tenantClient);

        var modelEntities = new List<MutationDto<RtEquipmentModelInputDto>>
        {
            new()
            {
                RtId = new OctoObjectId("64a2b2e4e1ee56e262e83d98"),
                Item = new RtEquipmentModelInputDto
                {
                    Description = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture),
                    Designation = message
                }
            }
        };


        var result = await assetRepository.UpdateEquipmentModelsAsync(modelEntities);
        Assert.Equal(1, result.List.Count);
        Assert.NotNull(result.List.First().RtId);
        Assert.Equal(message, result.List.First().Designation);
    }

    [Fact]
    public async void TestCreateGroup()
    {
        var tenantClient = _tenantFixture.GetTenantClient();

        var assetRepository = new AssetRepository(tenantClient);

        var groupEntities = new List<RtEquipmentGroupInputDto>
        {
            new()
            {
                Description = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture),
                Designation = "Hi test",
                Parent = new[]
                {
                    new RtAssociationInputDto
                        { ModOption = AssociationModOptionsDto.Create, Target = new RtEntityId("Meshmakers.Equipment.Model", new OctoObjectId("64a2b2e4e1ee56e262e83d98")) }
                }
            }
        };


        var result = await assetRepository.CreateEquipmentGroupsAsync(groupEntities);
        Assert.Equal(1, result.List.Count);
        Assert.NotNull(result.List.First().RtId);
        Assert.Equal("Hi test", result.List.First().Designation);
    }

    [Fact]
    public async void TestUpdateGroup()
    {
        var tenantClient = _tenantFixture.GetTenantClient();
        var message = Guid.NewGuid().ToString();

        var assetRepository = new AssetRepository(tenantClient);

        var groupEntities = new List<MutationDto<RtEquipmentGroupInputDto>>
        {
            new()
            {
                RtId = new OctoObjectId("64a2b2ff83f510d627206cfe"),
                Item = new RtEquipmentGroupInputDto
                {
                    Description = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture),
                    Designation = message
                }
            }
        };


        var result = await assetRepository.UpdateEquipmentGroupsAsync(groupEntities);
        Assert.Equal(1, result.List.Count);
        Assert.NotNull(result.List.First().RtId);
        Assert.Equal(message, result.List.First().Designation);
    }
    
    [Fact]
    public async void TestCreateMachine()
    {
        var tenantClient = _tenantFixture.GetTenantClient();

        var assetRepository = new AssetRepository(tenantClient);

        var machineEntities = new List<RtEquipmentMachineInputDto>
        {
            new()
            {
                Description = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture),
                Designation = "Hi test",
                Parent = new[]
                {
                    new RtAssociationInputDto
                        { ModOption = AssociationModOptionsDto.Create, Target = new RtEntityId("Meshmakers.Equipment.Group", new OctoObjectId("64a2b55a84c7869c60270d1a")) }
                }
            }
        };


        var result = await assetRepository.CreateEquipmentMachinesAsync(machineEntities);
        Assert.Equal(1, result.List.Count);
        Assert.NotNull(result.List.First().RtId);
        Assert.Equal("Hi test", result.List.First().Designation);
    }

    [Fact]
    public async void TestUpdateMachine()
    {
        var tenantClient = _tenantFixture.GetTenantClient();
        var message = Guid.NewGuid().ToString();

        var assetRepository = new AssetRepository(tenantClient);

        var machineEntities = new List<MutationDto<RtEquipmentMachineInputDto>>
        {
            new()
            {
                RtId = new OctoObjectId("64a2b6c3bbf4aa537f812b62"),
                Item = new RtEquipmentMachineInputDto
                {
                    Description = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture),
                    Designation = message
                }
            }
        };


        var result = await assetRepository.UpdateEquipmentMachinesAsync(machineEntities);
        Assert.Equal(1, result.List.Count);
        Assert.NotNull(result.List.First().RtId);
        Assert.Equal(message, result.List.First().Designation);
    }
}