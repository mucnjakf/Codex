using Codex.Api.Extensions;
using Codex.Application.Dtos;
using Codex.Application.Queries.Readers;
using Codex.Domain.Outcomes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Codex.Api.Endpoints.Readers;

internal sealed class GetReaderEndpoint : IEndpoint
{
    private sealed record Response(ReaderDto Data);

    internal const string EndpointName = "GetReader";

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app
            .MapGet("api/readers/{id:guid}", Handler)
            .WithName(EndpointName)
            .WithTags("Readers");
    }

    private static async Task<IResult> Handler(
        [FromRoute] Guid id,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        GetReaderQuery query = new(id);

        Result<ReaderDto> result = await sender.Send(query, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(new Response(result.Value))
            : result.ToProblemDetails();
    }
}