using System;
using System.Data;
using System.Data.SqlClient;
using NLog;
using upKeeper2Helpdesk.entities;

namespace upKeeper2Helpdesk.data
{
	public class ComputerData
	{
		private readonly string _ConnectionString;
		private static readonly Logger logger = LogManager.GetCurrentClassLogger();

		public ComputerData(string ConnectionString) {
			_ConnectionString = ConnectionString;
		}

		public void Save(Computer c) {

            string s = "";

            using (var connection = new SqlConnection(_ConnectionString))
			{
				try
				{
					using (var command = connection.CreateCommand())
					{
						command.CommandType = CommandType.Text;

						s = "update tblComputer set " +
										"SerialNumber='" + c.SerialNumber + "', " +
										"Manufacturer='" + c.Manufacturer + "', ";

						if (c.ComputerModel_Id != null)
							s = s + "MACAddress='" + c.MACAddress + "', ";
						else
							s = s + "MACAddress='', ";
						

						if (c.ComputerModel_Id != null)
							s = s + "ComputerModel_Id=" + c.ComputerModel_Id + ", ";
						else
							s = s + "ComputerModel_Id=Null, ";

						if (c.RAM_Id != null)
							s = s + "RAM_Id=" + c.RAM_Id + ", ";
						else
							s = s + "RAM_Id=Null, ";

						if (c.Processor_Id != null)
							s = s + "Processor_Id=" + c.Processor_Id + ", ";
						else
							s = s + "Processor_Id=Null, ";

						if (c.OS_Id != null)
							s = s + "OS_Id=" + c.OS_Id + ", ";
						else
							s = s + "OS_Id=Null, ";

						if (c.NIC_Id != null)
							s = s + "NIC_Id=" + c.NIC_Id + ", ";
						else
							s = s + "NIC_Id=Null, ";
						
						s = s + "BIOSVersion='" + (c.BIOSVersion.Length > 40 ? c.BIOSVersion.Substring(0, 40) : c.BIOSVersion) + "', ";

						if (c.ClientInformation != null && c.ClientInformation.User_Id != null) {
							s = s + "User_Id=" + c.ClientInformation.User_Id + ", ";
						}
						else {
							s = s + "User_Id=Null, "; 
						}

						if (c.Domain_Id != null)
						{
							s = s + "Domain_Id=" + c.Domain_Id + ", ";
						}
						else
						{
							s = s + "Domain_Id=Null, ";
						}

						s = s + "ScanDate='" + c.ScanDate + "', " +
								"SyncChangedDate=getdate() " +
								"where computerName='" + c.Name + "' and Customer_Id=" + c.Customer_Id;

						command.CommandText = s;
						connection.Open();
						command.ExecuteNonQuery();

						string del = "delete from tblSoftware where Computer_Id=" + c.Computer_Id.ToString();
						
						command.CommandText = del;
						command.ExecuteNonQuery();

						if (c.Software != null) {
							foreach (var software in c.Software) {
								string sql = "insert into tblSoftware (Computer_Id, Name, Version, Manufacturer, Registration_code) values(" + c.Computer_Id + ", '" + software.Name.Replace("'", "").Substring(0, Math.Min(software.Name.Replace("'", "").Length, 100)) + "', '" + software.Version + "', '" + software.Manufacturer + "', '" + software.Registration_code + "')";

								command.CommandText = sql;
								command.ExecuteNonQuery();
							}
						}

						if (c.Hotfix != null)
						{
							foreach (var hotfix in c.Hotfix)
							{
								string sql = "insert into tblSoftware (Computer_Id, Name) values(" + c.Computer_Id + ", '" + hotfix.HotFixId + "')";

								command.CommandText = sql;
								command.ExecuteNonQuery();
							}
						}

						command.CommandText = "delete from tblLogicalDrive where Computer_Id=" + c.Computer_Id.ToString();
						command.ExecuteNonQuery();

						if (c.LogicalDrives != null)
						{
							foreach (var logicalDrive in c.LogicalDrives)
							{
								string sql = "insert into tblLogicalDrive (Computer_Id, DriveLetter, DriveType, TotalBytes, FreeBytes, FileSystemName) values(" + c.Computer_Id + ", '" + (logicalDrive.DriveLetter.Length > 10 ? logicalDrive.DriveLetter.Substring(0, 10) : logicalDrive.DriveLetter) + "', " + logicalDrive.DriveType + ", " + logicalDrive.TotalBytes + ", " + logicalDrive.FreeBytes + ", '')";

								command.CommandText = sql;
								command.ExecuteNonQuery();
							}
						}
					}
				}
				catch (Exception ex)
				{
					logger.Error("Save: " + ex);
                    throw new System.InvalidOperationException(s);

                }
			}
		}

