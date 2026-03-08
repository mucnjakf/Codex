namespace Codex.Domain.UnitTests.Bootstrapper.Data;

internal static class AuthorData
{
    internal static Guid Id => Guid.CreateVersion7();

    internal static string FirstName => "Author first name";

    internal static string LastName => "Author last name";

    internal static string Biography => "Author biography";
}