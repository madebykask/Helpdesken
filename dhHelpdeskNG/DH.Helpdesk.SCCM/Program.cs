using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using DH.Helpdesk.SCCM.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace DH.Helpdesk.SCCM
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Run();


        }

        private static void Run()
        {
            //Get the configuration object
            ADALConfiguration ADALConfiguration = GetConfiguration();


            //Check if the configuration is valid
            if (!ADALConfiguration.ValidConfiguration())
            {
                throw new Exception("Configuration is invalid");
            }

            //Get the token
            string token = GetToken(ADALConfiguration);

            //Fetch the data ASYNC
            var result = FetchAllData(token).Result.ToList();

            //Check if fetch was ok
            if (!FetchIsOK(result))
            {
                throw new Exception("Fetch was not ok");
            }

            //Put all the fetches into better objects
            Wrapper wrapper = FormWrapper(result);

            //Split the wrapper by resourceID and putinto model
            List<Models.Computer> computers = FormModel(wrapper);


        }

        private static List<Models.Computer> FormModel(Wrapper wrapper)
        {
            List<Models.Computer> computers = new List<Models.Computer>();

            foreach (var ComputerSystem in wrapper.ComputerSystemWrapper.value)
            {
                //Create the object
                Models.Computer computer = new Models.Computer();

                //Set the resource ID
                computer.ResourceID = ComputerSystem.ResourceID;

                //Start mapping the object
                var computerSystemWrapper = wrapper.ComputerSystemWrapper.value.Where(x => x.ResourceID == ComputerSystem.ResourceID).DefaultIfEmpty(null).FirstOrDefault();
                var operatingSystemWrapper = wrapper.OperatingSystemWrapper.value.Where(x => x.ResourceID == ComputerSystem.ResourceID).DefaultIfEmpty(null).FirstOrDefault();
                var PCBiosWrapper = wrapper.PCBiosWrapper.value.Where(x => x.ResourceID == ComputerSystem.ResourceID).DefaultIfEmpty(null).FirstOrDefault();
                var RSystemWrapper = wrapper.RSystemWrapper.value.Where(x => x.ResourceID == ComputerSystem.ResourceID).FirstOrDefault();
                var videoControllerData = wrapper.VideoControllerDataWrapper.value.Where(x => x.DeviceID == "VideoController1").Where(x => x.ResourceID == ComputerSystem.ResourceID).DefaultIfEmpty(null).FirstOrDefault();
                var x86PCMemory = wrapper.X86PCMemoryWrapper.value.Where(x => x.ResourceID == ComputerSystem.ResourceID).DefaultIfEmpty(null).FirstOrDefault();


                computer._ComputerSystem = new Models.ComputerSystem()
                {
                    Manufacturer = computerSystemWrapper.Manufacturer,
                    Model = computerSystemWrapper.Model,
                    Name = computerSystemWrapper.Name,
                    TimeStamp = computerSystemWrapper.TimeStamp,
                    UserName = computerSystemWrapper.UserName,

                };

                computer._OperatingSystem = new Models.OperatingSystem()
                {

                    Caption = operatingSystemWrapper.Caption,
                    CSDVersion = operatingSystemWrapper.CSDVersion,
                    Version = operatingSystemWrapper.Version,

                };

                computer._PCBios = new Models.PCBios()
                {

                    SerialNumber = PCBiosWrapper.SerialNumber,
                    ReleaseDate = PCBiosWrapper.ReleaseDate,
                    SMBIOSBIOSVersion = PCBiosWrapper.SMBIOSBIOSVersion,

                };

                computer._RSystem = new Models.RSystem()
                {

                    Company = RSystemWrapper.Company,

                };

                computer._VideoControllerData = new Models.VideoControllerData()
                {

                    Name = videoControllerData.Name,
                    
                };

                computer._X86PCMemory = new Models.X86PCMemory()
                {

                    TotalPhysicalMemory = x86PCMemory.TotalPhysicalMemory,

                };

                computers.Add(computer); 


            }

            return computers;
        }            
            

        private static Wrapper FormWrapper(List<RestSharp.RestResponse> restResponses)
        {
            Wrapper wrapper = new Wrapper()
            {
                ComputerSystemWrapper = JsonConvert.DeserializeObject<ComputerSystemWrapper>(restResponses[0].Content),
                OperatingSystemWrapper = JsonConvert.DeserializeObject<OperatingSystemWrapper>(restResponses[1].Content),
                PCBiosWrapper = JsonConvert.DeserializeObject<PCBiosWrapper>(restResponses[2].Content),
                RSystemWrapper = JsonConvert.DeserializeObject<RSystemWrapper>(restResponses[3].Content),
                VideoControllerDataWrapper = JsonConvert.DeserializeObject<VideoControllerDataWrapper>(restResponses[4].Content),
                X86PCMemoryWrapper = JsonConvert.DeserializeObject<X86PCMemoryWrapper>(restResponses[5].Content)
            };

            return wrapper;

        }


        private static bool FetchIsOK(List<RestSharp.RestResponse> restResponses)
        {
            foreach (var restResponse in restResponses)
            {
                if (restResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return false;
                }
            }
            return true;
        }

        private static async Task<IEnumerable<RestSharp.RestResponse>> FetchAllData(string token)
        {
            //Fetch everything
            Task<RestSharp.RestResponse> computerSystemWrapper = FetchDataSingular(token, System.Configuration.ConfigurationManager.AppSettings["SCCM_URL_Computer_System"].ToString());

            Task<RestSharp.RestResponse> operatingSystemWrapper = FetchDataSingular(token, System.Configuration.ConfigurationManager.AppSettings["SCCM_URL_Operating_System"].ToString());

            Task<RestSharp.RestResponse> PCBiosWrapper = FetchDataSingular(token, System.Configuration.ConfigurationManager.AppSettings["SCCM_URL_PC_BIOS"].ToString());

            Task<RestSharp.RestResponse> RSystemWrapper = FetchDataSingular(token, System.Configuration.ConfigurationManager.AppSettings["SCCM_URL_R_System"].ToString());

            Task<RestSharp.RestResponse> videoControllerDataWrapper = FetchDataSingular(token, System.Configuration.ConfigurationManager.AppSettings["SCCM_URL_Video_Controller"].ToString());

            Task<RestSharp.RestResponse> X86PCMemoryWrapper = FetchDataSingular(token, System.Configuration.ConfigurationManager.AppSettings["SCCM_URL_X86_PC_Memory"].ToString());

            var result = await Task.WhenAll(computerSystemWrapper, operatingSystemWrapper, PCBiosWrapper, RSystemWrapper, videoControllerDataWrapper, X86PCMemoryWrapper);

            return result;
        }

        private static Task<RestSharp.RestResponse> FetchDataSingular(string token, string endPath)
        {
            //Get all devices
            Request request = new Request(token);
            var response = request.Get(endPath);

            return response;
        }


        private static ADALConfiguration GetConfiguration()
        {
            int ADAL_Retry_Connection_Count;
            if (!Int32.TryParse(System.Configuration.ConfigurationManager.AppSettings["ADAL_Retry_Connection_Count"].ToString(), out ADAL_Retry_Connection_Count))
            {
                ADAL_Retry_Connection_Count = 0;
            }

            int ADAL_Retry_Connection_Increment_MS;
            if (!Int32.TryParse(System.Configuration.ConfigurationManager.AppSettings["ADAL_Retry_Connection_Increment_MS"].ToString(), out ADAL_Retry_Connection_Increment_MS))
            {
                ADAL_Retry_Connection_Increment_MS = 0;
            }

            ADALConfiguration ADALConfiguration = new ADALConfiguration(
                System.Configuration.ConfigurationManager.AppSettings["ADAL_Microsoft_Base_Url"].ToString(),
                System.Configuration.ConfigurationManager.AppSettings["ADAL_Tenant_Name"].ToString(),
                System.Configuration.ConfigurationManager.AppSettings["ADAL_Client_ID"].ToString(),
                System.Configuration.ConfigurationManager.AppSettings["ADAL_Resource_ID"].ToString(),
                System.Configuration.ConfigurationManager.AppSettings["ADAL_Username"].ToString(),
                System.Configuration.ConfigurationManager.AppSettings["ADAL_Password"].ToString(),
                ADAL_Retry_Connection_Count,
                ADAL_Retry_Connection_Increment_MS
            );

            return ADALConfiguration;
        }

        private static string GetToken(ADALConfiguration ADALConfiguration)
        {
            // Get OAuth token using client credentials 
            string tenantName = ADALConfiguration.ADAL_Tenant_Name;
            string authString = ADALConfiguration.ADAL_Microsoft_Base_Url + tenantName;

            AuthenticationContext authenticationContext = new AuthenticationContext(authString, false);

            // Config for OAuth client credentials  
            string clientId = ADALConfiguration.ADAL_Client_ID;
            string resource = ADALConfiguration.ADAL_Resource_ID;
            string token;

            var credentials = new UserPasswordCredential(ADALConfiguration.ADAL_Username, ADALConfiguration.ADAL_Password);

            try
            {
                AuthenticationResult authenticationResult = authenticationContext.AcquireTokenAsync(resource, clientId, credentials).Result;
                token = authenticationResult.AccessToken;
                return token;
            }
            catch (AuthenticationException ex)
            {
                Console.WriteLine("Acquiring a token failed with the following error: {0}", ex.Message);
                if (ex.InnerException != null)
                {

                    Console.WriteLine("Error detail: {0}", ex.InnerException.Message);
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {

                    Console.WriteLine("Error detail: {0}", ex.InnerException.Message);
                }
            }

            //Retry logic
            if (ADALConfiguration.ADAL_Retry_Connection_Count < 5)
            {
                ADALConfiguration.ADAL_Retry_Connection_Count++;
                System.Threading.Thread.Sleep(ADALConfiguration.ADAL_Retry_Connection_Increment_MS);
                return GetToken(ADALConfiguration);
            }
            else
            {
                throw new Exception("Acquiring a token failed");
            }

        }
    }
}
