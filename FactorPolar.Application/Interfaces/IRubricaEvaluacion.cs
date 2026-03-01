using FactorPolar.Domain.Entities;

namespace FactorPolar.Application.Interfaces
{
    public interface IRubricaEvaluacion
    {
        Task<List<RubricaEvaluacion>?> GetAllAsync();
        Task<List<RubricaEvaluacion>?> GetByIdGrupoAsync(int idgrupo);
    }
}
