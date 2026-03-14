using Codex.Application.Data;
using Codex.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Codex.Infrastructure.EfCore.Repositories;

internal sealed class PostEfCoreRepository(ApplicationDbContext dbContext) : IPostRepository
{
    public async Task<(IReadOnlyList<Post>, int)> GetPaginatedAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Post> query = dbContext.Posts;

        int totalCount = await query.CountAsync(cancellationToken);

        List<Post> posts = await query
            .Include(post => post.Author)
            .Include(post => post.Category)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (posts.AsReadOnly(), totalCount);
    }

    public async Task<Post?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await dbContext.Posts
            .Include(post => post.Author)
            .Include(post => post.Category)
            .SingleOrDefaultAsync(post => post.Id == id, cancellationToken);

    public async Task CreateAsync(Post post, CancellationToken cancellationToken = default)
        => await dbContext.Posts.AddAsync(post, cancellationToken);

    public void Delete(Post post)
        => dbContext.Posts.Remove(post);

    public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await dbContext.Posts.AnyAsync(post => post.Id == id, cancellationToken);

    public async Task<bool> ExistsByAuthorIdAsync(Guid authorId, CancellationToken cancellationToken = default)
        => await dbContext.Posts.AnyAsync(post => post.AuthorId == authorId, cancellationToken);

    public async Task<bool> ExistsByCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken = default)
        => await dbContext.Posts.AnyAsync(post => post.CategoryId == categoryId, cancellationToken);
}