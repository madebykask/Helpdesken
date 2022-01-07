using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using DH.Helpdesk.Dal.Repositories;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.WebApi.Infrastructure;
using DH.Helpdesk.WebApi.Infrastructure.Config;
using DH.Helpdesk.WebApi.Models;

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
            var user = _masterDataService.GetUserByEmail("katarina.ask@dhsolutions.se");
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

        /// <summary>
        /// Creates new access token from refresh token
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Access token</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> Refresh([FromBody]RefreshTokenInputModel model)
        {
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
}