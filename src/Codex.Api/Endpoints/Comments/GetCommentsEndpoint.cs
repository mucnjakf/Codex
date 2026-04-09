using Codex.Api.Extensions;
using Codex.Application.Dtos;
using Codex.Application.Dtos.Pagination;
using Codex.Application.Queries.Comments;
using Codex.Domain.Outcomes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Codex.Api.Endpoints.Comments;

internal sealed class GetCommentsEndpoint : IEndpoint
{
    private sealed record Response(PaginationDto<CommentDto> Data);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app
            .MapGet("api/comments", Handler)
            .WithName("GetComments")
            .WithTags("Comments");
    }

    private static async Task<IResult> Handler(
        [FromQuery] int pageNumber,
        [FromQuery] int pageSize,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        GetCommentsQuery query = new(pageNumber, pageSize);

        Result<PaginationDto<CommentDto>> result = await sender.Send(query, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(new Response(result.Value))
            : result.ToProblemDetails();
    }
}