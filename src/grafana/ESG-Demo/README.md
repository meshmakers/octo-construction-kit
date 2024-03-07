# Setting Up the Grafana Dashboard

Follow these steps to import the Grafana dashboard efficiently:

## Step 1: Configure a New Data Source

1. **Install the Connection Plugin**: Begin by installing the `JSON API` plugin, essential for integrating the new data
   source. Access detailed instructions on installation and configuration in the official
   documentation: [JSON API plugin documentation](https://grafana.com/docs/plugins/marcusolsson-json-datasource/latest/).

## Step 2: Adapt the JSON for Import

2. **Locate the Data Source ID**:
    - Navigate to `Connections` in the main menu.
    - Proceed to `Data Sources`.
    - Identify the new data source's ID in the URL, found as the last segment.

3. **Modify the JSON File**:
    - Open `esg_dashboard_grafana.json`.
    - Replace the existing data source ID (`bab32cae-9bdc-4f15-ab3a-2e74851b0ae9`) with the new one you've located.

## Step 3: Verify Dashboard Functionality

- **Consult the API Documentation**: Ensure your data source is compatible by consulting the API documentation available
  in `esg-dashboard_openapi.json`. If the data source aligns with the required responses, the dashboard should function
  properly.

Following these steps, you should be able to successfully import and activate your Grafana dashboard with the new data
source.
