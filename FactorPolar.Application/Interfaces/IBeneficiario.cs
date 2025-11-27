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
    }
}
