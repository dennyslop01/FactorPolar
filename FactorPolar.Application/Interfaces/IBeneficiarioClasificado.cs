using FactorPolar.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactorPolar.Application.Interfaces
{
    public interface IBeneficiarioClasificado
    {
        Task<List<BeneficiarioClasificado?>> GetAlllAsync();
        Task<BeneficiarioClasificado?> GetByIdlAsync(int id);
        Task<bool> ActivarEnVivo(int id);
        Task<bool> CancelarEnVivo(int id);
        Task<bool> CerrarEnVivo(int id);
    }
}
