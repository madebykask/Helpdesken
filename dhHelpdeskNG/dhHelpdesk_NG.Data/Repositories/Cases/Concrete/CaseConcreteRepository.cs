using DbExtensions;
using DH.Helpdesk.Common.Constants;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Domain;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DH.Helpdesk.BusinessData.Models.Case.Output;

namespace DH.Helpdesk.Dal.Repositories.Cases.Concrete
{
    public class CaseConcreteRepository
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["HelpdeskSqlServerDbContext"].ConnectionString;

        public bool DeleteCases(List<int> caseIds, int jobTimeout = 0)
        {
            bool ret = false;

            if (caseIds.Count() > 0)
            {
                DataTable casesTable = new DataTable();
                casesTable.Columns.Add(new DataColumn("Id", typeof(int)));

                // populate DataTable from your List here
                foreach (var id in caseIds)
                {
                    casesTable.Rows.Add(id);
                }

                //SqlConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder(_connectionString)
                //{
                //    ConnectTimeout = 4000,
                //    AsynchronousProcessing = true
                //};
                using (var connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    using (var command = new SqlCommand { Connection = connection, CommandType = CommandType.StoredProcedure})
                    {
                        SqlParameter param = command.Parameters.AddWithValue("@Cases", casesTable);
                        param.SqlDbType = SqlDbType.Structured;
                        param.TypeName = "dbo.IdsList";
                        //param.Direction = ParameterDirection.Output;
                        command.CommandText = "sp_DeleteCases";
                        if (jobTimeout > 0)
                        {
                            command.CommandTimeout = jobTimeout;
                        }
                        try
                        {
                            //var result = command.BeginExecuteNonQuery(null, command);
                            //var rowsAffected = command.EndExecuteNonQuery(result);
                            var result = command.ExecuteNonQuery();
                            
                            if (result > 0)
                            {
                                ret = true;
                            }
                        }
                        catch (SqlException ex)
                        {
                            throw ex;
                        }

                        catch (Exception ex)
                        {
                            throw ex;
                        }

                        finally
                        {
                            connection.Close();
                        }
                    }
                }
            }
            return ret;

        }
    }
}
