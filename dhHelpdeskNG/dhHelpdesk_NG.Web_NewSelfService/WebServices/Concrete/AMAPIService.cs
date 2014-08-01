

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
    
    public class AMAPIService : IAMAPIService
    {                    

        #region Public Methods and Operators
 
        public async Task<bool> IsEmployeeManager (string employeeNumber)        
        {            
            var res = await WS_IsEmployeeManager(employeeNumber);            
            return res;            
        }
        
        public async Task<string> GetEmployeeFor(string managerEmployeeNum)
        {
            var res = await WS_GetEmployeeFor(managerEmployeeNum);
            return res;
        }

        [AllowAnonymous]
        private async Task<bool> WS_IsEmployeeManager(string employeeNumber)        
        {
            var handler = new HttpClientHandler { Credentials = new NetworkCredential("AdminTest1", "asd123!") };
            using (var client = new HttpClient(handler))
            {
                var uri = ConfigurationManager.AppSettings["dh_AccessManagmentUriAddress"].ToString();
                client.BaseAddress = new Uri(uri);
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
        private async Task<string> WS_GetEmployeeFor(string managerEmployeeNum)
        {
            var handler = new HttpClientHandler { Credentials = new NetworkCredential("AdminTest1", "asd123!") };
            using (var client = new HttpClient(handler))
            {
                var uri = ConfigurationManager.AppSettings["dh_AccessManagmentUriAddress"].ToString();
                client.BaseAddress = new Uri(uri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync(String.Format("api/Subordinates/{0}", managerEmployeeNum));
                if (response.IsSuccessStatusCode)
                {
                    var res = await response.Content.ReadAsStringAsync();
                    return res;
                }

            }            
            return "";

        }

        #endregion
    }
}