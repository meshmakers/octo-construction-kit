octo-cli -c EnableCommunication
octo-cli -c EnableStreamData
octo-cli -c ImportRt -f ./_general/rt-adapters-mesh.yaml -w -r
octo-cli -c ImportRt -f ./_general/rt-pipeline-excel.yaml -w -r
