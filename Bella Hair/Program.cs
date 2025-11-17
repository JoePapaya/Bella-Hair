using BellaHair.Application;
using BellaHair.Application.Interfaces;
using BellaHair.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Blazor services...
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Our data service
builder.Services.AddSingleton<IDataService, InMemoryDataService>();

var app = builder.Build();

// rest of Program.cs...
