﻿using Microsoft.IdentityModel.Clients.ActiveDirectory;
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
using DH.Helpdesk.SCCM.DB;
using DH.Helpdesk.SCCM.Other;
using System.Threading;

namespace DH.Helpdesk.SCCM
{
    internal class Program
    {

        private static long actions = 0;
        
        
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
            var result = FetchBaseData(token).Result.ToList();

            //Check if fetch was ok
            if (!FetchIsOK(result))
            {
                throw new Exception("Fetch was not ok");
            }

            //Parse the data
            var rSystemWrapper = JsonConvert.DeserializeObject<GenericValueWrapper<RSystem>>(result[0].Content).value;

            //Limit the data for debugging purposes
            var setting_Limit_Devices = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["Setting_Limit_Devices"].ToString());

            if (setting_Limit_Devices != 0)
            {
                rSystemWrapper = rSystemWrapper.Take(setting_Limit_Devices).ToList();
            }
            

            //Chunk the data
            var chunkedData = rSystemWrapper.ChunkBy(rSystemWrapper.Count / Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["Setting_Chunk_Page_Size"].ToString()));

            //Run all the threads
            var threadResult = runThreads(chunkedData, token).Result;


            //Combine the thread result
            List<Models.Device> computers = new List<Models.Device>();
            foreach (var SingleThread in threadResult)
            {
                computers.AddRange(SingleThread);
            }

            var test = computers;

            UpdateOrCreateComputerInDB(computers);

        }

        private static async Task<List<Models.Device>[]> runThreads(List<List<RSystem>> chunkedData, string token)
        {
            List<Task<List<Models.Device>>> threadedComputers = new List<Task<List<Models.Device>>>();

            var threadIndex = 1;
            foreach (var data in chunkedData)
            {
                Console.WriteLine("Init thread " + threadIndex);

                Models.CustomThreadObject customThreadObject = new Models.CustomThreadObject(data, threadIndex, token);

                Task<List<Models.Device>> computers = FormModel(customThreadObject.Token, customThreadObject.RSystem, customThreadObject.ThreadNumber);

                threadIndex++;

                threadedComputers.Add(computers);
            }

            return await Task.WhenAll(threadedComputers);

        }

        
        private static void UpdateOrCreateComputerInDB(List<Models.Device> computers)
        {

            Connector connector = new Connector(System.Configuration.ConfigurationManager.ConnectionStrings["conHD"].ToString());

            foreach (var computer in computers)
            {
                ComputerDB computerDB;
                ServerDB serverDB;


                //Computer
                if (computer._OperatingSystem.ComputerRole == 0)
                {
                    computerDB = connector.GetComputer(computer);

                    //If null, Create
                    if (computerDB == null)
                    {
                        computerDB = connector.InsertComputerAndReturn(computer);
                    }

                    //Run data manipulation logic
                    HandleComputerLogic(computerDB, computer, connector);
                }
                //Server
                else if (computer._OperatingSystem.ComputerRole == 1)
                {
                    serverDB = connector.GetServer(computer);

                    //If null, Create
                    if (serverDB == null)
                    {
                        serverDB = connector.InsertServerAndReturn(computer);
                    }

                    //Run data manipulation logic
                    HandleServerLogic(serverDB, computer, connector);

                }
                
            }
        }
        
        private static void HandleServerLogic(ServerDB serverDB, Models.Device reference, Connector connector)
        {
            serverDB.ScanDate = reference._ComputerSystem.TimeStamp;

            serverDB.OS_Id = connector.ExistsObject(Other.Enums.ObjectType.OS, reference._OperatingSystem.Caption);
            serverDB.OS_Version = reference._OperatingSystem.Version;
            serverDB.OS_SP = reference._OperatingSystem.CSDVersion;

            serverDB.Domain_Id = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["DB_Domain_Id"].ToString());

            var userName = reference._ComputerSystem.UserName;

            if (!String.IsNullOrEmpty(userName))
            {
                var splittedUserName = userName.Split('\\')[1];
                userName = splittedUserName;
            }

            var computerUserID = connector.GetComputerUserByUserId(Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["DB_Customer_Id"].ToString()), userName);

            if (computerUserID != null)
            {
                serverDB.User_Id = computerUserID.Value;
            }

            serverDB.Manufacturer = reference._ComputerSystem.Manufacturer;

            serverDB.ComputerModel_Id = connector.ExistsObject(Other.Enums.ObjectType.ComputerModel, reference._ComputerSystem.Model);
            serverDB.ComputerModel = reference._ComputerSystem.Model;

            serverDB.SerialNumber = reference._PCBios.SerialNumber;
            serverDB.BIOSDate = reference._PCBios.ReleaseDate;
            serverDB.BIOSVersion = reference._PCBios.SMBIOSBIOSVersion;

            serverDB.ChassisType = Helpers.getChassisTypeName(Convert.ToInt32(reference._Enclosure.ChassisTypes));

            serverDB.Processor_Id = connector.ExistsObject(Other.Enums.ObjectType.Processor, reference._Processor.Name);

            serverDB.RAM_Id = connector.ExistsObject(Other.Enums.ObjectType.RAM, Helpers.getRAM(reference._X86PCMemory.TotalPhysicalMemory));

            serverDB.NIC_Id = connector.ExistsObject(Other.Enums.ObjectType.NetworkAdapter, reference._NetworkAdapter.FirstOrDefault().Name);

            string ipAdress = null;
            string macAdress = null;
            foreach (var networkAdapterConfiguration in reference._NetworkAdapterConfiguration)
            {
                if (networkAdapterConfiguration.IPAddress != null)
                {
                    var splitIdAddress = networkAdapterConfiguration.IPAddress.Split(',');
                    ipAdress = splitIdAddress[0];
                    macAdress = networkAdapterConfiguration.MacAddress;

                    break;
                }
            }

            serverDB.IPAddress = ipAdress;
            serverDB.MACAddress = macAdress;

            serverDB.SoundCard = reference._SoundDevice.Name;

            serverDB.VideoCard = reference._VideoControllerData.Name;

            //Save computer

            connector.SaveServer(serverDB);
        }

        private static void HandleComputerLogic(ComputerDB computerDB, Models.Device reference, Connector connector)
        {
            computerDB.ScanDate = reference._ComputerSystem.TimeStamp;

            computerDB.OS_Id = connector.ExistsObject(Other.Enums.ObjectType.OS, reference._OperatingSystem.Caption);
            computerDB.OS_Version = reference._OperatingSystem.Version;
            computerDB.OS_SP = reference._OperatingSystem.CSDVersion;

            computerDB.Domain_Id = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["DB_Domain_Id"].ToString());

            var userName = reference._ComputerSystem.UserName;

            if (!String.IsNullOrEmpty(userName))
            {
                var splittedUserName = userName.Split('\\')[1];
                userName = splittedUserName;
            }

            var computerUserID = connector.GetComputerUserByUserId(Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["DB_Customer_Id"].ToString()), userName);

            if (computerUserID != null)
            {
                computerDB.User_Id = computerUserID.Value;
            }

            computerDB.Manufacturer = reference._ComputerSystem.Manufacturer;

            computerDB.ComputerModel_Id = connector.ExistsObject(Other.Enums.ObjectType.ComputerModel, reference._ComputerSystem.Model);
            computerDB.ComputerModel = reference._ComputerSystem.Model;

            computerDB.SerialNumber = reference._PCBios.SerialNumber;
            computerDB.BIOSDate = reference._PCBios.ReleaseDate;
            computerDB.BIOSVersion = reference._PCBios.SMBIOSBIOSVersion;

            computerDB.ChassisType = Helpers.getChassisTypeName(Convert.ToInt32(reference._Enclosure.ChassisTypes));

            computerDB.Processor_Id = connector.ExistsObject(Other.Enums.ObjectType.Processor, reference._Processor.Name);

            computerDB.RAM_Id = connector.ExistsObject(Other.Enums.ObjectType.RAM, Helpers.getRAM(reference._X86PCMemory.TotalPhysicalMemory));

            computerDB.NIC_Id = connector.ExistsObject(Other.Enums.ObjectType.NetworkAdapter, reference._NetworkAdapter.FirstOrDefault().Name);

            string ipAdress = null;
            string macAdress = null;
            foreach (var networkAdapterConfiguration in reference._NetworkAdapterConfiguration)
            {
                if (networkAdapterConfiguration.IPAddress != null)
                {
                    var splitIdAddress = networkAdapterConfiguration.IPAddress.Split(',');
                    ipAdress = splitIdAddress[0];
                    macAdress = networkAdapterConfiguration.MacAddress;

                    break;
                }
            }

            computerDB.IPAddress = ipAdress;
            computerDB.MACAddress = macAdress;

            computerDB.SoundCard = reference._SoundDevice.Name;

            computerDB.VideoCard = reference._VideoControllerData.Name;

            foreach (var program in reference._Programs)
            {
                Software r = new Software()
                {
                    
                    DisplayName = program.DisplayName,
                    Publisher = program.Publisher,
                    Version = program.Version,
                };

                computerDB.Softwares.Add(r);
            }

            foreach (var drive in reference._LogicalDisk)
            {
                LogicalDrive r = new LogicalDrive()
                {
                    DeviceId = drive.DeviceId,
                    DriveType = drive.DriveType,
                    FileSystem = drive.FileSystem,
                    FreeSpace = drive.FreeSpace,
                    Name = drive.Name,
                    Size = drive.Size,
                };

                computerDB.LogicalDrives.Add(r);
            }


            //Save computer

            connector.SaveComputer(computerDB);


        }




        private static Task<List<Models.Device>> FormModel(string token, List<RSystem> rSystemWrapper, int thread)
        {
            List<Models.Device> devices = new List<Models.Device>();

            TaskCompletionSource<List<Models.Device>> tcs = new TaskCompletionSource<List<Models.Device>>();

            Task.Run(() =>
            {

                foreach (var RSystem in rSystemWrapper)
                {

                    actions++;
                    Console.WriteLine("Thread: " + thread + " - " + RSystem.ResourceID + " Action: " + actions);

                    //Fetch additioan data for each computer
                    var computerAdditionalData = FetchAdditionalData(token, RSystem.ResourceID).Result.ToList();


                    //Error check
                    if (!FetchIsOK(computerAdditionalData))
                    {
                        throw new Exception("Additional Fetch was not ok");
                    }


                    var wrapper = FormWrapper(computerAdditionalData);


                    //Create the object
                    Models.Device computer = new Models.Device();

                    //Set the resource ID
                    computer.ResourceID = RSystem.ResourceID;


                    //Start mapping the object
                    var computerSystemWrapper = wrapper.ComputerSystem.value.DefaultIfEmpty(null).FirstOrDefault();
                    var operatingSystemWrapper = wrapper.OperatingSystem.value.DefaultIfEmpty(null).FirstOrDefault();
                    var PCBiosWrapper = wrapper.PCBios.value.DefaultIfEmpty(null).FirstOrDefault();
                    var videoControllerDataWrapper = wrapper.VideoControllerData.value.Where(x => x.DeviceID == "VideoController1").DefaultIfEmpty(null).FirstOrDefault();
                    var x86PCMemoryWrapper = wrapper.X86PCMemory.value.DefaultIfEmpty(null).FirstOrDefault();
                    var enclosureWrapper = wrapper.Enclosure.value.DefaultIfEmpty(null).FirstOrDefault();
                    var processorWrapper = wrapper.Processor.value.DefaultIfEmpty(null).FirstOrDefault();
                    var networkAdapterWrapper = wrapper.NetworkAdapter.value.DefaultIfEmpty(null);                              //IS LIST
                    var networkAdapterConfigurationWrapper = wrapper.NetworkAdapterConfiguration.value.DefaultIfEmpty(null);    //IS LIST
                    var soundDeviceWrapper = wrapper.SoundDevice.value.DefaultIfEmpty(null).FirstOrDefault();
                    var programsWrapper = wrapper.Programs.value.DefaultIfEmpty(null);                                          //IS LIST
                    var logicalDiskWrapper = wrapper.LogicalDisk.value.DefaultIfEmpty(null);                                    //IS LIST



                    //Set the base
                    computer._RSystem = new Models.RSystem();
                    //{

                    //    Company = RSystem.Company,

                    //};


                    //Check if found
                    if (computerSystemWrapper != null)
                    {
                        computer._ComputerSystem = new Models.ComputerSystem()
                        {
                            Manufacturer = computerSystemWrapper.Manufacturer,
                            Model = computerSystemWrapper.Model,
                            Name = computerSystemWrapper.Name,
                            TimeStamp = computerSystemWrapper.TimeStamp,
                            UserName = computerSystemWrapper.UserName,

                        };
                    }

                    //Check if found
                    if (operatingSystemWrapper != null)
                    {
                        computer._OperatingSystem = new Models.OperatingSystem()
                        {

                            Caption = operatingSystemWrapper.Caption,
                            CSDVersion = operatingSystemWrapper.CSDVersion,
                            Version = operatingSystemWrapper.Version,

                        };
                    }

                    //Check if found
                    if (PCBiosWrapper != null)
                    {
                        computer._PCBios = new Models.PCBios()
                        {

                            SerialNumber = PCBiosWrapper.SerialNumber,
                            ReleaseDate = PCBiosWrapper.ReleaseDate,
                            SMBIOSBIOSVersion = PCBiosWrapper.SMBIOSBIOSVersion,

                        };
                    }

                    //Check if found
                    if (videoControllerDataWrapper != null)
                    {
                        computer._VideoControllerData = new Models.VideoControllerData()
                        {

                            Name = videoControllerDataWrapper.Name,

                        };
                    }

                    //Check if found
                    if (x86PCMemoryWrapper != null)
                    {
                        computer._X86PCMemory = new Models.X86PCMemory()
                        {

                            TotalPhysicalMemory = x86PCMemoryWrapper.TotalPhysicalMemory,

                        };
                    }

                    //Check if found
                    if (enclosureWrapper != null)
                    {
                        computer._Enclosure = new Models.Enclosure()
                        {

                            ChassisTypes = enclosureWrapper.ChassisTypes,

                        };
                    }

                    //Check if found
                    if (enclosureWrapper != null)
                    {
                        computer._Enclosure = new Models.Enclosure()
                        {

                            ChassisTypes = enclosureWrapper.ChassisTypes,

                        };
                    }

                    //Check if found
                    if (processorWrapper != null)
                    {
                        computer._Processor = new Models.Processor()
                        {

                            Name = processorWrapper.Name,

                        };
                    }

                    //Check if found
                    if (networkAdapterWrapper != null)
                    {
                        computer._NetworkAdapter = new List<Models.NetworkAdapter>();
  
                        foreach (var networkAdapterWrapperSingle in networkAdapterWrapper)
                        {
                            computer._NetworkAdapter.Add(new Models.NetworkAdapter()
                            {

                                Name = networkAdapterWrapperSingle.Name,

                            });
                        }
                    }

                    //Check if found
                    if (networkAdapterConfigurationWrapper != null)
                    {
                        computer._NetworkAdapterConfiguration = new List<Models.NetworkAdapterConfiguration>();

                        foreach (var networkAdapterConfigurationWrapperSingle in networkAdapterConfigurationWrapper)
                        {
                            computer._NetworkAdapterConfiguration.Add(new Models.NetworkAdapterConfiguration()
                            {

                                IPAddress = networkAdapterConfigurationWrapperSingle.IPAddress,
                                MacAddress = networkAdapterConfigurationWrapperSingle.MacAddress,

                            });
                        }

                    }

                    //Check if found
                    if (soundDeviceWrapper != null)
                    {
                        computer._SoundDevice = new Models.SoundDevice()
                        {

                            Name = soundDeviceWrapper.Name,

                        };
                    }

                    //Check if found
                    if (programsWrapper != null)
                    {
                        computer._Programs = new List<Models.Programs>();

                        foreach (var programsWrapperSingle in programsWrapper)
                        {
                            computer._Programs.Add(new Models.Programs()
                            {

                                DisplayName = programsWrapperSingle.DisplayName,
                                Publisher = programsWrapperSingle.Publisher,
                                Version = programsWrapperSingle.Version,

                            });
                        }


                    }

                    //Check if found
                    if (logicalDiskWrapper != null)
                    {
                        computer._LogicalDisk = new List<Models.LogicalDisk>();

                        foreach (var logicalDiskWrapperSingle in logicalDiskWrapper)
                        {
                            computer._LogicalDisk.Add(new Models.LogicalDisk()
                            {

                                DeviceId = logicalDiskWrapperSingle.DeviceId,
                                DriveType = logicalDiskWrapperSingle.DriveType,
                                FileSystem = logicalDiskWrapperSingle.FileSystem,
                                FreeSpace = logicalDiskWrapperSingle.FreeSpace,
                                Name = logicalDiskWrapperSingle.Name,
                                Size = logicalDiskWrapperSingle.Size,

                            });
                        }

                    }

                    //Add to computers
                    devices.Add(computer);

                }

                tcs.SetResult(devices);

            });

            return tcs.Task;
        }


        private static Wrapper FormWrapper(List<RestSharp.RestResponse> restResponses)
        {


            Wrapper wrapper = new Wrapper()
            {
                ComputerSystem = JsonConvert.DeserializeObject<GenericValueWrapper<ComputerSystem>>(restResponses[0].Content),
                OperatingSystem = JsonConvert.DeserializeObject<GenericValueWrapper<Entities.OperatingSystem>>(restResponses[1].Content),
                PCBios = JsonConvert.DeserializeObject<GenericValueWrapper<PCBios>>(restResponses[2].Content),
                VideoControllerData = JsonConvert.DeserializeObject<GenericValueWrapper<VideoControllerData>>(restResponses[3].Content),
                X86PCMemory = JsonConvert.DeserializeObject<GenericValueWrapper<X86PCMemory>>(restResponses[4].Content),
                Enclosure = JsonConvert.DeserializeObject<GenericValueWrapper<Enclosure>>(restResponses[5].Content),
                Processor = JsonConvert.DeserializeObject<GenericValueWrapper<Processor>>(restResponses[6].Content),
                NetworkAdapter = JsonConvert.DeserializeObject<GenericValueWrapper<NetworkAdapter>>(restResponses[7].Content),
                NetworkAdapterConfiguration = JsonConvert.DeserializeObject<GenericValueWrapper<NetworkAdapterConfiguration>>(restResponses[8].Content),
                SoundDevice = JsonConvert.DeserializeObject<GenericValueWrapper<SoundDevice>>(restResponses[9].Content),
                Programs = JsonConvert.DeserializeObject<GenericValueWrapper<Programs>>(restResponses[10].Content),
                LogicalDisk = JsonConvert.DeserializeObject<GenericValueWrapper<LogicalDisk>>(restResponses[11].Content),
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

        private static async Task<IEnumerable<RestSharp.RestResponse>> FetchBaseData(string token)
        {
            //Fetch everything
            Task<RestSharp.RestResponse> RSystemWrapper = FetchDataSingular(token, System.Configuration.ConfigurationManager.AppSettings["SCCM_URL_R_System"].ToString());

            var result = await Task.WhenAll(RSystemWrapper);

            return result;
        }

        private static async Task<IEnumerable<RestSharp.RestResponse>> FetchAdditionalData(string token, long resourceId)
        {
            //Fetch everything
            Task<RestSharp.RestResponse> computerSystemWrapper = FetchDataSingular(token, System.Configuration.ConfigurationManager.AppSettings["SCCM_URL_R_System"].ToString() + "(" + resourceId + ")" + "/" + System.Configuration.ConfigurationManager.AppSettings["SCCM_URL_Computer_System"].ToString());
            
            Task<RestSharp.RestResponse> operatingSystemWrapper = FetchDataSingular(token, System.Configuration.ConfigurationManager.AppSettings["SCCM_URL_R_System"].ToString() + "(" + resourceId + ")" + "/" + System.Configuration.ConfigurationManager.AppSettings["SCCM_URL_Operating_System"].ToString());

            Task<RestSharp.RestResponse> PCBiosWrapper = FetchDataSingular(token, System.Configuration.ConfigurationManager.AppSettings["SCCM_URL_R_System"].ToString() + "(" + resourceId + ")" + "/" + System.Configuration.ConfigurationManager.AppSettings["SCCM_URL_PC_BIOS"].ToString());

            Task<RestSharp.RestResponse> videoControllerDataWrapper = FetchDataSingular(token, System.Configuration.ConfigurationManager.AppSettings["SCCM_URL_R_System"].ToString() + "(" + resourceId + ")" + "/" + System.Configuration.ConfigurationManager.AppSettings["SCCM_URL_Video_Controller"].ToString());

            Task<RestSharp.RestResponse> X86PCMemoryWrapper = FetchDataSingular(token, System.Configuration.ConfigurationManager.AppSettings["SCCM_URL_R_System"].ToString() + "(" + resourceId + ")" + "/" + System.Configuration.ConfigurationManager.AppSettings["SCCM_URL_X86_PC_Memory"].ToString());

            Task<RestSharp.RestResponse> enclosureWrapper = FetchDataSingular(token, System.Configuration.ConfigurationManager.AppSettings["SCCM_URL_R_System"].ToString() + "(" + resourceId + ")" + "/" + System.Configuration.ConfigurationManager.AppSettings["SCCM_URL_Enclosure"].ToString());

            Task<RestSharp.RestResponse> processorWrapper = FetchDataSingular(token, System.Configuration.ConfigurationManager.AppSettings["SCCM_URL_R_System"].ToString() + "(" + resourceId + ")" + "/" + System.Configuration.ConfigurationManager.AppSettings["SCCM_URL_Processor"].ToString());

            Task<RestSharp.RestResponse> networkAdapterWrapper = FetchDataSingular(token, System.Configuration.ConfigurationManager.AppSettings["SCCM_URL_R_System"].ToString() + "(" + resourceId + ")" + "/" + System.Configuration.ConfigurationManager.AppSettings["SCCM_URL_Network_Adapter"].ToString());

            Task<RestSharp.RestResponse> networkAdapterConfigurationWrapper = FetchDataSingular(token, System.Configuration.ConfigurationManager.AppSettings["SCCM_URL_R_System"].ToString() + "(" + resourceId + ")" + "/" + System.Configuration.ConfigurationManager.AppSettings["SCCM_URL_Network_Adapter_Configuration"].ToString());

            Task<RestSharp.RestResponse> soundDeviceWrapper = FetchDataSingular(token, System.Configuration.ConfigurationManager.AppSettings["SCCM_URL_R_System"].ToString() + "(" + resourceId + ")" + "/" + System.Configuration.ConfigurationManager.AppSettings["SCCM_URL_Sound_Device"].ToString());

            Task<RestSharp.RestResponse> programsWrapper = FetchDataSingular(token, System.Configuration.ConfigurationManager.AppSettings["SCCM_URL_R_System"].ToString() + "(" + resourceId + ")" + "/" + System.Configuration.ConfigurationManager.AppSettings["SCCM_URL_Add_Remove_Programs"].ToString());

            Task<RestSharp.RestResponse> logicalDisksWrapper = FetchDataSingular(token, System.Configuration.ConfigurationManager.AppSettings["SCCM_URL_R_System"].ToString() + "(" + resourceId + ")" + "/" + System.Configuration.ConfigurationManager.AppSettings["SCCM_URL_Logical_Disk"].ToString());

            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            var result = await Task.WhenAll(computerSystemWrapper, operatingSystemWrapper, PCBiosWrapper, videoControllerDataWrapper, X86PCMemoryWrapper, enclosureWrapper, processorWrapper, networkAdapterWrapper, networkAdapterConfigurationWrapper, soundDeviceWrapper, programsWrapper, logicalDisksWrapper);
            stopwatch.Stop();
            Console.WriteLine("Time taken to fetch all data: " + stopwatch.ElapsedMilliseconds);
            
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
