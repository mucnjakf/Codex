using Codex.Application.Data;
using Codex.Application.Dtos;
using Codex.Application.Dtos.Mappers;
using Codex.Application.Dtos.Pagination;
using Codex.Application.Mediator;
using Codex.Domain.Entities;
using Codex.Domain.Outcomes;

namespace Codex.Application.Queries.Posts;

public sealed record GetPostsQuery(int PageNumber, int PageSize)
    : PaginationQueryDto(PageNumber, PageSize), IQuery<PaginationDto<PostDto>>;

internal sealed class GetPostsQueryHandler(
    IPostRepository postRepository)
    : IQueryHandler<GetPostsQuery, PaginationDto<PostDto>>
{
    public async Task<Result<PaginationDto<PostDto>>> Handle(
        GetPostsQuery query,
        CancellationToken cancellationToken)
    {
        (IReadOnlyList<Post> posts, int totalCount) = await postRepository
            .GetPaginatedAsync(query.PageNumber, query.PageSize, cancellationToken);

        return new PaginationDto<PostDto>(
            posts.Select(post => post.ToPostDto()).ToList(),
            query.PageNumber,
            query.PageSize,
            totalCount);
    }
}