using System;
using System.Collections.Generic;

namespace Avalonia.Demo.Models
{
    public partial class Product
    {
        public Product()
        {
            ProductPhotos = new HashSet<ProductPhoto>();
            ProductSales = new HashSet<ProductSale>();
            AttachedProducts = new HashSet<Product>();
            MainProducts = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public decimal Cost { get; set; }
        public string? Description { get; set; }
        public string? MainImagePath { get; set; }
        public bool IsActive { get; set; }
        public int? ManufacturerId { get; set; }

        public virtual Manufacturer? Manufacturer { get; set; }
        public virtual ICollection<ProductPhoto> ProductPhotos { get; set; }
        public virtual ICollection<ProductSale> ProductSales { get; set; }

        public virtual ICollection<Product> AttachedProducts { get; set; }
        public virtual ICollection<Product> MainProducts { get; set; }
    }
}