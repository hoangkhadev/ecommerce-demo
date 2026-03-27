using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Ecommerce.Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Table name
            builder.ToTable("users");

            // Primary key
            builder.HasKey(u => u.Id);

            // Properties
            builder.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(150);

            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.Property(builder => builder.PasswordHash)
                .IsRequired();

            builder.Property(u => u.Role)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue(UserRole.User.ToString());

            builder.HasData(new User
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Username = "admin",
                Role = UserRole.Admin.ToString(),
                Email = "admin@gmail.com",
                PasswordHash = "$2a$11$LRd5H.ft39KbYuNrSilGquI0CydD7pHfftTqyqRb7IyZ3C1G/XjeW"
            });
        }
    }
}
