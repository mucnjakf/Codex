using Codex.Infrastructure.EfCore;
using Microsoft.EntityFrameworkCore;

namespace Codex.Api.Extensions;

internal static class DatabaseExtensions
{
    internal static void ApplyMigrations(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.Migrate();
    }
}