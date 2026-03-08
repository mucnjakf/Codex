using Codex.Domain.Entities;

namespace Codex.Tests.Data;

public static class ReaderData
{
    public static Guid Id => Guid.CreateVersion7();

    public static string FirstName => "Reader first name";

    public static string LastName => "Reader last name";

    public static Reader Reader => Reader.Create(FirstName, LastName).Value;
}