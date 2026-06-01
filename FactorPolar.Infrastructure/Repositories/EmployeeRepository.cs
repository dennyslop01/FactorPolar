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
    public class EmployeeRepository(IDbContextFactory<FactorDbContext> contextFactory) : IEmployee
    {
        private readonly IDbContextFactory<FactorDbContext> _contextFactory = contextFactory;

        public Employee Create(Employee employee)
        {
            using var _context = _contextFactory.CreateDbContext();
            
            Employee? aux = GetByEmail(employee.Email);
            if (aux == null)
            {
                _context.Employees.Add(employee);
                _context.SaveChanges();
                return employee;
            }
            else
            {
                return aux;
            }
        }

        public Employee? GetByEmail(string email)
        {
            try
            {
                using var _context = _contextFactory.CreateDbContext();

                Employee? employee = _context.Employees
                    .Where(x => x.Email == email).FirstOrDefault();
                return employee;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener el empleado por email: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        public async Task<Employee?> GetByEmailAsync(string email)
        {
            try
            {
                using var _context = _contextFactory.CreateDbContext();

                Employee? employee = await _context.Employees
                    .Where(x => x.Email == email).FirstOrDefaultAsync();
                return employee;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener el empleado por email: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        public Employee? GetById(int id)
        {
            using var _context = _contextFactory.CreateDbContext();

            Employee? employee = _context.Employees
                .FirstOrDefault(x => x.Id == id);
            return employee!;
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            using var _context = _contextFactory.CreateDbContext();

            Employee? employee = await _context.Employees
                .Where(x => x.Id == id).FirstOrDefaultAsync();
            return employee;
        }

        public async Task<bool> UpdateAceptarTCAsync(int id, int momento)
        {
            using var _context = _contextFactory.CreateDbContext();

            Employee? employee = _context.Employees
                .FirstOrDefault(x => x.Id == id);
            if (employee != null)
            {
                if (momento == 1)
                    employee.FechaAceptarTC = DateTime.Now;
                else
                    employee.FechaAceptarTC2 = DateTime.Now;

                //_context.Employees.Update(employee);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateDispositivoAsync(string email, bool movil)
        {
            using var _context = _contextFactory.CreateDbContext();

            Employee? employee = _context.Employees
                .FirstOrDefault(x => x.Email == email);
            if (employee != null)
            {
                if (movil)
                    employee.IngresoMovil = 1;
                else
                    employee.IngresoCompu = 1;

                //_context.Employees.Update(employee);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
