using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Octo.Sdk.Packages.Industry.Basic.SystemTests.Configuration;

namespace Octo.Sdk.Packages.Industry.Basic.SystemTests;

public class SystemTestFixture
{
    private readonly SystemTestConfiguration _configuration;

    public SystemTestFixture()
    {
        _configuration = new SystemTestConfiguration();

        var serviceCollection = new ServiceCollection();

        serviceCollection.Configure<SystemTestOptions>(options =>
            _configuration.GetSection("systemTest").Bind(options));
        ServiceProvider = serviceCollection.BuildServiceProvider();
    }

    public ServiceProvider ServiceProvider { get; }

    public T GetOptions<T>(string sectionName)
    {
        var option = Activator.CreateInstance<T>();
        _configuration.GetSection(sectionName).Bind(option);
        return option;
    }
}