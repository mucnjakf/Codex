using Codex.Application.Data;
using Codex.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Codex.Infrastructure.EfCore.Repositories;

internal sealed class CategoryEfCoreRepository(ApplicationDbContext dbContext) : ICategoryRepository
{
    public async Task<(IReadOnlyList<Category>, int)> GetPaginatedAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Category> query = dbContext.Categories;

        int totalCount = await query.CountAsync(cancellationToken);

        List<Category> categories = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (categories.AsReadOnly(), totalCount);
    }

    public async Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await dbContext.Categories.SingleOrDefaultAsync(category => category.Id == id, cancellationToken);

    public async Task CreateAsync(Category category, CancellationToken cancellationToken = default)
        => await dbContext.Categories.AddAsync(category, cancellationToken);

    public void Delete(Category category)
        => dbContext.Categories.Remove(category);

    public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await dbContext.Categories.AnyAsync(category => category.Id == id, cancellationToken);
}