using LearnSmartCoding.EssentialProducts.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnSmartCoding.EssentialProducts.Data
{
    public class EssentialProductsDbContext : DbContext
    {
        public EssentialProductsDbContext(DbContextOptions<EssentialProductsDbContext> options)
            : base(options)
        {

        }

        public DbSet<Category> Category { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductImage> ProductImage { get; set; }
        public DbSet<ProductOwner> ProductOwner { get; set; }
        public DbSet<WishlistItem> WishlistItem { get; set; }
    }
}
