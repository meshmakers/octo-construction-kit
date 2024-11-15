using Microsoft.Extensions.Configuration;

namespace Octo.Sdk.Packages.Industry.Basic.SystemTests.Configuration;

public class SystemTestConfiguration
{
    private readonly IConfiguration _configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.test.json", true)
        .Build();

    public IConfigurationSection GetSection(string section)
    {
        return _configuration.GetSection(section);
    }
}
