namespace DH.Helpdesk.Dal.Repositories.ReportService.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.OleDb;
    using System.Globalization;
    using System.Linq;
    using System.Text;    
    using DH.Helpdesk.Dal.Repositories.ReportService;
    using DH.Helpdesk.BusinessData.Models.ReportService;
    using System.Data.SqlClient;
    
    public class ReportServiceRepository : IReportServiceRepository
    {
        private readonly string _ConnectionString;

        public ReportServiceRepository()
        {
            this._ConnectionString = ConfigurationManager.ConnectionStrings["HelpdeskSqlServerDbContext"].ConnectionString;
        }

        public ReportData GetReportData(string reportIdentity, ReportSelectedFilter filters)
        {
            var reportData = new ReportData(reportIdentity);
            reportData.AddDataSets(GetDataSetsFor(reportIdentity));
            return reportData;
        }

        private IList<ReportDataSet> GetDataSetsFor(string reportIdentity)
        {
            var reportDataSets = new List<ReportDataSet>();
            var queries = GetQueriesFor(reportIdentity);
            foreach (var query in queries)
            {
                var curDataTable = GetDataTable(query.Value);
                reportDataSets.Add(new ReportDataSet(query.Key, curDataTable));
            }
            return reportDataSets;
        }

        private List<KeyValuePair<string, string>> GetQueriesFor(string reportIdentity)
        {
            switch (reportIdentity)
            {
                case "CasesPerSource":
                    var ret = new List<KeyValuePair<string, string>>()
                        {
                          new KeyValuePair<string, string>(
                            "CasesPerDate",
                            "SELECT COUNT(tblCase.Casenumber) AS Volume, tblCustomer.Name, " +
                                   "tblDate.CalendarYearMonth, tblCustomer.Id, tblRegistrationSourceCustomer.SourceName " +
                            "FROM tblCustomer INNER JOIN " +
                                 "tblCase ON tblCustomer.Id = tblCase.Customer_Id LEFT OUTER JOIN " +
                                 "tblRegistrationSourceCustomer ON tblCase.RegistrationSourceCustomer_Id = tblRegistrationSourceCustomer.Id " +
                                 "RIGHT OUTER JOIN tblDate ON CAST(tblCase.RegTime AS Date) = tblDate.FullDate " +
                            "GROUP BY tblCustomer.Name, tblDate.CalendarYearMonth, tblCustomer.Id, tblRegistrationSourceCustomer.SourceName " +
                            "ORDER BY tblDate.CalendarYearMonth")
                        };
                    return ret;

                default:
                    return new List<KeyValuePair<string, string>>();
            }
        }

        private DataTable GetDataTable(string sqlQuery)
        {
            DataTable ret = new DataTable(); 
            using (SqlConnection connection = new SqlConnection(_ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand { Connection = connection, CommandType = CommandType.Text })
                {
                    cmd.CommandText = sqlQuery;
                    connection.Open();
                    using (SqlDataReader dataReader = cmd.ExecuteReader())
                    {                        
                        ret.Load(dataReader);
                        connection.Close();                        
                    }
                }
            }
            return ret; 
        }

    }
}