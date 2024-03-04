octo-cli -c EnableCommunication
octo-cli -c EnableTimeseries

octo-cli -c ImportRt -f ./maintenance/rt-maintenance.yaml -w
octo-cli -c ImportRt -f ./maintenance/rt-adapters.yaml -w
