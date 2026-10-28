using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using CustomerApp.Client.Models;
using Microsoft.Extensions.Localization;

namespace CustomerApp.Client.Services;

public sealed class HttpRegistrationService(HttpClient httpClient, IStringLocalizer<RegistrationModel> localizer) : IRegistrationService
{
    public async Task<bool> IsInviteCodeAvailableAsync(string code, CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, "api/invite-codes/validate")
        {
            Content = JsonContent.Create(code)
        };
        request.Headers.AcceptLanguage.ParseAdd(CultureInfo.CurrentUICulture.Name);
        using var response = await httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<bool>(cancellationToken);
    }

    public async Task<RegistrationResult> RegisterAsync(RegistrationModel model, CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, "api/registrations")
        {
            Content = JsonContent.Create(model)
        };
        request.Headers.AcceptLanguage.ParseAdd(CultureInfo.CurrentUICulture.Name);
        using var response = await httpClient.SendAsync(request, cancellationToken);

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

        return RegistrationResult.Failed(string.Empty, localizer["Registration failed. Please try again later."]);
    }

    private sealed class ValidationProblemResponse
    {
        public Dictionary<string, string[]>? Errors { get; set; }
    }
}
