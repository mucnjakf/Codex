namespace Codex.Application.Data;

public interface ICommentRepository
{
    Task<bool> ExistsByPostIdAsync(Guid postId, CancellationToken cancellationToken = default);

    Task<bool> ExistsByReaderIdAsync(Guid readerId, CancellationToken cancellationToken = default);
}