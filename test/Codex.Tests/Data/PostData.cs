using System.Reflection;
using Codex.Domain.Entities;

namespace Codex.Tests.Data;

public static class PostData
{
    public static Guid Id => Guid.CreateVersion7();

    public static string Title => "Post title";

    public static string Content => "Post content";

    public static Post Post => Post.Create(Title, Content, AuthorData.Id, CategoryData.Id).Value;

    public static Post PostWithAuthorAndCategory()
    {
        Author author = Author.Create(
                "Author first name",
                "Author last name",
                "Author biography")
            .Value;

        Category category = Category.Create("Category name").Value;

        Post post = Post.Create(Title, Content, author.Id, category.Id).Value;

        typeof(Post)
            .GetProperty("Author", BindingFlags.Public | BindingFlags.Instance)!
            .SetValue(post, author);

        typeof(Post)
            .GetProperty("Category", BindingFlags.Public | BindingFlags.Instance)!
            .SetValue(post, category);

        return post;
    }
}