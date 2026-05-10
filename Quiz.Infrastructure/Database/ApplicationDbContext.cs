using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Quiz.Application.Abstractions.Repositories;
using Quiz.Domain.Games;
using Quiz.Domain.Quotes;
using Quiz.Domain.Roles;
using Quiz.Domain.Users;

namespace Quiz.Infrastructure.Database;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityUserContext<IdentityUser<string>, string>(options), IUnitOfWork
{
    public new DbSet<User> Users { get; init; }

    public DbSet<Role> Roles { get; init; }

    public DbSet<UserRole> UserRoles { get; init; }

    public DbSet<Quote> Quotes { get; init; }

    public DbSet<Game> Games { get; init; }

    public DbSet<GameQuestion> GameQuestions { get; init; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Ignore<IdentityUserClaim<string>>();

        builder.Ignore<IdentityRoleClaim<string>>();

        builder.Ignore<IdentityUserLogin<string>>();

        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
    {
        return await this.Database.BeginTransactionAsync(cancellationToken);
    }
}