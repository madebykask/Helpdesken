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

        public ComputerDB GetComputer(Computer computer)
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

        public ServerDB GetServer(Computer computer)
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
                            res.ServerName = Convert.ToString(reader["ServerName"].ToString());
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

        public ComputerDB InsertComputerAndReturn(Computer computer)
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

        public ServerDB InsertServerAndReturn(Computer computer)
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
    }
}
