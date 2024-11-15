using System.Globalization;
using Meshmakers.Octo.Communication.Contracts.DataTransferObjects;
using Meshmakers.Octo.ConstructionKit.Contracts;
using Meshmakers.Octo.Runtime.Contracts;
using Meshmakers.Octo.Sdk.Packages.Industry.Basic.DataTransferObjects;
using Meshmakers.Octo.Sdk.Packages.Industry.Basic.Repositories;
using Xunit;

namespace Octo.Sdk.Packages.Industry.Basic.SystemTests;

public class AssetRepositoryTests(TenantFixture tenantFixture) : IClassFixture<TenantFixture>
{
    [Fact]
    public async Task TestCreateAlarmCommentAsync()
    {
        var tenantClient = tenantFixture.GetTenantClient();
        var assetRepository = new AssetRepository(tenantClient);

        var result = await assetRepository.CreateAlarmCommentAsync([
            new RtEventCommentInputDto
            {
                ReceivedDateTime = DateTime.UtcNow,
                Comment = "Test comment",
                Parent =
                [
                    new RtAssociationInputDto
                    {
                        Target = new RtEntityIdDto { CkTypeId = "Meshmakers.Alarm", RtId = new OctoObjectId("64a2c3ad1254d840838bbd09")},
                        ModOption = AssociationModOptionsDto.Create
                    }
                ]
            }
        ]);
        
        Assert.NotNull(result);
        Assert.True(result.TotalCount == 1);
    }

    [Fact]
    public async Task TestGetAlarmCommentsAsync()
    {
        var tenantClient = tenantFixture.GetTenantClient();
        var assetRepository = new AssetRepository(tenantClient);
        
        var result = await assetRepository.GetCommentsForAlarmByRtIdAsync("64a2c3ad1254d840838bbd09");
        
        Assert.NotNull(result);
        Assert.True(result.AlarmComments?.Items?.Count() > 1);
    }


    [Fact]
    public async Task TestGetEquipmentModelAsync()
    {
        var tenantClient = tenantFixture.GetTenantClient();

        var assetRepository = new AssetRepository(tenantClient);

        var result = await assetRepository.GetEquipmentModelAsync("Demo");
        Assert.Single(result.List);
        Assert.Equal("Demo", result.List.First().Designation);
    }

    [Fact]
    public async Task TestGetEquipmentByGroupRtIdAsync()
    {
        var tenantClient = tenantFixture.GetTenantClient();

        var assetRepository = new AssetRepository(tenantClient);

        var equipmentGroup = await assetRepository.GetEquipmentByGroupRtIdAsync(new OctoObjectId("64a2b55a84c7869c60270d1a"));
        Assert.NotNull(equipmentGroup);
        Assert.True(equipmentGroup.MachinesChildren?.Items?.Count() >= 3);
    }


    [Fact]
    public async Task TestGetAlarmsByMachineRtIdAndStateAsync()
    {
        var tenantClient = tenantFixture.GetTenantClient();

        var assetRepository = new AssetRepository(tenantClient);

        var result = await assetRepository.GetAlarmsByMachineRtIdAndStateAsync(new OctoObjectId("64a2b64c3da56d342f1c3880"),
            AlarmStates.Received);
        Assert.Equal(1, result?.List.Count);
        Assert.Equal(AlarmStates.Received, result?.List.First().State);
    }

    [Fact]
    public async Task TestGetAlarmByRtIdQueryAsync()
    {
        var tenantClient = tenantFixture.GetTenantClient();

        var assetRepository = new AssetRepository(tenantClient);

        var result = await assetRepository.GetAlarmByRtIdQueryAsync(new OctoObjectId("64a2c3ee53e42e7e7eaa25e2"));
        Assert.Single(result.List);
        Assert.Equal(new OctoObjectId("64a2c3ee53e42e7e7eaa25e2"), result.List.First().RtId);
    }
    
    [Fact]
    public async Task TestGetAlarmByWellKnownNameAsync()
    {
        var tenantClient = tenantFixture.GetTenantClient();

        var assetRepository = new AssetRepository(tenantClient);

        var list = new List<string> { "alarmTest1", "alarmTest2", "alarmTest3" };
        var result = await assetRepository.GetAlarmByWellKnownName(list);
        Assert.Equal(3, result.List.Count);
        Assert.Contains(result.List, dto => dto.RtId == new OctoObjectId("64a2c3ee53e42e7e7eaa25e2"));
        Assert.Contains(result.List, dto => dto.RtId == new OctoObjectId("64a2c3b3015933f66e1f240b"));
        Assert.Contains(result.List, dto => dto.RtId == new OctoObjectId("64a2c3ad1254d840838bbd09"));
    }
    
