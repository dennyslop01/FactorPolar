using FactorPolar.Application.Interfaces;
using FactorPolar.Domain.Entities;
using FactorPolar.Infrastructure.DataContext;
using FactorPolar.Infrastructure.Repositories;
using FactorPolar.Infrastructure.Services;
using FactorPolar.Webapp.Components;
using ITfoxtec.Identity.Saml2;
using ITfoxtec.Identity.Saml2.MvcCore.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using System.Security.Cryptography.X509Certificates;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContextFactory<FactorDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConn")));


builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.Configure<IGoogleDrive>(builder.Configuration.GetSection("GoogleDrive"));

builder.Services.AddDistributedMemoryCache(); // Requerido para almacenar la sesión en memoria
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Tiempo de expiración
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddMudServices();
builder.Services.AddScoped<ICredentialRepository, CredentialRepository>();
builder.Services.AddScoped<IEmployee, EmployeeRepository>();
builder.Services.AddScoped<IBeneficiario, BeneficiarioRepository>();
builder.Services.AddScoped<IUsuario, UsuarioRepository>();
builder.Services.AddScoped<IGrupoEvaluacion, GrupoEvaluacionRepository>();
builder.Services.AddScoped<IRubricaEvaluacion, RubricaEvaluacionRepository>();
builder.Services.AddScoped<IBeneficiarioRubrica, BeneficiarioRubricaRepository>();
builder.Services.AddScoped<GoogleDriveService>();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddHttpClient();

builder.Services.AddServerSideBlazor()
    .AddHubOptions(options =>
    {
        // Tiempo entre pings automáticos para mantener la conexión viva
        options.KeepAliveInterval = TimeSpan.FromSeconds(90);
        // Límite de tamaño de mensaje (vital para evitar cierres por archivos grandes)
        options.MaximumReceiveMessageSize = 512 * 1024 * 1024; // 512 MB
        // Aumentar Timeout es obligatorio para redes móviles 4G/3G
        options.ClientTimeoutInterval = TimeSpan.FromMinutes(20);
        options.HandshakeTimeout = TimeSpan.FromMinutes(10);
    });


builder.Services.Configure<Saml2Configuration>(options =>
{
    var samlConfig = builder.Configuration.GetSection("Saml2");
    var idpConfig = samlConfig.GetSection("IdP");
    var spConfig = samlConfig.GetSection("SP");

    // A. Configuración SP (Nosotros)
    // En ITfoxtec, 'Issuer' es NUESTRO EntityID
    options.Issuer = spConfig["EntityId"].Trim();
    //options.Issuer = "FactorPolar.Webapp"; //options.Issuer = spConfig["EntityId"].Trim();
    //options.Issuer = "FactorPolar.Webapp.Des"; //options.Issuer = spConfig["EntityId"].Trim();

    // B. Configuración IdP (Polar / Azure)
    // 'AllowedIssuer' es el EntityID de ELLOS
    options.AllowedAudienceUris.Add(spConfig["EntityId"].Trim());
    //options.AllowedAudienceUris.Add("FactorPolar.Webapp"); //options.AllowedIssuer = spConfig["EntityId"].Trim();
    //options.AllowedAudienceUris.Add("FactorPolar.Webapp.Des"); //options.AllowedIssuer = spConfig["EntityId"].Trim();

    options.SingleSignOnDestination = new Uri(idpConfig["SingleSignOnDestination"]);
    options.SingleLogoutDestination = new Uri(idpConfig["SingleLogoutDestination"]);

    // Configuración de validación (Importante para evitar errores de certificado en local)
    options.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.None;// ChainTrust;
    options.RevocationMode = X509RevocationMode.NoCheck;

    // C. Carga del Certificado de Polar
    // Asegúrate de que el archivo "Polar_Azure.cer" esté en la raíz del proyecto  
    try
    {
        // Cargar Certificado Público de Polar
        options.SignatureValidationCertificates.Add(new X509Certificate2("Polar_Azure.cer"));
    }
    catch (Exception ex)
    {
        // En desarrollo, esto ayuda a saber si no encuentra el archivo .cer
        Console.WriteLine($"ERROR FATAL CARGANDO CERTIFICADO: {ex.Message}");
    }
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => // <--- ESTA LÍNEA ES CRÍTICA
    {
        options.LoginPath = "/Saml2/Login";
        options.AccessDeniedPath = "/Error";
    });


builder.Services.AddSaml2();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAntiforgery();

app.UseAuthentication();

app.UseSaml2();
app.UseAuthorization();

app.MapControllers();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
