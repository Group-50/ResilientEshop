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