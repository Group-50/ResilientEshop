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
        app.MapGet("/products/by", GetProductsByIds);
        app.MapGet("/products/{id:int}", GetProductById);
        app.MapGet("/products/type/{typeId:int}", GetProductsByTypeId);
        app.MapGet("/products/type/all/brand/{brandId:int}", GetProductsByBrandId);
        return app;
    }

    public static async Task<Results<Ok<List<ProductDto>>, NotFound<string>>> GetAllProducts(
        CatalogDbContext dbContext)
    {
        var catalogItems = await dbContext.CatalogItems.Include(ci => ci.CatalogBrand)
            .Include(ci => ci.CatalogType).ToListAsync();
        if (catalogItems.Count < 1)
        {
            return TypedResults.NotFound("Unable to find the specified products.");
        }
        var products = catalogItems.Select(item => item.AsProductDto()).ToList();
        return TypedResults.Ok(products);
    }

    public static async Task<Results<Ok<List<ProductDto>>, NotFound<string>>> GetProductsByIds(
        CatalogDbContext dbContext, int[] ids)
    {
        var catalogItems = await dbContext.CatalogItems
            .Include(ci => ci.CatalogType)
            .Include(ci => ci.CatalogBrand)
            .Where(ci => ids.Contains(ci.Id))
            .ToListAsync();
        if (catalogItems.Count == 1)
        {
            return TypedResults.NotFound("Could not find the specified items.");
        }

        var products = catalogItems.Select(ci => ci.AsProductDto()).ToList();
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

    public static async Task<Results<Ok<List<ProductDto>>, NotFound<string>, BadRequest<string>>> GetProductsByTypeId(
        CatalogDbContext dbContext, int typeId)
    {
        if (typeId <= 0)
        {
            return TypedResults.BadRequest("Catalog Type Id is invalid");
        }

        var catalogItems = await dbContext.CatalogItems
            .Include(ci => ci.CatalogType)
            .Include(ci => ci.CatalogBrand)
            .Where(ci => ci.CatalogTypeId == typeId)
            .ToListAsync();
        if (catalogItems.Count == 0)
        {
            return TypedResults.NotFound("Could not find any products for the specified type.");
        }
        //TODO: Refactor this
        var products = catalogItems.Select(ci => ci.AsProductDto()).ToList();
        return TypedResults.Ok(products);
    }

    public static async Task<Results<Ok<List<ProductDto>>, NotFound<string>, BadRequest<string>>> GetProductsByBrandId(
        CatalogDbContext dbContext, int brandId)
    {
        if (brandId <= 0)
        {
            return TypedResults.BadRequest("Brand Id is invalid");
        }

        var catalogItems = await dbContext.CatalogItems
            .Include(ci => ci.CatalogType)
            .Include(ci => ci.CatalogBrand)
            .Where(ci => ci.CatalogBrandId == brandId)
            .ToListAsync();
        if (catalogItems.Count <= 0)
        {
            return TypedResults.NotFound("Could not find any products for the specified brand.");
        }

        var products = catalogItems.Select(ci => ci.AsProductDto()).ToList();
        return TypedResults.Ok(products);
    }
}