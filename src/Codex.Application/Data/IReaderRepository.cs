using Codex.Domain.Entities;

namespace Codex.Application.Data;

public interface IReaderRepository
{
    Task<(IReadOnlyList<Reader>, int)> GetPaginatedAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<Reader?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task CreateAsync(Reader reader, CancellationToken cancellationToken = default);

    Task DeleteAsync(Reader reader, CancellationToken cancellationToken = default);

    Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken = default);
}