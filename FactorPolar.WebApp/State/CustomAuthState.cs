using FactorPolar.Domain.Constants;
using FactorPolar.Domain.Entities;
using Microsoft.AspNetCore.Components.Authorization;
using NetcodeHub.Packages.Extensions.LocalStorage;
using System.Security.Claims;
using System.Text.Json;

namespace FactorPolar.WebApp.State
{
    public class CustomAuthState(ILocalStorageService localStorageService) : AuthenticationStateProvider
    {
        private ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try 
            {
                var token = await localStorageService.GetItemAsStringAsync(Constant.Key);
                if (!string.IsNullOrEmpty(token))
                {
                    var tokenModel = JsonSerializer.Deserialize<Token>(token);
                    claimsPrincipal = SetClaimPrincipal(tokenModel!.UserId, tokenModel!.email, tokenModel!.name);
                    return await Task.FromResult(new AuthenticationState(claimsPrincipal));
                }
                return await Task.FromResult(new AuthenticationState(claimsPrincipal));
            }
            catch { return await Task.FromResult(new AuthenticationState(claimsPrincipal)); }
        }

        private ClaimsPrincipal SetClaimPrincipal(string userId, string email, string name)
        {
            try
            {
                Claim[] claims = [new(ClaimTypes.NameIdentifier, userId), new(ClaimTypes.Email, email), new(ClaimTypes.Name, name)];
                return new ClaimsPrincipal(new ClaimsIdentity(claims, Constant.Schema));
            }
            catch { return new ClaimsPrincipal(new ClaimsIdentity()); }
        }

        public void NotifyAuthStateChanged() =>
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
    }
}
