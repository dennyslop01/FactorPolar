using FactorPolar.Application.Interfaces;
using FactorPolar.Domain.Entities;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Microsoft.Extensions.Configuration;

namespace FactorPolar.Infrastructure.Services
{
    public class GoogleAuthorizationService(IGoogleAuthHelper googleHelper, IConfiguration config, ICredentialRepository repository) : IGoogleAuthorization
    {
        private string RedirecUrl = config["Google:RedirectUri"]!;
        public async Task<UserCredential> ExchangeCodeForToken(string code)
        {
            var flow = new GoogleAuthorizationCodeFlow(
                new GoogleAuthorizationCodeFlow.Initializer
                {
                    ClientSecrets = googleHelper.GetClientSecrets(),
                    Scopes = googleHelper.GetScopes()
                });

            var token = await flow.ExchangeCodeForTokenAsync(
                "user", code, RedirecUrl, CancellationToken.None);

            Credential credential = new Credential()
            {
                AccessToken = token.AccessToken,
                RefreshToken = token.RefreshToken,
                ExpiresInSeconds = token.ExpiresInSeconds,
                IdToken = token.IdToken,
                UserId = Guid.NewGuid(),
                IssuedUtc = token.IssuedUtc
            };

            await repository.CreateCredential(credential);

            return new UserCredential(flow, "user", token);
        }

        public string GetGoogleAuthorizationUrl() =>
            new GoogleAuthorizationCodeFlow(
                new GoogleAuthorizationCodeFlow.Initializer
                {
                    ClientSecrets = googleHelper.GetClientSecrets(),
                    Scopes = googleHelper.GetScopes(),
                    Prompt = "consent"
                }).CreateAuthorizationCodeRequest(RedirecUrl).Build().ToString();

        public async Task<UserCredential> ValidateToken(string accessToken)
        {
            Credential credential = await repository.GetCredentialToken(accessToken);

            if (credential == null)
                throw new Exception("No Authentication Token found");

            var flow = new GoogleAuthorizationCodeFlow(
                new GoogleAuthorizationCodeFlow.Initializer
                {
                    ClientSecrets = googleHelper.GetClientSecrets(),
                    Scopes = googleHelper.GetScopes()
                });

            var tokenResponse = new TokenResponse
            {
                AccessToken = credential.AccessToken,
                RefreshToken = credential.RefreshToken,
                ExpiresInSeconds = credential.ExpiresInSeconds,
                IdToken = credential.IdToken,
                IssuedUtc = credential.IssuedUtc
            };
            return new UserCredential(flow, "user", tokenResponse);
        }
    }
}