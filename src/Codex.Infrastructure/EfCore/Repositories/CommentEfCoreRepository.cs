using Codex.Application.Data;
using Codex.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Codex.Infrastructure.EfCore.Repositories;

internal sealed class CommentEfCoreRepository(ApplicationDbContext dbContext) : ICommentRepository
{
    public async Task<(IReadOnlyList<Comment>, int)> GetPaginatedAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Comment> query = dbContext.Comments;

        int totalCount = await query.CountAsync(cancellationToken);

        List<Comment> comments = await query
            .Include(comment => comment.Post)
            .ThenInclude(post => post.Author)
            .Include(comment => comment.Post)
            .ThenInclude(post => post.Category)
            .Include(comment => comment.Reader)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (comments.AsReadOnly(), totalCount);
    }

    public async Task<Comment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await dbContext.Comments
            .Include(comment => comment.Post)
            .ThenInclude(post => post.Author)
            .Include(comment => comment.Post)
            .ThenInclude(post => post.Category)
            .Include(comment => comment.Reader)
            .SingleOrDefaultAsync(comment => comment.Id == id, cancellationToken);

    public async Task CreateAsync(Comment comment, CancellationToken cancellationToken = default)
        => await dbContext.Comments.AddAsync(comment, cancellationToken);

    public void Delete(Comment comment)
        => dbContext.Comments.Remove(comment);

    public async Task<bool> ExistsByPostIdAsync(Guid postId, CancellationToken cancellationToken = default)
        => await dbContext.Comments.AnyAsync(comment => comment.PostId == postId, cancellationToken);

    public async Task<bool> ExistsByReaderIdAsync(Guid readerId, CancellationToken cancellationToken = default)
        => await dbContext.Comments.AnyAsync(comment => comment.ReaderId == readerId, cancellationToken);
}