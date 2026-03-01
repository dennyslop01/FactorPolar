using FactorPolar.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FactorPolar.Infrastructure.DataContext
{
    public class FactorDbContext(DbContextOptions<FactorDbContext> options) : DbContext(options)
    {
        public DbSet<Credential> Credentials { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Beneficiario> Beneficiarios { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<GrupoEvaluacion> GruposEvaluacion { get; set; }
        public DbSet<RubricaEvaluacion> RubricasEvaluacion { get; set; }
    }
}
