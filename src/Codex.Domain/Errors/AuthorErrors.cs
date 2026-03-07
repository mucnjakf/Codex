using Codex.Domain.Outcomes;

namespace Codex.Domain.Errors;

internal static class AuthorErrors
{
    internal static readonly Error FirstNameIsRequired = new(
        "Author.FirstNameIsRequired",
        "The author first name is required");

    internal static readonly Error LastNameIsRequired = new(
        "Author.LastNameIsRequired",
        "The author last name is required");

    internal static readonly Error BiographyIsRequired = new(
        "Author.BiographyIsRequired",
        "The author biography is required");
}