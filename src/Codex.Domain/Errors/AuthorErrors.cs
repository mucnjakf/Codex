using Codex.Domain.Outcomes;

namespace Codex.Domain.Errors;

public static class AuthorErrors
{
    internal static readonly Error FirstNameIsRequired = Error.Validation(
        "Author.FirstNameIsRequired",
        "Author first name is required");

    internal static readonly Error LastNameIsRequired = Error.Validation(
        "Author.LastNameIsRequired",
        "Author last name is required");

    internal static readonly Error BiographyIsRequired = Error.Validation(
        "Author.BiographyIsRequired",
        "Author biography is required");

    public static readonly Error NotFound = Error.NotFound(
        "Author.NotFound",
        "Author not found");

    public static readonly Error CannotDeleteContainsPosts = Error.Conflict(
        "Author.CannotDeleteContainsPosts",
        "Cannot delete author that contains posts");
}