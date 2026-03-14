using Codex.Domain.Outcomes;

namespace Codex.Domain.Errors;

public static class CommentErrors
{
    internal static readonly Error ContentIsRequired = new(
        "Comment.ContentIsRequired",
        "Comment content is required");

    internal static readonly Error PostIdIsRequired = new(
        "Comment.PostIdIsRequired",
        "Comment post ID is required");

    internal static readonly Error ReaderIdIsRequired = new(
        "Comment.ReaderIdIsRequired",
        "Comment reader ID is required");

    public static readonly Error NotFound = new(
        "Comment.NotFound",
        "Comment not found");
}