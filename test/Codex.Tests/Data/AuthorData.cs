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
}