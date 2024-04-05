namespace CatalogService.Models;

public class CatalogItemPrice
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public decimal Price { get; set; }
    public DateTime EffectiveFrom { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    // public string CreatedBy { get; set; }
    
    protected CatalogItemPrice(){}

    protected CatalogItemPrice(int productId, decimal price, DateTime? effectiveFrom)
    {
        ProductId = productId;
        Price = price;
        EffectiveFrom = effectiveFrom ?? DateTime.UtcNow;
    }

    public static CatalogItemPrice Create(int productId, decimal price, DateTime? effectiveFrom = null)
    {
        return new CatalogItemPrice(productId, price, effectiveFrom);
    }
}