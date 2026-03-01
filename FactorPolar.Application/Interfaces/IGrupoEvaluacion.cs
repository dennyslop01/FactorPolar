using FactorPolar.Domain.Entities;

namespace FactorPolar.Application.Interfaces
{
    public interface IGrupoEvaluacion
    {
        Task<List<GrupoEvaluacion>?> GetAllAsync();
    }
}
