using Codex.Domain.Entities;

namespace Codex.Application.Data;

public interface IPostRepository
{
    Task<(IReadOnlyList<Post>, int)> GetPaginatedAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<Post?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task CreateAsync(Post post, CancellationToken cancellationToken = default);

    Task DeleteAsync(Post post, CancellationToken cancellationToken = default);

    Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<bool> ExistsByAuthorIdAsync(Guid authorId, CancellationToken cancellationToken = default);

    Task<bool> ExistsByCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken = default);
}