using Codex.Domain.Outcomes;

namespace Codex.Domain.Errors;

internal static class ReaderErrors
{
    internal static readonly Error FirstNameIsRequired = new(
        "Reader.FirstNameIsRequired",
        "Reader first name is required");

    internal static readonly Error LastNameIsRequired = new(
        "Reader.LastNameIsRequired",
        "Reader last name is required");
}