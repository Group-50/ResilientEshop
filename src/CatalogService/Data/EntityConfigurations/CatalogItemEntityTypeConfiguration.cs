using CatalogService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatalogService.Data.EntityConfigurations;

public class CatalogItemEntityTypeConfiguration
    : IEntityTypeConfiguration<CatalogItem>
{
    public void Configure(EntityTypeBuilder<CatalogItem> builder)
    {
        builder.ToTable("catalog");

        builder.Property(ci => ci.Name)
            .HasMaxLength(80);

        builder.HasOne(ci => ci.CatalogType)
            .WithMany();

        builder.HasOne(ci => ci.CatalogBrand)
            .WithMany();

        builder.HasMany(ci => ci.PriceHistory)
            .WithOne()
            .HasForeignKey(p => p.ProductId)
            .IsRequired();

    }
}