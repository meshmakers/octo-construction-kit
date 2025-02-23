param (
    [string]$tenantId = "meshtest"
)
octo-cli -c Config -asu "https://assets.local-1.srv.mm.local/" -isu "https://connect.local-1.srv.mm.local" -bsu "https://bots.local-1.srv.mm.local/" -csu "https://communication.local-1.srv.mm.local/" -tid $tenantId
octo-cli -c Login -i


