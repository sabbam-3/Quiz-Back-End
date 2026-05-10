using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Quiz.Infrastructure.Database.Configurations;

internal sealed class IdentityConfiguration : IEntityTypeConfiguration<IdentityUser<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUser<string>> builder)
    {
        builder.ToTable("UserIdentities", "Identity");

        builder.Ignore(u => u.EmailConfirmed);
        builder.Ignore(u => u.PhoneNumber);
        builder.Ignore(u => u.PhoneNumberConfirmed);
        builder.Ignore(u => u.TwoFactorEnabled);
        builder.Ignore(u => u.LockoutEnd);
        builder.Ignore(u => u.LockoutEnabled);
        builder.Ignore(u => u.AccessFailedCount);
    }
}