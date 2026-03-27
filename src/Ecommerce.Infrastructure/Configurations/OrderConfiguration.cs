using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Infrastructure.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            // Table name
            builder.ToTable("orders");

            // Primary key
            builder.HasKey(o => o.Id);

            // Properties
            builder.Property(o => o.UserId)
                .IsRequired();

            builder.Property(o => o.TotalAmount)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(o => o.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(o => o.Status)
                .HasDefaultValue(OrderStatus.Pending.ToString());
        }
    }
}
