using System.Collections.Concurrent;

namespace CustomerApp.Services;

public sealed class RegisteredEmailStore
{
    private readonly ConcurrentDictionary<string, byte> emails = new(StringComparer.OrdinalIgnoreCase);

    public RegisteredEmailStore(IConfiguration configuration)
    {
        foreach (var email in configuration.GetSection("RegisteredEmails").Get<string[]>() ?? [])
        {
            if (!string.IsNullOrWhiteSpace(email))
            {
                emails.TryAdd(email.Trim(), 0);
            }
        }
    }

    public bool IsAvailable(string email) =>
        !string.IsNullOrWhiteSpace(email) && !emails.ContainsKey(email.Trim());

    /// <summary>Atomically reserves the email address. Returns false when it's already registered.</summary>
    public bool TryReserve(string email) =>
        !string.IsNullOrWhiteSpace(email) && emails.TryAdd(email.Trim(), 0);

    public void Release(string email) => emails.TryRemove(email.Trim(), out _);
}
