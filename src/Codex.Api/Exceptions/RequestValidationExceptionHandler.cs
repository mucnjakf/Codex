using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Codex.Api.Exceptions;

internal sealed class RequestValidationExceptionHandler(IProblemDetailsService problemDetailsService)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not ValidationException validationException)
        {
            return false;
        }

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        var context = new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Request Validation",
                Type = validationException.GetType().Name,
                Detail = "Request validation error occured while processing your request"
            }
        };

        Dictionary<string, string[]> errors = validationException.Errors
            .GroupBy(validationFailure => validationFailure.PropertyName)
            .ToDictionary(
                group => group.Key.ToLowerInvariant(),
                group => group.Select(failure => failure.ErrorMessage).ToArray());

        context.ProblemDetails.Extensions.Add("errors", errors);

        return await problemDetailsService.TryWriteAsync(context);
    }
}