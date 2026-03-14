using Codex.Domain.Entities;

namespace Codex.Application.Data;

public interface ICommentRepository
{
    Task<Comment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<bool> ExistsByPostIdAsync(Guid postId, CancellationToken cancellationToken = default);

    Task<bool> ExistsByReaderIdAsync(Guid readerId, CancellationToken cancellationToken = default);
}