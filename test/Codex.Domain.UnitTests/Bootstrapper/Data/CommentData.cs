namespace Codex.Domain.UnitTests.Bootstrapper.Data;

internal static class CommentData
{
    internal static string Content => "Comment content";

    internal static Guid PostId => Guid.CreateVersion7();

    internal static Guid ReaderId => Guid.CreateVersion7();
}