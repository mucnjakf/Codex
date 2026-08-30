using Codex.Api.Extensions;
using Codex.Application.Commands.Readers;
using Codex.Application.Dtos;
using Codex.Domain.Outcomes;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Codex.Api.Endpoints.Readers;

public sealed class CreateReaderEndpoint : IEndpoint
{
    public sealed record Request(string FirstName, string LastName);

    private sealed record Response(ReaderDto Data);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app
            .MapPost("api/readers", Handler)
            .WithName("CreateReader")
            .WithTags("Readers");
    }

    private static async Task<IResult> Handler(
        [FromBody] Request request,
        [FromServices] IValidator<Request> validator,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        CreateReaderCommand command = new(request.FirstName, request.LastName);

        Result<ReaderDto> result = await sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? Results.CreatedAtRoute(
                GetReaderEndpoint.EndpointName,
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
        }
    }
}