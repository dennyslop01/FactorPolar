using FactorPolar.Application.Interfaces;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Oauth2.v2;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactorPolar.Infrastructure.Services
{
    public class GoogleAuthHelperSevice(IConfiguration config) : IGoogleAuthHelper
    {
        public ClientSecrets GetClientSecrets()
        {
            string clientId = config["Google:ClientId"]!;
            string clientSecret = config["Google:ClientSecret"]!;
            return new() { ClientId = clientId, ClientSecret = clientSecret };

        }

        public string[] GetScopes()
        {
            var scopes = new[]
            {
                Oauth2Service.Scope.Openid,
                Oauth2Service.Scope.UserinfoEmail,
                Oauth2Service.Scope.UserinfoProfile
            };

            return scopes;
        }

        public string ScopeToString() => string.Join(" ", GetScopes());
    }
}
