using Codex.Api.Configuration;
using Codex.Api.Extensions;
using Codex.Application.Commands.Comments;
using Codex.Application.Dtos;
using Codex.Domain.Outcomes;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Codex.Api.Endpoints.Comments;

public sealed class CreateCommentEndpoint : IEndpoint
{
    public sealed record Request(string Content, Guid PostId, Guid ReaderId);

    private sealed record Response(CommentDto Data);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app
            .MapPost("api/comments", Handler)
            .WithName("CreateComment")
            .WithTags("Comments");
    }

    private static async Task<IResult> Handler(
        [FromBody] Request request,
        [FromServices] IValidator<Request> validator,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        CreateCommentCommand command = new(request.Content, request.PostId, request.ReaderId);

        Result<CommentDto> result = await sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? Results.CreatedAtRoute(
                GetCommentEndpoint.EndpointName,
                new { id = result.Value.Id },
                new Response(result.Value))
            : result.ToProblemDetails();
    }

    public sealed class RequestValidator : AbstractValidator<Request>
    {
        public RequestValidator()
        {
            RuleFor(request => request.Content)
                .NotEmpty().WithMessage("Content is required")
                .MaximumLength(250).WithMessage("Content must be at most 250 characters");

            RuleFor(request => request.PostId)
                .NotEmpty().WithMessage("Post ID is required");

            RuleFor(request => request.ReaderId)
                .NotEmpty().WithMessage("Reader ID is required");
        }
    }
}