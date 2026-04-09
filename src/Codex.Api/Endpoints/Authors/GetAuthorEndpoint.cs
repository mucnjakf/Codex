using Codex.Api.Extensions;
using Codex.Application.Dtos;
using Codex.Application.Queries.Authors;
using Codex.Domain.Outcomes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Codex.Api.Endpoints.Authors;

internal sealed class GetAuthorEndpoint : IEndpoint
{
    private sealed record Response(AuthorDto Data);

    internal const string EndpointName = "GetAuthor";

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app
            .MapGet("api/authors/{id:guid}", Handler)
            .WithName(EndpointName)
            .WithTags("Authors");
    }

    private static async Task<IResult> Handler(
        [FromRoute] Guid id,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        GetAuthorQuery query = new(id);

        Result<AuthorDto> result = await sender.Send(query, cancellationToken);

        Response response = new(result.Value);

        return result.IsSuccess
            ? Results.Ok(response)
            : result.ToProblemDetails();
    }
}