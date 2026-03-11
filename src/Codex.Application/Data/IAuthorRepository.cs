using Codex.Domain.Entities;

namespace Codex.Application.Data;

public interface IAuthorRepository
{
    Task<(IReadOnlyList<Author>, int)> GetPaginatedAsNoTrackingAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<Author?> GetAsNoTrackingAsync(Guid id, CancellationToken cancellationToken = default);

    Task CreateAsync(Author author, CancellationToken cancellationToken = default);
}