namespace Codex.Application.Data;

public interface ICommentRepository
{
    Task<bool> ExistsByPostIdAsync(Guid postId, CancellationToken cancellationToken = default);
}