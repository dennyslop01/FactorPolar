using FactorPolar.Application.Interfaces;
using FactorPolar.Domain.Constants;
using FactorPolar.Infrastructure.DataContext;
using FactorPolar.Infrastructure.Repositories;
using FactorPolar.Infrastructure.Services;
using FactorPolar.Infrastructure.TokenHandler;
using FactorPolar.WebApp.Components;
using FactorPolar.WebApp.State;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using NetcodeHub.Packages.Extensions.LocalStorage;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContext<FactorDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConn"));
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = Constant.Schema;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddScheme<AuthenticationSchemeOptions,
             GoogleAccessTokenAuthenticationHandler>(Constant.Schema, null)
.AddGoogle(options =>
{
    options.ClientId = builder.Configuration["Google:ClientId"]!;
    options.ClientSecret = builder.Configuration["Google:ClientSecret"]!;
    options.CallbackPath = $"/{builder.Configuration["Google:RedirectUri"]}"!;
});

builder.Services.AddMudServices();
builder.Services.AddScoped<IGoogleAuthHelper, GoogleAuthHelperSevice>();
builder.Services.AddScoped<IGoogleAuthorization, GoogleAuthorizationService>();
builder.Services.AddScoped<ICredentialRepository, CredentialRepository>();

builder.Services.AddCascadingAuthenticationState();
//builder.Services.AddHttpClient(Constant.Client, client =>
//{
//    client.BaseAddress = new Uri("https://localhost:1010/");
//});

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthState>();
builder.Services.AddAuthorizationCore();
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
app.UseAuthorization();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
