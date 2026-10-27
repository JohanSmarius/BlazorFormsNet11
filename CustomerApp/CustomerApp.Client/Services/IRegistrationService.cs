using CustomerApp.Client.Models;

namespace CustomerApp.Client.Services;

public interface IRegistrationService
{
    Task<RegistrationResult> RegisterAsync(RegistrationModel model, CancellationToken cancellationToken = default);
}
