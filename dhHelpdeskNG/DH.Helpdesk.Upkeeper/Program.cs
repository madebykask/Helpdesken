using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using NLog;
using upKeeper2Helpdesk.api;
using upKeeper2Helpdesk.data;
using upKeeper2Helpdesk.entities;

namespace upKeeper2Helpdesk
{
	class Program
	{
		private static readonly Logger logger = LogManager.GetCurrentClassLogger();

		static void Main(string[] args)
		{
			try
			{
				DateTime startTime = DateTime.Now;

				var builder = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", false);

				IConfigurationRoot configuration = builder.Build();
				
				BaseAPI api = new BaseAPI(configuration.GetSection("UpKeeperUrl").Value);

                Token t = api.Login(configuration.GetSection("UserName").Value, configuration.GetSection("Password").Value);

				int Customer_Id = 1;
				if (configuration.GetSection("Customer_Id").Value != null)
				{
					bool success = Int32.TryParse(configuration.GetSection("Customer_Id").Value, out int number);
					if (success)
					{
						Customer_Id = number;
					}
				}

				bool CreateInventory = false;
				if (configuration.GetSection("CreateInventory").Value != null && configuration.GetSection("CreateInventory").Value.ToLower() == "true") {
					CreateInventory = true;
				}

				bool UpdateInventory = false;
				if (configuration.GetSection("UpdateInventory").Value != null && configuration.GetSection("UpdateInventory").Value.ToLower() == "true")
				{
					UpdateInventory = true;
				}

				bool UpdateUpKeeper = true;
				if (configuration.GetSection("UpdateUpKeeper").Value != null && configuration.GetSection("UpdateUpKeeper").Value.ToLower() == "false")
				{
					UpdateUpKeeper = false;
				}


				if (t != null)
				{
					var data = new ComputerData(configuration.GetSection("HelpdeskDB").Value);
					var idx = 0;

					// Hämta alla datorer
					IEnumerable<IDictionary<string, string>> computers = api.GetComputerNames(t);

					logger.Debug("Antal datorer: " + computers.Count());

					foreach (Dictionary<string, string> c in computers)
					{
						if ((DateTime.Now - startTime).TotalSeconds > 300)
						{
							t = api.Login(configuration.GetSection("UserName").Value, configuration.GetSection("Password").Value);

							startTime = DateTime.Now;
						}

						idx += 1;

                        var computerId = c["Key"].ToString();

						logger.Debug("computerId: " + computerId);

						var computer = api.GetComputer(computerId, t);

						if (computer != null) {
							computer.Customer_Id = Customer_Id;

							logger.Debug("Synkroniserar: " + computer.Name + ", " + idx.ToString());

							computer = data.GetComputerInfo(computer);

							if (computer.Computer_Id == null && CreateInventory == true)
							{
								data.Create(computer);

								computer = data.GetComputerInfo(computer);
							}

							if (computer.Computer_Id == null)
							{
								logger.Error(computer.Name + " saknas i DH Helpdesk");
							}

							else if (computer.ScrapDate != null && computer.ScrapDate < DateTime.Today.AddDays(-7))
							{
								logger.Debug("DeleteComputer ");

								api.DeleteComputer(computerId, t);

								logger.Debug(computer.Name + " borttagen i upKeeper");
							}
							else if (UpdateInventory == true)
							{

								var hardware = api.GetHardware(computerId, t);

								if (hardware != null && hardware.Properties != null) {
									if (hardware.Properties.FirstOrDefault(x => x.Property == "Time of inventory") != null)
									{
										if (DateTime.TryParse(hardware.Properties.FirstOrDefault(x => x.Property == "Time of inventory").Value, out DateTime dt))
										{
											computer.ScanDate = dt;
										}
									}
								}

								if (computer.ScanDate == DateTime.MinValue)
								{
									logger.Error(computer.Name + " ej scannad");
								}
								else
								{
									if (hardware != null && hardware.Properties != null)
									{
										if (hardware.Properties.FirstOrDefault(x => x.Property == "Identifying Number") != null)
										{
											computer.SerialNumber = hardware.Properties.FirstOrDefault(x => x.Property == "Identifying Number").Value;
										}

										if (hardware.Properties.FirstOrDefault(x => x.Property == "Manufacturer") != null)
										{
											computer.Manufacturer = hardware.Properties.FirstOrDefault(x => x.Property == "Manufacturer").Value;
										}

										if (hardware.Properties.FirstOrDefault(x => x.Property == "Model") != null)
										{
											computer.ComputerModel_Id = data.ExistsObject(ObjectType.ComputerModel, hardware.Properties.FirstOrDefault(x => x.Property == "Model").Value);
										}

										if (hardware.Properties.FirstOrDefault(x => x.Property == "Total Physical Memory") != null)
										{
											computer.RAM_Id = data.ExistsObject(ObjectType.RAM, hardware.Properties.FirstOrDefault(x => x.Property == "Total Physical Memory").Value);
										}

										if (hardware.Properties.FirstOrDefault(x => x.Property == "Processor") != null)
										{
											computer.Processor_Id = data.ExistsObject(ObjectType.Processor, hardware.Properties.FirstOrDefault(x => x.Property == "Processor").Value);
										}

										if (hardware.Properties.FirstOrDefault(x => x.Property == "Operating System") != null)
										{
											computer.OS_Id = data.ExistsObject(ObjectType.OS, hardware.Properties.FirstOrDefault(x => x.Property == "Operating System").Value);
										}

										if (hardware.NetworkAdapters != null && hardware.NetworkAdapters.FirstOrDefault(x => x.Property == "Name") != null)
										{
											computer.NIC_Id = data.ExistsObject(ObjectType.NetworkAdapter, hardware.NetworkAdapters.FirstOrDefault(x => x.Property == "Name").Value);
										}

										if (hardware.Properties.FirstOrDefault(x => x.Property == "BIOS Version") != null)
										{
											computer.BIOSVersion = hardware.Properties.FirstOrDefault(x => x.Property == "BIOS Version").Value;
										}

										if (hardware.Properties.FirstOrDefault(x => x.Property == "Domain") != null)
										{
											computer.Domain_Id = data.GetDomainByName(1, hardware.Properties.FirstOrDefault(x => x.Property == "Domain").Value);
										}

									}

									computer.ClientInformation.User_Id = data.GetComputerUserById(1, computer.ClientInformation.LastLoggedInUser);

									computer.Software = api.GetComputerSoftware(computerId, t);

									computer.Hotfix = api.GetComputerUpdates(computerId, t);

									if (hardware.Disks != null)
									{
										computer.LogicalDrives = new List<LogicalDrive>();

										for (int i = 0; i < hardware.Disks.Count; i += 3)
										{
											var ld = new LogicalDrive
											{
												DriveType = 3
											};

											var cp = hardware.Disks[i];
											ld.DriveLetter = cp.Value;

											cp = hardware.Disks[i + 1];
											ld.TotalBytes = cp.Value.ConvertToBytes();

											cp = hardware.Disks[i + 2];
											ld.FreeBytes = cp.Value.ConvertToBytes();
											computer.LogicalDrives.Add(ld);
										}

									}

									logger.Debug(computer.Name + " sparas");

									data.Save(computer);

									if (UpdateUpKeeper == true && (computer.ScrapDate == null || computer.ScrapDate > DateTime.Today.AddDays(-7)))
									{
										logger.Debug(computer.Name + " Uppdatera upKeeper, location " + computer.Location2);

										// Uppdatera upKeeper
										var computerDetail = api.GetComputerDetail(computerId, t);

										if (computerDetail != null) {
											if (computer.Location2.Length > 60)
											{
												computerDetail.Location = computer.Location2.Substring(0, 60);
											}
											else
											{
												computerDetail.Location = computer.Location2;
											}

											string ret = api.SaveComputerDetails(computerId, computerDetail, t);
										}
									}

								}

							}
						}

					}

					// Uppdatera program så att de syns i licensmodulen
					data.UpdateApplication(Customer_Id);

				}
			}
			catch (Exception ex) {
				logger.Error(ex.Message);
			}
			
		}
	}
}
