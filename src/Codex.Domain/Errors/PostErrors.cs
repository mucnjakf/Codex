using Codex.Domain.Outcomes;

namespace Codex.Domain.Errors;

internal static class PostErrors
{
    internal static readonly Error TitleIsRequired = new(
        "Post.TitleIsRequired",
        "The post title is required");

    internal static readonly Error ContentIsRequired = new(
        "Post.ContentIsRequired",
        "The post content is required");

    internal static readonly Error AuthorIdIsRequired = new(
        "Post.AuthorIdIsRequired",
        "The post author ID is required");

    internal static readonly Error CategoryIdIsRequired = new(
        "Post.CategoryIdIsRequired",
        "The post category ID is required");
}