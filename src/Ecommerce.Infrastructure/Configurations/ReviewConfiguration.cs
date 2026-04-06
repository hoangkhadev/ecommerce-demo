using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Infrastructure.Configurations
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.ToTable("reviews");
            builder.HasKey(x => x.Id);



            builder.HasOne(x => x.Product)
                .WithMany(p => p.Reviews)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.User)
             .WithMany()
             .HasForeignKey(x => x.UserId);

            builder.HasOne(x => x.Order)
              .WithMany()
              .HasForeignKey(x => x.OrderId);

            builder.HasQueryFilter(x => x.Product.DeletedAt == null);
        }
    }
}
