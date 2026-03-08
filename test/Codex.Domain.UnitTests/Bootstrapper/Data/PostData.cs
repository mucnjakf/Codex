namespace Codex.Domain.UnitTests.Bootstrapper.Data;

internal static class PostData
{
    internal static Guid Id => Guid.CreateVersion7();

    internal static string Title => "Post title";

    internal static string Content => "Post content";
}