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
    using DH.Helpdesk.BusinessData.OldComponents;
    using DH.Helpdesk.BusinessData.OldComponents.DH.Helpdesk.BusinessData.Utils;
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
            reportData.AddDataSets(GetDataSetsFor(reportIdentity, filters));
            return reportData;
        }

        private IList<ReportDataSet> GetDataSetsFor(string reportIdentity, ReportSelectedFilter filters)
        {
            var reportDataSets = new List<ReportDataSet>();
            var queries = GetQueriesFor(reportIdentity, filters);
            foreach (var query in queries)
            {
                var curDataTable = GetDataTable(query.Value);
                reportDataSets.Add(new ReportDataSet(query.Key, curDataTable));
            }
            return reportDataSets;
        }

        private List<KeyValuePair<string, string>> GetQueriesFor(string reportIdentity, ReportSelectedFilter filters)
        {
            var ret = new List<KeyValuePair<string, string>>();
            var _whereClause = GetWhereClauseBy(filters);
            switch (reportIdentity)
            {
                case "CasesPerSource":                    
                    ret.Add(                               
                          new KeyValuePair<string, string>(
                            "CasesPerDate",
                            "SELECT COUNT(tblCase.Casenumber) AS Volume, tblCustomer.Name, " +
                                   "tblDate.CalendarYearMonth, tblCustomer.Id, tblRegistrationSourceCustomer.SourceName " +
                            "FROM tblCustomer INNER JOIN " +
                                 "tblCase ON tblCustomer.Id = tblCase.Customer_Id LEFT OUTER JOIN " +
                                 "tblRegistrationSourceCustomer ON tblCase.RegistrationSourceCustomer_Id = tblRegistrationSourceCustomer.Id " +
                                 "RIGHT OUTER JOIN tblDate ON CAST(tblCase.RegTime AS Date) = tblDate.FullDate " +
                            _whereClause +  
                            "GROUP BY tblCustomer.Name, tblDate.CalendarYearMonth, tblCustomer.Id, tblRegistrationSourceCustomer.SourceName " +
                            "ORDER BY tblDate.CalendarYearMonth")
                            );
                    break;

                case "CasesPerDate":
                    ret.Add(                               
                          new KeyValuePair<string, string>(
                            "CasesPerDate",
                            "SELECT COUNT(tblCase.Casenumber) AS Volume, tblCustomer.Name,format(tblDate.DateKey,'####-##-##') as DateKey, tblCustomer.Id " + 
                            "FROM  tblCustomer INNER JOIN " + 
                                "tblCase ON tblCustomer.Id = tblCase.Customer_Id RIGHT OUTER JOIN " +
                                "tblDate ON CAST(tblCase.RegTime AS Date) = tblDate.FullDate "  +
                            _whereClause +  
                            "GROUP BY tblDate.DateKey, tblCustomer.Name, tblCustomer.Id " +
                            "ORDER BY tblDate.DateKey")
                            );
                    break; 

                default:
                    return ret;
            }

            return ret;
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

        private string GetWhereClauseBy(ReportSelectedFilter filters)
        {
            var _whereStr = " Where 1=1 ";

            if (filters.SelectedCustomers.Any())
                _whereStr += string.Format("AND tblCase.Customer_Id in ({0}) ", filters.SelectedCustomers.GetSelectedStr().SafeForSqlInject());

            if (filters.SelectedAdministrator.Any())
                _whereStr += string.Format("AND tblCase.Performer_User_Id in ({0}) ", filters.SelectedAdministrator.GetSelectedStr().SafeForSqlInject());

            if (filters.SelectedCustomers.Count == 1)
            {
                if (filters.SeletcedDepartments.Any())
                    _whereStr += string.Format("AND tblCase.Department_Id in ({0}) ", filters.SeletcedDepartments.GetSelectedStr().SafeForSqlInject());

                if (filters.SelectedWorkingGroups.Any())                
                    _whereStr += string.Format("AND tblCase.WorkingGroup_Id in ({0}) ", filters.SelectedWorkingGroups.GetSelectedStr().SafeForSqlInject());

                if (filters.SelectedCaseTypes.Any())                
                    _whereStr += string.Format("AND tblCase.CaseType_Id in ({0}) ", filters.SelectedCaseTypes.GetSelectedStr().SafeForSqlInject());
            }

            if (filters.CaseCreationDate.FromDate.HasValue) 
                _whereStr += string.Format("AND tblCase.RegTime >= '{0}' ", filters.CaseCreationDate.FromDate.Value);

            if (filters.CaseCreationDate.FromDate.HasValue)
                _whereStr += string.Format("AND tblCase.RegTime <= '{0}' ", filters.CaseCreationDate.ToDate.Value);

            return _whereStr;
        }
    }
}