using DH.Helpdesk.SCCM.Entities;
using DH.Helpdesk.SCCM.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DH.Helpdesk.SCCM.Other.Enums;

namespace DH.Helpdesk.SCCM.DB
{
    public class Connector
    {


        private string ConnectionString;

        private int ConnectionTimeout = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["DB_Connection_Timeout_Seconds"].ToString());

        public Connector(string ConnectionString)
        {
            this.ConnectionString = ConnectionString;
        }

        public ComputerDB GetComputer(Device computer)
        {

            ComputerDB res = null;

            using (var connection = new SqlConnection(ConnectionString))
            {
                using (var command = new SqlCommand { Connection = connection, CommandType = CommandType.Text, CommandTimeout = ConnectionTimeout })
                {

                    var customerID = System.Configuration.ConfigurationManager.AppSettings["DB_Customer_Id"].ToString();

                    command.CommandText = "SELECT TOP 1 * FROM tblComputer where Customer_Id = " + customerID + " AND UPPER(ComputerName) = " + computer._ComputerSystem.Name.ToUpper();


                    var reader = command.ExecuteReader();


                    try
                    {
                        while (reader.Read())
                        {

                            res = new ComputerDB();

                            res.Id = Convert.ToInt32(reader["Id"].ToString());
                            res.ComputerName = Convert.ToString(reader["ComputerName"].ToString());
                            res.Customer_Id = Convert.ToInt32(reader["Customer_Id"].ToString());

                        }
                    }
                    finally
                    {
                        reader.Close();
                    }


                }
            }

            return res;
        }

        public ServerDB GetServer(Device computer)
        {
            ServerDB res = null;

            using (var connection = new SqlConnection(ConnectionString))
            {
                using (var command = new SqlCommand { Connection = connection, CommandType = CommandType.Text, CommandTimeout = ConnectionTimeout })
                {

                    var customerID = System.Configuration.ConfigurationManager.AppSettings["DB_Customer_Id"].ToString();

                    command.CommandText = "SELECT TOP 1 * FROM tblServer where Customer_Id = " + customerID + " AND UPPER(ServerName) = " + computer._ComputerSystem.Name.ToUpper();


                    var reader = command.ExecuteReader();


                    try
                    {
                        while (reader.Read())
                        {
                            res = new ServerDB();


                            res.Id = Convert.ToInt32(reader["Id"].ToString());
                            res.ComputerName = Convert.ToString(reader["ServerName"].ToString());
                            res.Customer_Id = Convert.ToInt32(reader["Customer_Id"].ToString());
                        }
                    }
                    finally
                    {
                        reader.Close();
                    }


                }
            }

            return res;
        }

