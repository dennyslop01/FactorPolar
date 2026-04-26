using FactorPolar.Domain.Entities;

namespace FactorPolar.Application.Interfaces
{
    public interface IBeneficiarioRubricaEnVivo
    {
        Task<List<BeneficiarioRubricaEnVivo?>> GetAlllAsync();
        Task<BeneficiarioRubricaEnVivo?> GetByIdlAsync(int id);
        Task<List<BeneficiarioRubricaEnVivo?>> GetByIdUserlAsync(int iduser);
        Task<List<BeneficiarioRubricaEnVivo?>> GetByIdUserBenefilAsync(int iduser, int idbenefi);
        Task<List<BeneficiarioRubricaEnVivo?>> GetByIdBenefiAsync(int idbenefi);
        Task<bool> CreateAsync(int idusuario, int idrubrica, int idbenefi, int puntuacion);
    }
}
