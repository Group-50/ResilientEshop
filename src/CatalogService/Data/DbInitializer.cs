using CatalogService.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Data;

public class DbInitializer
{
    public static void InitDb(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        {
            SeedData(scope.ServiceProvider.GetService<CatalogDbContext>());
        }
    }
    //TODO: Refactor
    private static void SeedData(CatalogDbContext context)
    {
        context.Database.Migrate();

        if (context.CatalogItems.Any())
        {
            Console.WriteLine("Data already in DB - No seeding required");
        }

        var catalogBrands = new List<CatalogBrand>()
        {
            new CatalogBrand
            {
                Brand = "Razer"
            },
            new CatalogBrand
            {
                Brand = "Ducky"
            },
            new CatalogBrand
            {
                Brand = "Keychron"
            },
            new CatalogBrand
            {
                Brand = "Logitech"
            }
        };

        var catalogTypes = new List<CatalogType>()
        {
            new CatalogType
            {
                Type = "Keyboard"
            },
            new CatalogType
            {
                Type = "Mouse"
            }
        };

        var catalogItems = new List<CatalogItem>()
        {
            new CatalogItem("Razer BlackWidow", "A keyboard",null, 1, 1),
            new CatalogItem("Razer DeathStalker", "A keyboard", null, 1, 1),
            new CatalogItem("Razer Basilisk V3", "A mouse", null, 2, 1),
            
            new CatalogItem("Ducky One 2", "A keyboard", null, 1, 2),
            new CatalogItem("Ducky Feather", "A mouse", null, 2, 2),
            
            new CatalogItem("Keychron C1", "A keyboard", null, 1, 3),
            new CatalogItem("Keychron K1 Pro", "A keyboard", null, 1, 3),
            new CatalogItem("Keychron K10", "A keyboard", null, 1, 3),
            
            new CatalogItem("Logitech G PRO X TKL", "A keyboard", null, 1, 4),
            new CatalogItem("Logitech G203", "A mouse", null, 2, 4),
            new CatalogItem("Logitech G502", "A mouse", null, 2, 4),
            new CatalogItem("Logitech G Pro Wireless", "A mouse", null, 1, 4),
            new CatalogItem("Logitech G305", "A mouse", null, 2, 4)
        };

        var itemPrices = new List<CatalogItemPrice>()
        {
            CatalogItemPrice.Create(1, 269, DateTime.UtcNow),
            CatalogItemPrice.Create(2, 249, DateTime.UtcNow),
            CatalogItemPrice.Create(3, 89, DateTime.UtcNow),
            CatalogItemPrice.Create(4, 119, DateTime.UtcNow),
            CatalogItemPrice.Create(5, 89, DateTime.UtcNow),
            CatalogItemPrice.Create(6, 99, DateTime.UtcNow),
            CatalogItemPrice.Create(7, 139, DateTime.UtcNow),
            CatalogItemPrice.Create(8, 179, DateTime.UtcNow),
            CatalogItemPrice.Create(9, 269, DateTime.UtcNow),
            CatalogItemPrice.Create(10, 49, DateTime.UtcNow),
            CatalogItemPrice.Create(10, 55, DateTime.UtcNow.AddDays(4)),
            CatalogItemPrice.Create(11, 99, DateTime.UtcNow),
            CatalogItemPrice.Create(12, 199, DateTime.UtcNow),
            CatalogItemPrice.Create(13, 209, DateTime.UtcNow.AddDays(-4)),
            CatalogItemPrice.Create(13, 55, DateTime.UtcNow)

        };
        
        context.AddRange(catalogBrands);
        context.AddRange(catalogTypes);
        context.SaveChanges();
        context.AddRange(catalogItems);
        context.SaveChanges();
        context.AddRange(itemPrices);
        context.SaveChanges();
    }
}