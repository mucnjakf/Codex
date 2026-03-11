using System.Reflection;
using Codex.Domain.Entities;

namespace Codex.Tests.Data;

public static class AuthorData
{
    public static Guid Id => Guid.CreateVersion7();

    public static string FirstName => "Author first name";

    public static string LastName => "Author last name";

    public static string Biography => "Author biography";

    public static Author Author => Author.Create(FirstName, LastName, Biography).Value;

    public static Author AuthorWithPosts()
    {
        Author author = Author.Create(FirstName, LastName, Biography).Value;

        Post post = Post.Create(
                "Post title",
                "Post content",
                Guid.CreateVersion7(),
                author.Id)
            .Value;

        typeof(Author)
            .GetField("_posts", BindingFlags.NonPublic | BindingFlags.Instance)!
            .SetValue(author, new List<Post> { post });

        return author;
    }
}