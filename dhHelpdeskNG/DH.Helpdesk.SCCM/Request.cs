using log4net.Core;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Authenticators.OAuth2;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DH.Helpdesk.SCCM
{
    public class Request
    {

        static readonly log4net.ILog log =
    log4net.LogManager.GetLogger
    (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        private string BaseURL { get; } = System.Configuration.ConfigurationManager.AppSettings["SCCM_URL_Base"].ToString();

        private HttpClient client;

        public Request(string token)
        {
            if (string.IsNullOrEmpty(BaseURL))
            {
                throw new Exception("BaseURL is not valid");
            }


            // Initialize a new HttpClient instance
            client = new HttpClient
            {
                // Set the base address
                BaseAddress = new Uri(BaseURL)
            };

            // Set the Authorization header with the token
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


            this.Token = token;
        }

        public string Token { get; set; }

        public async Task<HttpResponseMessage> Get(string endPath)
        {
            try
            {

                //log.Info($"baseurl: {BaseURL}");
                //log.Info($"Attempting GET request to: {endPath}");

                var response = await client.GetAsync(endPath);

                //log.Info($"GET request completed. Result Status: {response.StatusCode}");

                return response;
            }
            catch (Exception ex)
            {
                log.Error($"Exception occurred in GET request to: {endPath}");
                log.Error($"Exception Message: {ex.Message}");
                log.Error($"Exception StackTrace: {ex.StackTrace}");

                if (ex.InnerException != null)
                {
                    log.Error($"Inner Exception Message: {ex.InnerException.Message}");
                    log.Error($"Inner Exception StackTrace: {ex.InnerException.StackTrace}");
                }

                throw;
            }
        }

    }
}
