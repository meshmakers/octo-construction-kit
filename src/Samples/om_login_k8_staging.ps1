param (
    [string]$tenantId = "meshtest"
)
octo-cli -c Config -asu "https://assets.staging.meshmakers.cloud/" -isu "https://connect.staging.meshmakers.cloud" -bsu "https://bots.staging.meshmakers.cloud/" -csu "https://communication.staging.meshmakers.cloud/" -rsu "https://reporting.staging.meshmakers.cloud/"  -tid $tenantId
octo-cli -c Login -i


