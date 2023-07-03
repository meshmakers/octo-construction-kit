// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace Octo.Sdk.Packages.Industry.Basic.SystemTests.Configuration;

// ReSharper disable once ClassNeverInstantiated.Global
public class SystemTestOptions
{
    public string? TenantId { get; set; }
    public string? AssetRepoServiceUri { get; set; } = "https://localhost:5001";
}
