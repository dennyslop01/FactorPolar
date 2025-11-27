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
    public class EmployeeRepository(FactorDbContext context) : IEmployee
    {
        private readonly FactorDbContext _context = context;
        public Employee Create(Employee employee)
        {
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

        public Employee? GetById(int id)
        {
            Employee? employee = _context.Employees
                .FirstOrDefault(x => x.Id == id);
            return employee!;
        }
    }
}
