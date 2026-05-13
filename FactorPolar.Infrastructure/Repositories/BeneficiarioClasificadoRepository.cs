using FactorPolar.Application.Interfaces;
using FactorPolar.Domain.Entities;
using FactorPolar.Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactorPolar.Infrastructure.Repositories
{
    public class BeneficiarioClasificadoRepository(IDbContextFactory<FactorDbContext> contextFactory) : IBeneficiarioClasificado
    {
        private readonly IDbContextFactory<FactorDbContext> _contextFactory = contextFactory;

        public async Task<bool> ActivarEnVivo(int id)
        {
            using var _context = _contextFactory.CreateDbContext();

            var benefi = await _context.BeneficiariosClasificados
                .Where(x => x.Beneficiario.Id == id)
                .FirstOrDefaultAsync();
            
            if(benefi == null) return false;

            benefi.Estado = 1;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CancelarEnVivo(int id)
        {
            using var _context = _contextFactory.CreateDbContext();

            var benefi = await _context.BeneficiariosClasificados
                .Where(x => x.Beneficiario.Id == id)
                .FirstOrDefaultAsync();

            if (benefi == null) return false;

            benefi.Estado = 0;

            await _context.SaveChangesAsync();

            IBeneficiarioRubricaEnVivo rubricaEnVivo = new BeneficiarioRubricaEnVivoRepository(_contextFactory);
            await rubricaEnVivo.DeleteAsync(id);
            return true;
        }

        public async Task<bool> CerrarEnVivo(int id)
        {
            using var _context = _contextFactory.CreateDbContext();

            var benefi = await _context.BeneficiariosClasificados
                .Where(x => x.Beneficiario.Id == id)
                .FirstOrDefaultAsync();

            if (benefi == null) return false;

            benefi.Estado = 2;

            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<List<BeneficiarioClasificado?>> GetAlllAsync()
        {
            using var _context = _contextFactory.CreateDbContext();

            var beneficlaf = await _context.BeneficiariosClasificados
                .Include(b => b.Beneficiario)
                .ToListAsync();

            return beneficlaf;
        }

        public async Task<BeneficiarioClasificado?> GetByIdlAsync(int id)
        {
            using var _context = _contextFactory.CreateDbContext();

            var beneficlaf = await _context.BeneficiariosClasificados
                .Include(b => b.Beneficiario)
                .FirstOrDefaultAsync(b => b.Id == id);

            return beneficlaf;
        }
    }
}
