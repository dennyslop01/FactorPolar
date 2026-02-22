using FactorPolar.Application.Interfaces;
using FactorPolar.Domain.Entities;
using FactorPolar.Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactorPolar.Infrastructure.Repositories
{
    // Cambiamos el constructor para recibir la fábrica
    public class UsuarioRepository(IDbContextFactory<FactorDbContext> contextFactory) : IUsuario
    {
        private readonly IDbContextFactory<FactorDbContext> _contextFactory = contextFactory;

        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            // Creamos una instancia local para esta operación específica
            using var context = _contextFactory.CreateDbContext();

            try
            {
                Usuario? usuario = await context.Usuarios
                    .Where(x => x.Email == email)
                    .FirstOrDefaultAsync();
                return usuario;
            }
            catch (Exception ex)
            {
                // Es recomendable usar un Logger en lugar de Console.WriteLine
                Console.WriteLine($"Error al obtener el empleado por email: {ex.Message}");
                throw;
            }
        }
    }
}
