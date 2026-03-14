using Codex.Application.Data;
using Codex.Infrastructure.EfCore;
using Codex.Infrastructure.EfCore.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Codex.Infrastructure;

public static class InfrastructureModule
{
    public static IServiceCollection AddInfrastructureModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("Default"));
        });

        services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<IAuthorRepository, AuthorEfCoreRepository>();
        services.AddScoped<ICategoryRepository, CategoryEfCoreRepository>();
        services.AddScoped<ICommentRepository, CommentEfCoreRepository>();
        services.AddScoped<IPostRepository, PostEfCoreRepository>();

        return services;
    }
}