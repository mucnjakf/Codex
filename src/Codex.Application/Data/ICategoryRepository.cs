using Codex.Domain.Entities;

namespace Codex.Application.Data;

public interface ICategoryRepository
{
    Task<(IReadOnlyList<Category>, int)> GetPaginatedAsNoTrackingAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<Category?> GetAsNoTrackingAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Category?> GetAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Category?> GetWithPostsAsync(Guid id, CancellationToken cancellationToken = default);

    Task CreateAsync(Category category, CancellationToken cancellationToken = default);

    Task DeleteAsync(Category category, CancellationToken cancellationToken = default);
}