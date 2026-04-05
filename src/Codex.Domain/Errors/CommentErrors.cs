using Codex.Domain.Outcomes;

namespace Codex.Domain.Errors;

public static class CommentErrors
{
    internal static readonly Error ContentIsRequired = Error.Validation(
        "Comment.ContentIsRequired",
        "Comment content is required");

    internal static readonly Error PostIdIsRequired = Error.Validation(
        "Comment.PostIdIsRequired",
        "Comment post ID is required");

    internal static readonly Error ReaderIdIsRequired = Error.Validation(
        "Comment.ReaderIdIsRequired",
        "Comment reader ID is required");

    public static readonly Error NotFound = Error.NotFound(
        "Comment.NotFound",
        "Comment not found");
}