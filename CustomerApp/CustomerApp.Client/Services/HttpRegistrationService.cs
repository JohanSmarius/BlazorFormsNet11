using System.Net;
using System.Net.Http.Json;
using CustomerApp.Client.Models;

namespace CustomerApp.Client.Services;

public sealed class HttpRegistrationService(HttpClient httpClient) : IRegistrationService
{
    public async Task<RegistrationResult> RegisterAsync(RegistrationModel model, CancellationToken cancellationToken = default)
    {
        using var response = await httpClient.PostAsJsonAsync("api/registrations", model, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            return RegistrationResult.Success();
        }

        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            var problem = await response.Content.ReadFromJsonAsync<ValidationProblemResponse>(cancellationToken);
            if (problem?.Errors is { Count: > 0 } errors)
            {
                return RegistrationResult.Failed(errors);
            }
        }

        return RegistrationResult.Failed(string.Empty, "Registration failed. Please try again later.");
    }

    private sealed class ValidationProblemResponse
    {
        public Dictionary<string, string[]>? Errors { get; set; }
    }
}
