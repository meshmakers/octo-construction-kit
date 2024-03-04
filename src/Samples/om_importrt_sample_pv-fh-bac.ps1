octo-cli -c EnableCommunication
octo-cli -c EnableTimeseries

octo-cli -c ImportRt -f ./application/samples/pv/rt-plug-fh-bac.json -w
octo-cli -c ImportRt -f ./application/samples/pv/rt-pv-fh-bac.json -w
