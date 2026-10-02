using Microsoft.Extensions.Configuration;
using Template.Api.Status;
using Xunit;

namespace Template.Api.Tests.Unit;

public class StatusProviderTests
{
    [Fact]
    public void GetStatus_ReturnsConfiguredValues()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["TemplateApi:ServiceName"] = "svc-under-test",
                ["TemplateApi:Version"] = "1.2.3"
            })
            .Build();
        var now = new DateTimeOffset(2026, 10, 2, 12, 0, 0, TimeSpan.Zero);
        var provider = new StatusProvider(
            configuration, new FakeHostEnvironment("Staging"), new FixedTimeProvider(now));

        var status = provider.GetStatus();

        Assert.Equal("svc-under-test", status.Service);
        Assert.Equal("1.2.3", status.Version);
        Assert.Equal("Staging", status.Environment);
        Assert.Equal(now, status.Utc);
    }

    [Fact]
    public void GetStatus_UsesFallbacks_WhenConfigurationMissing()
    {
        var configuration = new ConfigurationBuilder().Build();
        var provider = new StatusProvider(
            configuration, new FakeHostEnvironment("Production"), TimeProvider.System);

        var status = provider.GetStatus();

        Assert.Equal("unknown", status.Service);
        Assert.Equal("0.0.0", status.Version);
        Assert.Equal("Production", status.Environment);
    }
}
