using FactorPolar.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactorPolar.Application.Interfaces
{
    public interface IBeneficiario
    {
        Beneficiario? GetByEmployeeEmail(string email);
        Beneficiario? Create(Beneficiario beneficiario, string email);
        Task<List<Beneficiario?>> GetByEmployeeEmailAsync(string email);
        Task<Beneficiario?> GetByIdlAsync(int id);
        Task<Beneficiario?> UpdateAsync(int id, int opcion);
        Task<Beneficiario?> UpdateAcademicDataAsync(int id, BenefiModel benefi);
    }
}
