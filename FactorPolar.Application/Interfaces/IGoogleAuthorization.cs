using Google.Apis.Auth.OAuth2;

namespace FactorPolar.Application.Interfaces
{
    public interface IGoogleAuthorization
    {
        string GetGoogleAuthorizationUrl();
        Task<UserCredential> ExchangeCodeForToken(string code);
        Task<UserCredential> ValidateToken(string code);
    }
}
