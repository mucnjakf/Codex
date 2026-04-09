using Codex.Api.Extensions;
using Codex.Application.Commands.Categories;
using Codex.Application.Dtos;
using Codex.Domain.Outcomes;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Codex.Api.Endpoints.Categories;

public sealed class CreateCategoryEndpoint : IEndpoint
{
    public sealed record Request(string Name);

    private sealed record Response(CategoryDto Data);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app
            .MapPost("api/categories", Handler)
            .WithName("CreateCategory")
            .WithTags("Categories");
    }

    private static async Task<IResult> Handler(
        [FromBody] Request request,
        [FromServices] IValidator<Request> validator,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        CreateCategoryCommand command = new(request.Name);

        Result<CategoryDto> result = await sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? Results.CreatedAtRoute(
                GetCategoryEndpoint.EndpointName,
                new { id = result.Value.Id },
                new Response(result.Value))
            : result.ToProblemDetails();
    }

    public sealed class RequestValidator : AbstractValidator<Request>
    {
        public RequestValidator()
        {
            RuleFor(request => request.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(30).WithMessage("Name must be at most 30 characters");
        }
    }
}