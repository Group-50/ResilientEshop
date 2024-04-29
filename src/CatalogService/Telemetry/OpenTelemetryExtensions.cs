using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using OpenTelemetry.Exporter;
using OpenTelemetry.Instrumentation.AspNetCore;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace CatalogService.Telemetry;

public static class OpenTelemetryExtensions
{
    static Action<WebApplicationBuilder, ResourceBuilder> configureResource = (builder, r) => r.AddService(
        serviceName: builder.Configuration.GetValue<string>("ServiceName") ?? "otel-test",
        serviceVersion: typeof(Program).Assembly.GetName().Version?.ToString() ?? "unknown",
        serviceInstanceId: Environment.MachineName);

    public static WebApplicationBuilder AddOpenTelemetryMetrics(this WebApplicationBuilder builder)
    {
        Action<ResourceBuilder> configureResources = r => r.AddService(
            serviceName: builder.Configuration.GetValue("ServiceName", defaultValue: "otel-test")!,
            serviceVersion: typeof(Program).Assembly.GetName().Version?.ToString() ?? "unknown",
            serviceInstanceId: Environment.MachineName);
        
        var resourceBuilder = ResourceBuilder.CreateDefault();
        var tracingExporter = builder.Configuration.GetValue("UseTracingExporter", defaultValue: "console")!
            .ToLowerInvariant();
        var metricsExporter = builder.Configuration.GetValue("UseMetricsExporter", defaultValue: "console")!
            .ToLowerInvariant();

        builder.Services.AddOpenTelemetry()
            .ConfigureResource(configureResources)
            .WithTracing(tracing =>
            {
                tracing.SetSampler(new AlwaysOnSampler())
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddGrpcClientInstrumentation();
                builder.Services.Configure<AspNetCoreTraceInstrumentationOptions>(
                    builder.Configuration.GetSection("AspNetCoreInstrumentation"));

                switch (tracingExporter)
                {
                    case "zipkin":
                        tracing.AddZipkinExporter();
                        tracing.ConfigureServices(services =>
                        {
                            services.Configure<ZipkinExporterOptions>(builder.Configuration.GetSection("Zipkin"));
                        });
                        break;
                    case "otlp":
                        tracing.AddOtlpExporter(options =>
                        {
                            options.Endpoint = new Uri(builder.Configuration.GetValue("Otlp:Endpoint",
                                defaultValue: "http://localhost:4317")!);
                        });
                        break;
                    default:
                        tracing.AddConsoleExporter();
                        break;
                }
                
                
            })
            .WithMetrics(metrics =>
            {
                metrics.SetResourceBuilder(resourceBuilder);
                metrics.AddRuntimeInstrumentation()
                    .AddMeter(
                        "Microsoft.AspNetCore.Hosting",
                        "Microsoft.AspNetCore.Server.Kestrel",
                        "System.Net.Http"
                    );
                    
                switch (metricsExporter)
                {
                    case "prometheus":
                        metrics.AddPrometheusExporter();
                        break;
                    case "otlp":
                        metrics.AddOtlpExporter(options =>
                        {
                            options.Endpoint = new Uri(builder.Configuration.GetValue("Otlp:Endpoint",
                                defaultValue: "http://localhost:4317")!);
                        });
                        break;
                    default:
                        metrics.AddConsoleExporter();
                        break;
                }
            });
        AddOpenTelemetryLogging(builder);
        
        return builder;
    }

    public static WebApplicationBuilder AddOpenTelemetryLogging(this WebApplicationBuilder builder)
    {
        void ConfigureResources(ResourceBuilder r) => r.AddService(serviceName: builder.Configuration.GetValue("ServiceName", defaultValue: "otel-test")!, serviceVersion: typeof(Program).Assembly.GetName().Version?.ToString() ?? "unknown", serviceInstanceId: Environment.MachineName);

        var logExporter = builder.Configuration.
            GetValue("UseLogExporter", defaultValue: "console")!.ToLowerInvariant();
        builder.Logging.ClearProviders();
        builder.Logging.AddOpenTelemetry(logging =>
        {
            var resourceBuilder = ResourceBuilder.CreateDefault();
            ConfigureResources(resourceBuilder);
            logging.SetResourceBuilder(resourceBuilder);

            switch (logExporter)
            {
                case "otlp":
                    logging.AddOtlpExporter(options =>
                    {
                        options.Endpoint =
                            new Uri(builder.Configuration.GetValue("Otlp:Endpoint",
                                defaultValue: "http://localhost:4317")!);
                    });
                    break;
                default:
                    logging.AddConsoleExporter();
                    break;
            }

        });
        return builder;
    }

    public static WebApplicationBuilder AddDefaultHealthChecks(this WebApplicationBuilder builder)
    {
        builder.Services.AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy(), ["live"]);
        return builder;
    }

    public static WebApplication MapDefaultHealthEndpoints(this WebApplication app)
    {
        // app.MapPrometheusScrapingEndpoint();
        app.MapHealthChecks("/health");
        app.MapHealthChecks("alive", new HealthCheckOptions
        {
            Predicate = r => r.Tags.Contains("live")
        });
        return app;
    }
}