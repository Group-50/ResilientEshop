namespace CatalogService.Dtos;

public class ProductDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public string? ImageUrl { get; set; }
    public required string Type { get; set; }
    public required string Brand { get; set; }
    // public decimal Price { get; set; }
}