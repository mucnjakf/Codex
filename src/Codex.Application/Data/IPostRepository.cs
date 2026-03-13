using Codex.Domain.Entities;

namespace Codex.Application.Data;

public interface IPostRepository
{
    Task<Post?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<bool> ExistsByAuthorIdAsync(Guid authorId, CancellationToken cancellationToken = default);

    Task<bool> ExistsByCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken = default);
}