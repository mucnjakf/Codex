using Codex.Domain.Entities;

namespace Codex.Domain.UnitTests.Bootstrapper.Data;

internal static class CommentData
{
    internal static string Content => "Comment content";

    internal static Comment Comment => Comment.Create(Content, PostData.Id, ReaderData.Id).Value;
}