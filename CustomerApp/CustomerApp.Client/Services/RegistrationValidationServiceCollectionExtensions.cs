using Microsoft.Extensions.DependencyInjection;

namespace CustomerApp.Client.Services;

public static class RegistrationValidationServiceCollectionExtensions
{
    /// <summary>Registers localization and generated validation metadata from the client assembly for both hosts.</summary>
    public static IServiceCollection AddRegistrationValidation(this IServiceCollection services)
    {
        services.AddLocalization(options => options.ResourcesPath = "Resources");
        return services.AddValidation();
    }
}
