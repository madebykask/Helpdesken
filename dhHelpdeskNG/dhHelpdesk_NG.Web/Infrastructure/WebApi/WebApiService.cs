
using System;
using System.Net.Http;
using System.Threading.Tasks;
using DH.Helpdesk.Web.Models.WebApi;
using System.Net.Http.Headers;
using System.Collections.Generic;

namespace DH.Helpdesk.Web.Infrastructure.WebApi
{
    public interface IWebApiService
    {
        Task<SimpleToken> GetAccessToken(string userId, string password);
    }

    public class WebApiService: IWebApiService
    {
        private readonly string _TOKEN_END_POINT = "/token";
        private readonly string _BASE_URL = "";

        public WebApiService(string baseUrl)
        {
            _BASE_URL = baseUrl;
        }

        public async Task<SimpleToken> GetAccessToken(string userId, string password)
        {            
            var res = await GetToken(userId, password);
            return res;
        }

        private async Task<SimpleToken> GetToken(string userId, string password)
        {
            /* IMPORTANT: Active this line only on the debug mode  */
            //ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            
            var handler = new HttpClientHandler();
            using (var client = new HttpClient(handler))
            {                
                client.BaseAddress = new Uri(_BASE_URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
               
                var inputData = new FormUrlEncodedContent(new[] {
                  new KeyValuePair<string, string>("username", userId),
                  new KeyValuePair<string, string>("password", password),
                  new KeyValuePair<string, string>("grant_type", "password")                  
                });

                inputData.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded");                
                var response = await client.PostAsync(_TOKEN_END_POINT, inputData);                    
                if (response.IsSuccessStatusCode)
                {
                    var res = await response.Content.ReadAsAsync<SimpleToken>();
                    return res;
                }
            }

            return null;
        }
    }
}
