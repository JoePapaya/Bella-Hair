using Bella_Hair.Components;
using BellaHair.Application.Interfaces;
using BellaHair.Infrastructure;
using BellaHair.Application.Services;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Db + data
builder.Services.AddDbContextFactory<BellaHairDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IDataService, EfDataService>();

// Application services
builder.Services.AddScoped<IStatisticsApplicationService, StatisticsApplicationService>();
builder.Services.AddScoped<IBookingApplicationService, BookingApplicationService>();
builder.Services.AddScoped<IBookingValidationService, BookingValidationService>();
builder.Services.AddScoped<IRabatService, RabatService>();
builder.Services.AddScoped<ILoyaltyService, LoyaltyService>();
builder.Services.AddScoped<IFakturaApplicationService, FakturaApplicationService>();

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
