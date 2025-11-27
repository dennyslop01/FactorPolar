using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using FactorPolar.Application.Interfaces;
using FactorPolar.CargaEmpleados;
using FactorPolar.Infrastructure.DataContext;
using FactorPolar.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

class Program
{
    static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        // Ejecutar la lógica principal de la aplicación
        await host.Services.GetRequiredService<ConsoleAppService>().RunAsync();
    }

    static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                // Acceder a la configuración cargada por el host
                IConfiguration configuration = hostContext.Configuration;

                // Usar GetConnectionString para leer del apartado "ConnectionStrings"
                string connectionString = configuration.GetConnectionString("DefaultConn");

                // Configurar DbContext usando la cadena de conexión del appsettings.json
                services.AddDbContext<FactorDbContext>(options =>
                    options.UseSqlServer(connectionString));

                // Registrar el repositorio y la clase principal
                services.AddScoped<IEmployee, EmployeeRepository>();
                services.AddScoped<IBeneficiario, BeneficiarioRepository>();
                services.AddTransient<ConsoleAppService>();
            });
}

//// Build configuration from appsettings.json
//IConfiguration configuration = new ConfigurationBuilder()
//    .SetBasePath(Directory.GetCurrentDirectory())
//    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
//    .Build();

//// Create the Host builder
//var builder = Host.CreateDefaultBuilder(args);
//builder.ConfigureServices((hostContext, services) =>
//{
//    // Register your application's main logic as a service
//    services.AddHostedService<ConsoleAppService>();
//    // Add other services here (e.g., DbContext, logging providers)
//    // services.AddDbContext<ApplicationContext>(options => ...);
//    services.AddDbContext<FactorDbContext>(options =>
//    {
//        options.UseSqlServer(configuration.GetConnectionString("DefaultConn"));
//        options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
//    });
//    services.AddSingleton<IConfiguration>(configuration);

//    services.AddScoped<IEmployee, EmployeeRepository>();

//});

//// Build and run the host (this runs the registered hosted services)
//var host = builder.Build();
//await host.RunAsync();
