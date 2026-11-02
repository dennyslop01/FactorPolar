using FactorPolar.Domain.Entities;

namespace FactorPolar.Application.Interfaces
{
    public interface IGrupoEvaluacion
    {
        Task<List<GrupoEvaluacion>?> GetAllAsync();
        Task<GrupoEvaluacion?> GetByIdAsync(int id);
        Task AddAsync(GrupoEvaluacion entity);
        void Update(GrupoEvaluacion entity);
        void Delete(int id);
    }
}
