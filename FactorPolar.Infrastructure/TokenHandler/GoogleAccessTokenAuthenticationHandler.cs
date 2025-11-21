using FactorPolar.Application.Interfaces;
using FactorPolar.Domain.Constants;
using FactorPolar.Domain.Entities;
using FactorPolar.Infrastructure.DataContext;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace FactorPolar.Infrastructure.TokenHandler
{
    public class GoogleAccessTokenAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions>
                options, ILoggerFactory logger, UrlEncoder encoder, TimeProvider timeProvider,
        IGoogleAuthorization googleAuthorization, ICredentialRepository repository) :
        AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
    {
        private readonly TimeProvider timeProvider = timeProvider;

        protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("No existe Authorization Header");

            string authHeader = Request.Headers.Authorization!;
            if (!authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                return AuthenticateResult.Fail("Invalid Authorization Header");

            string accessToken = authHeader["Bearer ".Length..].Trim();
            var userCredential = await googleAuthorization.ValidateToken(accessToken);
            Credential? user = await GetUserCredential(userCredential.Token.AccessToken);
            if (user == null)
                AuthenticateResult.Fail("InvalidAccess Token Provided");

            List<Claim> claims = [new(ClaimTypes.NameIdentifier, user!.UserId.ToString())];
            var identity = new ClaimsIdentity(claims, Constant.Schema);

            return AuthenticateResult.Success(
                new AuthenticationTicket(
                    new ClaimsPrincipal(identity), Constant.Schema));
        }

        private async Task<Credential?> GetUserCredential(string accessToken) => 
             await repository.GetCredentialToken(accessToken);
    }
}
