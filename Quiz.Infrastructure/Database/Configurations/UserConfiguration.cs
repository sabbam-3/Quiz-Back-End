using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quiz.Domain.Users;

namespace Quiz.Infrastructure.Database.Configurations;

internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.Property(u => u.FirstName)
            .HasMaxLength(100);

        builder.HasIndex(u => u.IdentityId)
            .IsUnique();

        builder.Property(u => u.LastName)
            .HasMaxLength(100);

        builder.HasIndex(u => new { u.Email, u.IsDeleted })
             .IsUnique();

        builder.HasMany(u => u.Games)
            .WithOne(g => g.User)
            .HasForeignKey(g => g.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(u => !u.IsDeleted);
    }
}