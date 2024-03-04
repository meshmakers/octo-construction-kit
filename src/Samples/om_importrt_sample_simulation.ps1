octo-cli -c EnableCommunication
octo-cli -c EnableTimeseries
octo-cli -c ImportRt -f ./simulator/rt-simulator.yaml -w
octo-cli -c ImportRt -f ./simulator/rt-adapters.yaml -w