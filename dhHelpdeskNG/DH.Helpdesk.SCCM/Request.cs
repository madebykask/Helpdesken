using log4net.Core;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Authenticators.OAuth2;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.SCCM
{
    public class Request
    {

        static readonly log4net.ILog log =
    log4net.LogManager.GetLogger
    (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        private string BaseURL { get; } = System.Configuration.ConfigurationManager.AppSettings["SCCM_URL_Base"].ToString();

        private RestClient client;

        public Request(string Token)
        {

            //Do not accept an empty BaseURL
            if (String.IsNullOrEmpty(BaseURL))
            {
                throw new Exception("BaseURL is not valid");
            }

            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(Token, "Bearer");

            var options = new RestClientOptions(BaseURL)
            {
                Authenticator = authenticator
            };

            var localClient = new RestClient(options);

            client = localClient;



            //Declare the token
            this.Token = Token;
        }
   
        public string Token { get; set; }
        
        public Task<RestResponse> Get(string endPath) {

            try
            {

                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                log.Info($"baseurl: {BaseURL}");
                log.Info($"Attempting GET request to: {endPath}");


                var request = new RestRequest(endPath, Method.Get);

                log.Info($"Executing GET request...");

                Task<RestResponse> t = client.ExecuteAsync(request);

                return t;
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
