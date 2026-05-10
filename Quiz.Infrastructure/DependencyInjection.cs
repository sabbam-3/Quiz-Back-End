using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quiz.Application.Abstractions.Authentication;
using Quiz.Application.Abstractions.Repositories;
using Quiz.Infrastructure.Authentication;
using Quiz.Infrastructure.Configurations;
using Quiz.Infrastructure.Database;
using Quiz.Infrastructure.Repositories;

namespace Quiz.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("QuizDb")));

        services.AddIdentityCore<IdentityUser<string>>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.AddHttpContextAccessor();
        services.AddScoped<IUserContext, UserContext>();
        services.AddScoped<ITokenManager, TokenManager>();
        services.AddScoped<IIdentityProviderService, IdentityProviderService>();

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserRoleRepository, UserRoleRepository>();
        services.AddScoped<IQuoteRepository, QuoteRepository>();
        services.AddScoped<IGameRepository, GameRepository>();

        services.Configure<JwtConfiguration>(configuration.GetSection("Jwt"));

        return services;
    }
}