using Codex.Domain.Outcomes;

namespace Codex.Domain.Errors;

internal static class AuthorErrors
{
    internal static readonly Error FirstNameIsRequired = new(
        "Author.FirstNameIsRequired",
        "Author first name is required");

    internal static readonly Error LastNameIsRequired = new(
        "Author.LastNameIsRequired",
        "Author last name is required");

    internal static readonly Error BiographyIsRequired = new(
        "Author.BiographyIsRequired",
        "Author biography is required");
}