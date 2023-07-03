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
                  rtCreationDateTime
                  receivedDateTime
                  message
                  state
                  clearedDateTime
                  acknowledgedDateTime
                }
              }
            }
          }
        }
      }
    ";

    public const String GetAlarmByRtIdQuery = @"
      query($alarmRtId: OctoObjectIdType!) {
        meshmakersAlarmConnection(rtId: $alarmRtId) {
          items {
            rtId
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
}