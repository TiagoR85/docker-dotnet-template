using Prometheus;
using Template.Api.Status;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddSingleton<IStatusProvider, StatusProvider>();
builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseHttpMetrics(options => options.ReduceStatusCodeCardinality());

app.MapGet("/api/v1/status", (IStatusProvider statusProvider) =>
        Results.Ok(statusProvider.GetStatus()))
    .WithName("GetStatus")
    .WithTags("status");

app.MapHealthChecks("/health/live");
app.MapHealthChecks("/health/ready");
app.MapMetrics();

app.Run();

public partial class Program;
