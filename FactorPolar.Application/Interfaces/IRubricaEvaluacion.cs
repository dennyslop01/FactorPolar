using FactorPolar.Domain.Entities;

namespace FactorPolar.Application.Interfaces
{
    public interface IRubricaEvaluacion
    {
        Task<List<RubricaEvaluacion>?> GetAllAsync();
        Task<List<RubricaEvaluacion>?> GetByIdGrupoAsync(int idgrupo);
        Task<RubricaEvaluacion?> GetByIdAsync(int id);
        Task AddAsync(RubricaEvaluacion entity);
        void Update(RubricaEvaluacion entity);
        void Delete(int id);
    }
}
