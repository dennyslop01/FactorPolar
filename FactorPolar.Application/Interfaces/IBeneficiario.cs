using FactorPolar.Domain.Entities;

namespace FactorPolar.Application.Interfaces
{
    public interface IBeneficiario
    {
        Beneficiario? GetByEmployeeEmail(string email);
        Beneficiario? Create(Beneficiario beneficiario, string email);
        Task<List<Beneficiario?>> GetByEmployeeEmailAsync(string email);
        Task<List<Beneficiario?>> GetByParticipantesAsync();
        Task<Beneficiario?> GetByIdlAsync(int id);
        Task<Beneficiario?> UpdateAsync(int id, int opcion);
        Task<Beneficiario?> UpdateAcademicDataAsync(int id, BenefiModel benefi);
        Task<bool> UpdateRutaNotaAsync(int id, string rutaNota);
        Task<bool> UpdateRutaVideoAsync(int id, string rutaVideo);
    }
}
