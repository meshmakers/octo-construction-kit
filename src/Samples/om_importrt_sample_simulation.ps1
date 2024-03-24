octo-cli -c EnableCommunication
octo-cli -c ImportRt -f ./simulator/rt-simulator.yaml -w
octo-cli -c ImportRt -f ./simulator/rt-adapters-mesh.yaml -w
octo-cli -c ImportRt -f ./simulator/rt-adapters-edge.yaml -w