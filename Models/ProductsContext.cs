
using Microsoft.EntityFrameworkCore;

namespace ProductsAPI.Models
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           modelBuilder.Entity<Product>().HasData(new Product() { ProductId=1, ProductName="IPhone 14", Price=60000,IsActive=true });
      modelBuilder.Entity<Product>().HasData(new Product() { ProductId=2, ProductName="IPhone 15", Price=60000,IsActive=false });
      modelBuilder.Entity<Product>().HasData(new Product() { ProductId=3, ProductName="IPhone 16", Price=60000,IsActive=true });
      
      
        }

        public DbSet<Product> Products { get; set; }
    }
}
