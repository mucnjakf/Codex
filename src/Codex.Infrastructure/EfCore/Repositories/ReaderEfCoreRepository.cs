using Codex.Application.Data;
using Codex.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Codex.Infrastructure.EfCore.Repositories;

internal sealed class ReaderEfCoreRepository(ApplicationDbContext dbContext) : IReaderRepository
{
    public async Task<(IReadOnlyList<Reader>, int)> GetPaginatedAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Reader> query = dbContext.Readers;

        int totalCount = await query.CountAsync(cancellationToken);

        List<Reader> readers = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (readers.AsReadOnly(), totalCount);
    }

    public async Task<Reader?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await dbContext.Readers.SingleOrDefaultAsync(reader => reader.Id == id, cancellationToken);

    public async Task CreateAsync(Reader reader, CancellationToken cancellationToken = default)
        => await dbContext.Readers.AddAsync(reader, cancellationToken);

    public void Delete(Reader reader)
        => dbContext.Readers.Remove(reader);

    public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await dbContext.Readers.AnyAsync(reader => reader.Id == id, cancellationToken);
}