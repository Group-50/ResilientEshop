using CatalogService.Dtos;

namespace CatalogService.Models;

public class CatalogType
{
    public int Id { get; set; }
    public required string Type { get; set; }

    public override string ToString()
    {
        return Type;
    }
}

public static class CatalogTypeMappingExtensions
{
    public static CatalogTypeDto AsCatalogTypeDto(this CatalogType type)
    {
        return new()
        {
            Id = type.Id,
            TypeName = type.Type
        };
    }
}