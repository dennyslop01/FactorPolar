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

        public async Task<List<Usuario>> GetAllAsync()
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<Usuario>()
                .ToListAsync();
        }

        public async Task<Usuario?> GetByIdAsync(int id)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<Usuario>()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }
        public async Task AddAsync(Usuario entity)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            await context.Set<Usuario>().AddAsync(entity);

            await context.SaveChangesAsync();
        }

        public void Update(Usuario entity)
        {
            using var context = _contextFactory.CreateDbContext();
            context.Set<Usuario>().Update(entity);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = context.Set<Usuario>().Find(id);
            if (entity != null)
            {
                context.Set<Usuario>().Remove(entity);
                context.SaveChanges();
            }
        }
    }
}