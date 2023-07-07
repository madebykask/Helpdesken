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
using DH.Helpdesk.SCCM.DB;
using DH.Helpdesk.SCCM.Other;
using System.Threading;
using System.Windows.Forms;
using System.Net;
using RestSharp;
using System.Net.Http;
using System.Web;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace DH.Helpdesk.SCCM
{
    internal class Program
    {

        static readonly log4net.ILog log =
            log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static long actions = 0;


        [STAThread]
        static void Main(string[] args)
        {
            log.Info("-----------------------------------------");
            log.Info("-----------------------------------------");
            log.Info("Main started");
            
            Run();
            
            log.Info("Main stopped");
        }

        
        private async static void Run()
        {

            //Get the configuration object
            ADALConfiguration ADALConfiguration = GetConfiguration();

            log.Info("Application is running");
            //Check if the configuration is valid
            if (!ADALConfiguration.ValidConfiguration())
            {
                throw new Exception("Configuration is invalid");
            }


            log.Info("Acquiring token");
            //Get the token
            string token = GetToken(ADALConfiguration);

            TokenUtility(token);


            try
            {
                //Fetch the data ASYNC
                log.Info("Fetch the data ASYNC");
                var result = FetchBaseData(token).Result;

                //Check if fetch was ok
                var resultList = new List<HttpResponseMessage>();
                resultList.Add(result);
                
                if (!FetchIsOK(resultList))
                {
                    throw new Exception("Fetch was not ok");
                }

                //Parse the data
                var rSystemWrapper = JsonConvert.DeserializeObject<GenericValueWrapper<RSystem>>(await result.Content.ReadAsStringAsync()).value;

                //Limit the data
                var setting_Limit_Devices = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["Setting_Limit_Devices"].ToString());

                //Check if to take random
                var setting_auto_mower = System.Configuration.ConfigurationManager.AppSettings["Setting_Auto_Mower"].ToString();

                // Shuffle and take random elements if setting_auto_mower is true
                if (setting_auto_mower == "true")
                {
                    var random = new Random();
                    rSystemWrapper = rSystemWrapper.OrderBy(x => random.Next()).ToList();
                }

                if (setting_Limit_Devices != 0)
                {
                    rSystemWrapper = rSystemWrapper.Take(setting_Limit_Devices).ToList();
                }


                //Chunk the data
                log.Info("Chunk the data");
                var chunkedData = rSystemWrapper.ChunkBy(rSystemWrapper.Count / Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["Setting_Chunk_Page_Size"].ToString()));

                //Run all the threads
                log.Info("Run all the threads");
                var threadResult = runThreads(chunkedData, token).Result;


                //Combine the thread result
                log.Info("Combine the thread result");
                List<Models.Device> computers = new List<Models.Device>();
                foreach (var SingleThread in threadResult)
                {
                    computers.AddRange(SingleThread);
                }

                var test = computers;

                log.Info("UpdateOrCreateComputerInDB");
                UpdateOrCreateComputerInDB(computers);

                Connector connector = new Connector(System.Configuration.ConfigurationManager.ConnectionStrings["conHD"].ToString());
                int Customer_Id = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["DB_Customer_Id"].ToString());

                log.Info("UpdateApplication");
                connector.UpdateApplication(Customer_Id);

                log.Info("Application has stopped running");
            }
            catch (Exception ex)
            {
                log.Info(ex.StackTrace);
                throw;
            }
            

            
        }

        private static void TokenUtility(string token)
        {
            try
            {
                //If to show the token in the console
                if (Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["Show_Token"].ToString()) == 1)
                {
                    Console.WriteLine("TOKEN");
                    Console.WriteLine("------------------------------------|.|------------------------------------");
                    Console.WriteLine(token);
                    Console.WriteLine("------------------------------------|.|------------------------------------");

                    //If to copy to clipboard
                    if (Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["Copy_Token_To_Clipboard"].ToString()) == 1)
                    {
                        Clipboard.SetText(token);
                        Console.WriteLine("Copied token to clipboard");
                        Console.WriteLine("------------------------------------|.|------------------------------------");
                    }
                }
            }
            catch (Exception ex)
            {
                var errorMsg = "Show_Token || Copy_Token_To_Clipboard has invalid configuration";
                log.Error(errorMsg);
                Console.WriteLine(errorMsg);
            }
        }

        private static async Task<List<Models.Device>[]> runThreads(List<List<RSystem>> chunkedData, string token)
        {
            List<Task<List<Models.Device>>> threadedComputers = new List<Task<List<Models.Device>>>();

            try
            {
                var threadIndex = 1;
                foreach (var data in chunkedData)
                {
                    var info = "Init thread " + threadIndex;
                    Console.WriteLine(info);
                    log.Info(info);

                    Models.CustomThreadObject customThreadObject = new Models.CustomThreadObject(data, threadIndex, token);

                    Task<List<Models.Device>> computers = FormModel(customThreadObject.Token, customThreadObject.RSystem, customThreadObject.ThreadNumber);

                    threadIndex++;

                    threadedComputers.Add(computers);
                }

                return await Task.WhenAll(threadedComputers);
            }
            catch (Exception ex)
            {
                throw;
            }
            


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


            var userName = reference._ComputerSystem.UserName;

            if (!String.IsNullOrEmpty(userName))
            {
                var splittedUserName = userName.Split('\\');

                if (splittedUserName.Length > 1)
                {
                    userName = splittedUserName[1];
                }
                else
                {
                    userName = splittedUserName[0];
                }
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


            var userName = reference._ComputerSystem.UserName;

            if (!String.IsNullOrEmpty(userName))
            {
                var splittedUserName = userName.Split('\\');

                if (splittedUserName.Length > 1)
                {
                    userName = splittedUserName[1];
                }
                else
                {
                    userName = splittedUserName[0];
                }
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

            if (reference._SoundDevice != null)
            {
                computerDB.SoundCard = reference._SoundDevice.Name;
            }

            if (reference._VideoControllerData != null)
            {
                computerDB.VideoCard = reference._VideoControllerData.Name;
            }
            

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

            try
            {
                Task.Run(() =>
                {

                    foreach (var RSystem in rSystemWrapper)
                    {

                        actions++;
                        var info = "Thread: " + thread + " - " + RSystem.ResourceID + " Action: " + actions;

                        log.Info(info);
                        Console.WriteLine(info);

                        //Fetch additioan data for each computer
                        var computerAdditionalData = FetchAdditionalData(token, RSystem.ResourceID).Result.ToList();


                        //Error check
                        if (!FetchIsOK(computerAdditionalData))
                        {
                            var errorMsg = "Additional Fetch was not ok";
                            log.Error(errorMsg);
                            throw new Exception(errorMsg);
                        }


                        var wrapper = FormWrapper(computerAdditionalData).Result;


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
                            //Check what type of user to use
                            var userName = computerSystemWrapper.UserName;
                            if (Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["UseLastLoginUser"].ToString()) == 1)
                            {
                                userName = RSystem.LastLogonUserName;
                            }

                            computer._ComputerSystem = new Models.ComputerSystem()
                            {
                                Manufacturer = computerSystemWrapper.Manufacturer,
                                Model = computerSystemWrapper.Model,
                                Name = computerSystemWrapper.Name,
                                TimeStamp = computerSystemWrapper.TimeStamp,
                                UserName = userName

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
                                if (networkAdapterWrapperSingle != null)
                                {
                                    computer._NetworkAdapter.Add(new Models.NetworkAdapter()
                                    {

                                        Name = networkAdapterWrapperSingle.Name,

                                    });
                                }
                            }
                        }

                        //Check if found
                        if (networkAdapterConfigurationWrapper != null)
                        {
                            computer._NetworkAdapterConfiguration = new List<Models.NetworkAdapterConfiguration>();

                            foreach (var networkAdapterConfigurationWrapperSingle in networkAdapterConfigurationWrapper)
                            {
                                if (networkAdapterConfigurationWrapperSingle != null)
                                {
                                    computer._NetworkAdapterConfiguration.Add(new Models.NetworkAdapterConfiguration()
                                    {

                                        IPAddress = networkAdapterConfigurationWrapperSingle.IPAddress,
                                        MacAddress = networkAdapterConfigurationWrapperSingle.MacAddress,

                                    });
                                }
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
                                if (programsWrapperSingle != null)
                                {
                                    computer._Programs.Add(new Models.Programs()
                                    {

                                        DisplayName = programsWrapperSingle.DisplayName,
                                        Publisher = programsWrapperSingle.Publisher,
                                        Version = programsWrapperSingle.Version,

                                    });
                                }
                            }
                        }

                        //Check if found
                        if (logicalDiskWrapper != null)
                        {
                            computer._LogicalDisk = new List<Models.LogicalDisk>();

                            foreach (var logicalDiskWrapperSingle in logicalDiskWrapper)
                            {
                                if (logicalDiskWrapperSingle != null)
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

                        }

                        //Add to computers
                        devices.Add(computer);

                    }

                    tcs.SetResult(devices);

                });

                return tcs.Task;
            }
            catch (Exception ex)
            {
                throw;
            }
            

            
        }


        private static async Task<Wrapper> FormWrapper(List<HttpResponseMessage> httpResponses)
        {
            Wrapper wrapper = new Wrapper()
            {
                ComputerSystem = JsonConvert.DeserializeObject<GenericValueWrapper<ComputerSystem>>(await httpResponses[0].Content.ReadAsStringAsync()),
                OperatingSystem = JsonConvert.DeserializeObject<GenericValueWrapper<Entities.OperatingSystem>>(await httpResponses[1].Content.ReadAsStringAsync()),
                PCBios = JsonConvert.DeserializeObject<GenericValueWrapper<PCBios>>(await httpResponses[2].Content.ReadAsStringAsync()),
                VideoControllerData = JsonConvert.DeserializeObject<GenericValueWrapper<VideoControllerData>>(await httpResponses[3].Content.ReadAsStringAsync()),
                X86PCMemory = JsonConvert.DeserializeObject<GenericValueWrapper<X86PCMemory>>(await httpResponses[4].Content.ReadAsStringAsync()),
                Enclosure = JsonConvert.DeserializeObject<GenericValueWrapper<Enclosure>>(await httpResponses[5].Content.ReadAsStringAsync()),
                Processor = JsonConvert.DeserializeObject<GenericValueWrapper<Processor>>(await httpResponses[6].Content.ReadAsStringAsync()),
                NetworkAdapter = JsonConvert.DeserializeObject<GenericValueWrapper<NetworkAdapter>>(await httpResponses[7].Content.ReadAsStringAsync()),
                NetworkAdapterConfiguration = JsonConvert.DeserializeObject<GenericValueWrapper<NetworkAdapterConfiguration>>(await httpResponses[8].Content.ReadAsStringAsync()),
                SoundDevice = JsonConvert.DeserializeObject<GenericValueWrapper<SoundDevice>>(await httpResponses[9].Content.ReadAsStringAsync()),
                Programs = JsonConvert.DeserializeObject<GenericValueWrapper<Programs>>(await httpResponses[10].Content.ReadAsStringAsync()),
                LogicalDisk = JsonConvert.DeserializeObject<GenericValueWrapper<LogicalDisk>>(await httpResponses[11].Content.ReadAsStringAsync()),
            };

            return wrapper;
        }


        private static bool FetchIsOK(List<HttpResponseMessage> httpResponses)
        {
            foreach (var httpResponse in httpResponses)
            {
                if (!httpResponse.IsSuccessStatusCode)
                {
                    log.Error($"Status Code: {httpResponse.StatusCode}");

                    // Log headers, if any.
                    if (httpResponse.Headers != null)
                    {
                        log.Error($"Response Headers: {string.Join(", ", httpResponse.Headers.Select(h => $"{h.Key}: {string.Join(", ", h.Value)}"))}");
                    }

                    // Log the request details.
                    if (httpResponse.RequestMessage != null)
                    {
                        log.Error($"Request Uri: {httpResponse.RequestMessage.RequestUri}! Request Method: {httpResponse.RequestMessage.Method}!");

                        // Log request headers.
                        if (httpResponse.RequestMessage.Headers != null)
                        {
                            log.Error($"Request Headers: {string.Join(", ", httpResponse.RequestMessage.Headers.Select(h => $"{h.Key}: {string.Join(", ", h.Value)}"))}");
                        }
                    }

                    return false;
                }
            }
            return true;
        }

        private static async Task<HttpResponseMessage> FetchBaseData(string token)
        {
            try
            {
                // Fetch everything
                Task<HttpResponseMessage> RSystemWrapper = FetchDataSingular(token, System.Configuration.ConfigurationManager.AppSettings["SCCM_URL_R_System"].ToString());

                var result = await RSystemWrapper;

                log.Info($"Fetch completed. Result Status: {result.StatusCode}");
                log.Info($"Result Content: {await result.Content.ReadAsStringAsync()}");

                // Log error details if the response indicates a failure
                if (!result.IsSuccessStatusCode)
                {
                    log.Error($"Error occurred while fetching. Status: {result.StatusCode}");
                }

                return result;
            }
            catch (Exception ex)
            {
                log.Error($"Error Message: {ex.Message}");
                log.Error($"Error StackTrace: {ex.StackTrace}");

                if (ex.InnerException != null)
                {
                    log.Error($"Inner Error Message: {ex.InnerException.Message}");
                    log.Error($"Inner Error StackTrace: {ex.InnerException.StackTrace}");
                }

                throw;
            }
        }

        private static async Task<IEnumerable<HttpResponseMessage>> FetchAdditionalData(string token, long resourceId)
        {
            try
            {
                //Fetch everything
                Task<HttpResponseMessage> computerSystemWrapper = FetchDataSingular(token, System.Configuration.ConfigurationManager.AppSettings["SCCM_URL_R_System"].ToString() + "(" + resourceId + ")" + "/" + System.Configuration.ConfigurationManager.AppSettings["SCCM_URL_Computer_System"].ToString());

                Task<HttpResponseMessage> operatingSystemWrapper = FetchDataSingular(token, System.Configuration.ConfigurationManager.AppSettings["SCCM_URL_R_System"].ToString() + "(" + resourceId + ")" + "/" + System.Configuration.ConfigurationManager.AppSettings["SCCM_URL_Operating_System"].ToString());

                Task<HttpResponseMessage> PCBiosWrapper = FetchDataSingular(token, System.Configuration.ConfigurationManager.AppSettings["SCCM_URL_R_System"].ToString() + "(" + resourceId + ")" + "/" + System.Configuration.ConfigurationManager.AppSettings["SCCM_URL_PC_BIOS"].ToString());

                Task<HttpResponseMessage> videoControllerDataWrapper = FetchDataSingular(token, System.Configuration.ConfigurationManager.AppSettings["SCCM_URL_R_System"].ToString() + "(" + resourceId + ")" + "/" + System.Configuration.ConfigurationManager.AppSettings["SCCM_URL_Video_Controller"].ToString());

                Task<HttpResponseMessage> X86PCMemoryWrapper = FetchDataSingular(token, System.Configuration.ConfigurationManager.AppSettings["SCCM_URL_R_System"].ToString() + "(" + resourceId + ")" + "/" + System.Configuration.ConfigurationManager.AppSettings["SCCM_URL_X86_PC_Memory"].ToString());

                Task<HttpResponseMessage> enclosureWrapper = FetchDataSingular(token, System.Configuration.ConfigurationManager.AppSettings["SCCM_URL_R_System"].ToString() + "(" + resourceId + ")" + "/" + System.Configuration.ConfigurationManager.AppSettings["SCCM_URL_Enclosure"].ToString());

                Task<HttpResponseMessage> processorWrapper = FetchDataSingular(token, System.Configuration.ConfigurationManager.AppSettings["SCCM_URL_R_System"].ToString() + "(" + resourceId + ")" + "/" + System.Configuration.ConfigurationManager.AppSettings["SCCM_URL_Processor"].ToString());

                Task<HttpResponseMessage> networkAdapterWrapper = FetchDataSingular(token, System.Configuration.ConfigurationManager.AppSettings["SCCM_URL_R_System"].ToString() + "(" + resourceId + ")" + "/" + System.Configuration.ConfigurationManager.AppSettings["SCCM_URL_Network_Adapter"].ToString());

                Task<HttpResponseMessage> networkAdapterConfigurationWrapper = FetchDataSingular(token, System.Configuration.ConfigurationManager.AppSettings["SCCM_URL_R_System"].ToString() + "(" + resourceId + ")" + "/" + System.Configuration.ConfigurationManager.AppSettings["SCCM_URL_Network_Adapter_Configuration"].ToString());

                Task<HttpResponseMessage> soundDeviceWrapper = FetchDataSingular(token, System.Configuration.ConfigurationManager.AppSettings["SCCM_URL_R_System"].ToString() + "(" + resourceId + ")" + "/" + System.Configuration.ConfigurationManager.AppSettings["SCCM_URL_Sound_Device"].ToString());

                Task<HttpResponseMessage> programsWrapper = FetchDataSingular(token, System.Configuration.ConfigurationManager.AppSettings["SCCM_URL_R_System"].ToString() + "(" + resourceId + ")" + "/" + System.Configuration.ConfigurationManager.AppSettings["SCCM_URL_Add_Remove_Programs"].ToString());

                Task<HttpResponseMessage> logicalDisksWrapper = FetchDataSingular(token, System.Configuration.ConfigurationManager.AppSettings["SCCM_URL_R_System"].ToString() + "(" + resourceId + ")" + "/" + System.Configuration.ConfigurationManager.AppSettings["SCCM_URL_Logical_Disk"].ToString());

                Stopwatch stopwatch = new Stopwatch();

                stopwatch.Start();
                var result = await Task.WhenAll(computerSystemWrapper, operatingSystemWrapper, PCBiosWrapper, videoControllerDataWrapper, X86PCMemoryWrapper, enclosureWrapper, processorWrapper, networkAdapterWrapper, networkAdapterConfigurationWrapper, soundDeviceWrapper, programsWrapper, logicalDisksWrapper);
                stopwatch.Stop();
                Console.WriteLine("Time taken to fetch all data: " + stopwatch.ElapsedMilliseconds);

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }


        }

        private static async Task<HttpResponseMessage> FetchDataSingular(string token, string endPath)
        {
            try
            {
                Request request = new Request(token);
                var response = await request.Get(endPath);

                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
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
                var errorMsg = $"Acquiring a token failed with the following error: {ex.Message}";
                log.Error(errorMsg);
                Console.WriteLine(errorMsg);
                
                if (ex.InnerException != null)
                {
                    var innerExMsg = $"Error detail: {ex.InnerException.Message}";
                    log.Error(innerExMsg);
                    Console.WriteLine(innerExMsg);
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    var innerExMsg = $"Error detail: {ex.InnerException.Message}";
                    log.Error(innerExMsg);
                    Console.WriteLine(innerExMsg);
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
                var errorMsg = "Acquiring a token failed";
                log.Error(errorMsg);
                throw new Exception(errorMsg);
            }

        }


    }

}
