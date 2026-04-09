using Codex.Api.Extensions;
using Codex.Application.Commands.Posts;
using Codex.Domain.Outcomes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Codex.Api.Endpoints.Posts;

internal sealed class PublishPostEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app
            .MapPost("api/posts/{id:guid}/publish", Handler)
            .WithName("PublishPost")
            .WithTags("Posts");
    }

    private static async Task<IResult> Handler(
        [FromRoute] Guid id,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        PublishPostCommand command = new(id);

        Result result = await sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? Results.NoContent()
            : result.ToProblemDetails();
    }
}