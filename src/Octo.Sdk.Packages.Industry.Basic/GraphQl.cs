namespace Meshmakers.Octo.Sdk.Packages.Industry.Basic;

internal static class GraphQl
{
    public const string GetEquipmentByGroupRtIdQuery = @"
      query($equipmentGroupRtId: OctoObjectIdType!) {
        meshmakersEquipmentGroupConnection(rtId: $equipmentGroupRtId) {
          items {
            rtId
            designation
            children {
              meshmakersEquipmentMachineConnection {
                items {
                  rtId
                  manufacturer
                  modelNumber
                }
              }
            }
          }
        }
      }
    ";
    
    public const string GetEquipmentModelQuery = @"
           query($equipmentModelName: SimpleScalarType!) {
              meshmakersEquipmentModelConnection(
                fieldFilter: [
                  {
                    attributeName: ""designation""
                    operator: EQUALS
                    comparisonValue: $equipmentModelName
                  }
                ]
              ) {
                items {
                  rtId
                  designation
                  children {
                    meshmakersEquipmentGroupConnection {
                      items {
                        rtId
                        designation
                        children {
                          meshmakersEquipmentGroupConnection {
                            items {
                              rtId
                              designation
                              children {
                                meshmakersEquipmentMachineConnection {
                                  items {
                                    rtId
                                    manufacturer
                                    modelNumber
                                  }
                                }
                              }
                            }
                          }
                        }
                      }
                    }
                  }
                }
              }
            }";

    public const string AlarmsByMachineRtIdAndStateQuery = @"
      query($machineRtId: OctoObjectIdType!, $alarmState: SimpleScalarType!) {
        meshmakersEquipmentMachineConnection(rtId: $machineRtId) {
          items {
            rtId
            children {
              meshmakersAlarmConnection(
                fieldFilter: [
                  {
                    attributeName: ""state""
                    operator: EQUALS
                    comparisonValue: $alarmState
                  }
                ]
              ) {
                items {
                  rtId
                  rtWellKnownName
                  rtCreationDateTime
                  receivedDateTime
                  message
                  state
                  clearedDateTime
                  acknowledgedDateTime
                  group
                  classification
                }
              }
            }
          }
        }
      }
    ";

    public const string GetAlarmByRtIdQuery = @"
      query($alarmRtId: OctoObjectIdType!) {
        meshmakersAlarmConnection(rtId: $alarmRtId) {
          items {
            rtId
            rtWellKnownName
            rtCreationDateTime
            receivedDateTime
            message
            state
            clearedDateTime
            acknowledgedDateTime
          }
        }
      }
    ";

    public const string GetAlarmByWellKnownNameQuery = @"
      query($foreignKeyNames: SimpleScalarType!) {
        meshmakersAlarmConnection(fieldFilter: [{attributeName: ""rtWellKnownName"", operator: IN, comparisonValue:$foreignKeyNames}]) {
          items {
            rtId
            rtWellKnownName
            rtCreationDateTime
            receivedDateTime
            message
            state
            clearedDateTime
            acknowledgedDateTime
            group
            classification
          }
        }
      }
    ";

    public const string CreateAlarmsMutation = @"
      mutation($alarmEntities: [MeshmakersAlarmInput]!) {
        createMeshmakersAlarms(entities: $alarmEntities) {
          rtId
          rtWellKnownName
          rtCreationDateTime
          receivedDateTime
          message
          state
          clearedDateTime
          acknowledgedDateTime
          group
          classification
        }
      }
    ";

    public const string UpdateAlarmsMutation = @"
      mutation($alarmEntities: [UpdateMeshmakersAlarmInput]!) {
        updateMeshmakersAlarms(entities: $alarmEntities) {
          rtId
          rtWellKnownName
          rtCreationDateTime
          receivedDateTime
          message
          state
          clearedDateTime
          acknowledgedDateTime
          group
          classification
        }
      }
    ";

    public const string CreateEquipmentModel = @"
      mutation($modelEntities: [MeshmakersEquipmentModelInput]!) {
        createMeshmakersEquipmentModels(entities: $modelEntities) {
          rtId
          designation
          description    
        }
      }
    ";
    
    public const string UpdateEquipmentModel = @"
      mutation($modelEntities: [UpdateMeshmakersEquipmentModelInput]!) {
        updateMeshmakersEquipmentModels(entities: $modelEntities) {
          rtId
          designation
          description    
        }
      }
    ";
    
    public const string CreateEquipmentGroup = @"
      mutation($groupEntities: [MeshmakersEquipmentGroupInput]!) {
        createMeshmakersEquipmentGroups(entities: $groupEntities) {
          rtId
          designation
          description    
        }
      }
    ";
    
    public const string UpdateEquipmentGroup = @"
      mutation($groupEntities: [UpdateMeshmakersEquipmentGroupInput]!) {
        updateMeshmakersEquipmentGroups(entities: $groupEntities) {
          rtId
          designation
          description    
        }
      }
    ";

    public const string CreateEquipmentMachine = @"
      mutation($machineEntities: [MeshmakersEquipmentMachineInput]!) {
        createMeshmakersEquipmentMachines(entities: $machineEntities) {
          rtId
          designation
          description
          manufacturer
          modelNumber
          serialNumber
        }
      }
    ";
    
    public const string UpdateEquipmentMachine = @"
      mutation($machineEntities: [UpdateMeshmakersEquipmentMachineInput]!) {
        updateMeshmakersEquipmentMachines(entities: $machineEntities) {
          rtId
          designation
          description
          manufacturer
          modelNumber
          serialNumber
        }
      }
    ";
}