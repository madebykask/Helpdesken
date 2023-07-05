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
        public async Task<HttpResponseMessage> Login([FromBody] LoginUserInputModel model)
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
            var validMicrosoftUser = CheckValidUserLogin(model);

            if (validMicrosoftUser)
            {
                //Get Helpdesk User if exists from verifyed email
                var users = _masterDataService.GetUsersByEmail(model.Email);

                var user = users.FirstOrDefault(x => x.IsActive == 1);

                if(user == null)
                {
                    var responseString = "{\"error\":\"Invalid user!\",\"error_description\":\"The user name or password is incorrect.\"}";
                    var responseMsg = new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent(responseString, Encoding.UTF8, "application/json")
                    };
                    return responseMsg;
                }

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

        private bool CheckValidUserLogin(MicrosoftUserModel model)
        {
            // The Client ID is used by the application to uniquely identify itself to Microsoft identity platform.
            string clientId = System.Configuration.ConfigurationManager.AppSettings["MicrosoftClientId"] != null ?
                              System.Configuration.ConfigurationManager.AppSettings["MicrosoftClientId"] : "";

            // Tenant is the tenant ID (e.g. contoso.onmicrosoft.com, or 'common' for multi-tenant)
            string tenant = System.Configuration.ConfigurationManager.AppSettings["MicrosoftTenant"] != null ?
                                   System.Configuration.ConfigurationManager.AppSettings["MicrosoftTenant"] : "";

            string auth = System.Configuration.ConfigurationManager.AppSettings["MicrosoftAuthority"] != null ?
                                   System.Configuration.ConfigurationManager.AppSettings["MicrosoftAuthority"] : "";
            // Authority is the URL for authority, composed by Microsoft identity platform endpoint and the tenant name (e.g. https://login.microsoftonline.com/contoso.onmicrosoft.com/v2.0)
            string authority = String.Format(System.Globalization.CultureInfo.InvariantCulture, auth, tenant);
            try
            {
                string token = model.IdToken;
                string stsDiscoveryEndpoint = "https://login.microsoftonline.com/common/v2.0/.well-known/openid-configuration";

                var configManager = new ConfigurationManager<OpenIdConnectConfiguration>(
                                        stsDiscoveryEndpoint,
                                        new OpenIdConnectConfigurationRetriever());

                OpenIdConnectConfiguration config = configManager.GetConfigurationAsync().Result;
                TokenValidationParameters validationParameters = new TokenValidationParameters
                {
                    //ClientId
                    ValidAudience = clientId,
                    ValidIssuer = authority,

                    ValidateAudience = true,
                    ValidateIssuer = false,
                    IssuerSigningKeys = config.SigningKeys,
                    ValidateLifetime = true
                };

                JwtSecurityTokenHandler tokendHandler = new JwtSecurityTokenHandler();

                SecurityToken jwt;

                var result = tokendHandler.ValidateToken(token, validationParameters, out jwt);

                var securityToken = jwt as JwtSecurityToken;
                var userName = securityToken.Claims.FirstOrDefault(x => x.Type.Equals("preferred_username", StringComparison.OrdinalIgnoreCase))?.Value;
                var validuser = !string.IsNullOrEmpty(userName) && (userName.Trim() == model.Email.Trim());
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// Creates new access token from refresh token
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Access token</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> Refresh([FromBody] RefreshTokenInputModel model)
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