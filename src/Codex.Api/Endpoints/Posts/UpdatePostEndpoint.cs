using Codex.Api.Extensions;
using Codex.Application.Commands.Posts;
using Codex.Domain.Outcomes;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Codex.Api.Endpoints.Posts;

public sealed class UpdatePostEndpoint : IEndpoint
{
    public sealed record Request(string Title, string Content, Guid CategoryId);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app
            .MapPut("api/posts/{id:guid}", Handler)
            .WithName("UpdatePost")
            .WithTags("Posts");
    }

    private static async Task<IResult> Handler(
        [FromRoute] Guid id,
        [FromBody] Request request,
        [FromServices] IValidator<Request> validator,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        UpdatePostCommand command = new(id, request.Title, request.Content, request.CategoryId);

        Result result = await sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? Results.NoContent()
            : result.ToProblemDetails();
    }

    public sealed class RequestValidator : AbstractValidator<Request>
    {
        public RequestValidator()
        {
            RuleFor(request => request.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(100).WithMessage("Title must be at most 100 characters");

            RuleFor(request => request.Content)
                .NotEmpty().WithMessage("Content is required")
                .MaximumLength(1000).WithMessage("Content must be at most 1000 characters");

            RuleFor(request => request.CategoryId)
                .NotEmpty().WithMessage("Category ID is required");
        }
    }
}