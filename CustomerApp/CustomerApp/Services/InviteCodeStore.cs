using System.Collections.Concurrent;

namespace CustomerApp.Services;

public sealed class InviteCodeStore
{
    private readonly ConcurrentDictionary<string, byte> availableCodes;

    public InviteCodeStore(IConfiguration configuration)
    {
        var codes = configuration.GetSection("InviteCodes").Get<string[]>() ?? [];
        availableCodes = new ConcurrentDictionary<string, byte>(
            codes.Where(c => !string.IsNullOrWhiteSpace(c)).Select(c => KeyValuePair.Create(c.Trim(), (byte)0)),
            StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>Atomically checks that the code is available and marks it as used. Returns false when the code is unknown or already used.</summary>
    public bool TryRedeem(string code) =>
        !string.IsNullOrWhiteSpace(code) && availableCodes.TryRemove(code.Trim(), out _);
}
