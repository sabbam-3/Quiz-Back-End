using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quiz.Domain.Games;

namespace Quiz.Infrastructure.Database.Configurations;

internal sealed class GameConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder.Property(g => g.Mode)
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(g => g.Status)
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.HasMany(g => g.Questions)
            .WithOne(gq => gq.Game)
            .HasForeignKey(gq => gq.GameId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(g => !g.IsDeleted);
    }
}