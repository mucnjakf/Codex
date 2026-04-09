using Codex.Api.Extensions;
using Codex.Application.Dtos;
using Codex.Application.Dtos.Pagination;
using Codex.Application.Queries.Readers;
using Codex.Domain.Outcomes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Codex.Api.Endpoints.Readers;

internal sealed class GetAuthorsEndpoint : IEndpoint
{
    private sealed record Response(PaginationDto<ReaderDto> Data);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app
            .MapGet("api/readers", Handler)
            .WithName("GetReaders")
            .WithTags("Readers");
    }

    private static async Task<IResult> Handler(
        [FromQuery] int pageNumber,
        [FromQuery] int pageSize,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        GetReadersQuery query = new(pageNumber, pageSize);

        Result<PaginationDto<ReaderDto>> result = await sender.Send(query, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(new Response(result.Value))
            : result.ToProblemDetails();
    }
}