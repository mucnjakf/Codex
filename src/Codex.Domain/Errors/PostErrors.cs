using Codex.Domain.Outcomes;

namespace Codex.Domain.Errors;

public static class PostErrors
{
    internal static readonly Error TitleIsRequired = Error.Validation(
        "Post.TitleIsRequired",
        "Post title is required");

    internal static readonly Error ContentIsRequired = Error.Validation(
        "Post.ContentIsRequired",
        "Post content is required");

    internal static readonly Error AuthorIdIsRequired = Error.Validation(
        "Post.AuthorIdIsRequired",
        "Post author ID is required");

    internal static readonly Error CategoryIdIsRequired = Error.Validation(
        "Post.CategoryIdIsRequired",
        "Post category ID is required");

    internal static readonly Error PublishOnlyDraft = Error.Conflict(
        "Post.PublishOnlyDraft",
        "Post must be in draft status to be published");

    public static readonly Error NotFound = Error.NotFound(
        "Post.NotFound",
        "Post not found");

    public static readonly Error CannotDeleteContainsComments = Error.Conflict(
        "Post.CannotDeleteContainsComments",
        "Cannot delete post that contains comments");
}