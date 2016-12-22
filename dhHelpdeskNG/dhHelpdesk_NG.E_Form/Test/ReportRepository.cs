namespace ECTReport
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;

    using ECT.Model;
    using ECT.Model.Entities.Reports;
using System;

    public class ReportRepository
    {
        private readonly string _connectionString;

        public ReportRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<string> GetFormColumnNames(int customerId)
        {
            var sql = "select tblFormField.FormFieldName as column_name from tblFormField inner join tblForm on tblFormField.Form_Id = tblForm.Id "
                       + "where tblForm.Customer_Id = @customerId and tblForm.ExternalPage = 1 group by tblFormField.FormFieldName order by min(tblFormField.Id)";

            var columnNames = new List<string>();
            using(var connection = new SqlConnection(this._connectionString))
            {
                connection.Open();

                var getFormColumnNames = new SqlCommand
                {
                    Connection = connection,
                    CommandText = sql,
                };

                getFormColumnNames.Parameters.AddWithValue("@customerId", customerId);

                using(var dr = getFormColumnNames.ExecuteReader())
                {
                    while(dr.Read())
                    {
                        columnNames.Add(dr.SafeGetString("column_name"));
                    }

                    dr.Close();
                }
            }

            return columnNames;
        }

        public List<string> GetColumnNames()
        {
            var columnNames = new List<string>();
            using (var connection = new SqlConnection(this._connectionString))
            {
                connection.Open();

                var getColumnNames = new SqlCommand
                                         {
                                             Connection = connection,
                                             CommandText =
                                                 "select column_name from information_schema.columns "
                                                 + "where table_name = 'vw_Case_Formfield' "
                                                 + "order by ordinal_position"
                                         };
                using (var dr = getColumnNames.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        columnNames.Add(dr.SafeGetString("column_name"));
                    }

                    dr.Close();
                }
            }

            return columnNames;
        }

        public IEnumerable<HelpDeskFieldData> GetCaseHelpDeskData(int customerId) 
        {
            return GetCaseHelpDeskData(customerId, null);
        }

        public IEnumerable<HelpDeskFieldData> GetCaseHelpDeskData(int customerId, DateTime? customDate)
        {
            using (var connection = new SqlConnection(this._connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "ECT_Get_CaseData_v2";
                    command.Parameters.Add(new SqlParameter { ParameterName = "CustomerId", SqlDbType = SqlDbType.Int, Value = customerId });
                    command.Parameters.Add(new SqlParameter { ParameterName = "CustomDate", SqlDbType = SqlDbType.DateTime, Value = customDate });
                    connection.Open();

                    using (var dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            yield return new HelpDeskFieldData
                            {
                                CaseNumber = dr.SafeGetString("Casenumber"),
                                EmployeeNumber = dr.SafeGetString("EmployeeNumber"),
                                FirstName = dr.SafeGetString("FirstName"),
                                LastName = dr.SafeGetString("LastName"),
                                DateOfBirth = dr.SafeGetString("DateOfBirth"),
                                SubProcess = dr.SafeGetString("SubProcess"),
                                MainProcess = dr.SafeGetString("MainProcess"),                              
                                Company = dr.SafeGetString("Company"),
                                Unit = dr.SafeGetString("Unit"),
                                Department = dr.SafeGetString("Department"),
                                Function = dr.SafeGetString("Function"),
                                Status = dr.SafeGetString("Status"),
                                NumberOFRecords = dr.SafeGetString("NumberOFRecords"),
                                WorkingGroup = dr.SafeGetString("WorkingGroup"),
                                SLA = dr.SafeGetString("SLA"),
                                AdminName = dr.SafeGetString("AdminName"),
                                AdminLastName = dr.SafeGetString("AdminLastName"),
                                RegisteredByUser = 
                                    dr.SafeGetString("Initiator") != string.Empty ? 
                                        dr.SafeGetString("Initiator") + "\\" + dr.SafeGetString("RegUserId") : 
                                        dr.SafeGetString("RegUserId"),
                                Eyequality = dr.SafeGetString("StatusName"),
                                RegisterTime = dr.SafeGetDateTime("RegistrationTime"),
                                FinishingDate = dr.SafeGetDateTime("FinishingDate"),
                                EffectiveDate = dr.SafeGetDateTime("EffectiveDate")                                
                            };
                        }
                    }
                }
            }
        }

        public IEnumerable<FormFieldData> GetCaseFormData(int customerId)
        {
            return GetCaseFormData(customerId, null);
        }

        public IEnumerable<FormFieldData> GetCaseFormData(int customerId, DateTime? customDate)
        {
            using (var connection = new SqlConnection(this._connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "ECT_Get_CaseData_v2";
                    command.Parameters.Add(new SqlParameter { ParameterName = "CustomerId", SqlDbType = SqlDbType.Int, Value = customerId });
                    command.Parameters.Add(new SqlParameter { ParameterName = "CustomDate", SqlDbType = SqlDbType.DateTime, Value = customDate });
                    connection.Open();

                    using (var dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            yield return new FormFieldData
                            {
                                CaseId = dr.SafeGetInteger("Case_Id"),
                                RegTime = dr.SafeGetDateTime("RegistrationTime"),
                                CaseNumber = dr.SafeGetString("Casenumber"),
                                EmployeeNumber = dr.SafeGetString("EmployeeNumber"),
                                FormFieldName = dr.SafeGetString("FormFieldName"),
                                FormFieldValue = dr.SafeGetString("FormFieldValue")
                            };
                        }
                    }
                }
            }
        }
    }
}
