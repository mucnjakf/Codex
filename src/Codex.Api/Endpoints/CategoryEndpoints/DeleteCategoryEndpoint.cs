using Codex.Api.Extensions;
using Codex.Application.Commands.Categories;
using Codex.Domain.Outcomes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Codex.Api.Endpoints.CategoryEndpoints;

internal sealed class DeleteCategoryEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app
            .MapDelete("api/categories/{id:guid}", Handler)
            .WithName("DeleteCategory")
            .WithTags("Categories");
    }

    private static async Task<IResult> Handler(
        [FromRoute] Guid id,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new DeleteCategoryCommand(id);

        Result result = await sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? Results.NoContent()
            : result.ToProblemDetails();
    }
}