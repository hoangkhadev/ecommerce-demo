using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Infrastructure.Configurations
{
    public class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            // Table name
            builder.ToTable("carts");

            // Primary key
            builder.HasKey(c => c.Id);

            // Properties
            builder.Property(c => c.UserId)
                .IsRequired();

            builder.HasIndex(c => c.UserId)
                .IsUnique();

            builder.HasOne<User>()
                .WithOne(u => u.Cart)
                .HasForeignKey<Cart>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
