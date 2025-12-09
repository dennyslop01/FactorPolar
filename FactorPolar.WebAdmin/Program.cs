using FactorPolar.Application.Interfaces;
using FactorPolar.Infrastructure.DataContext;
using FactorPolar.Infrastructure.Repositories;
using FactorPolar.WebAdmin.Components;
using FactorPolar.WebAdmin.State;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using NetcodeHub.Packages.Extensions.LocalStorage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContext<FactorDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConn"));
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});


builder.Services.AddMudServices();
builder.Services.AddScoped<IEmployee, EmployeeRepository>();
builder.Services.AddScoped<IBeneficiario, BeneficiarioRepository>();

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
