

/*
   Note :
    * Web Services in NewSelefService is defined here 
      because DH-HelpDesk is using framework  4.0, after upgradion framework to 4.5 
      WebService folder can be move to the Services project folder.
 */

namespace DH.Helpdesk.NewSelfService.WebServices
{    
    using System;
    using System.IO;
    using System.Web;    
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Common.Enums;
    using System.Globalization;
    using System.Threading.Tasks;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Web.Mvc;
    using System.Configuration;
    using Newtonsoft.Json;
    using DH.Helpdesk.Common.Classes.ServiceAPI.AMAPI.Output;


    public class APIInfo
    {
        public APIInfo()
        {
        }

        public string UriPath { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
    }

   


    public class AMAPIService : IAMAPIService
    {
        
        #region Public Methods and Operators
 
        public async Task<bool> IsEmployeeManager (string employeeNumber)        
        {            
            var res = await WS_IsEmployeeManager(employeeNumber);            
            return res;            
        }

        public async Task<APIEmployee> GetEmployeeFor(string employeeNum)
        {
            var res = await WS_GetEmployeesFor(employeeNum);
            return res;
        }

        [AllowAnonymous]
        private async Task<bool> WS_IsEmployeeManager(string employeeNumber)        
        {
            /* IMPORTANT: Active this line only on debug mode */
            //ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            var apiInfo = GetApiInfo();
            var handler = new HttpClientHandler { Credentials = new NetworkCredential(apiInfo.UserName, apiInfo.Password) };
            using (var client = new HttpClient(handler))
            {                
                client.BaseAddress = new Uri(apiInfo.UriPath);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync(String.Format("api/ismanager/{0}", employeeNumber));
                if (response.IsSuccessStatusCode)
                {            
                    var res = await response.Content.ReadAsAsync<bool>();
                    return res;
                }                
            }

            return false;            
            
        }

        [AllowAnonymous]
        private async Task<APIEmployee> WS_GetEmployeesFor(string employeeNum)
        {
            /* IMPORTANT: Active this line only on debug mode */
            //ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            var res = new APIEmployee() { IsManager = false, Subordinates = null };         
            var apiInfo = GetApiInfo();
            var handler = new HttpClientHandler { Credentials = new NetworkCredential(apiInfo.UserName, apiInfo.Password) };
            using (var client = new HttpClient(handler))
            {                
                client.BaseAddress = new Uri(apiInfo.UriPath);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                
                var response = await client.GetAsync(String.Format("api/manager/{0}/$all", employeeNum));
                if (response.IsSuccessStatusCode)
                {
                    var res1 = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<APIEmployee>(res1);                    
                }
            }            
            return res;

        }

        private APIInfo GetApiInfo()
        {
            var info = new APIInfo()
            {   
                UriPath = ConfigurationManager.AppSettings["AM_Api_UriPath"].ToString(),
                UserName = ConfigurationManager.AppSettings["AM_Api_UserName"].ToString(),
                Password = ConfigurationManager.AppSettings["AM_Api_Password"].ToString()
            };

            return info;
        }
        
        #endregion
    }
}