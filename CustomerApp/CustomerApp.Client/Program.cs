using System.Globalization;
using CustomerApp.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddRegistrationValidation();
builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IRegistrationService, HttpRegistrationService>();

var host = builder.Build();
var js = host.Services.GetRequiredService<IJSRuntime>();
try
{
    var culture = CultureInfo.GetCultureInfo(await js.InvokeAsync<string>("appCulture.get"));
    CultureInfo.DefaultThreadCurrentCulture = culture;
    CultureInfo.DefaultThreadCurrentUICulture = culture;
}
catch (JSException exception)
{
    host.Services.GetRequiredService<ILoggerFactory>().CreateLogger("CultureInitialization")
        .LogError(exception, "Unable to read the server-selected culture.");
    throw;
}

await host.RunAsync();
