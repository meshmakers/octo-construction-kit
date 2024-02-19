octo-cli -c importck -f ../ConstructionKits/Octo.Sdk.Packages.Basic/ConstructionKit/ck-basic.yaml -w
octo-cli -c importck -f ../ConstructionKits/Octo.Sdk.Packages.Industry.Basic/ConstructionKit/ck-industrybasic.yaml -w
octo-cli -c importck -f ../ConstructionKits/Octo.Sdk.Packages.Industry.Energy/ConstructionKit/ck-industryenergy.yaml -w
octo-cli -c importck -f ../ConstructionKits/Octo.Sdk.Packages.Industry.Water/ConstructionKit/ck-industrywater.yaml -w

# add the communication construction kit. beacause this is just a sample, it is okay to hardcode to another repository
# anyways the communication controller should import this construction kit when the tenant is activated
octo-cli -c importck -f ../../../octo-communication-controller-services/src/SystemCommunicationCkModel/ConstructionKit/ck-system.communication.yaml -w