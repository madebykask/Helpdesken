using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Stores;


namespace Helpdesk.WebApi.Config.IdentityServer
{
    public class CustomClientStore: IClientStore
    {
        private static IEnumerable<Client> AllClients { get; } = new[]
        {
            new Client
            {
                ClientId = "js",
                ClientName = "JavaScript Client",
                AllowedGrantTypes = GrantTypes.Implicit,
                AllowAccessTokensViaBrowser = true,
                RequireClientSecret = false,
                RequireConsent = false,
                RedirectUris =           { "http://localhost:8111/callback.html" },
                PostLogoutRedirectUris = { "http://localhost:8111/index.html" },
                //AllowedCorsOrigins =     { "http://localhost:8111" }, // if commented, than DefaultCorsPolicyService is used.

                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    //IdentityServerConstants.StandardScopes.Profile,
                    Settings.Authorization.Scopes.ServerApiScopeName
                }
            }
        };

        public Task<Client> FindClientByIdAsync(string clientId)
        {
            return Task.FromResult(AllClients.FirstOrDefault(c => c.ClientId == clientId));
        }
    }
}
