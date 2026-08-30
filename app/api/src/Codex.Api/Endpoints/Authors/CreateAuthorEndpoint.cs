using Codex.Api.Extensions;
using Codex.Application.Commands.Authors;
using Codex.Application.Dtos;
using Codex.Domain.Outcomes;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Codex.Api.Endpoints.Authors;

public sealed class CreateAuthorEndpoint : IEndpoint
{
    public sealed record Request(string FirstName, string LastName, string Biography);

    private sealed record Response(AuthorDto Data);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app
            .MapPost("api/authors", Handler)
            .WithName("CreateAuthor")
            .WithTags("Authors");
    }

    private static async Task<IResult> Handler(
        [FromBody] Request request,
        [FromServices] IValidator<Request> validator,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        CreateAuthorCommand command = new(request.FirstName, request.LastName, request.Biography);

        Result<AuthorDto> result = await sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? Results.CreatedAtRoute(
                GetAuthorEndpoint.EndpointName,
                new { id = result.Value.Id },
                new Response(result.Value))
            : result.ToProblemDetails();
    }

    public sealed class RequestValidator : AbstractValidator<Request>
    {
        public RequestValidator()
        {
            RuleFor(request => request.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .MaximumLength(50).WithMessage("First name must be at most 50 characters");

            RuleFor(request => request.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .MaximumLength(50).WithMessage("Last name must be at most 50 characters");

            RuleFor(request => request.Biography)
                .NotEmpty().WithMessage("Biography is required")
                .MaximumLength(100).WithMessage("Biography must be at most 100 characters");
        }
    }
}