using Codex.Domain.Entities;

namespace Codex.Application.Data;

public interface IAuthorRepository
{
    Task<(IReadOnlyList<Author>, int)> GetPaginatedAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<Author?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task CreateAsync(Author author, CancellationToken cancellationToken = default);

    Task DeleteAsync(Author author, CancellationToken cancellationToken = default);
}