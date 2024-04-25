using CatalogService.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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

app.MapGet("/products", async (CatalogDbContext dbContext) =>
{
    var products = await dbContext.CatalogItems.ToListAsync();
    return Results.Ok(products);
});

app.MapGet("/products/by", async (CatalogDbContext dbContext, string ids) =>
{
    if (string.IsNullOrEmpty(ids))
    {
        return Results.BadRequest("No IDs provided");
    }
    // Converting string to int
    var idList = ids.Split(',').Select(id => int.Parse(id.Trim())).ToList();

    // running a qeury on the db with the int
    var products = await dbContext.CatalogItems
                                  .Where(product => idList.Contains(product.Id))
                                  .ToListAsync();
    return products.Any() ? Results.Ok(products) : Results.NotFound("No products found with the specified IDs");


});

app.MapGet("/products/by/{prodId}", async (CatalogDbContext dbContext, int prodId) =>
{
    //running a query on db to check for prodId
    var product = await dbContext.CatalogItems.FirstOrDefaultAsync(p => p.Id == prodId);
    // checking for result found
    return product != null ? Results.Ok(product) : Results.NotFound($"Product with ID {prodId} not found.");

});

app.MapGet("/products/by/type/{typeId}", async (CatalogDbContext dbContext, int typeId) =>
{
    //running a query on db to check for type/typeid
    var products = await dbContext.CatalogItems.Where(product => product.CatalogTypeId == typeId).ToListAsync();

    // checking for result found
    return products.Any() ? Results.Ok(products) : Results.NotFound($"No products found for type ID {typeId}.");

});

app.MapGet("/products/by/brand/{brandId}", async (CatalogDbContext dbContext, int brandId) =>
{
    //running a query on db to check for brand/brandid
    var products = await dbContext.CatalogItems.Where(product => product.CatalogBrandId == brandId).ToListAsync();

    // Checking for result found
    return products.Any() ? Results.Ok(products) : Results.NotFound($"No products found for brand ID {brandId}.");

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