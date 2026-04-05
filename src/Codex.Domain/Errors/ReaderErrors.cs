using Codex.Domain.Outcomes;

namespace Codex.Domain.Errors;

public static class ReaderErrors
{
    internal static readonly Error FirstNameIsRequired = Error.Validation(
        "Reader.FirstNameIsRequired",
        "Reader first name is required");

    internal static readonly Error LastNameIsRequired = Error.Validation(
        "Reader.LastNameIsRequired",
        "Reader last name is required");

    public static readonly Error NotFound = Error.NotFound(
        "Reader.NotFound",
        "Reader not found");

    public static readonly Error CannotDeleteContainsComments = Error.Conflict(
        "Reader.CannotDeleteContainsComments",
        "Cannot delete reader that contains comments");
}