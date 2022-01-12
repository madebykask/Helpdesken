using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Azure.Identity;
using DH.Helpdesk.Dal.Repositories;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.WebApi.Infrastructure;
using DH.Helpdesk.WebApi.Infrastructure.Config;
using DH.Helpdesk.WebApi.Models;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace DH.Helpdesk.WebApi.Controllers
{
    /// <summary>
    /// Contains Authentication/Authorization
    /// </summary>
    public class AccountController : BaseApiController
    {
        /// <summary>
        /// User Authentication.
        /// </summary>
        /// <param name="model"></param>
        /// <returns> Returns access token, type, expiration date, refresh token </returns>
        /// Will have a look in this first i think
        /// 
        private readonly IMasterDataService _masterDataService;
        public AccountController(IMasterDataService masterDataService)
        {
            _masterDataService = masterDataService;
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> Login([FromBody]LoginUserInputModel model)
        {
            //var user = _masterDataService.GetUserByEmail("katarina.ask@dhsolutions.se");
            var request = HttpContext.Current.Request;
            var tokenServiceUrl = request.Url.GetLeftPart(UriPartial.Authority) + request.ApplicationPath + ConfigApi.Constants.TokenEndPoint;
            using (var client = new HttpClient())
            {
                var requestParams = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("username", model.Username),
                    new KeyValuePair<string, string>("password", model.Password),
                    new KeyValuePair<string, string>("client_id", model.ClientId)
                };
                var requestParamsFormUrlEncoded = new FormUrlEncodedContent(requestParams);
                var tokenServiceResponse = await client.PostAsync(tokenServiceUrl, requestParamsFormUrlEncoded);
                var responseString = await tokenServiceResponse.Content.ReadAsStringAsync();
                var responseCode = tokenServiceResponse.StatusCode;
                var responseMsg = new HttpResponseMessage(responseCode)
                {
                    Content = new StringContent(responseString, Encoding.UTF8, "application/json")
                };
                return responseMsg;
            }
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> SignInWithMicrosoft([FromBody] MicrosoftUserModel model)
        {
            //Just testing...
            //We must validate the user somehow
            string clientId = "f263307c-2182-44c0-9c28-1b8e88c00a7b";
            string clientSecret = "MbM7Q~dREOk~UJJNgl0Q_vKIWXdiiLkNluwDD";
            string accessToken = model.AccessToken;
            string assertionType = "urn:ietf:params:oauth:grant-type:jwt-bearer";
            //string[] scopes = new string[] { "api://f263307c-2182-44c0-9c28-1b8e88c00a7b/access_as_user" };
            string[] scopes = new string[] { "api://f263307c-2182-44c0-9c28-1b8e88c00a7b/.default" };
            string graphAccessToken = null;

            try
            {
                var app = ConfidentialClientApplicationBuilder
                            .Create(clientId).WithTenantId("a1f945cf-f91f-4b88-a250-a58e3dd50140").WithClientSecret(clientSecret).Build();


                // I get an accesToken from result.AccessToken but dont know what to do with it
                var result = app.AcquireTokenForClient(scopes)
                                .ExecuteAsync().GetAwaiter().GetResult();

                //Testing this
                var testUsers = await TestMS(result.AccessToken);

                //This does not work

                var userAssertion = new UserAssertion(accessToken, assertionType);

                var result2 = app.AcquireTokenOnBehalfOf(scopes, userAssertion)
                .ExecuteAsync().GetAwaiter().GetResult();
                //This always throws 
                //AADSTS50013: Assertion failed signature validation. [Reason - The provided signature value did not match the expected signature value.
                graphAccessToken = result2.AccessToken;
            }
            catch (MsalServiceException ex)
            {
                string aj = ex.Message;
            }
            //IConfidentialClientApplication app;
            //app = ConfidentialClientApplicationBuilder.Create("f263307c-2182-44c0-9c28-1b8e88c00a7b")
            //                                           .WithClientSecret("MbM7Q~dREOk~UJJNgl0Q_vKIWXdiiLkNluwDD")
            //                                          .WithAuthority(new Uri("https://login.microsoftonline.com/common/"))
            //                                          .Build();
            var testMe = await TestMS(accessToken);
            var validuser = true;
            //validuser = CheckUserTokenWithMicrosoft();
            if(validuser)
            {
                //Get User if exists
                var user = _masterDataService.GetUserByEmail(model.Email);
                var request = HttpContext.Current.Request;
                var tokenServiceUrl = request.Url.GetLeftPart(UriPartial.Authority) + request.ApplicationPath + ConfigApi.Constants.TokenEndPoint;
                using (var client = new HttpClient())
                {
                    var requestParams = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("username", user.UserID),
                    new KeyValuePair<string, string>("password", user.Password),
                    new KeyValuePair<string, string>("client_id", model.ClientId)
                };
                    var requestParamsFormUrlEncoded = new FormUrlEncodedContent(requestParams);
                    var tokenServiceResponse = await client.PostAsync(tokenServiceUrl, requestParamsFormUrlEncoded);
                    var responseString = await tokenServiceResponse.Content.ReadAsStringAsync();
                    var responseCode = tokenServiceResponse.StatusCode;
                    var responseMsg = new HttpResponseMessage(responseCode)
                    {
                        Content = new StringContent(responseString, Encoding.UTF8, "application/json")
                    };
                    return responseMsg;
                }
            }
            else
            {
                return null;
            }

        }
        public JwtSecurityToken Validate(string token)
        {
            string stsDiscoveryEndpoint = "https://login.microsoftonline.com/common/.well-known/openid-configuration";

            ConfigurationManager<OpenIdConnectConfiguration> configManager = new ConfigurationManager<OpenIdConnectConfiguration>(stsDiscoveryEndpoint, new OpenIdConnectConfigurationRetriever());

            OpenIdConnectConfiguration config = configManager.GetConfigurationAsync().Result;

            TokenValidationParameters validationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateLifetime = false
            };

            JwtSecurityTokenHandler tokendHandler = new JwtSecurityTokenHandler();

            SecurityToken jwt;

            var result = tokendHandler.ValidateToken(token, validationParameters, out jwt);

            return jwt as JwtSecurityToken;
        }
        public async Task<HttpResponseMessage> TestMS(string token)
        {
            try
            {
                
                var graphserviceClient = new GraphServiceClient(
                new DelegateAuthenticationProvider(
                    (requestMessage) =>
                    {
                        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        return Task.FromResult(0);
                    }));

                //This is wrong
                
                var apa = await graphserviceClient.Users.Request().GetAsync();
            }
            catch(Exception ex)
            {
                //Code: InvalidAuthenticationToken\r\nMessage: Invalid x5t claim
                //{"Code: Authorization_RequestDenied\r\nMessage: Insufficient privileges to complete the operation.\r\nInner error:\r\n\tAdditionalData:\r\n\tdate: 2022-01-12T09:14:51\r\n\trequest-id: 120ffb4d-3522-44a5-a22f-5e66d8200587\r\n\tclient-request-id: 120ffb4d-3522-44a5-a22f-5e66d8200587\r\nClientRequestId: 120ffb4d-3522-44a5-a22f-5e66d8200587\r\n"}
                string aj = ex.Message;
            }
            return null;
        }
        /// <summary>
        /// Creates new access token from refresh token
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Access token</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> Refresh([FromBody]RefreshTokenInputModel model)
        {
            //Maybe issue here if logged in with microsoft...
            //Check here too?
            //Check the user again
            //Check the claimns for authorization...
            //UserClaims, Email

            var request = HttpContext.Current.Request;
            var tokenServiceUrl = request.Url.GetLeftPart(UriPartial.Authority) + request.ApplicationPath + ConfigApi.Constants.TokenEndPoint;
            using (var client = new HttpClient())
            {
                var requestParams = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("grant_type", "refresh_token"),
                    new KeyValuePair<string, string>("refresh_token", model.RefreshToken),
                    new KeyValuePair<string, string>("client_id", model.ClientId)
                };
                var requestParamsFormUrlEncoded = new FormUrlEncodedContent(requestParams);
                var tokenServiceResponse = await client.PostAsync(tokenServiceUrl, requestParamsFormUrlEncoded);
                var responseString = await tokenServiceResponse.Content.ReadAsStringAsync();
                var responseCode = tokenServiceResponse.StatusCode;
                var responseMsg = new HttpResponseMessage(responseCode)
                {
                    Content = new StringContent(responseString, Encoding.UTF8, "application/json")
                };
                return responseMsg;
            }
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            throw new NotImplementedException();
        }
    }

    internal class MSGraphUser
    {
    }
}