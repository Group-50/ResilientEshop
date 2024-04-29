using CatalogService.Data;
using CatalogService.Dtos;
using CatalogService.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Endpoints;

public static class ProductApi
{
    public static IEndpointRouteBuilder MapProductApi(this IEndpointRouteBuilder app)
    {
        app.MapGet("/products", GetAllProducts);
        app.MapGet("products/{id:int}", GetProductById);
        return app;
    }

    public static async Task<Results<Ok<List<ProductDto>>, BadRequest<string>>> GetAllProducts(
        CatalogDbContext dbContext)
    {
        var catalogItems = await dbContext.CatalogItems.Include(ci => ci.CatalogBrand)
            .Include(ci => ci.CatalogType).ToListAsync();
        if (catalogItems.Count < 1)
        {
            return TypedResults.BadRequest("Could not find any products.");
        }
        var products = catalogItems.Select(item => item.AsProductDto()).ToList();
        return TypedResults.Ok(products);
    }

    public static async Task<Results<Ok<ProductDto>, NotFound, BadRequest<string>>> GetProductById(
        CatalogDbContext dbContext, int id)
    {
        if (id <= 0)
        {
            return TypedResults.BadRequest("Invalid Product Id");
        }
        var product = await dbContext.CatalogItems.Include(ci => ci.CatalogBrand)
            .Include(ci => ci.CatalogType)
            .SingleOrDefaultAsync(ci => ci.Id == id);
        if (product is null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(product.AsProductDto());
    }
}