param (
    [string]$tenantId = "meshtest"
)
octo-cli -c Config -asu "https://assets.meshmakers.cloud/" -isu "https://connect.meshmakers.cloud" -bsu "https://bots.meshmakers.cloud/" -csu "https://communication.meshmakers.cloud/" -tid $tenantId
octo-cli -c Login -i


