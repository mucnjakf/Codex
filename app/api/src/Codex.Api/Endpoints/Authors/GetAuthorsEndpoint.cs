using Codex.Api.Configuration;
using Codex.Api.Extensions;
using Codex.Application.Dtos;
using Codex.Application.Dtos.Pagination;
using Codex.Application.Queries.Authors;
using Codex.Domain.Outcomes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Codex.Api.Endpoints.Authors;

internal sealed class GetAuthorsEndpoint : IEndpoint
{
    private sealed record Response(PaginationDto<AuthorDto> Data);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app
            .MapGet("api/authors", Handler)
            .WithName("GetAuthors")
            .WithTags("Authors");
    }

    private static async Task<IResult> Handler(
        [FromQuery] int pageNumber,
        [FromQuery] int pageSize,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        GetAuthorsQuery query = new(pageNumber, pageSize);

        Result<PaginationDto<AuthorDto>> result = await sender.Send(query, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(new Response(result.Value))
            : result.ToProblemDetails();
    }
}