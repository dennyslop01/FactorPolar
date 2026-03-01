using FactorPolar.Application.Interfaces;
using FactorPolar.Domain.Entities;
using FactorPolar.Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;

namespace FactorPolar.Infrastructure.Repositories
{
    public class RubricaEvaluacionRepository(IDbContextFactory<FactorDbContext> contextFactory) : IRubricaEvaluacion
    {
        private readonly IDbContextFactory<FactorDbContext> _contextFactory = contextFactory;

        public async Task<List<RubricaEvaluacion>?> GetAllAsync()
        {
            using var _context = _contextFactory.CreateDbContext();

            var rubricas = await _context.RubricasEvaluacion
                .Include(b => b.GrupoEvaluacion)
                .ToListAsync();

            return rubricas;
        }

        public async Task<List<RubricaEvaluacion>?> GetByIdGrupoAsync(int idgrupo)
        {
            using var _context = _contextFactory.CreateDbContext();

            var rubricas = await _context.RubricasEvaluacion
                .Include(b => b.GrupoEvaluacion)
                .Where(b => b.GrupoEvaluacion.Id == idgrupo)
                .ToListAsync();

            return rubricas;
        }
    }
}
