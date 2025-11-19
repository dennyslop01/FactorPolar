using FactorPolar.WebApp.Components;
using FactorPolar.WebApp.State;
using Microsoft.AspNetCore.Components.Authorization;
using NetcodeHub.Packages.Extensions.LocalStorage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();


builder.Services.AddCascadingAuthenticationState();
//builder.Services.AddHttpClient(Constant.Client, client =>
//{
//    client.BaseAddress = new Uri("https://localhost:1010/");
//});

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthState>();
builder.Services.AddAuthenticationCore();
builder.Services.AddNetcodeHubLocalStorageService();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
