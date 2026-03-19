using FactorPolar.Application.Interfaces;
using FactorPolar.Domain.Entities;
using FactorPolar.Infrastructure.DataContext;
using Google.Apis.Drive.v3.Data;
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

        public async Task<bool> CloseEvaluationAsync(int idusuario, int idbenefi)
        {
            using var _context = _contextFactory.CreateDbContext();

            var usuario = await _context.Usuarios.Where(x => x.Id == idusuario).FirstOrDefaultAsync();
            if (usuario == null)
            {
                throw new Exception("Usuario no existe");
            }

            var beneficiario = await _context.Beneficiarios.Where(x => x.Id == idbenefi).FirstOrDefaultAsync();
            if (beneficiario == null)
            {
                throw new Exception("Beneficiario no existe");
            }

            //var rubricas = await _context.BeneficiariosRubricas
            //    .Where(x => x.Beneficiario.Id == idbenefi && x.Usuario.Id == idusuario).ToListAsync();

            await _context.BeneficiariosRubricas
                .Where(x => x.Beneficiario.Id == idbenefi && x.Usuario.Id == idusuario)
                .ExecuteUpdateAsync(setters => setters
                .SetProperty(b => b.CloseDate, DateTime.Now));

            //await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CreateAsync(int idusuario, int idrubrica, int idbenefi, int puntuacion)
        {
            using var _context = _contextFactory.CreateDbContext();

            var usuario = await _context.Usuarios.Where(x => x.Id == idusuario).FirstOrDefaultAsync();
            if (usuario == null)
            {
                throw new Exception("Usuario no existe");
            }

            var rubrica = await _context.RubricasEvaluacion
                .Include(b => b.GrupoEvaluacion)
                .Where(x => x.Id == idrubrica).FirstOrDefaultAsync();
            if (rubrica == null)
            {
                throw new Exception("Rubrica no existe");
            }

            var beneficiario = await _context.Beneficiarios.Where(x => x.Id == idbenefi).FirstOrDefaultAsync();
            if (beneficiario == null)
            {
                throw new Exception("Beneficiario no existe");
            }

            decimal factor = 0;
            if (rubrica.GrupoEvaluacion.Id == 1 || rubrica.GrupoEvaluacion.Id == 5)
            {
                if (puntuacion < 1 || puntuacion > 2)
                {
                    throw new Exception("Puntaje no admitido");
                }
                else
                {
                    if (puntuacion == 2)
                    {
                        puntuacion = 0;
                    }
                    factor = puntuacion;
                }
            }
            else
            {
                factor = puntuacion * rubrica.Factor;
            }

            var rubricaExist = await _context.BeneficiariosRubricas
                .Where(x => x.RubricaEvaluacion.Id == idrubrica && x.Beneficiario.Id == idbenefi && x.Usuario.Id == idusuario)
                .FirstOrDefaultAsync();

            if (rubricaExist == null)
            {
                var benefirub = new BeneficiarioRubrica
                {
                    Usuario = usuario,
                    RubricaEvaluacion = rubrica,
                    Beneficiario = beneficiario,
                    Puntuacion = puntuacion,
                    FactorResultado = factor,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now
                };
                await _context.BeneficiariosRubricas.AddAsync(benefirub);
            }
            else
            {
                rubricaExist.Usuario = usuario;
                rubricaExist.RubricaEvaluacion = rubrica;
                rubricaExist.Beneficiario = beneficiario;
                rubricaExist.Puntuacion = puntuacion;
                rubricaExist.FactorResultado = factor;
                rubricaExist.UpdateDate = DateTime.Now;
            }
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<BeneficiarioRubrica?>> GetAlllAsync()
        {
            using var _context = _contextFactory.CreateDbContext();

            var benefirub = await _context.BeneficiariosRubricas
                .Include(b => b.Usuario)
                .Include(b => b.RubricaEvaluacion)
                .Include(b => b.RubricaEvaluacion.GrupoEvaluacion)
                .Include(b => b.Beneficiario)
                .ToListAsync();

            return benefirub;
        }

        public async Task<List<BeneficiarioRubrica?>> GetByIdBenefiAsync(int idbenefi)
        {
            using var _context = _contextFactory.CreateDbContext();

            var benefirub = await _context.BeneficiariosRubricas
                .Include(b => b.Usuario)
                .Include(b => b.RubricaEvaluacion)
                .Include(b => b.RubricaEvaluacion.GrupoEvaluacion)
                .Include(b => b.Beneficiario)
                .Where(x => x.Beneficiario.Id == idbenefi)
                .ToListAsync();

            return benefirub;
        }

        public async Task<BeneficiarioRubrica?> GetByIdlAsync(int id)
        {
            using var _context = _contextFactory.CreateDbContext();

            var benefirub = await _context.BeneficiariosRubricas
                .Include(b => b.Usuario)
                .Include(b => b.RubricaEvaluacion)
                .Include(b => b.RubricaEvaluacion.GrupoEvaluacion)
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
                .Include(b => b.RubricaEvaluacion)
                .Include(b => b.RubricaEvaluacion.GrupoEvaluacion)
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
                .Include(b => b.RubricaEvaluacion)
                .Include(b => b.RubricaEvaluacion.GrupoEvaluacion)
                .Include(b => b.Beneficiario)
                .Where(x => x.Usuario.Id == iduser)
                .ToListAsync();

            return benefirub;
        }
    }
}
