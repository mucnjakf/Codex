using Codex.Domain.Entities;

namespace Codex.Application.Data;

public interface IAuthorRepository : IBaseRepository
{
    Task<(IReadOnlyList<Author>, int)> GetPaginatedAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<Author?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task CreateAsync(Author author, CancellationToken cancellationToken = default);

    void Delete(Author author);

    Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken = default);
}