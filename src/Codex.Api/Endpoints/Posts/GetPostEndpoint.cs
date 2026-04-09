using Codex.Api.Extensions;
using Codex.Application.Dtos;
using Codex.Application.Queries.Posts;
using Codex.Domain.Outcomes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Codex.Api.Endpoints.Posts;

internal sealed class GetPostEndpoint : IEndpoint
{
    private sealed record Response(PostDto Data);

    internal const string EndpointName = "GetPost";

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app
            .MapGet("api/posts/{id:guid}", Handler)
            .WithName(EndpointName)
            .WithTags("Posts");
    }

    private static async Task<IResult> Handler(
        [FromRoute] Guid id,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        GetPostQuery query = new(id);

        Result<PostDto> result = await sender.Send(query, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(new Response(result.Value))
            : result.ToProblemDetails();
    }
}