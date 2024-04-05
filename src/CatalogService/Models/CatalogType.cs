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