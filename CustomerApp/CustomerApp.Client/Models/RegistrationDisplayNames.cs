using System.Globalization;
using System.Resources;

namespace CustomerApp.Client.Models;

// DisplayName reads DisplayAttribute directly, outside the validation-localization pipeline.
public static class RegistrationDisplayNames
{
    private static readonly ResourceManager Resources = new(
        "CustomerApp.Client.Resources.Models.RegistrationModel", typeof(RegistrationModel).Assembly);

    public static string FirstName => GetString("First name");
    public static string LastName => GetString("Last name");
    public static string Email => GetString("Email address");
    public static string InviteCode => GetString("Invite code");

    private static string GetString(string key) =>
        Resources.GetString(key, CultureInfo.CurrentUICulture)
        ?? throw new MissingManifestResourceException($"Missing registration display name: {key}");
}
