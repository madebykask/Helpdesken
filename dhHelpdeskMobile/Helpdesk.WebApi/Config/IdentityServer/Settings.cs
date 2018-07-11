using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace Helpdesk.WebApi.Config.IdentityServer
{
    public static class Settings
    {
        public static class Authorization
        {
            public const string AuthorityName = "https://localhost:449";

            public static class Scopes
            {
                public const string ServerApiScopeName = "dhhelpdeskapi";
            }
        };

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                //new IdentityResources.Profile(),
            };

        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource(Authorization.Scopes.ServerApiScopeName, "DH Helpdesk API")
            };
        }

        // public static IEnumerable<Client> GetClients()
        //{
        //    // client credentials client
        //    return new List<Client>
        //    {

        //        // OpenID Connect implicit flow client (MVC)
        //    //    new Client
        //    //    {
        //    //        ClientId = "mvc",
        //    //        ClientName = "MVC Client",
        //    //        AllowedGrantTypes = GrantTypes.Implicit,

        //    //        RedirectUris = { "https://localhost:5002/signin-oidc" },
        //    //        PostLogoutRedirectUris = { "https://localhost:5002/signout-callback-oidc" },

        //    //        AllowedScopes =
        //    //        {
        //    //            IdentityServerConstants.StandardScopes.OpenId,
        //    //            //IdentityServerConstants.StandardScopes.Profile
        //    //        }
        //    //    }
        //        new Client
        //        {
        //            ClientId = "js",
        //            ClientName = "JavaScript Client",
        //            AllowedGrantTypes = GrantTypes.Implicit,
        //            AllowAccessTokensViaBrowser = true,

        //            RedirectUris =           { "http://localhost:8111/callback.html" },
        //            PostLogoutRedirectUris = { "http://localhost:8111/index.html" },
        //            //AllowedCorsOrigins =     { "http://localhost:8111" }, // if commented, than DefaultCorsPolicyService is used.

        //            AllowedScopes =
        //            {
        //                IdentityServerConstants.StandardScopes.OpenId,
        //                //IdentityServerConstants.StandardScopes.Profile,
        //                Authorization.Settings.ServerApiScopeName
        //            }
        //        }
        //    };
        //}

        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "hg",
                    Password = "hg",

                    Claims = new List<Claim>
                    {
                        new Claim("name", "Alice"),
                        new Claim("website", "https://alice.com")
                    }
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "js",
                    Password = "js",

                    Claims = new List<Claim>
                    {
                        new Claim("name", "Bob"),
                        new Claim("website", "https://bob.com")
                    }
                }
            };
}
    }
}
