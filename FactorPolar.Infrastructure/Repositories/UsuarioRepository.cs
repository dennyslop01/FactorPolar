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
    public class UsuarioRepository(FactorDbContext context) : IUsuario
    {
        private readonly FactorDbContext _context = context;
        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            try
            {
                Usuario? usuario = _context.Usuarios
                    .Where(x => x.Email == email).FirstOrDefault();
                return usuario;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener el empleado por email: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }
    }
}
