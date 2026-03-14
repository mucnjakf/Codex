using Codex.Application.Data;
using Codex.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Codex.Infrastructure.EfCore.Repositories;

internal sealed class AuthorEfCoreRepository(ApplicationDbContext dbContext) : IAuthorRepository
{
    public async Task<(IReadOnlyList<Author>, int)> GetPaginatedAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Author> query = dbContext.Authors;

        int totalCount = await query.CountAsync(cancellationToken);

        List<Author> authors = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (authors.AsReadOnly(), totalCount);
    }

    public async Task<Author?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await dbContext.Authors.SingleOrDefaultAsync(author => author.Id == id, cancellationToken);

    public async Task CreateAsync(Author author, CancellationToken cancellationToken = default)
        => await dbContext.Authors.AddAsync(author, cancellationToken);

    public void Delete(Author author)
        => dbContext.Authors.Remove(author);

    public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await dbContext.Authors.AnyAsync(author => author.Id == id, cancellationToken);
}