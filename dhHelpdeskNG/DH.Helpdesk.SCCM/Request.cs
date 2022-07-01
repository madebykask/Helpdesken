using RestSharp;
using RestSharp.Authenticators.OAuth2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.SCCM
{
    public class Request
    {
        private string BaseURL { get; } = System.Configuration.ConfigurationManager.AppSettings["SCCM_URL_Base"].ToString();

        private RestClient client;

        public Request(string Token)
        {
            //Do not accept an empty BaseURL
            if (String.IsNullOrEmpty(BaseURL))
            {
                throw new Exception("BaseURL is not valid");
            }

            //Setup the client
            client = new RestClient(BaseURL);
            client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(Token, "Bearer");

            //Declare the token
            this.Token = Token;
        }
   
        public string Token { get; set; }
        
        public RestResponse Get(string endPath) {
            

            var request = new RestRequest(endPath, Method.Get);
 
            var response = client.Execute(request);

            return response;       


        }

    }
}
