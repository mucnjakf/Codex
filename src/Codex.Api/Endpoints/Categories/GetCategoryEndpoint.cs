using Codex.Api.Extensions;
using Codex.Application.Dtos;
using Codex.Application.Queries.Categories;
using Codex.Domain.Outcomes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Codex.Api.Endpoints.Categories;

internal sealed class GetCategoryEndpoint : IEndpoint
{
    private sealed record Response(CategoryDto Data);

    internal const string EndpointName = "GetCategory";

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app
            .MapGet("api/categories/{id:guid}", Handler)
            .WithName(EndpointName)
            .WithTags("Categories");
    }

    private static async Task<IResult> Handler(
        [FromRoute] Guid id,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        GetCategoryQuery query = new(id);

        Result<CategoryDto> result = await sender.Send(query, cancellationToken);

        Response response = new(result.Value);

        return result.IsSuccess
            ? Results.Ok(response)
            : result.ToProblemDetails();
    }
}