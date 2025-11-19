using Google.Apis.Auth.OAuth2;

namespace FactorPolar.Application.Interfaces
{
    public interface IGoogleAuthHelper
    {
        string[] GetScopes();
        string ScopeToString();
        ClientSecrets GetClientSecrets();
    }
}
