using Codex.Api.Extensions;
using Codex.Application.Dtos;
using Codex.Application.Queries.Comments;
using Codex.Domain.Outcomes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Codex.Api.Endpoints.Comments;

internal sealed class GetCommentEndpoint : IEndpoint
{
    private sealed record Response(CommentDto Data);

    internal const string EndpointName = "GetComment";

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app
            .MapGet("api/comments/{id:guid}", Handler)
            .WithName(EndpointName)
            .WithTags("Comments");
    }

    private static async Task<IResult> Handler(
        [FromRoute] Guid id,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        GetCommentQuery query = new(id);

        Result<CommentDto> result = await sender.Send(query, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(new Response(result.Value))
            : result.ToProblemDetails();
    }
}