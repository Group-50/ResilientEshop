using CatalogService.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

//configuring logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("Logs/catalogservice.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog(); 

//adding services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<CatalogDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CatalogDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

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

app.MapGet("/products", async (CatalogDbContext dbContext, ILogger<Program> logger) =>
{
    logger.LogInformation("Fetching all products");
    var products = await dbContext.CatalogItems.ToListAsync();
    logger.LogInformation($"Retrieved {products.Count} products");
    return Results.Ok(products);
});

app.MapGet("/products/by", async (CatalogDbContext dbContext, string ids, ILogger<Program> logger) =>
{
    if (string.IsNullOrEmpty(ids))
    {
        logger.LogWarning("No IDs provided for products lookup");
        return Results.BadRequest("No IDs provided");
    }

    // Converting string to int
    var idList = ids.Split(',').Select(id => int.Parse(id.Trim())).ToList();
    logger.LogInformation($"Looking up products with IDs: {string.Join(", ", idList)}");

    // Running a query on the db with the int
    var products = await dbContext.CatalogItems
                                  .Where(product => idList.Contains(product.Id))
                                  .ToListAsync();

    if (products.Any())
    {
        logger.LogInformation($"Found {products.Count} products for given IDs.");
        return Results.Ok(products);
    }
    else
    {
        logger.LogWarning($"No products found with the specified IDs: {string.Join(", ", idList)}");
        return Results.NotFound("No products found with the specified IDs");
    }
});

app.MapGet("/products/by/{prodId}", async (CatalogDbContext dbContext, int prodId, ILogger<Program> logger) =>
{
    logger.LogInformation($"Attempting to fetch product with ID: {prodId}");

    // Running a query on db to check for prodId
    var product = await dbContext.CatalogItems.FirstOrDefaultAsync(p => p.Id == prodId);

    // Checking for result found
    if (product != null)
    {
        logger.LogInformation($"Product with ID {prodId} found.");
        return Results.Ok(product);
    }
    else
    {
        logger.LogWarning($"Product with ID {prodId} not found.");
        return Results.NotFound($"Product with ID {prodId} not found.");
    }
});


app.MapGet("/products/by/type/{typeId}", async (CatalogDbContext dbContext, int typeId, ILogger<Program> logger) =>
{
    logger.LogInformation($"Fetching products by type ID: {typeId}");

    // Running a query on db to check for type/typeid
    var products = await dbContext.CatalogItems.Where(product => product.CatalogTypeId == typeId).ToListAsync();

    // Checking for result found
    if (products.Any())
    {
        logger.LogInformation($"Found {products.Count} products for type ID {typeId}.");
        return Results.Ok(products);
    }
    else
    {
        logger.LogWarning($"No products found for type ID {typeId}.");
        return Results.NotFound($"No products found for type ID {typeId}.");
    }
});


app.MapGet("/products/by/brand/{brandId}", async (CatalogDbContext dbContext, int brandId, ILogger<Program> logger) =>
{
    logger.LogInformation($"Fetching products by brand ID: {brandId}");

    // Running a query on db to check for brand/brandid
    var products = await dbContext.CatalogItems.Where(product => product.CatalogBrandId == brandId).ToListAsync();

    // Checking for result found
    if (products.Any())
    {
        logger.LogInformation($"Found {products.Count} products for brand ID {brandId}.");
        return Results.Ok(products);
    }
    else
    {
        logger.LogWarning($"No products found for brand ID {brandId}.");
        return Results.NotFound($"No products found for brand ID {brandId}.");
    }
});


app.MapGet("/catalogbrand", async (CatalogDbContext dbContext) =>
{
    //need to test and see if it works
    var catalogBrand = await dbContext.CatalogBrands.ToListAsync();
    return Results.Ok(catalogBrand);
});


app.MapGet("/catalogtype", async (CatalogDbContext dbContext) =>
{
    //need to test this and see if it works
    var catalogType = await dbContext.CatalogTypes.ToListAsync();
    return Results.Ok(catalogType);
});









DbInitializer.InitDb(app);

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}