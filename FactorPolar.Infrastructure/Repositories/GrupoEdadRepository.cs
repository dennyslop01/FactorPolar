using FactorPolar.Application.Interfaces;
using FactorPolar.Domain.Entities;
using FactorPolar.Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;

namespace FactorPolar.Infrastructure.Repositories
{
    public class GrupoEdadRepository(IDbContextFactory<FactorDbContext> contextFactory) : IGrupoEdad
    {
        private readonly IDbContextFactory<FactorDbContext> _contextFactory = contextFactory;

        public async Task<List<GrupoEdad>?> GetAllAsync()
        {
            using var _context = _contextFactory.CreateDbContext();

            var grupos = await _context.GruposEdades
                .OrderBy(g => g.EdadMinima)
                .ToListAsync();

            return grupos!;
        }

        public async Task<GrupoEdad?> GetByIdAsync(int id)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<GrupoEdad>()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task AddAsync(GrupoEdad entity)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            await context.Set<GrupoEdad>().AddAsync(entity);

            await context.SaveChangesAsync();
        }

        public void Update(GrupoEdad entity)
        {
            using var context = _contextFactory.CreateDbContext();
            context.Set<GrupoEdad>().Update(entity);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = context.Set<GrupoEdad>().Find(id);
            if (entity != null)
            {
                context.Set<GrupoEdad>().Remove(entity);
                context.SaveChanges();
            }
        }
    }
}

