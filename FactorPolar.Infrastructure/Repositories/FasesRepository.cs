using FactorPolar.Application.Interfaces;
using FactorPolar.Domain.Entities;
using FactorPolar.Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactorPolar.Infrastructure.Repositories
{
    public class FasesRepository(IDbContextFactory<FactorDbContext> contextFactory) : IFases
    {
        private readonly IDbContextFactory<FactorDbContext> _contextFactory = contextFactory;

        public async Task<List<Fases>?> GetAllAsync()
        {
            using var _context = _contextFactory.CreateDbContext();

            var fases = await _context.Fases.ToListAsync();

            return fases!;
        }

        public async Task<Fases?> GetByIdAsync(int id)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<Fases>()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task AddAsync(Fases entity)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            await context.Set<Fases>().AddAsync(entity);

            await context.SaveChangesAsync();

            if (entity.Estatus == 1)
            {
                await InhabilitarOtrasFasesAsync(entity.Id);
            }
        }

        public void Update(Fases entity)
        {
            using var context = _contextFactory.CreateDbContext();
            context.Set<Fases>().Update(entity);
            context.SaveChanges();

            if (entity.Estatus == 1)
            {
                var otrasFases = context.Fases
                    .Where(f => f.Id != entity.Id && f.Estatus != 0)
                    .ToList();
                foreach (var fase in otrasFases)
                {
                    fase.Estatus = 0; // Inhabilitar la fase
                    context.Fases.Update(fase);
                }
                if (otrasFases.Count > 0)
                {
                    context.SaveChanges();
                }
            }
        }

        public void Delete(int id)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = context.Set<Fases>().Find(id);
            if (entity != null)
            {
                context.Set<Fases>().Remove(entity);
                context.SaveChanges();
            }
        }

        private async Task InhabilitarOtrasFasesAsync(int idFaseActiva)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            var otrasFases = await context.Fases
                .Where(f => f.Id != idFaseActiva)
                .ToListAsync();
            foreach (var fase in otrasFases)
            {
                fase.Estatus = 0; // Inhabilitar la fase
                context.Fases.Update(fase);
            }
            await context.SaveChangesAsync();
        }
    }
}
