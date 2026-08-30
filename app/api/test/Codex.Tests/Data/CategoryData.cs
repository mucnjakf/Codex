using System.Reflection;
using Codex.Domain.Entities;

namespace Codex.Tests.Data;

public static class CategoryData
{
    public static Guid Id => Guid.CreateVersion7();

    public static string Name => "Category name";

    public static Category Category => Category.Create(Name).Value;

    public static Category CategoryWithPosts()
    {
        Category category = Category.Create(Name).Value;

        Post post = Post.Create(
                "Post title",
                "Post content",
                Guid.CreateVersion7(),
                category.Id)
            .Value;

        typeof(Category)
            .GetField("_posts", BindingFlags.NonPublic | BindingFlags.Instance)!
            .SetValue(category, new List<Post> { post });

        return category;
    }
}