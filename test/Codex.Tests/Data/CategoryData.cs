using Codex.Domain.Entities;

namespace Codex.Tests.Data;

public static class CategoryData
{
    public static Guid Id => Guid.CreateVersion7();

    public static string Name => "Category name";

    public static Category Category => Category.Create(Name).Value;
}