using CustomerApp.Client.Models;
using CustomerApp.Client.Services;
using Microsoft.Extensions.Localization;

namespace CustomerApp.Services;

public sealed class ServerRegistrationService(
    InviteCodeStore inviteCodeStore,
    RegisteredEmailStore registeredEmailStore,
    ILogger<ServerRegistrationService> logger,
    IStringLocalizer<RegistrationModel> localizer) : IRegistrationService
{
    public Task<bool> IsInviteCodeAvailableAsync(string code, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(inviteCodeStore.IsAvailable(code));
    }

    public Task<bool> IsEmailAvailableAsync(string email, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(registeredEmailStore.IsAvailable(email));
    }

    public Task<RegistrationResult> RegisterAsync(RegistrationModel model, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        // Reserve the email first so an invite code isn't consumed by a registration that fails on a duplicate email.
        if (!registeredEmailStore.TryReserve(model.Email))
        {
            return Task.FromResult(RegistrationResult.Failed(
                nameof(RegistrationModel.Email),
                localizer["This email address is already registered."]));
        }

        if (!inviteCodeStore.TryRedeem(model.InviteCode))
        {
            registeredEmailStore.Release(model.Email);
            return Task.FromResult(RegistrationResult.Failed(
                nameof(RegistrationModel.InviteCode),
                localizer["The invite code is invalid or has already been used."]));
        }

        logger.LogInformation("Registered {FirstName} {LastName} using an invite code.", model.FirstName, model.LastName);

        return Task.FromResult(RegistrationResult.Success());
    }
}
