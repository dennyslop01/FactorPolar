using FactorPolar.Domain.Entities;

namespace FactorPolar.Application.Interfaces
{
    public interface IEmployee
    {
        Employee? GetById(int id);
        Employee? GetByEmail(string email);
        Employee? Create(Employee employee);
    }
}
