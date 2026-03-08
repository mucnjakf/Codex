using Codex.Domain.Entities;

namespace Codex.Tests.Data;

public static class PostData
{
    public static Guid Id => Guid.CreateVersion7();

    public static string Title => "Post title";

    public static string Content => "Post content";

    public static Post Post => Post.Create(Title, Content, AuthorData.Id, CategoryData.Id).Value;
}