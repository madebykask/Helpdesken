using DH.Helpdesk.Services.Services;
using DH.Helpdesk.WebApi.Infrastructure;
using DH.Helpdesk.WebApi.Infrastructure.Config;
using DH.Helpdesk.WebApi.Models;
using Microsoft.Graph;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

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

            string token = model.IdToken;
                string stsDiscoveryEndpoint = "https://login.microsoftonline.com/common/v2.0/.well-known/openid-configuration";

            var configManager = new ConfigurationManager<OpenIdConnectConfiguration>(
                                    stsDiscoveryEndpoint,
                                    new OpenIdConnectConfigurationRetriever());

            OpenIdConnectConfiguration config = configManager.GetConfigurationAsync().Result;
            TokenValidationParameters validationParameters = new TokenValidationParameters
            {
                //decode the JWT to see what these values should be
                ValidAudience = "f263307c-2182-44c0-9c28-1b8e88c00a7b",
                ValidIssuer = "https://login.microsoftonline.com/a1f945cf-f91f-4b88-a250-a58e3dd50140/v2.0",

                ValidateAudience = true,
                ValidateIssuer = true,
                IssuerSigningKeys = config.SigningKeys,
                ValidateLifetime = true
            };

            JwtSecurityTokenHandler tokendHandler = new JwtSecurityTokenHandler();

            SecurityToken jwt;

            var result = tokendHandler.ValidateToken(token, validationParameters, out jwt);

            var securityToken = jwt as JwtSecurityToken;
            var userName = securityToken.Claims.FirstOrDefault(x => x.Type.Equals("preferred_username", StringComparison.OrdinalIgnoreCase))?.Value;
            var validuser = !string.IsNullOrEmpty(userName) && (userName.Trim() == model.Email.Trim());

            if(validuser)
            {
                //Get User if exists
                var user = _masterDataService.GetUserByEmail(userName);
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
                
                var apa = await graphserviceClient.Me.Request().GetAsync();
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