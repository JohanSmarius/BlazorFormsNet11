using System.ComponentModel.DataAnnotations;
using CustomerApp.Client.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace CustomerApp.Client.Models;

public sealed class AvailableInviteCodeAttribute : AsyncValidationAttribute
{
    protected override async Task<ValidationResult?> IsValidAsync(
        object? value,
        ValidationContext validationContext,
        CancellationToken cancellationToken)
    {
        if (value is not string code || string.IsNullOrWhiteSpace(code))
        {
            return ValidationResult.Success;
        }

        var registrationService = validationContext.GetRequiredService<IRegistrationService>();
        // Localize each outcome separately so a common attribute template doesn't mask network failures.
        var localizer = validationContext.GetRequiredService<IStringLocalizer<RegistrationModel>>();
        try
        {
            return await registrationService.IsInviteCodeAvailableAsync(code, cancellationToken)
                ? ValidationResult.Success
                : new ValidationResult(localizer["The invite code is invalid or has already been used."], [validationContext.MemberName!]);
        }
        catch (HttpRequestException)
        {
            return new ValidationResult(
                localizer["Unable to validate the invite code. Please try again later."],
                [validationContext.MemberName!]);
        }
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) =>
        throw new InvalidOperationException("Invite code validation requires asynchronous validation.");
}