		public void Create(Computer c)
		{
			string s = "";

			using (var connection = new SqlConnection(_ConnectionString))
			{
				try
				{
					using (var command = connection.CreateCommand())
					{
						command.CommandType = CommandType.Text;

						s = "insert into tblComputer (Customer_Id, computerName) values(" + c.Customer_Id + ", '" + c.Name + "')";
						
						command.CommandText = s;
						connection.Open();
						command.ExecuteNonQuery();


					}
				}
				catch (Exception ex)
				{
					throw new System.InvalidOperationException(s);

				}
			}
		}

		public int ExistsObject(ObjectType Type, string Name)
		{
			using (var connection = new SqlConnection(_ConnectionString))
			{
				using (var command = connection.CreateCommand())
				{
					command.CommandType = CommandType.StoredProcedure;

					if (Type == ObjectType.ComputerModel)
						command.CommandText = "dhExistsComputerModel";
					else if (Type == ObjectType.RAM)
						command.CommandText = "dhExistsRAM";
					else if (Type == ObjectType.Processor)
						command.CommandText = "dhExistsProcessor";
					else if (Type == ObjectType.OS)
						command.CommandText = "dhExistsOS";
					else if (Type == ObjectType.NetworkAdapter)
						command.CommandText = "dhExistsNetworkAdapter";

					command.Parameters.Add(new SqlParameter("@Namn", SqlDbType.NVarChar)).Value = Name;
					command.Parameters.Add("@RETURN", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
					
					connection.Open();

					command.ExecuteScalar();
						
					return (int)command.Parameters["@RETURN"].Value;
				}
			}
		}


		public int? GetComputerUserById(int Customer_Id, string UserId)
		{
			int? id = null;

			using (var connection = new SqlConnection(_ConnectionString))
			{
				using (var command = connection.CreateCommand())
				{
					command.CommandType = CommandType.Text;
					command.CommandText = "SELECT tblComputerUsers.Id FROM tblComputerUsers WHERE UserId = '" + UserId + "' AND tblComputerUsers.Customer_Id=" + Customer_Id;
					connection.Open();

					using (var dr = command.ExecuteReader())
					{
						while (dr.Read())
						{
							id = dr.SafeGetInteger("Id");

							break;
						}
					}

					return id;
				}
			}
		}

		public int? GetDomainByName(int Customer_Id, string DomainName)
		{
			int? id = null;

			string[] a = DomainName.Split('.');

			using (var connection = new SqlConnection(_ConnectionString))
			{
				using (var command = connection.CreateCommand())
				{
					command.CommandType = CommandType.Text;
					command.CommandText = "SELECT tblDomain.Id FROM tblDomain WHERE DomainName = '" + a[0] + "' AND tblDomain.Customer_Id=" + Customer_Id;
					connection.Open();

					using (var dr = command.ExecuteReader())
					{
						while (dr.Read())
						{
							id = dr.SafeGetInteger("Id");

							break;
						}
					}

					return id;
				}
			}
		}

		public Computer GetComputerInfo(Computer Computer)
		{
			using (var connection = new SqlConnection(_ConnectionString))
			{
				using (var command = connection.CreateCommand())
				{
					command.CommandType = CommandType.Text;
					command.CommandText = "SELECT tblComputer.Id, tblComputer.Location2, tblComputer.ScrapDate, tblComputer.Customer_Id FROM tblComputer WHERE ComputerName = '" + Computer.Name + "' AND tblComputer.Customer_Id=" + Computer.Customer_Id;
					connection.Open();

					using (var dr = command.ExecuteReader())
					{
						while (dr.Read())
						{
							Computer.Computer_Id = dr.SafeGetInteger("Id");
							Computer.Location2 = dr.SafeGetString("Location2");
							Computer.ScrapDate = dr.SafeGetNullableDateTime("ScrapDate");
							Computer.Customer_Id = dr.SafeGetInteger("Customer_Id");

							break;
						}
					}

					return Computer;
				}
			}
		}

		public void UpdateApplication(int Customer_Id) 
		{

			string s = "";

			using (var connection = new SqlConnection(_ConnectionString))
			{
				try
				{
					using (var command = connection.CreateCommand())
					{
						command.CommandType = CommandType.Text;

						// Ta bort gamla program som inte används
						s = "DELETE FROM tblApplication WHERE Id NOT IN (SELECT Application_Id FROM tblProduct_tblApplication)";

						command.CommandText = s;
						connection.Open();
						command.ExecuteNonQuery();

						// Lägg in alla program som inventeras i licensmodulen
						s = "INSERT INTO tblApplication(Customer_Id, Application) " +
							"SELECT DISTINCT " + Customer_Id + ", Name FROM tblSoftware WHERE Name Not IN (SELECT Application FROM tblApplication WHERE Customer_Id=" + Customer_Id + ")";

						command.CommandText = s;
						command.ExecuteNonQuery();
					}
				}
				catch 
				{
					throw new System.InvalidOperationException(s);

				}
			}

		}
	}
}
