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

        public async Task<RubricaEvaluacion?> GetByIdAsync(int id)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<RubricaEvaluacion>()
                .Include(b => b.GrupoEvaluacion)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }
        public async Task AddAsync(RubricaEvaluacion entity)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            if (entity.GrupoEvaluacion != null)
            {
                context.Attach(entity.GrupoEvaluacion);
            }
            await context.Set<RubricaEvaluacion>().AddAsync(entity);

            await context.SaveChangesAsync();
        }

        public void Update(RubricaEvaluacion entity)
        {
            using var context = _contextFactory.CreateDbContext();
            if (entity.GrupoEvaluacion != null)
            {
                context.Entry(entity.GrupoEvaluacion).State = EntityState.Unchanged;
            }
            context.Set<RubricaEvaluacion>().Update(entity);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = context.Set<RubricaEvaluacion>().Find(id);
            if (entity != null)
            {
                context.Set<RubricaEvaluacion>().Remove(entity);
                context.SaveChanges();
            }
        }
    }
}
