namespace Template.Api.Status;

public sealed record ServiceStatus(
    string Service,
    string Version,
    string Environment,
    DateTimeOffset Utc);

public interface IStatusProvider
{
    ServiceStatus GetStatus();
}

public sealed class StatusProvider(
    IConfiguration configuration,
    IHostEnvironment environment,
    TimeProvider timeProvider) : IStatusProvider
{
    public ServiceStatus GetStatus() => new(
        Service: configuration["TemplateApi:ServiceName"] ?? "unknown",
        Version: configuration["TemplateApi:Version"] ?? "0.0.0",
        Environment: environment.EnvironmentName,
        Utc: timeProvider.GetUtcNow());
}
