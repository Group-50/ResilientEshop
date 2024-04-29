using CatalogService.Data;
using CatalogService.Dtos;
using CatalogService.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Endpoints;

public static class CatalogTypeApi
{
    public static IEndpointRouteBuilder MapCatalogTypeApi(this IEndpointRouteBuilder app)
    {
        app.MapGet("/catalogtypes", GetCatalogTypes);
        return app;
    }

    public static async Task<Ok<List<CatalogTypeDto>>> GetCatalogTypes(CatalogDbContext dbContext)
    {
        var catalogTypes = await dbContext.CatalogTypes.ToListAsync();

        var result = catalogTypes.Select(ct => ct.AsCatalogTypeDto()).ToList();

        return TypedResults.Ok(result);

    }
}