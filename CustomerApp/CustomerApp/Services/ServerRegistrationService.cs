using CustomerApp.Client.Models;
using CustomerApp.Client.Services;
using Microsoft.Extensions.Localization;

namespace CustomerApp.Services;

public sealed class ServerRegistrationService(InviteCodeStore inviteCodeStore, ILogger<ServerRegistrationService> logger, IStringLocalizer<RegistrationModel> localizer) : IRegistrationService
{
    public Task<bool> IsInviteCodeAvailableAsync(string code, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(inviteCodeStore.IsAvailable(code));
    }

    public Task<RegistrationResult> RegisterAsync(RegistrationModel model, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (!inviteCodeStore.TryRedeem(model.InviteCode))
        {
            return Task.FromResult(RegistrationResult.Failed(
                nameof(RegistrationModel.InviteCode),
                localizer["The invite code is invalid or has already been used."]));
        }

        logger.LogInformation("Registered {FirstName} {LastName} using an invite code.", model.FirstName, model.LastName);

        return Task.FromResult(RegistrationResult.Success());
    }
}
