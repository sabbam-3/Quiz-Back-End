using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quiz.Domain.Games;

namespace Quiz.Infrastructure.Database.Configurations;

internal sealed class GameQuestionConfiguration : IEntityTypeConfiguration<GameQuestion>
{
    public void Configure(EntityTypeBuilder<GameQuestion> builder)
    {
        builder.Property(gq => gq.AnswerGiven)
            .HasMaxLength(200);

        builder.HasOne(gq => gq.Quote)
            .WithMany()
            .HasForeignKey(gq => gq.QuoteId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(gq => !gq.IsDeleted);
    }
}