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
    public class BeneficiarioRubricaRepository(IDbContextFactory<FactorDbContext> contextFactory) : IBeneficiarioRubrica
    {
        private readonly IDbContextFactory<FactorDbContext> _contextFactory = contextFactory;

        public async Task<List<BeneficiarioRubrica?>> GetAlllAsync()
        {
            using var _context = _contextFactory.CreateDbContext();

            var benefirub = await _context.BeneficiariosRubricas
                .Include(b => b.Usuario)
                .Include(b => b.Rubrica)
                .Include(b => b.Beneficiario)
                .ToListAsync();

            return benefirub;
        }

        public async Task<BeneficiarioRubrica?> GetByIdlAsync(int id)
        {
            using var _context = _contextFactory.CreateDbContext();

            var benefirub = await _context.BeneficiariosRubricas
                .Include(b => b.Usuario)
                .Include(b => b.Rubrica)
                .Include(b => b.Beneficiario)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            return benefirub;
        }

        public async Task<List<BeneficiarioRubrica?>> GetByIdUserBenefilAsync(int iduser, int idbenefi)
        {
            using var _context = _contextFactory.CreateDbContext();

            var benefirub = await _context.BeneficiariosRubricas
                .Include(b => b.Usuario)
                .Include(b => b.Rubrica)
                .Include(b => b.Beneficiario)
                .Where(x => x.Usuario.Id == iduser && x.Beneficiario.Id == idbenefi)
                .ToListAsync();

            return benefirub;
        }

        public async Task<List<BeneficiarioRubrica?>> GetByIdUserlAsync(int iduser)
        {
            using var _context = _contextFactory.CreateDbContext();

            var benefirub = await _context.BeneficiariosRubricas
                .Include(b => b.Usuario)
                .Include(b => b.Rubrica)
                .Include(b => b.Beneficiario)
                .Where(x => x.Usuario.Id == iduser)
                .ToListAsync();

            return benefirub;
        }
    }
}
