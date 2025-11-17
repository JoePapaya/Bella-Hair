using Bella_Hair.Components;                 // matcher RootNamespace + ".Components"
using BellaHair.Application.Interfaces;
using BellaHair.Infrastructure;
using BellaHair.Infrastructure;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("BellaHairDb");

builder.Services.AddDbContext<BellaHairDbContext>(options =>
    options.UseSqlServer(connectionString));


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
