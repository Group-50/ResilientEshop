using CatalogService.Data;
using CatalogService.Endpoints;
using CatalogService.Telemetry;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using Polly;
using ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// builder.Services.AddOpenTelemetry()
//     .WithMetrics(metrics =>
//     {
//         metrics.AddPrometheusExporter();
//         
//         metrics.AddView("http.server.request.duration",
//             new ExplicitBucketHistogramConfiguration
//             {
//                 Boundaries = new double[] { 0, 0.005, 0.01, 0.025, 0.05,
//                     0.075, 0.1, 0.25, 0.5, 0.75, 1, 2.5, 5, 7.5, 10 }
//             });
//         
//     });


// builder.AddOpenTelemetry();
var conn = builder.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine(conn);
builder.Services.AddDbContext<CatalogDbContext>(options =>
{
    options.UseNpgsql(conn);
});

builder.AddServiceDefaults();

var app = builder.Build();

//OTEL


// app.UseOpenTelemetryPrometheusScrapingEndpoint();
// app.MapPrometheusScrapingEndpoint();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.MapProductApi();
app.MapCatalogTypeApi();
app.MapCatalogBrandApi();


var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

var retryPolicy = Policy
    .Handle<NpgsqlException>()
    .WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(10));

retryPolicy.ExecuteAndCapture(() => DbInitializer.InitDb(app));



app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}