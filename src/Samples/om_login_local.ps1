param (
    [string]$tenantId = "meshtest"
)
octo-cli -c Config -asu "https://localhost:5001/" -isu "https://localhost:5003/" -bsu "https://localhost:5009/" -csu "https://localhost:5015/" -rsu "https://localhost:5007/" -tid $tenantId
octo-cli -c Login -i