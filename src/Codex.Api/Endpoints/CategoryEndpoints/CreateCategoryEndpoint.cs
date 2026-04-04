using Codex.Application.Commands.Categories;
using Codex.Application.Dtos;
using Codex.Domain.Outcomes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Codex.Api.Endpoints.CategoryEndpoints;

internal sealed record CreateCategoryRequest(string Name);

internal sealed record CreateCategoryResponse(CategoryDto Category);

internal sealed class CreateCategoryEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app
            .MapPost("api/categories", Handler)
            .WithName("CreateCategory")
            .WithTags("Categories");
    }

    private static async Task<IResult> Handler(
        [FromBody] CreateCategoryRequest request,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new CreateCategoryCommand(request.Name);

        Result<CategoryDto> result = await sender.Send(command, cancellationToken);

        var response = new CreateCategoryResponse(result.Value);

        return result.IsSuccess
            ? Results.CreatedAtRoute(GetCategoryEndpoint.EndpointName, new { id = response.Category.Id }, response)
            : Results.BadRequest(result.Error); // TODO: implement problem details
    }
}