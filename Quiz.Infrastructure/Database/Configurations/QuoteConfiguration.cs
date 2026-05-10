using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quiz.Domain.Quotes;

namespace Quiz.Infrastructure.Database.Configurations;

internal sealed class QuoteConfiguration : IEntityTypeConfiguration<Quote>
{
    public void Configure(EntityTypeBuilder<Quote> builder)
    {
        builder.Property(q => q.Content)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(q => q.AuthorName)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(q => new { q.AuthorName, q.Content, q.IsDeleted })
            .IsUnique();

        builder.HasQueryFilter(q => !q.IsDeleted);
    }
}