using Codex.Domain.Outcomes;

namespace Codex.Domain.Errors;

public static class PostErrors
{
    internal static readonly Error TitleIsRequired = new(
        "Post.TitleIsRequired",
        "Post title is required");

    internal static readonly Error ContentIsRequired = new(
        "Post.ContentIsRequired",
        "Post content is required");

    internal static readonly Error AuthorIdIsRequired = new(
        "Post.AuthorIdIsRequired",
        "Post author ID is required");

    internal static readonly Error CategoryIdIsRequired = new(
        "Post.CategoryIdIsRequired",
        "Post category ID is required");

    internal static readonly Error PublishOnlyDraft = new(
        "Post.PublishOnlyDraft",
        "Post must be in draft status to be published");

    public static readonly Error NotFound = new(
        "Post.NotFound",
        "Post not found");
}