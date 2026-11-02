using FactorPolar.Domain.Entities;

namespace FactorPolar.Application.Interfaces
{
    public interface IGrupoEdad
    {
        Task<List<GrupoEdad>?> GetAllAsync();
        Task<GrupoEdad?> GetByIdAsync(int id);
        Task AddAsync(GrupoEdad entity);
        void Update(GrupoEdad entity);
        void Delete(int id);
    }
}

