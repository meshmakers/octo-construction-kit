namespace Meshmakers.Octo.Sdk.Packages.Industry.Basic;

internal static class GraphQl
{
    public const string GetEquipmentByGroupRtIdQuery = @"
        query($equipmentGroupRtId: OctoObjectIdType!) {
          meshmakersEquipmentGroupConnection(
            rtId:$equipmentGroupRtId
          ) {
            items {
              designation
              children {
                meshmakersEquipmentMillingMachineConnection {
                  items {
                    rtId
                    manufacturer
                    modelNumber
                  }
                }
                meshmakersEquipmentInjectionMouldingConnection {
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
              meshmakersEquipmentModelConnection(fieldFilter: [{attributeName: ""designation"", operator:EQUALS, comparisonValue: $equipmentModelName}]) {
                items {
                    designation
                    children {
                        meshmakersEquipmentGroupConnection {
                            items {
                                designation
                                children {
                                    meshmakersEquipmentGroupConnection {
                                        items {
                                            designation
                                            children {
                                                meshmakersEquipmentMillingMachineConnection {
                                                    items {
                                                        manufacturer
                                                        modelNumber
                                                    }
                                                }
                                                meshmakersEquipmentInjectionMouldingConnection {
                                                    items {
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
            }
            ";
}