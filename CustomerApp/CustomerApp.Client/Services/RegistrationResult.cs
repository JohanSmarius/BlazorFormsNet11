namespace CustomerApp.Client.Services;

public sealed class RegistrationResult
{
    public bool Succeeded { get; init; }

    public IDictionary<string, string[]> Errors { get; init; } = new Dictionary<string, string[]>();

    public static RegistrationResult Success() => new() { Succeeded = true };

    public static RegistrationResult Failed(IDictionary<string, string[]> errors) => new() { Errors = errors };

    public static RegistrationResult Failed(string field, string message) =>
        Failed(new Dictionary<string, string[]> { [field] = [message] });
}