        public ComputerDB InsertComputerAndReturn(Device computer)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                using (var command = new SqlCommand { Connection = connection, CommandType = CommandType.Text, CommandTimeout = ConnectionTimeout })
                {

                    var customerID = System.Configuration.ConfigurationManager.AppSettings["DB_Customer_Id"].ToString();

                    command.CommandText = "INSERT INTO tblComputer (ComputerName, Customer_Id) VALUES (@ComputerName, @Customer_Id)";
                    command.Parameters.AddWithValue("@ComputerName", computer._ComputerSystem.Name);
                    command.Parameters.AddWithValue("@Customer_Id", customerID);
                    command.ExecuteNonQuery();
                }
            }

            return GetComputer(computer);
        }

        public ServerDB InsertServerAndReturn(Device computer)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                using (var command = new SqlCommand { Connection = connection, CommandType = CommandType.Text, CommandTimeout = ConnectionTimeout })
                {

                    var customerID = System.Configuration.ConfigurationManager.AppSettings["DB_Customer_Id"].ToString();

                    command.CommandText = "INSERT INTO tblServer (ServerName, Customer_Id) VALUES (@ServerName, @Customer_Id)";
                    command.Parameters.AddWithValue("@ServerName", computer._ComputerSystem.Name);
                    command.Parameters.AddWithValue("@Customer_Id", customerID);
                    command.ExecuteNonQuery();
                }
            }

            return GetServer(computer);
        }

        public int ExistsObject(ObjectType Type, string Name)
        {
            using (var connection = new SqlConnection(ConnectionString))
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

        public int? GetComputerUserByUserId(int CustomerId, string username)
        {

            int? result = null;

            using (var connection = new SqlConnection(ConnectionString))
            {
                using (var command = new SqlCommand { Connection = connection, CommandType = CommandType.Text, CommandTimeout = ConnectionTimeout })
                {

                    var customerID = System.Configuration.ConfigurationManager.AppSettings["DB_Customer_Id"].ToString();

                    command.CommandText = "SELECT TOP 1 tblComputerUsers.*, tblDepartment.Region_Id FROM tblComputerUsers LEFT JOIN tblDepartment ON tblComputerUsers.Department_Id=tblDepartment.Id WHERE tblComputerUsers.Customer_Id=" + customerID + " AND  LOWER(tblComputerUsers.UserId) = '" + username.ToLower() + "'";


                    var reader = command.ExecuteReader();


                    try
                    {
                        while (reader.Read())
                        {
                            result = Convert.ToInt32(reader["Id"].ToString());
                        }
                    }
                    finally
                    {
                        reader.Close();
                    }


                }
            }

            return result;
        }

        public void SaveServer(ServerDB c)
        {

            string s = "";

            using (var connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;

                        s = "update tblComputer set " +
                                        "SerialNumber='" + c.SerialNumber + "', " +
                                        "Manufacturer='" + c.Manufacturer + "', ";

                        if (c.ComputerModel_Id != 0)
                            s = s + "MACAddress='" + c.MACAddress + "', ";
                        else
                            s = s + "MACAddress='', ";


                        if (c.ComputerModel_Id != 0)
                            s = s + "ComputerModel_Id=" + c.ComputerModel_Id + ", ";
                        else
                            s = s + "ComputerModel_Id=Null, ";

                        if (c.RAM_Id != 0)
                            s = s + "RAM_Id=" + c.RAM_Id + ", ";
                        else
                            s = s + "RAM_Id=Null, ";

                        if (c.Processor_Id != 0)
                            s = s + "Processor_Id=" + c.Processor_Id + ", ";
                        else
                            s = s + "Processor_Id=Null, ";

                        if (c.OS_Id != 0)
                            s = s + "OS_Id=" + c.OS_Id + ", ";
                        else
                            s = s + "OS_Id=Null, ";

                        if (c.NIC_Id != 0)
                            s = s + "NIC_Id=" + c.NIC_Id + ", ";
                        else
                            s = s + "NIC_Id=Null, ";

                        s = s + "BIOSVersion='" + (c.BIOSVersion.Length > 40 ? c.BIOSVersion.Substring(0, 40) : c.BIOSVersion) + "', ";

                        if (c.User_Id != 0)
                        {
                            s = s + "User_Id=" + c.User_Id + ", ";
                        }
                        else
                        {
                            s = s + "User_Id=Null, ";
                        }

                        if (c.Domain_Id != 0)
                        {
                            s = s + "Domain_Id=" + c.Domain_Id + ", ";
                        }
                        else
                        {
                            s = s + "Domain_Id=Null, ";
                        }
                        if (c.WarrantyEndDate != null)
                        {
                            s = s + "WarrantyEndDate='" + c.WarrantyEndDate + "', ";
                        }
                        else
                        {
                            s = s + "WarrantyEndDate=Null, ";
                        }
                        s = s + "ScanDate='" + c.ScanDate + "', " +
                                "SyncChangedDate=getdate() " +
                                "where computerName='" + c.ComputerName + "' and Customer_Id=" + c.Customer_Id;

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

        public void SaveComputer(ComputerDB c)
        {

            string s = "";

            using (var connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;

                        s = "update tblComputer set " +
                                        "SerialNumber='" + c.SerialNumber + "', " +
                                        "Manufacturer='" + c.Manufacturer + "', ";

                        if (c.ComputerModel_Id != 0)
                            s = s + "MACAddress='" + c.MACAddress + "', ";
                        else
                            s = s + "MACAddress='', ";


                        if (c.ComputerModel_Id != 0)
                            s = s + "ComputerModel_Id=" + c.ComputerModel_Id + ", ";
                        else
                            s = s + "ComputerModel_Id=Null, ";

                        if (c.RAM_Id != 0)
                            s = s + "RAM_Id=" + c.RAM_Id + ", ";
                        else
                            s = s + "RAM_Id=Null, ";

                        if (c.Processor_Id != 0)
                            s = s + "Processor_Id=" + c.Processor_Id + ", ";
                        else
                            s = s + "Processor_Id=Null, ";

                        if (c.OS_Id != 0)
                            s = s + "OS_Id=" + c.OS_Id + ", ";
                        else
                            s = s + "OS_Id=Null, ";

                        if (c.NIC_Id != 0)
                            s = s + "NIC_Id=" + c.NIC_Id + ", ";
                        else
                            s = s + "NIC_Id=Null, ";

                        s = s + "BIOSVersion='" + (c.BIOSVersion.Length > 40 ? c.BIOSVersion.Substring(0, 40) : c.BIOSVersion) + "', ";

                        if (c.User_Id != 0)
                        {
                            s = s + "User_Id=" + c.User_Id + ", ";
                        }
                        else
                        {
                            s = s + "User_Id=Null, ";
                        }

                        if (c.Domain_Id != 0)
                        {
                            s = s + "Domain_Id=" + c.Domain_Id + ", ";
                        }
                        else
                        {
                            s = s + "Domain_Id=Null, ";
                        }
                        if (c.WarrantyEndDate != null)
                        {
                            s = s + "WarrantyEndDate='" + c.WarrantyEndDate + "', ";
                        }
                        else
                        {
                            s = s + "WarrantyEndDate=Null, ";
                        }
                        s = s + "ScanDate='" + c.ScanDate + "', " +
                                "SyncChangedDate=getdate() " +
                                "where computerName='" + c.ComputerName + "' and Customer_Id=" + c.Customer_Id;

                        command.CommandText = s;
                        connection.Open();
                        command.ExecuteNonQuery();

                        string del = "delete from tblSoftware where Computer_Id=" + c.Id.ToString();

                        command.CommandText = del;
                        command.ExecuteNonQuery();

                        if (c.Softwares != null)
                        {
                            foreach (var software in c.Softwares)
                            {
                                string sql = "insert into tblSoftware (Computer_Id, Name, Version, Manufacturer) values(" + c.Id + ", '" + software.DisplayName.Replace("'", "").Substring(0, Math.Min(software.DisplayName.Replace("'", "").Length, 100)) + "', '" + software.Version + "', '" + software.Publisher + "', '" + ")";

                                command.CommandText = sql;
                                command.ExecuteNonQuery();
                            }
                        }


                        command.CommandText = "delete from tblLogicalDrive where Computer_Id=" + c.Id.ToString();
                        command.ExecuteNonQuery();

                        if (c.LogicalDrives != null)
                        {
                            foreach (var logicalDrive in c.LogicalDrives)
                            {
                                string sql = "insert into tblLogicalDrive (Computer_Id, DriveLetter, DriveType, TotalBytes, FreeBytes, FileSystemName) values(" + c.Id + ", '" + (logicalDrive.Name.Length > 10 ? logicalDrive.Name.Substring(0, 10) : logicalDrive.Name) + "', " + logicalDrive.DriveType + ", " + logicalDrive.Size + ", " + logicalDrive.FreeSpace + ", '')";

                                command.CommandText = sql;
                                command.ExecuteNonQuery();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new System.InvalidOperationException(s);

                }
            }
        }
    }
}
