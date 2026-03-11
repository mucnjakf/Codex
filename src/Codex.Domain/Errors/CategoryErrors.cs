using Codex.Domain.Outcomes;

namespace Codex.Domain.Errors;

public static class CategoryErrors
{
    internal static readonly Error NameIsRequired = new(
        "Category.NameIsRequired",
        "Category name is required");

    public static readonly Error NotFound = new(
        "Category.NotFound",
        "Category not found");

    public static readonly Error CannotDeleteContainsPosts = new(
        "Category.CannotDeleteContainsPosts",
        "Cannot delete category that contains posts");
}