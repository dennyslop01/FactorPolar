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
            EmployeeRepository employeeRepository = new EmployeeRepository(_context);

            Employee? auxemp = employeeRepository.GetByEmail(email);
            if (auxemp == null)
            {
                Console.WriteLine("El empleado no existe.");
                return null!;
            }
            else
            {
                Beneficiario? benefiaux = _context.Beneficiarios
                    .FirstOrDefault(b => b.Denominacion == beneficiario.Denominacion && b.CedulaIdentidad == beneficiario.CedulaIdentidad && b.FullName == beneficiario.FullName)!;

                if (benefiaux == null)
                {
                    beneficiario.Employee = auxemp;
                    _context.Beneficiarios.Add(beneficiario);
                    _context.Entry(beneficiario.Employee).State = EntityState.Unchanged;

                    _context.SaveChanges();
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
    }
}
