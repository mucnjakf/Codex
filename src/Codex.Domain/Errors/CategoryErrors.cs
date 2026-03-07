using Codex.Domain.Outcomes;

namespace Codex.Domain.Errors;

internal static class CategoryErrors
{
    internal static readonly Error NameIsRequired = new(
        "Category.NameIsRequired",
        "The category name is required");
}