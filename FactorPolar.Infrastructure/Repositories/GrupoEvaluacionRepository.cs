using FactorPolar.Application.Interfaces;
using FactorPolar.Domain.Entities;
using FactorPolar.Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace FactorPolar.Infrastructure.Repositories
{
    public class GrupoEvaluacionRepository(IDbContextFactory<FactorDbContext> contextFactory) : IGrupoEvaluacion
    {
        private readonly IDbContextFactory<FactorDbContext> _contextFactory = contextFactory;

        public async Task<List<GrupoEvaluacion>?> GetAllAsync()
        {
            using var _context = _contextFactory.CreateDbContext();

            var grupos = await _context.GruposEvaluacion.ToListAsync();

            return grupos!;
        }
    }
}
