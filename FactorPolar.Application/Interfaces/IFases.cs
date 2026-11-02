using FactorPolar.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactorPolar.Application.Interfaces
{
    public interface IFases
    {
        Task<List<Fases>?> GetAllAsync();
        Task<Fases?> GetByIdAsync(int id);
        Task AddAsync(Fases entity);
        void Update(Fases entity);
        void Delete(int id);
    }
}
