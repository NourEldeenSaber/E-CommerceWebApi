using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Presistence.Data.Configurations
{
    internal class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {

            builder.Property(p => p.Price)
                    .HasColumnType("decimal(15,2)");
            
            #region Relations

            builder.HasOne(P => P.ProductBrand)
                        .WithMany()
                        .HasForeignKey(fk => fk.BrandId);

            builder.HasOne(P => P.ProductType)
                    .WithMany()
                    .HasForeignKey(fk => fk.TypeId); 

            #endregion
        }
    }
}
