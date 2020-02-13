using System;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using DH.Helpdesk.BusinessData.Models.Employee;
using DH.Helpdesk.BusinessData.Models.WebApi;
using System.Web.Http;
using Newtonsoft.Json;

namespace DH.Helpdesk.Services.Services.WebApi
{
    public class WebApiService : IWebApiService
    {
        private readonly WebApiCredentialModel _apiInfo;

        public WebApiService(WebApiCredentialModel apiInfo)
        {
            _apiInfo = apiInfo;

			System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
		}

        public async Task<EmployeeModel> GetEmployeeFor(string employeeNumber)
        {
            var res = await WS_GetEmployeesFor(employeeNumber);
            return res;
        }

        public async Task<bool> IsEmployeeManager(string employeeNumber)
        {
            var res = await WS_IsEmployeeManager(employeeNumber);
            return res;
        }

        [AllowAnonymous]
        private async Task<EmployeeModel> WS_GetEmployeesFor(string employeeNum)
        {
            /* IMPORTANT: Active this line only on debug mode */
            //ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            var res = new EmployeeModel() { IsManager = false, Subordinates = null };            
            var handler = new HttpClientHandler { Credentials = new NetworkCredential(_apiInfo.UserName, _apiInfo.Password) };
            using (var client = new HttpClient(handler))
            {
				

				client.BaseAddress = new Uri(_apiInfo.UriPath);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.GetAsync(string.Format("api/managerEx/{0}", employeeNum));
                if (response.IsSuccessStatusCode)
                {
                    var res1 = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<EmployeeModel>(res1);
                }
            }

            return res;
        }

        [AllowAnonymous]
        private async Task<bool> WS_IsEmployeeManager(string employeeNumber)
        {
            /* IMPORTANT: Active this line only on debug mode */
            //ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            
            var handler = new HttpClientHandler { Credentials = new NetworkCredential(_apiInfo.UserName, _apiInfo.Password) };
            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri(_apiInfo.UriPath);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync(string.Format("api/ismanager/{0}", employeeNumber));
                if (response.IsSuccessStatusCode)
                {
                    var res = await response.Content.ReadAsAsync<bool>();
                    return res;
                }
            }

            return false;
        }
    }
}
