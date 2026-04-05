using Codex.Api.Extensions;
using Codex.Application.Dtos;
using Codex.Application.Queries.Categories;
using Codex.Domain.Outcomes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Codex.Api.Endpoints.CategoryEndpoints;

internal sealed class GetCategoryEndpoint : IEndpoint
{
    private sealed record Response(CategoryDto Category);

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
        var query = new GetCategoryQuery(id);

        Result<CategoryDto> result = await sender.Send(query, cancellationToken);

        var response = new Response(result.Value);

        return result.IsSuccess
            ? Results.Ok(response)
            : result.ToProblemDetails();
    }
}