using FakeItEasy;
using Meshmakers.Octo.Sdk.ServiceClient.AssetRepositoryServices.Tenants;
using Microsoft.Extensions.Options;
using Octo.Sdk.Packages.Industry.Basic.SystemTests.Configuration;

namespace Octo.Sdk.Packages.Industry.Basic.SystemTests;

public class TenantFixture: SystemTestFixture
{
    public ITenantClient GetTenantClient()
    {
        var clientAccessToken = A.Fake<ITenantClientAccessToken>();

        var options = GetOptions<SystemTestOptions>("SystemTest");

        var tenantClient = new TenantClient(new OptionsWrapper<TenantClientOptions>(
            new TenantClientOptions
            {
                EndpointUri = options.AssetRepoServiceUri,
                TenantId = options.TenantId
            }), clientAccessToken);

        return tenantClient;
    }
}