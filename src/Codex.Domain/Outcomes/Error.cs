namespace Codex.Domain.Outcomes;

public sealed class Error(string Code, string Message)
{
    internal static readonly Error None = new(string.Empty, string.Empty);

    internal static readonly Error NullValue = new("Error.NullValue", "Null value was provided");
}