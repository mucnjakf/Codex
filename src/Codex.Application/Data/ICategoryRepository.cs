using Codex.Domain.Entities;

namespace Codex.Application.Data;

public interface ICategoryRepository : IBaseRepository
{
    Task<(IReadOnlyList<Category>, int)> GetPaginatedAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task CreateAsync(Category category, CancellationToken cancellationToken = default);

    void Delete(Category category);

    Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken = default);
}