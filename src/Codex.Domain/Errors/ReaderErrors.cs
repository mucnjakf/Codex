using Codex.Domain.Outcomes;

namespace Codex.Domain.Errors;

public static class ReaderErrors
{
    internal static readonly Error FirstNameIsRequired = new(
        "Reader.FirstNameIsRequired",
        "Reader first name is required");

    internal static readonly Error LastNameIsRequired = new(
        "Reader.LastNameIsRequired",
        "Reader last name is required");

    public static readonly Error NotFound = new(
        "Reader.NotFound",
        "Reader not found");

    public static readonly Error CannotDeleteContainsComments = new(
        "Reader.CannotDeleteContainsComments",
        "Cannot delete reader that contains comments");
}