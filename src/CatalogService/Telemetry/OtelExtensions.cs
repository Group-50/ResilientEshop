using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace CatalogService.Telemetry;

public static class OtelExtensions
{

    public static WebApplicationBuilder AddOpenTelemetry(this WebApplicationBuilder builder)
    {
        var resourceBuilder = ResourceBuilder.CreateDefault().AddService(builder.Environment.ApplicationName);
        var otlpEndpoint = builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"];

        if (!string.IsNullOrWhiteSpace(otlpEndpoint))
        {
            builder.Logging.AddOpenTelemetry(logging =>
            {
                logging.SetResourceBuilder(resourceBuilder)
                       .AddOtlpExporter();
            });
        }

        builder.Services.AddOpenTelemetry()
            .WithMetrics(metrics =>
            {
                metrics.SetResourceBuilder(resourceBuilder)
                    .AddPrometheusExporter(o => o.DisableTotalNameSuffixForCounters = true)
                    .AddAspNetCoreInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddMeter(
                        "Microsoft.AspNetCore.Hosting",
                        "Microsoft.AspNetCore.Server.Kestrel",
                        "System.Net.Http"
                    );
                metrics.AddHttpClientInstrumentation();
                // .AddEventCountersInstrumentation(c =>
                // {
                //     // https://learn.microsoft.com/en-us/dotnet/core/diagnostics/available-counters
                //     c.AddEventSources(
                //         "Microsoft.AspNetCore.Hosting",
                //         "Microsoft-AspNetCore-Server-Kestrel",
                //         "System.Net.Http",
                //         "System.Net.Sockets",
                //         "System.Net.NameResolution",
                //         "System.Net.Security");
                // });
            })
            .WithTracing(tracing =>
            {
                // We need to use AlwaysSampler to record spans
                // from Todo.Web.Server, because there it no OpenTelemetry
                // instrumentation
                tracing.SetResourceBuilder(resourceBuilder)
                    .SetSampler(new AlwaysOnSampler())
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation();
                    // .AddEntityFrameworkCoreInstrumentation();

                if (!string.IsNullOrWhiteSpace(otlpEndpoint))
                {
                    tracing.AddOtlpExporter();
                }
            });


        return builder;
    }
}