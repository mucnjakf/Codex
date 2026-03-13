using Codex.Application.Data;
using Codex.Application.Dtos;
using Codex.Application.Dtos.Mappers;
using Codex.Application.Mediator;
using Codex.Domain.Entities;
using Codex.Domain.Errors;
using Codex.Domain.Outcomes;

namespace Codex.Application.Queries.Posts;

public sealed record GetPostQuery(Guid Id) : IQuery<PostDto>;

internal sealed class GetPostQueryHandler(
    IPostRepository postRepository)
    : IQueryHandler<GetPostQuery, PostDto>
{
    public async Task<Result<PostDto>> Handle(GetPostQuery query, CancellationToken cancellationToken)
    {
        Post? post = await postRepository.GetByIdAsync(query.Id, cancellationToken);

        if (post is null)
        {
            return Result.Failure<PostDto>(PostErrors.NotFound);
        }

        var postDto = post.ToPostDto();

        return Result.Success(postDto);
    }
}