using CustomerApp.Client.Models;
using CustomerApp.Client.Services;
using CustomerApp.Components;
using CustomerApp.Services;
using Microsoft.AspNetCore.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddRegistrationValidation();
builder.Services.AddValidation();
builder.Services.Configure<RequestLocalizationOptions>(options => options
    .SetDefaultCulture("en")
    .AddSupportedCultures("en", "nl")
    .AddSupportedUICultures("en", "nl"));
builder.Services.AddSingleton<InviteCodeStore>();
builder.Services.AddScoped<IRegistrationService, ServerRegistrationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseRequestLocalization();

app.MapStaticAssets();

app.MapPost("/api/invite-codes/validate", async ([Microsoft.AspNetCore.Mvc.FromBody] string code, IRegistrationService registrationService, CancellationToken cancellationToken) =>
    Results.Ok(await registrationService.IsInviteCodeAvailableAsync(code, cancellationToken)));

// RegistrationModel is validated asynchronously (AddValidation) before this handler runs.
app.MapPost("/api/registrations", async (RegistrationModel model, IRegistrationService registrationService, CancellationToken cancellationToken) =>
{
    var result = await registrationService.RegisterAsync(model, cancellationToken);
    return result.Succeeded ? Results.Ok() : Results.ValidationProblem(result.Errors);
});

app.MapRazorComponents<App>()
    .WithBrowserOptions(options => options.AddAutoPause(pause => pause.HiddenDelay = TimeSpan.FromSeconds(1)))
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(CustomerApp.Client._Imports).Assembly);

app.Run();
