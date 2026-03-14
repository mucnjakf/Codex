using System.Reflection;
using Codex.Domain.Entities;

namespace Codex.Tests.Data;

public static class CommentData
{
    public static Guid Id => Guid.CreateVersion7();

    public static string Content => "Comment content";

    public static Comment Comment => Comment.Create(Content, PostData.Id, ReaderData.Id).Value;

    public static Comment CommentWithPostAndReader()
    {
        Author author = Author.Create(
                "Author first name",
                "Author last name",
                "Author biography")
            .Value;

        Category category = Category.Create(
                "Category name")
            .Value;

        Post post = Post.Create(
                "Post title",
                "Post content",
                author.Id,
                category.Id)
            .Value;

        typeof(Post)
            .GetProperty("Author", BindingFlags.Public | BindingFlags.Instance)!
            .SetValue(post, author);

        typeof(Post)
            .GetProperty("Category", BindingFlags.Public | BindingFlags.Instance)!
            .SetValue(post, category);

        Reader reader = Reader.Create(
                "Reader first name",
                "Reader last name")
            .Value;

        Comment comment = Comment.Create(Content, post.Id, reader.Id).Value;

        typeof(Comment)
            .GetProperty("Post", BindingFlags.Public | BindingFlags.Instance)!
            .SetValue(comment, post);

        typeof(Comment)
            .GetProperty("Reader", BindingFlags.Public | BindingFlags.Instance)!
            .SetValue(comment, reader);

        return comment;
    }
}