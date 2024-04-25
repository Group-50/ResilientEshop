using System;
using CatalogService.Models;

namespace CatalogService.Models
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string Type { get; set; }
        public string Brand { get; set; }
        //public decimal Price { get; set; }
    }
}