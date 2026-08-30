using Codex.Api.Configuration;
using Codex.Api.Extensions;
using Codex.Application.Dtos;
using Codex.Application.Dtos.Pagination;
using Codex.Application.Queries.Categories;
using Codex.Domain.Outcomes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Codex.Api.Endpoints.Categories;

internal sealed class GetCategoriesEndpoint : IEndpoint
{
    private sealed record Response(PaginationDto<CategoryDto> Data);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app
            .MapGet("api/categories", Handler)
            .WithName("GetCategories")
            .WithTags("Categories");
    }

    private static async Task<IResult> Handler(
        [FromQuery] int pageNumber,
        [FromQuery] int pageSize,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        GetCategoriesQuery query = new(pageNumber, pageSize);

        Result<PaginationDto<CategoryDto>> result = await sender.Send(query, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(new Response(result.Value))
            : result.ToProblemDetails();
    }
}