using Codex.Domain.Outcomes;

namespace Codex.Domain.Errors;

public static class CategoryErrors
{
    internal static readonly Error NameIsRequired = Error.Validation(
        "Category.NameIsRequired",
        "Category name is required");

    public static readonly Error NotFound = Error.NotFound(
        "Category.NotFound",
        "Category not found");

    public static readonly Error CannotDeleteContainsPosts = Error.Conflict(
        "Category.CannotDeleteContainsPosts",
        "Cannot delete category that contains posts");
}