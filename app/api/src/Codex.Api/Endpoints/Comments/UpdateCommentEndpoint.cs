using Codex.Api.Extensions;
using Codex.Application.Commands.Comments;
using Codex.Domain.Outcomes;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Codex.Api.Endpoints.Comments;

public sealed class UpdateCommentEndpoint : IEndpoint
{
    public sealed record Request(string Content);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app
            .MapPut("api/comments/{id:guid}", Handler)
            .WithName("UpdateComment")
            .WithTags("Comments");
    }

    private static async Task<IResult> Handler(
        [FromRoute] Guid id,
        [FromBody] Request request,
        [FromServices] IValidator<Request> validator,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        UpdateCommentCommand command = new(id, request.Content);

        Result result = await sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? Results.NoContent()
            : result.ToProblemDetails();
    }

    public sealed class RequestValidator : AbstractValidator<Request>
    {
        public RequestValidator()
        {
            RuleFor(request => request.Content)
                .NotEmpty().WithMessage("Content is required")
                .MaximumLength(250).WithMessage("Content must be at most 250 characters");
        }
    }
}