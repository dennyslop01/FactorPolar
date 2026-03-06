using FactorPolar.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactorPolar.Application.Interfaces
{
    public interface IBeneficiarioRubrica
    {
        Task<List<BeneficiarioRubrica?>> GetAlllAsync();
        Task<BeneficiarioRubrica?> GetByIdlAsync(int id);
        Task<List<BeneficiarioRubrica?>> GetByIdUserlAsync(int iduser);
        Task<List<BeneficiarioRubrica?>> GetByIdUserBenefilAsync(int iduser, int idbenefi);
    }
}
