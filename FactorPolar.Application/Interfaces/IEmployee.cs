using FactorPolar.Domain.Entities;

namespace FactorPolar.Application.Interfaces
{
    public interface IEmployee
    {
        Employee? GetById(int id);
        Employee? GetByEmail(string email);
        Task<Employee?> GetByEmailAsync(string email);
        Employee? Create(Employee employee);
        Task<bool> UpdateDispositivoAsync(string email, bool movil);
        Task<Employee?> GetByIdAsync(int id);
        Task<bool> UpdateAceptarTCAsync(int id);

    }
}
