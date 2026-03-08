using Codex.Domain.Entities;

namespace Codex.Tests.Data;

public static class CommentData
{
    public static string Content => "Comment content";

    public static Comment Comment => Comment.Create(Content, PostData.Id, ReaderData.Id).Value;
}