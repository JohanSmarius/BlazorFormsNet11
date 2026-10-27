using CustomerApp.Client.Models;
using CustomerApp.Client.Services;
using CustomerApp.Components;
using CustomerApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddValidation();
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

app.MapStaticAssets();

// RegistrationModel is validated automatically (AddValidation) before this handler runs.
app.MapPost("/api/registrations", async (RegistrationModel model, IRegistrationService registrationService, CancellationToken cancellationToken) =>
{
    var result = await registrationService.RegisterAsync(model, cancellationToken);
    return result.Succeeded ? Results.Ok() : Results.ValidationProblem(result.Errors);
});

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(CustomerApp.Client._Imports).Assembly);

app.Run();
