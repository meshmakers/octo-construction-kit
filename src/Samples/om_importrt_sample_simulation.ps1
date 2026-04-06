octo-cli -c ImportRt -f ./simulator/rt-simulator-energy-community.yaml -w -r
octo-cli -c ImportRt -f ./simulator/rt-simulator-industry.yaml -w -r
octo-cli -c ImportRt -f ./simulator/rt-create-customers.yaml -w -r
octo-cli -c ImportRt -f ./simulator/rt-adapters-edge.yaml -w -r
octo-cli -c ImportRt -f ./_general/rt-adapters-mesh.yaml -w -r
octo-cli -c ImportRt -f ./simulator/rt-dataflow-cross-adapter.yaml -w -r