using FactorPolar.Application.Interfaces;
using FactorPolar.Domain.Entities;
using FactorPolar.Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;

namespace FactorPolar.Infrastructure.Repositories
{
    public class BeneficiarioRepository(FactorDbContext context) : IBeneficiario
    {
        private readonly FactorDbContext _context = context;
        public Beneficiario Create(Beneficiario beneficiario, string email)
        {
            EmployeeRepository employeeRepository = new(_context);

            Employee? auxemp = employeeRepository.GetByEmail(email);
            if (auxemp == null)
            {
                Console.WriteLine("El empleado no existe.");
                return null!;
            }
            else
            {
                Beneficiario? benefiaux = _context.Beneficiarios
                    .FirstOrDefault(b => b.Denominacion == beneficiario.Denominacion && 
                                    b.CedulaIdentidad == beneficiario.CedulaIdentidad && 
                                    b.FullName == beneficiario.FullName &&
                                    b.Promedio > 17)!;

                if (benefiaux == null)
                {
                    beneficiario.Employee = auxemp;
                    _context.Beneficiarios.Add(beneficiario);
                    _context.Entry(beneficiario.Employee).State = EntityState.Unchanged;

                    _context.SaveChanges();
                    _context.Entry(beneficiario.Employee).State = EntityState.Detached;
                    _context.Entry(beneficiario).State = EntityState.Detached;
                    return beneficiario;
                }
                else
                {
                    return benefiaux;
                }
            }
        }

        public Beneficiario GetByEmployeeEmail(string email)
        {
            var benefi = _context.Beneficiarios
                .Include(b => b.Employee)
                .FirstOrDefault(x => x.Employee.Email == email);
            return benefi!;
        }

        public async Task<List<Beneficiario?>> GetByEmployeeEmailAsync(string email)
        {
            var benefi = await _context.Beneficiarios
                .Include(b => b.Employee)
                .Where(x => x.Employee.Email == email)
                .AsQueryable().AsNoTracking()
                .ToListAsync();
            return benefi;
        }

        public async Task<Beneficiario?> GetByIdlAsync(int id)
        {
            var benefi = await _context.Beneficiarios
                .Include(b => b.Employee)
                .Where(x => x.Id == id)
                .AsQueryable().AsNoTracking()
                .FirstOrDefaultAsync();
            return benefi;
        }

        public async Task<Beneficiario?> UpdateAsync(int id, int opcion)
        {
            var benefi = await _context.Beneficiarios
                .Include(b => b.Employee)
                .Where(x => x.Id == id)
                .AsQueryable().AsNoTracking()
                .FirstOrDefaultAsync();
            if (benefi == null)
            {
                return null;
            }
            switch (opcion)
            {
                case 1:
                    benefi.Documento1 = "ACEPTO";
                    break;
                case 2:
                    benefi.Documento2 = "ACEPTO";
                    break;
                case 3:
                    benefi.Documento3 = "ACEPTO";
                    break;
            }
            _context.Beneficiarios.Update(benefi);
            _context.Entry(benefi.Employee).State = EntityState.Unchanged;

            await _context.SaveChangesAsync();
            _context.Entry(benefi.Employee).State = EntityState.Detached;
            _context.Entry(benefi).State = EntityState.Detached;
            return benefi;
        }

        public async Task<Beneficiario?> UpdateAcademicDataAsync(int id, BenefiModel benefi)
        {
            var beneficiario = await _context.Beneficiarios
                .Include(b => b.Employee)
                .Where(x => x.Id == id)
                .AsQueryable().AsNoTracking()
                .FirstOrDefaultAsync();
            if (beneficiario == null)
            {
                return null;
            }

            if(beneficiario.TipoInstitucion?.ToUpper() != benefi.TipoInstitucion.ToUpper())
                beneficiario.TipoInstitucionAux = benefi.TipoInstitucion.ToUpper();

            if (beneficiario.NombreInstitucion?.ToUpper() != benefi.NombreInstitucion.ToUpper())
                beneficiario.NombreInstitucionAux = benefi.NombreInstitucion.ToUpper();

            if (beneficiario.RifInstitucion?.ToUpper() != benefi.RifInstitucion.ToUpper())
                beneficiario.RifInstitucionAux = benefi.RifInstitucion.ToUpper();

            if (beneficiario.NivelEducativo?.ToUpper() != benefi.NivelEducativo.ToUpper())
                beneficiario.NivelEducativoAux = benefi.NivelEducativo.ToUpper();

            if (beneficiario.GradoEducativo?.ToUpper() != benefi.GradoEducativo.ToUpper())
                beneficiario.GradoEducativoAux = benefi.GradoEducativo.ToUpper();
            
            beneficiario.Promedio = benefi.Promedio;

            _context.Beneficiarios.Update(beneficiario);
            _context.Entry(beneficiario.Employee).State = EntityState.Unchanged;

            await _context.SaveChangesAsync();
            _context.Entry(beneficiario).State = EntityState.Detached;
            _context.Entry(beneficiario.Employee).State = EntityState.Detached;
            return beneficiario;
        }

        public async Task<List<Beneficiario?>> GetByParticipantesAsync()
        {
            var benefi = await _context.Beneficiarios
                .Include(b => b.Employee)
                .Where(x => x.Promedio >= 18)
                .AsQueryable().AsNoTracking()
                .ToListAsync();
            return benefi;
        }

        public async Task<bool> UpdateRutaNotaAsync(int id, string rutaNota, string nombre)
        {
            var beneficiario = await _context.Beneficiarios
                .Include(b => b.Employee)
                .Where(x => x.Id == id)
                .AsQueryable().AsNoTracking()
                .FirstOrDefaultAsync();
            if (beneficiario == null)
            {
                return false;
            }

            beneficiario.RutaNotas = rutaNota;
            beneficiario.NombreNotas = nombre;

            _context.Beneficiarios.Update(beneficiario);
            _context.Entry(beneficiario.Employee).State = EntityState.Unchanged;

            await _context.SaveChangesAsync();
            _context.Entry(beneficiario).State = EntityState.Detached;
            _context.Entry(beneficiario.Employee).State = EntityState.Detached;
            return true;
        }

        public async Task<bool> UpdateRutaVideoAsync(int id, string rutaVideo, string nombre)
        {
            var beneficiario = await _context.Beneficiarios
                .Include(b => b.Employee)
                .Where(x => x.Id == id)
                .AsQueryable().AsNoTracking()
                .FirstOrDefaultAsync();
            if (beneficiario == null)
            {
                return false;
            }

            beneficiario.RutaVideo = rutaVideo;
            beneficiario.NombreVideo = nombre;

            _context.Beneficiarios.Update(beneficiario);
            _context.Entry(beneficiario.Employee).State = EntityState.Unchanged;

            await _context.SaveChangesAsync();
            _context.Entry(beneficiario).State = EntityState.Detached;
            _context.Entry(beneficiario.Employee).State = EntityState.Detached;
            return true;
        }
    }
}
