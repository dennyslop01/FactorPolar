using FactorPolar.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactorPolar.Application.Interfaces
{
    public interface IUsuario
    {
        Task<Usuario?> GetByEmailAsync(string email);
        Task<List<Usuario>> GetAllAsync();
        Task<Usuario?> GetByIdAsync(int id);
        Task AddAsync(Usuario entity);
        void Update(Usuario entity);
        void Delete(int id);
    }
}
