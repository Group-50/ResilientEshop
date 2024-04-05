namespace CatalogService.Models;

public class CatalogItem
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string? ImageUrl { get; set; }
    
    public int CatalogTypeId { get; set; }

    public CatalogType CatalogType { get; set; } = null!;
    
    public int CatalogBrandId { get; set; }
    public CatalogBrand CatalogBrand { get; set; } = null!;
    
    public ICollection<CatalogItemPrice> PriceHistory { get; } = new List<CatalogItemPrice>();
    
    protected CatalogItem() {}

    public CatalogItem(string name, string description, string? imageUrl, int catalogTypeId, int catalogBrandId)
    {
        Name = name;
        Description = description;
        ImageUrl = imageUrl;
        CatalogTypeId = catalogTypeId;
        CatalogBrandId = catalogBrandId;

    }

    public void AddPriceChange(decimal newPrice, DateTime? effectiveFrom)
    {
        
    }
}