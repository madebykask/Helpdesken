using DH.Helpdesk.Common.Tools;

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
    using DH.Helpdesk.BusinessData.Enums.Case;
    
    public class ReportServiceRepository : IReportServiceRepository
    {
        private enum QueryType
        {
            SQLQUERY = 1,
            STOREPROCEURE = 2
        }

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
                if (query.Item3 == (int)QueryType.SQLQUERY)
                {
                    var curDataTable = GetDataTableByQuery(query.Item2);
                                    
                    reportDataSets.Add(new ReportDataSet(query.Item1, curDataTable));
                }
                else
                    if (query.Item3 == (int)QueryType.STOREPROCEURE )
                    {
                        var sp_params = new List<SqlParameter>();
                        sp_params = filters.GeneralParameter.Select(p=> new SqlParameter(p.ParamName, p.ParamValue)).ToList();                        
                        var curDataTable = GetDataTableBySP(query.Item2, sp_params);
                        if (curDataTable != null)
                            reportDataSets.Add(new ReportDataSet(query.Item1, curDataTable));
                    }
            }
            return reportDataSets;
        }

        private List<Tuple<string, string, int>> GetQueriesFor(string reportIdentity, ReportSelectedFilter filters)
        {

            /* TODO: It must change some how takes the query dynamicly from file */ 
            var ret = new List<Tuple<string, string, int>>();
            var _whereClause = GetWhereClauseBy(filters);
            switch (reportIdentity)
            {
                case "CasesPerSource":                    
                    ret.Add(                               
                          new Tuple<string, string, int>(
                            "CasesPerDate",
                            "SELECT COUNT(tblCase.Casenumber) AS Volume, tblCustomer.Name, " +
                                   "tblDate.CalendarYearMonth, tblCustomer.Id, tblRegistrationSourceCustomer.SourceName " +
                            "FROM tblCustomer INNER JOIN " +
                                 "tblCase ON tblCustomer.Id = tblCase.Customer_Id LEFT OUTER JOIN " +
                                 "tblRegistrationSourceCustomer ON tblCase.RegistrationSourceCustomer_Id = tblRegistrationSourceCustomer.Id " +
                                 "RIGHT OUTER JOIN tblDate ON CAST(tblCase.RegTime AS Date) = tblDate.FullDate " +
                            _whereClause +  
                            "GROUP BY tblCustomer.Name, tblDate.CalendarYearMonth, tblCustomer.Id, tblRegistrationSourceCustomer.SourceName " +
                            "ORDER BY tblDate.CalendarYearMonth",
                            (int) QueryType.SQLQUERY)                            
                            );
                    break;

                case "CasesPerDate":
                    ret.Add(                               
                          new Tuple<string, string, int>(
                            "CasesPerDate",
                            "SELECT COUNT(tblCase.Casenumber) AS Volume, cast(convert(date, cast(tblDate.DateKey as nvarchar),11) as nvarchar) as DateKey, tblCustomer.Id " + 
                            "FROM  tblCustomer INNER JOIN " + 
                                "tblCase ON tblCustomer.Id = tblCase.Customer_Id RIGHT OUTER JOIN " +
                                "tblDate ON CAST(tblCase.RegTime AS Date) = tblDate.FullDate "  +
                            _whereClause +  
                            "GROUP BY tblDate.DateKey, tblCustomer.Name, tblCustomer.Id " +
                            "ORDER BY tblDate.DateKey",(int) QueryType.SQLQUERY)                            
                            );
                    break;

                case "CasesPerCasetype":
                    ret.Add(
                          new Tuple<string, string, int>(
                            "CasesPerDate",
                            "SELECT COUNT(tblCase.Casenumber) AS Volume, tblDate.CalendarYearMonth, tblCaseType.CaseType " +
                            "FROM tblCase INNER JOIN " +
                                 "tblCaseType ON tblCase.CaseType_Id = tblCaseType.Id RIGHT OUTER JOIN "+
                                 "tblDate ON CAST(tblCase.RegTime AS Date) = tblDate.FullDate " + 
                            _whereClause +
                            "GROUP BY tblDate.CalendarYearMonth, tblCaseType.CaseType " +
                            "ORDER BY tblDate.CalendarYearMonth", (int)QueryType.SQLQUERY)
                            );
                    break;

                case "CasesPerWorkingGroup":
                    ret.Add(
                          new Tuple<string, string, int>(
                            "CasesPerWorkGroup",
                            "SELECT COUNT(tblCase.Casenumber) AS Volume, tblCustomer.Name, tblDate.DateKey, tblCustomer.Id, " +
                                   "tblCase.WorkingGroup_Id, tblWorkingGroup.WorkingGroup, tblDate.CalendarYearMonth " +
                            "FROM tblCustomer INNER JOIN " + 
                                 "tblCase ON tblCustomer.Id = tblCase.Customer_Id INNER JOIN " +
                                 "tblWorkingGroup ON tblCustomer.Id = tblWorkingGroup.Customer_Id AND "+
                                 "tblCase.WorkingGroup_Id = tblWorkingGroup.Id RIGHT OUTER JOIN " +
                                 "tblDate ON CAST(tblCase.RegTime AS Date) = tblDate.FullDate " +
                            _whereClause +
                            "GROUP BY tblDate.DateKey, tblCustomer.Name, tblCustomer.Id, tblCase.WorkingGroup_Id, " + 
                                     "tblWorkingGroup.WorkingGroup, tblDate.CalendarYearMonth " +
                            "ORDER BY tblDate.DateKey", (int)QueryType.SQLQUERY)
                            );                                        
                    break;

                case "CasesPerAdministrator":
                    ret.Add(
                          new Tuple<string, string, int>(
                            "CasesPerAdministrator",
                            "SELECT  tblUsers.FirstName + N' ' + tblUsers.SurName AS Name, " +
                                "COUNT(CASE WHEN tblCase.FinishingDate IS NULL THEN tblCase.Id ELSE NULL END) AS CasesInProgress, " +
                                "COUNT(CASE WHEN tblCase.FinishingDate IS NULL AND tblCase.StateSecondary_Id IS NOT NULL AND tblStateSecondary.IncludeInCaseStatistics = 0 THEN tblCase.Id ELSE NULL END) AS CasesInRest, " +
                                "COUNT(CASE WHEN tblCase.FinishingDate IS NULL THEN NULL ELSE tblCase.Id END) AS CasesClosed " +
                            "FROM tblCase " +
                                "INNER JOIN tblUsers ON tblCase.Performer_User_Id = tblUsers.Id " +
                                "LEFT OUTER JOIN tblStateSecondary ON tblCase.StateSecondary_Id = tblStateSecondary.Id " +
                            _whereClause +
                            " AND (tblCase.Deleted = 0 and tblUsers.UserGroup_Id <> '1') " +
                            "GROUP BY tblUsers.FirstName + N' ' + tblUsers.SurName", (int)QueryType.SQLQUERY)
                            );
                    break;

                case "CasesPerDepartment":
                    ret.Add(
                          new Tuple<string, string, int>(
                            "CasesPerDepartment",
                            "SELECT  tblDepartment.Department AS Name, " +
                                "COUNT(CASE WHEN tblCase.FinishingDate IS NULL THEN tblCase.Id ELSE NULL END) AS CasesInProgress, " +
                                "COUNT(CASE WHEN tblCase.FinishingDate IS NULL AND tblCase.StateSecondary_Id IS NOT NULL AND tblStateSecondary.IncludeInCaseStatistics = 0 " +
                                    "THEN tblCase.Id ELSE NULL END) AS CasesInRest, " +
                                "COUNT(CASE WHEN tblCase.FinishingDate IS NULL THEN NULL ELSE tblCase.Id END) AS CasesClosed " +
                            "FROM tblCase " +
                                "INNER JOIN tblDepartment ON tblCase.Department_Id = tblDepartment.Id " +
                                "LEFT OUTER JOIN tblStateSecondary ON tblCase.StateSecondary_Id = tblStateSecondary.Id " +
                            _whereClause +
                            " AND (tblCase.Deleted = 0) " +
                            "GROUP BY tblDepartment.Department", (int)QueryType.SQLQUERY)
                            );
                    break;

                case "CaseDetailsList":
                    ret.Add(
                            new Tuple<string, string, int>(
                              "PrintCase",
                              "sp_GetCaseInfo",
                              (int) QueryType.STOREPROCEURE                             
                           ));
                    break;
 
                default:
                    return ret;
            }

            return ret;
        }

        private DataTable GetDataTableByQuery(string sqlQuery)
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

        private DataTable GetDataTableBySP(string storedProcedureName,
                                                IEnumerable<SqlParameter> parameters)
        {        
            var ds = new DataSet();

            using (var conn = new SqlConnection(_ConnectionString))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = storedProcedureName;
                    cmd.CommandType = CommandType.StoredProcedure;
                    foreach (var parameter in parameters)
                    {
                        cmd.Parameters.Add(parameter);
                    }

                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(ds);
                    }
                }
            }
            if (ds.Tables.Count > 0)
                return ds.Tables[0];
            else
                return null;
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
                {
                    // Dep + OU
                    if (filters.SeletcedOUs.Any()) 
                        _whereStr += string.Format("AND (tblCase.Department_Id in ({0}) or tblCase.OU_Id in ({1})) ", 
                                                   filters.SeletcedDepartments.GetSelectedStr().SafeForSqlInject(),
                                                   filters.SeletcedOUs.GetSelectedStr().SafeForSqlInject());                        
                    else
                        _whereStr += string.Format("AND tblCase.Department_Id in ({0}) ", filters.SeletcedDepartments.GetSelectedStr().SafeForSqlInject());
                }
                else
                {
                    // OU only
                    if (filters.SeletcedOUs.Any())
                        _whereStr += string.Format("AND tblCase.OU_Id in ({0}) ", filters.SeletcedOUs.GetSelectedStr().SafeForSqlInject());                        
                }


                if (filters.SelectedWorkingGroups.Any())
                {
                    if (filters.SelectedWorkingGroups.Contains(0))
                    {
                        if (filters.SelectedWorkingGroups.Count == 1)
                            _whereStr += string.Format("AND tblCase.WorkingGroup_Id is null ", filters.SelectedWorkingGroups.GetSelectedStr().SafeForSqlInject());
                        else
                            _whereStr += string.Format("AND (tblCase.WorkingGroup_Id in ({0}) or tblCase.WorkingGroup_Id is null) ", filters.SelectedWorkingGroups.GetSelectedStr().SafeForSqlInject());
                    }
                    else
                        _whereStr += string.Format("AND tblCase.WorkingGroup_Id in ({0}) ", filters.SelectedWorkingGroups.GetSelectedStr().SafeForSqlInject());
                }
                else
                {
                    /* false condition to prevent fetch data*/
                    _whereStr += string.Format("AND tblCase.WorkingGroup_Id in (0) ");
                }

                if (filters.SelectedCaseTypes.Any())                
                    _whereStr += string.Format("AND tblCase.CaseType_Id in ({0}) ", filters.SelectedCaseTypes.GetSelectedStr().SafeForSqlInject());

                if (filters.SelectedProductAreas.Any())
                    _whereStr += string.Format("AND tblCase.ProductArea_Id in ({0}) ", filters.SelectedProductAreas.GetSelectedStr().SafeForSqlInject());

                if (filters.SelectedCaseStatus.Any())
                {
                    var progress = filters.SelectedCaseStatus.GetSelectedStr().SafeForSqlInject();
                    switch (progress)
                    {
                        case CaseProgressFilter.None:
                            break;
                        case CaseProgressFilter.ClosedCases:                            
                            _whereStr += "AND tblCase.FinishingDate is not null ";
                            break;
                        case CaseProgressFilter.CasesInProgress:
                            _whereStr += "AND tblCase.FinishingDate is null ";                            
                            break;                        
                        default:                            
                            break;
                    }                    
                }
            }

            if (filters.CaseCreationDate.FromDate.HasValue) 
                _whereStr += string.Format("AND tblCase.RegTime >= '{0}' ", filters.CaseCreationDate.FromDate.Value);

            if (filters.CaseCreationDate.ToDate.HasValue)
                _whereStr += string.Format("AND tblCase.RegTime <= '{0}' ", filters.CaseCreationDate.ToDate.Value);

            if (filters.CaseClosingDate.FromDate.HasValue)
                _whereStr += string.Format("AND tblCase.FinishingDate >= '{0}' ", filters.CaseClosingDate.FromDate.Value);

            if (filters.CaseClosingDate.ToDate.HasValue)
                _whereStr += string.Format("AND tblCase.FinishingDate <= '{0}' ", filters.CaseClosingDate.ToDate.Value.GetEndOfDay());

            return _whereStr;
        }
    }
}