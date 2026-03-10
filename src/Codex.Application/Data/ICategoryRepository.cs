using Codex.Domain.Entities;

namespace Codex.Application.Data;

public interface ICategoryRepository
{
    Task<(IReadOnlyList<Category>, int)> GetPaginatedAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<Category?> GetAsync(Guid id, CancellationToken cancellationToken = default);
}