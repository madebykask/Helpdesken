using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ECT.Model.Abstract;
using System.Data;
using System.Data.SqlClient;
using ECT.Model.Entities;
using System.Globalization;
using ECT.Model.Entities.Reports;
using System.Configuration;

namespace ECT.Model.Contrete
{
    public class GlobalViewRepository : IGlobalViewRepository
    {
        private readonly string _connectionString;

        public GlobalViewRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<GlobalView> GlobalViewSearch(string query, int customerId, int userId, string searchKey = null)
        {
            return GlobalViewSearch(query, customerId, null, userId, false, searchKey);
        }

        public IEnumerable<GlobalView> GlobalViewSearch(string query, int customerId, int userId, bool allCoWorkers, string searchKey = null)
        {
            return GlobalViewSearch(query, customerId, null, userId, allCoWorkers, searchKey);
        }

        public IEnumerable<GlobalView> GlobalViewSearch(string query, int customerId, int userId, bool allCoWorkers, string searchKey = null, string formFieldName = null)
        {
            return GlobalViewSearch(query, customerId, null, userId, allCoWorkers, searchKey, formFieldName);
        }

        public IEnumerable<GlobalView> GlobalViewSearch(string query, int customerId, string employeeNumbers, string searchKey = null)
        {
            return GlobalViewSearch(query, customerId, employeeNumbers, null, false, searchKey);
        }

        public IEnumerable<GlobalView> GlobalViewSearch(string query, int customerId, string employeeNumbers, int? userId, bool allCoWorkers, string searchKey = null, string formFieldName = null)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    command.Parameters.Add(new SqlParameter("@search", SqlDbType.NVarChar)).Value = query;
                    command.Parameters.Add(new SqlParameter("@customer_id", SqlDbType.Int)).Value = customerId;
                    command.Parameters.Add(new SqlParameter("@department_search_key", SqlDbType.NVarChar)).Value = searchKey;
                    command.Parameters.Add(new SqlParameter("@employeeNumbers", SqlDbType.NVarChar)).Value = employeeNumbers;
                    command.Parameters.Add(new SqlParameter("@allCoWorkers", SqlDbType.Bit)).Value = allCoWorkers;
                    command.Parameters.Add(new SqlParameter("@User_Id", SqlDbType.Int)).Value = userId;
                    command.Parameters.Add(new SqlParameter("@formFieldName", SqlDbType.NVarChar)).Value = formFieldName;

                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "ECT_GV_Get_Employee";
                    connection.Open();

