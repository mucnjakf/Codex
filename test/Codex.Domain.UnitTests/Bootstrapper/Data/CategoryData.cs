using Codex.Domain.Entities;

namespace Codex.Domain.UnitTests.Bootstrapper.Data;

internal static class CategoryData
{
    internal static Guid Id => Guid.CreateVersion7();

    internal static string Name => "Category name";

    internal static Category Category => Category.Create(Name).Value;
}