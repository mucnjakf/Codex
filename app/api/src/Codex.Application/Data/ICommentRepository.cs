using Codex.Domain.Entities;

namespace Codex.Application.Data;

public interface ICommentRepository : IBaseRepository
{
    Task<(IReadOnlyList<Comment>, int)> GetPaginatedAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<Comment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task CreateAsync(Comment comment, CancellationToken cancellationToken = default);

    void Delete(Comment comment);

    Task<bool> ExistsByPostIdAsync(Guid postId, CancellationToken cancellationToken = default);

    Task<bool> ExistsByReaderIdAsync(Guid readerId, CancellationToken cancellationToken = default);
}