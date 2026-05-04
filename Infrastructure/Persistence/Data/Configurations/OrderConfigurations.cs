using Domain.Entities.OrderModule;
using Domain.Entities.OrderModule.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Presistence.Data.Configurations
{
    internal class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(p => p.ShippingAddress, sh => sh.WithOwner());

            // Order 1 => M OrderItems
            builder.HasMany(o => o.OrderItems)
                .WithOne();

            builder.Property(o => o.PaymentStatus).HasConversion(
                ps => ps.ToString(), ps => Enum.Parse<OrderPaymentStatus>(ps));

            // Order 1 => M DeliveryMethod
            builder.HasOne(o => o.DeliveryMethod)
                .WithMany()
                .OnDelete(DeleteBehavior.SetNull);

            builder.Property(p => p.SubTotal).HasColumnType("decimal(18,4)");
               
        }
    }
}
