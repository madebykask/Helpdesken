using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using RestSharp;
using upKeeper2Helpdesk.entities;

namespace upKeeper2Helpdesk.api
{
    public class BaseAPI
    {
        private readonly string _BaseUrl;
        private static string version = System.Configuration.ConfigurationManager.AppSettings["UpkeeperVersion"];
        public BaseAPI(string BaseUrl)
        {
            _BaseUrl = BaseUrl;
        }

        public Token Login(string username, string password, string clientId)
        {

            if(version == "5")
            {
                var tokenUrl = System.Configuration.ConfigurationManager.AppSettings["TokenUrl"];
                var client = new RestClient(_BaseUrl + tokenUrl);
                var request = new RestRequest(Method.POST);
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Content-Type", "application/json");

                // Create an object to serialize into JSON
                var body = new
                {
                    Username = username,
                    Password = password,
                    ClientId = clientId
                };
                // Serialize the object to JSON and add it to the request body
                request.AddJsonBody(body);

                // Execute the request
                var response = client.Execute(request);

                Token t = null;

                // Check if the response is successful
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    t = JsonConvert.DeserializeObject<Token>(response.Content);
                    t.Access_token = t.Access_token_v5;
                }

                return t;
            }
            else
            {
                return LoginOldVersion(username, password);
            }

            
        }

        public Token LoginOldVersion(string Username, string Password)
        {

            var client = new RestClient(_BaseUrl + "/token");
            var request = new RestRequest(Method.POST);

            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("undefined", "UserName=" + Username + "&Password=" + Password + "&grant_type=password&client_id=ngAuthApp", ParameterType.RequestBody);
            var response = client.Execute(request);

            Token t = null;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                t = JsonConvert.DeserializeObject<Token>(response.Content);
            }

            return t;
        }
        public IEnumerable<IDictionary<string, string>> GetComputerNames(Token t, string UpKeeperOrgNo)
        {
            var client = new RestClient(_BaseUrl + "/api/" + UpKeeperOrgNo + "/ComputerNames");
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Authorization", "Bearer " + t.Access_token);
            IRestResponse response = client.Execute(request);

            var values = JsonConvert.DeserializeObject<IEnumerable<IDictionary<string, string>>>(response.Content);

            return values;
        }

        public Computer GetComputer(Token t, string ComputerId, string UpKeeperOrgNo)
        {
            Computer c = null;

            try
            {
                var client = new RestClient(_BaseUrl + "/api/" + UpKeeperOrgNo + "/ComputerDetail/" + ComputerId);
                var request = new RestRequest(Method.GET);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("Authorization", "Bearer " + t.Access_token);
                IRestResponse response = client.Execute(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    c = JsonConvert.DeserializeObject<Computer>(response.Content);

                    //if (c != null && c.MACAddress != null)
                    //{
                    //    c.MACAddress = c.MACAddress.Replace("-", ":");
                    //}
                }
                else
                {
                    Console.WriteLine(response.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return c;
        }

        public ComputerDetail GetComputerDetail(Token t, string ComputerId, string UpKeeperOrgNo)
        {
            var client = new RestClient(_BaseUrl + "/api/" + UpKeeperOrgNo + "/ComputerDetail/" + ComputerId);
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Authorization", "Bearer " + t.Access_token);
            IRestResponse response = client.Execute(request);

            var cd = JsonConvert.DeserializeObject<ComputerDetail>(response.Content);

            return cd;
        }

        public string GetComputerByName(Token t, string Name, string UpKeeperOrgNo)
        {
            var client = new RestClient(_BaseUrl + "/api/" + UpKeeperOrgNo + "/ ComputerByName/" + Name);
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Authorization", "Bearer " + t.Access_token);
            IRestResponse response = client.Execute(request);

            var c = JsonConvert.DeserializeObject<string>(response.Content);

            return c;
        }

        public Hardware GetHardware(Token t, string ComputerId, string UpKeeperOrgNo)
        {
            var client = new RestClient(_BaseUrl + "/api/" + UpKeeperOrgNo + "/Computers/" + ComputerId + "/HardwareInventoryBasic");
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Authorization", "Bearer " + t.Access_token);
            IRestResponse response = client.Execute(request);

            var h = JsonConvert.DeserializeObject<Hardware>(response.Content);

            return h;
        }

        public List<Software> GetComputerSoftware(Token t, string ComputerId, string UpKeeperOrgNo)
        {
            var sl = new List<Software>();

            try
            {
                var client = new RestClient(_BaseUrl + "/api/"  + UpKeeperOrgNo + "/Computers/" + ComputerId + "/SoftwareInventory");
                var request = new RestRequest(Method.GET);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("Authorization", "Bearer " + t.Access_token);
                IRestResponse response = client.Execute(request);

                var values = JsonConvert.DeserializeObject<IEnumerable<IDictionary<string, string>>>(response.Content);

                foreach (Dictionary<string, string> v in values)
                {
                    var s = new Software
                    {
                        Name = v["Name"].ToString(),
                        Version = v["Version"] != null ? v["Version"].ToString() : "",
                        Manufacturer = v["Publisher"] != null ? v["Publisher"].ToString() : "",
                        Registration_code = v["ProductId"] != null ? v["ProductId"].ToString() : ""
                    };

                    sl.Add(s);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return sl;
        }

        public List<Hotfix> GetComputerUpdates(Token t, string ComputerId, string UpKeeperOrgNo)
        {
            var hl = new List<Hotfix>();

            var client = new RestClient(_BaseUrl + "/api/"  + UpKeeperOrgNo + "/Computers/" + ComputerId + "/UpdateInventory");
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Authorization", "Bearer " + t.Access_token);
            IRestResponse response = client.Execute(request);

            var values = JsonConvert.DeserializeObject<IEnumerable<IDictionary<string, string>>>(response.Content);

            foreach (Dictionary<string, string> v in values)
            {
                var h = new Hotfix
                {
                    HotFixId = "Hotfix - " + v["HotFixId"].ToString()
                };

                hl.Add(h);
            }

            return hl;
        }

        public string SaveComputerDetails(Token t, string Id, ComputerDetail cd,string UpKeeperOrgNo)
        {
            try
            {

                var client = new RestClient(_BaseUrl + "/api/" + UpKeeperOrgNo + "/Computer/" + Id);
                var request = new RestRequest(Method.PUT);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("Authorization", "Bearer " + t.Access_token);
                request.AddParameter("application/json", JsonConvert.SerializeObject(cd), ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                return JsonConvert.DeserializeObject<string>(response.Content);
            }
            catch (JsonException ex)
            {
                //throw ex;

                return ex.InnerException.ToString();
            }
        }


        public void DeleteComputer(Token t, string Id, string UpKeeperOrgNo)
        {
            var client = new RestClient(_BaseUrl + "/api/" + UpKeeperOrgNo + "/Computer/" + Id);
            var request = new RestRequest(Method.DELETE);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Authorization", "Bearer " + t.Access_token);
            IRestResponse response = client.Execute(request);

            //var c = JsonConvert.DeserializeObject<string>(response.StatusCode.ToString());

            //return c;
        }
    }
}
