using CustomerApp.Client.Models;

namespace CustomerApp.Client.Services;

public interface IRegistrationService
{
    /// <summary>Checks availability without redeeming the invite code.</summary>
    Task<bool> IsInviteCodeAvailableAsync(string code, CancellationToken cancellationToken = default);

    Task<RegistrationResult> RegisterAsync(RegistrationModel model, CancellationToken cancellationToken = default);
}
