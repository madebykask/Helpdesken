using DH.Helpdesk.SCCM.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public bool ComputerExist(Computer computer)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                using (var command = new SqlCommand { Connection = connection, CommandType = CommandType.Text, CommandTimeout = ConnectionTimeout })
                {

                    var customerID = System.Configuration.ConfigurationManager.AppSettings["DB_Customer_Id"].ToString();

                    if (computer._OperatingSystem.ComputerRole == 0)
                    {
                        command.CommandText = "SELECT TOP 1 COUNT(*) as count FROM tblComputer where Customer_Id = " + customerID + " AND UPPER(ComputerName) = " + computer._ComputerSystem.Name.ToUpper();

                    }
                    else if (computer._OperatingSystem.ComputerRole == 1)
                    {
                        command.CommandText = "SELECT TOP 1 COUNT(*) as count FROM tblServer where Customer_Id = " + customerID + " AND UPPER(ServerName) = " + computer._ComputerSystem.Name.ToUpper();
                    }
                    else
                    {
                        throw new Exception("ComputerRole not supported");
                    }


                    var reader = command.ExecuteReader();

                    int count = 0;

                    try
                    {
                        while (reader.Read())
                        {
                            count = Convert.ToInt32(reader["count"].ToString());
                        }
                    }
                    finally
                    {
                        reader.Close();
                    }

                    if (count > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
        }


    }
}
