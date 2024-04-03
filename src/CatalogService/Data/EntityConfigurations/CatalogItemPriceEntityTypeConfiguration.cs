using CatalogService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatalogService.Data.EntityConfigurations;

public class CatalogItemPriceEntityTypeConfiguration
    : IEntityTypeConfiguration<CatalogItemPrice>
{
    public void Configure(EntityTypeBuilder<CatalogItemPrice> builder)
    {
        builder.ToTable("catalog_item_price");

        builder.Property(p => p.Price)
            .HasColumnType("money");
    }
}