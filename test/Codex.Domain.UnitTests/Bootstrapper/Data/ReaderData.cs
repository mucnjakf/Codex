using Codex.Domain.Entities;

namespace Codex.Domain.UnitTests.Bootstrapper.Data;

internal static class ReaderData
{
    internal static Guid Id => Guid.CreateVersion7();

    internal static string FirstName => "Reader first name";

    internal static string LastName => "Reader last name";

    internal static Reader Reader => Reader.Create(FirstName, LastName).Value;
}