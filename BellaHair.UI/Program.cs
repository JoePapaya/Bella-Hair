using Bella_Hair.Components;
using BellaHair.Application.Interfaces;
using BellaHair.Infrastructure;
using BellaHair.Application.Services;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// ?? Use DbContextFactory instead of a long-lived DbContext
builder.Services.AddDbContextFactory<BellaHairDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// IDataService still scoped
builder.Services.AddScoped<IDataService, EfDataService>();

builder.Services.AddScoped<IBookingApplicationService, BookingApplicationService>();
builder.Services.AddScoped<IBookingValidationService, BookingValidationService>();

builder.Services.AddScoped<IDataService, EfDataService>();

// Blazor / Razor Components
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
