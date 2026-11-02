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

        public async Task<GrupoEvaluacion?> GetByIdAsync(int id)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<GrupoEvaluacion>()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task AddAsync(GrupoEvaluacion entity)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            await context.Set<GrupoEvaluacion>().AddAsync(entity);

            await context.SaveChangesAsync();
        }

        public void Update(GrupoEvaluacion entity)
        {
            using var context = _contextFactory.CreateDbContext();
            context.Set<GrupoEvaluacion>().Update(entity);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = context.Set<GrupoEvaluacion>().Find(id);
            if (entity != null)
            {
                context.Set<GrupoEvaluacion>().Remove(entity);
                context.SaveChanges();
            }
        }
    }
}