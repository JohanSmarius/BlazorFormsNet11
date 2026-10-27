using CustomerApp.Client.Models;
using CustomerApp.Client.Services;

namespace CustomerApp.Services;

public sealed class ServerRegistrationService(InviteCodeStore inviteCodeStore, ILogger<ServerRegistrationService> logger) : IRegistrationService
{
    public Task<RegistrationResult> RegisterAsync(RegistrationModel model, CancellationToken cancellationToken = default)
    {
        if (!inviteCodeStore.TryRedeem(model.InviteCode))
        {
            return Task.FromResult(RegistrationResult.Failed(
                nameof(RegistrationModel.InviteCode),
                "The invite code is invalid or has already been used."));
        }

        logger.LogInformation("Registered {FirstName} {LastName} using an invite code.", model.FirstName, model.LastName);

        return Task.FromResult(RegistrationResult.Success());
    }
}
