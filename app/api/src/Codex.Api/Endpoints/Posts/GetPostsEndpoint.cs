using Codex.Api.Configuration;
using Codex.Api.Extensions;
using Codex.Application.Dtos;
using Codex.Application.Dtos.Pagination;
using Codex.Application.Queries.Posts;
using Codex.Domain.Outcomes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Codex.Api.Endpoints.Posts;

internal sealed class GetPostsEndpoint : IEndpoint
{
    private sealed record Response(PaginationDto<PostDto> Data);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app
            .MapGet("api/posts", Handler)
            .WithName("GetPosts")
            .WithTags("Posts");
    }

    private static async Task<IResult> Handler(
        [FromQuery] int pageNumber,
        [FromQuery] int pageSize,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        GetPostsQuery query = new(pageNumber, pageSize);

        Result<PaginationDto<PostDto>> result = await sender.Send(query, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(new Response(result.Value))
            : result.ToProblemDetails();
    }
}