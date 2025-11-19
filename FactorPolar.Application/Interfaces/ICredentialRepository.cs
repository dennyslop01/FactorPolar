using FactorPolar.Domain.Entities;

namespace FactorPolar.Application.Interfaces
{
    public interface ICredentialRepository
    {
        Task<Credential> GetCredentialToken(string accessToken);
        Task<Credential> GetCredentialUserId(Guid userId);
        Task<bool> CreateCredential(Credential credential);
    }
}
