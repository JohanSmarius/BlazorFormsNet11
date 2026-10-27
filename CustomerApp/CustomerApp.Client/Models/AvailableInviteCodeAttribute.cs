using System.ComponentModel.DataAnnotations;
using CustomerApp.Client.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CustomerApp.Client.Models;

public sealed class AvailableInviteCodeAttribute : AsyncValidationAttribute
{
    public AvailableInviteCodeAttribute()
    {
        ErrorMessage = "The invite code is invalid or has already been used.";
    }

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
        try
        {
            return await registrationService.IsInviteCodeAvailableAsync(code, cancellationToken)
                ? ValidationResult.Success
                : new ValidationResult(FormatErrorMessage(validationContext.DisplayName), [validationContext.MemberName!]);
        }
        catch (HttpRequestException)
        {
            return new ValidationResult(
                "Unable to validate the invite code. Please try again later.",
                [validationContext.MemberName!]);
        }
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) =>
        throw new InvalidOperationException("Invite code validation requires asynchronous validation.");
}
