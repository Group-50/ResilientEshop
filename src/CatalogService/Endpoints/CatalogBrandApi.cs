using CatalogService.Data;
using CatalogService.Dtos;
using CatalogService.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Endpoints;

public static class CatalogBrandApi
{
    public static IEndpointRouteBuilder MapCatalogBrandApi(this IEndpointRouteBuilder app)
    {
        app.MapGet("/catalogbrands", GetCatalogBrands);
        return app;
    }

    public static async Task<Ok<List<CatalogBrandDto>>> GetCatalogBrands(CatalogDbContext dbContext)
    {
        var catalogBrands = await dbContext.CatalogBrands.ToListAsync();

        var result = catalogBrands.Select(cb => cb.AsCatalogBrandDto()).ToList();

        return TypedResults.Ok(result);
    }
}