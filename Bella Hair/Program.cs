using Bella_Hair.Components;                 // matcher RootNamespace + ".Components"
using BellaHair.Application.Interfaces;
using BellaHair.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Blazor / Razor Components
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Vores DataService
builder.Services.AddSingleton<IDataService, InMemoryDataService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

// Kører App.razor som root-komponent
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
