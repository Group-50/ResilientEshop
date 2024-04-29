using CatalogService.Dtos;

namespace CatalogService.Models;

public class CatalogBrand
{
    public int Id { get; set; }
    
    public required string Brand { get; set; }

    public override string ToString()
    {
        return Brand;
    }
}

public static class CatalogBrandMappingExtensions
{
    public static CatalogBrandDto AsCatalogBrandDto(this CatalogBrand brand)
    {
        return new()
        {
            Id = brand.Id,
            BrandName = brand.Brand
        };
    }
}