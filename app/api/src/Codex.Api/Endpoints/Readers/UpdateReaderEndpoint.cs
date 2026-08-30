using Codex.Api.Configuration;
using Codex.Api.Extensions;
using Codex.Application.Commands.Readers;
using Codex.Domain.Outcomes;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Codex.Api.Endpoints.Readers;

public sealed class UpdateReaderEndpoint : IEndpoint
{
    public sealed record Request(string FirstName, string LastName);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app
            .MapPut("api/readers/{id:guid}", Handler)
            .WithName("UpdateReader")
            .WithTags("Readers");
    }

    private static async Task<IResult> Handler(
        [FromRoute] Guid id,
        [FromBody] Request request,
        [FromServices] IValidator<Request> validator,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        UpdateReaderCommand command = new(id, request.FirstName, request.LastName);

        Result result = await sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? Results.NoContent()
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
        }
    }
}