using Domain.Entities.OrderModule;
using System.Reflection;

namespace Presistence.Data
{
    public class StoreDbContext : DbContext
    {
        public StoreDbContext(DbContextOptions<StoreDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Apply configurations from presistence [AssemblyReference]
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AssemblyReference).Assembly); 
        }

        #region DbSets

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<ProductBrand> ProductBrands { get; set; } 
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<DeliveryMethod> DeliveryMethods { get; set; }

        #endregion
    }
}
