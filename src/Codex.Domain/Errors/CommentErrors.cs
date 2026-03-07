using Codex.Domain.Outcomes;

namespace Codex.Domain.Errors;

internal static class CommentErrors
{
    internal static readonly Error ContentIsRequired = new(
        "Comment.ContentIsRequired",
        "The comment content is required");

    internal static readonly Error PostIdIsRequired = new(
        "Comment.PostIdIsRequired",
        "The comment post ID is required");

    internal static readonly Error ReaderIdIsRequired = new(
        "Comment.ReaderIdIsRequired",
        "The comment reader ID is required");
}