    [Fact]
    public async Task TestGetMachinesAndAlarmsByGroupRtIdAsync()
    {
        var tenantClient = tenantFixture.GetTenantClient();

        var assetRepository = new AssetRepository(tenantClient);

        var result = await assetRepository.GetMachinesAndAlarmsByGroupRtIdAsync(new OctoObjectId("64a2b55a84c7869c60270d1a"),
            DateTime.MinValue, DateTime.MaxValue, "group");
        Assert.Single(result.List);
        Assert.NotNull(result.List.First().MachinesChildren?.Items?.First().AlarmChildren?.Groupings);
    }

    [Fact]
    public async Task TestGetAlarmsByMachineRtIdAsync()
    {
        var tenantClient = tenantFixture.GetTenantClient();

        var assetRepository = new AssetRepository(tenantClient);

        var result = await assetRepository.GetAlarmsByMachineRtIdAsync(new OctoObjectId("64a2b64c3da56d342f1c3880"),
            DateTime.MinValue, DateTime.MaxValue, "group");
        Assert.Single(result.List);
        Assert.NotNull(result.List.First().AlarmChildren?.Groupings);
    }

    
    [Fact]
    public async Task TestCreateAlarm()
    {
        var tenantClient = tenantFixture.GetTenantClient();

        var assetRepository = new AssetRepository(tenantClient);

        var wellKnown = Guid.NewGuid().ToString();

        var alarmList = new List<RtAlarmInputDto>
        {
            new()
            {
                RtWellKnownName = wellKnown,
                ReceivedDateTime = DateTime.UtcNow,
                Message = "Hi test",
                Parent =
                [
                    new RtAssociationInputDto
                    {
                        ModOption = AssociationModOptionsDto.Create, Target = new RtEntityIdDto {CkTypeId = "Meshmakers.Equipment.Machine",
                           RtId = new OctoObjectId("64a2b6c3bbf4aa537f812b62")}
                    }
                ]
            }
        };


        var result = await assetRepository.CreateAlarmsAsync(alarmList);
        Assert.Single(result.List);
        Assert.Equal("Hi test", result.List.First().Message);
        Assert.Equal(wellKnown, result.List.First().RtWellKnownName);
    }

    [Fact]
    public async Task TestUpdateAlarm()
    {
        var tenantClient = tenantFixture.GetTenantClient();
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
        Assert.Single(result.List);
        Assert.Equal(message, result.List.First().Message);
    }


    [Fact]
    public async Task TestCreateModel()
    {
        var tenantClient = tenantFixture.GetTenantClient();

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
        Assert.Single(result.List);
        Assert.Equal("Hi test", result.List.First().Designation);
    }

    [Fact]
    public async Task TestUpdateModel()
    {
        var tenantClient = tenantFixture.GetTenantClient();
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
        Assert.Single(result.List);
        Assert.Equal(message, result.List.First().Designation);
    }

    [Fact]
    public async Task TestCreateGroup()
    {
        var tenantClient = tenantFixture.GetTenantClient();

        var assetRepository = new AssetRepository(tenantClient);

        var groupEntities = new List<RtEquipmentGroupInputDto>
        {
            new()
            {
                Description = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture),
                Designation = "Hi test",
                Parent =
                [
                    new RtAssociationInputDto
                        { ModOption = AssociationModOptionsDto.Create, Target = new RtEntityIdDto{CkTypeId = "Meshmakers.Equipment.Model", RtId = new OctoObjectId("64a2b2e4e1ee56e262e83d98")} }
                ]
            }
        };


        var result = await assetRepository.CreateEquipmentGroupsAsync(groupEntities);
        Assert.Single(result.List);
        Assert.Equal("Hi test", result.List.First().Designation);
    }

    [Fact]
    public async Task TestUpdateGroup()
    {
        var tenantClient = tenantFixture.GetTenantClient();
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
        Assert.Single(result.List);
        Assert.Equal(message, result.List.First().Designation);
    }
    
    [Fact]
    public async Task TestCreateMachine()
    {
        var tenantClient = tenantFixture.GetTenantClient();

        var assetRepository = new AssetRepository(tenantClient);

        var machineEntities = new List<RtEquipmentMachineInputDto>
        {
            new()
            {
                Description = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture),
                Designation = "Hi test",
                Parent =
                [
                    new RtAssociationInputDto
                        { ModOption = AssociationModOptionsDto.Create, Target = new RtEntityIdDto{CkTypeId = "Meshmakers.Equipment.Group", RtId = new OctoObjectId("64a2b55a84c7869c60270d1a")} }
                ]
            }
        };


        var result = await assetRepository.CreateEquipmentMachinesAsync(machineEntities);
        Assert.Single(result.List);
        Assert.Equal("Hi test", result.List.First().Designation);
    }

    [Fact]
    public async Task TestUpdateMachine()
    {
        var tenantClient = tenantFixture.GetTenantClient();
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
        Assert.Single(result.List);
        Assert.Equal(message, result.List.First().Designation);
    }
}