                    using (var dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                            yield return new GlobalView
                            {
                                Id = dr.SafeGetInteger("Id"),
                                EmployeeNumber = dr.SafeGetString("EmployeeNumber"),
                                Name = dr.SafeGetString("FirstName"),
                                Surname = dr.SafeGetString("Surname"),
                                Company = dr.SafeGetString("Company"),
                                CompanyId = dr.SafeGetInteger("CompanyId"),
                                Unit = dr.SafeGetString("Unit"),
                                UnitId = dr.SafeGetInteger("UnitId"),
                                Department = dr.SafeGetString("Department"),
                                Function = dr.SafeGetString("ServiceArea"),
                                CaseNumber = dr.SafeGetString("CaseNumber"),
                                RegTime = dr.SafeGetNullableDateTime("RegTime"),
                                Email = dr.SafeGetString("Email"),
                                IKEANetworkID = dr.SafeGetString("NetworkID")//,
                                //WatchDate = dr.SafeGetString("WatchDate")
                            };
                    }
                }
            }
        }

        public IEnumerable<GlobalViewExtendedInfo> GetEmployeeExtendedInfo(Guid formGuid, string employeenumber)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    command.Parameters.Add(new SqlParameter("@FormGUID", SqlDbType.UniqueIdentifier)).Value = formGuid;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "ECT_GV_Get_Employee_ExtendedFields";
                    connection.Open();

                    using (var dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                            yield return new GlobalViewExtendedInfo
                            {
                                Id = dr.SafeGetInteger("Id"),
                                FormFieldName = dr.SafeGetString("FormFieldName"),
                                FormFieldValue = GetEmployeeExtendedInfoValue(int.Parse(employeenumber), dr.SafeGetString("FormFieldIdentifier")),
                                FormFieldIdentifier = dr.SafeGetString("FormFieldIdentifier"),
                                FormFieldCode = GetEmployeeExtendedInfoCode(int.Parse(employeenumber), dr.SafeGetString("FormFieldIdentifier")),
                                CustomerId = dr.SafeGetInteger("Customer_Id")
                            };




                    }

                }
            }
        }

        private string GetEmployeeExtendedInfoValue(int employeenumber, string formfieldidentifier)
        {
            string connectionstring_AM = ConfigurationManager.ConnectionStrings["DSN_AM"].ConnectionString;

            using (var connection = new SqlConnection(connectionstring_AM))
            {
                using (var command = connection.CreateCommand())
                {
                    command.Parameters.Add(new SqlParameter("@employeenumber", SqlDbType.Int)).Value = employeenumber;
                    command.Parameters.Add(new SqlParameter("@Destination", SqlDbType.NVarChar)).Value = formfieldidentifier;

                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "ECT_GV_Get_Employee_ExtendedFieldValue";
                    connection.Open();

                    var result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                        return (string)result;

                    return "";
                }
            }
        }


        private string GetEmployeeExtendedInfoCode(int employeenumber, string formfieldidentifier)
        {
            string connectionstring_AM = ConfigurationManager.ConnectionStrings["DSN_AM"].ConnectionString;

            using (var connection = new SqlConnection(connectionstring_AM))
            {
                using (var command = connection.CreateCommand())
                {
                    command.Parameters.Add(new SqlParameter("@employeenumber", SqlDbType.Int)).Value = employeenumber;
                    command.Parameters.Add(new SqlParameter("@Destination", SqlDbType.NVarChar)).Value = formfieldidentifier;

                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "ECT_GV_Get_Employee_ExtendedFieldCode";
                    connection.Open();

                    var result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                        return (string)result;

                    return "";
                }
            }
        }


        public IDictionary<string, string> GetGvDataDictionary(int caseId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    command.Parameters.Add(new SqlParameter("@Case_Id", SqlDbType.Int)).Value = caseId;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "ECT_Get_GWValues";
                    connection.Open();

                    var dictionary = new Dictionary<string, string>();
                    using (var dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            dictionary.Add(dr.SafeGetString("FormFieldName"), dr.SafeGetString("GWValue"));
                        }
                    }

                    return dictionary;
                }
            }
        }

        public IEnumerable<GVMapFields> GetAllGVFieldsName()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "ECT_Get_AllGVFieldsName";
                    connection.Open();

                    using (var dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            yield return new GVMapFields
                            {
                                Form_Id = dr.SafeGetInteger("Form_Id"),
                                GVFieldName = dr.SafeGetString("GVFieldName"),
                                FormField_Id = dr.SafeGetInteger("FormField_Id")
                            };
                        }
                    }
                }
            }
        }


        private DateTime? ConvertToDateTime(string value, string format)
        {
            DateTime dt;
            if (DateTime.TryParseExact(value, format, null, DateTimeStyles.None, out dt))
                return dt;
            return null;
        }



        public void SaveGlobalViewFields(int Case_Id, int FormField_Id, string GWValue)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.Parameters.Add(new SqlParameter("@Case_ID", SqlDbType.Int)).Value = Case_Id;
                    command.Parameters.Add(new SqlParameter("@FormField_Id", SqlDbType.Int)).Value = FormField_Id;
                    command.Parameters.Add(new SqlParameter("@GWValue", SqlDbType.NVarChar)).Value = GWValue;

                    command.CommandText = "ECT_Save_GlobalViewFields";
                    command.CommandType = CommandType.StoredProcedure;
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
