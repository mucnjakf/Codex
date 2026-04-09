using Codex.Api.Extensions;
using Codex.Application.Commands.Posts;
using Codex.Application.Dtos;
using Codex.Domain.Outcomes;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Codex.Api.Endpoints.Posts;

public sealed class CreatePostEndpoint : IEndpoint
{
    public sealed record Request(string Title, string Content, Guid AuthorId, Guid CategoryId);

    private sealed record Response(PostDto Data);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app
            .MapPost("api/posts", Handler)
            .WithName("CreatePost")
            .WithTags("Posts");
    }

    private static async Task<IResult> Handler(
        [FromBody] Request request,
        [FromServices] IValidator<Request> validator,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        CreatePostCommand command = new(request.Title, request.Content, request.AuthorId, request.CategoryId);

        Result<PostDto> result = await sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? Results.CreatedAtRoute(
                GetPostEndpoint.EndpointName,
                new { id = result.Value.Id },
                new Response(result.Value))
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

            RuleFor(request => request.AuthorId)
                .NotEmpty().WithMessage("Author ID is required");

            RuleFor(request => request.CategoryId)
                .NotEmpty().WithMessage("Category ID is required");
        }
    }
}