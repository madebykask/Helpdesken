using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using RestSharp;
using upKeeper2Helpdesk_New.entities;

namespace upKeeper2Helpdesk_New.api
{
    public class BaseAPI
    {
        private readonly string _BaseUrl;

        public BaseAPI(string BaseUrl)
        {
            _BaseUrl = BaseUrl;
        }

        public Token Login(string Username, string Password)
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
            //var client = new RestClient(_BaseUrl + "/api/68/ComputerNames"); //When connecting against UpKeeper test account
            //var client = new RestClient(_BaseUrl + "/api/1/ComputerNames");
            var client = new RestClient(_BaseUrl + "/api/" + UpKeeperOrgNo + "/ComputerNames");
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Authorization", "Bearer " + t.Access_token);
            IRestResponse response = client.Execute(request);

            var values = JsonConvert.DeserializeObject<IEnumerable<IDictionary<string, string>>>(response.Content);

            return values;
        }

        public Computer GetComputer(string ComputerId, Token t, string UpKeeperOrgNo)
        {
            Computer c = null;

            try
            {
                //var client = new RestClient(_BaseUrl + "/api/68/ComputerDetail/" + ComputerId); //When connecting against UpKeeper test account
                //var client = new RestClient(_BaseUrl + "/api/1/ComputerDetail/" + ComputerId);
                var client = new RestClient(_BaseUrl + "/api/" + UpKeeperOrgNo + "/ComputerDetail/" + ComputerId);
                var request = new RestRequest(Method.GET);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("Authorization", "Bearer " + t.Access_token);
                IRestResponse response = client.Execute(request);

                //Console.WriteLine(response.StatusCode);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    c = JsonConvert.DeserializeObject<Computer>(response.Content);

                    if (c != null && c.MACAddress != null)
                    {
                        c.MACAddress = c.MACAddress.Replace("-", ":");
                    }
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

        public ComputerDetail GetComputerDetail(string ComputerId, Token t, string UpKeeperOrgNo)
        {
            //var client = new RestClient(_BaseUrl + "/api/68/ComputerDetail/" + ComputerId); //When connecting against UpKeeper test account
            //var client = new RestClient(_BaseUrl + "/api/1/ComputerDetail/" + ComputerId);
            var client = new RestClient(_BaseUrl + "/api/" + UpKeeperOrgNo + "/ComputerDetail/" + ComputerId);
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Authorization", "Bearer " + t.Access_token);
            IRestResponse response = client.Execute(request);

            var cd = JsonConvert.DeserializeObject<ComputerDetail>(response.Content);

            return cd;
        }

        public string GetComputerByName(string Name, Token t, string UpKeeperOrgNo)
        {
            //var client = new RestClient(_BaseUrl + "/api/68/ComputerByName/" + Name); //When connecting against UpKeeper test account
            var client = new RestClient(_BaseUrl + "/api/" + UpKeeperOrgNo + "/ ComputerByName/" + Name);
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Authorization", "Bearer " + t.Access_token);
            IRestResponse response = client.Execute(request);

            var c = JsonConvert.DeserializeObject<string>(response.Content);

            return c;
        }

        public Hardware GetHardware(string ComputerId, Token t, string UpKeeperOrgNo)
        {
            //var client = new RestClient(_BaseUrl + "/api/68/Computers/" + ComputerId + "/HardwareInventoryBasic"); //When connecting against UpKeeper test account
            //var client = new RestClient(_BaseUrl + "/api/1/Computers/" + ComputerId + "/HardwareInventoryBasic");
            var client = new RestClient(_BaseUrl + "/api/" + UpKeeperOrgNo + "/Computers/" + ComputerId + "/HardwareInventoryBasic");
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Authorization", "Bearer " + t.Access_token);
            IRestResponse response = client.Execute(request);

            var h = JsonConvert.DeserializeObject<Hardware>(response.Content);

            return h;
        }

        public List<Software> GetComputerSoftware(string ComputerId, Token t, string UpKeeperOrgNo)
        {
            var sl = new List<Software>();

            try
            {
                //var client = new RestClient(_BaseUrl + "/api/68/Computers/" + ComputerId + "/SoftwareInventory"); //When connecting against UpKeeper test account
                //var client = new RestClient(_BaseUrl + "/api/1/Computers/" + ComputerId + "/SoftwareInventory");
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

        public List<Hotfix> GetComputerUpdates(string ComputerId, Token t, string UpKeeperOrgNo)
        {
            var hl = new List<Hotfix>();

            //var client = new RestClient(_BaseUrl + "/api/68/Computers/" + ComputerId + "/UpdateInventory"); //When connecting against UpKeeper test account
            //var client = new RestClient(_BaseUrl + "/api/1/Computers/" + ComputerId + "/UpdateInventory");
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

        public string SaveComputerDetails(string Id, ComputerDetail cd, Token t, string UpKeeperOrgNo)
        {
            try
            {
                //var client = new RestClient(_BaseUrl + "/api/68/Computer/" + Id); //When connecting against UpKeeper test account
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


        public void DeleteComputer(string Id, Token t, string UpKeeperOrgNo)
        {
            //var client = new RestClient(_BaseUrl + "/api/68/Computer/" + Id); //When connecting against UpKeeper test account
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
