using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq.Expressions;
using DH.Helpdesk.BusinessData.Enums.Reports;
using DH.Helpdesk.Common.Tools;
using DH.Helpdesk.Domain;
using Z.EntityFramework.Plus;

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
    using Infrastructure;
    using DbContext;

    public class ReportServiceRepository : IReportServiceRepository
    {
        private enum QueryType
        {
            SQLQUERY = 1,
            STOREPROCEURE = 2
        }

        private readonly string _ConnectionString;

        public ReportServiceRepository(IDatabaseFactory factory)
        {
            DatabaseFactory = factory;
            this._ConnectionString = ConfigurationManager.ConnectionStrings["HelpdeskSqlServerDbContext"].ConnectionString;
        }

        protected IDatabaseFactory DatabaseFactory { get; }

        protected HelpdeskDbContext _dataContext = null;
        protected HelpdeskDbContext DataContext => _dataContext ?? (_dataContext = DatabaseFactory.Get());

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
                    if (query.Item3 == (int)QueryType.STOREPROCEURE)
                {
                    var sp_params = new List<SqlParameter>();
                    sp_params = filters.GeneralParameter.Select(p => new SqlParameter(p.ParamName, p.ParamValue)).ToList();
                    var curDataTable = GetDataTableBySP(query.Item2, sp_params);
                    if (curDataTable != null)
                        reportDataSets.Add(new ReportDataSet(query.Item1, curDataTable));
                }
            }
            return reportDataSets;
        }

        public IList<HistoricalDataResult> GetHistoricalData(HistoricalDataFilter filter)
        {
            var result = DataContext.Database.SqlQuery<HistoricalDataResult>("ReportGetHistoricalData @caseStatus, @changeFrom, @changeTo, @customerID, @changeWorkingGroups," +
                                                                             " @registerFrom, @registerTo, @closeFrom, @closeTo, @includeCasesWithHistoricalNoWorkingGroup," +
                                                                             " @includeCasesWithNoWorkingGroup, @administrators, @departments," +
                                                                             " @caseTypes, @productAreas, @workingGroups",
                new SqlParameter("@caseStatus", GetNullableValue(filter.CaseStatus)),
                new SqlParameter("@changeFrom", GetNullableValue(filter.ChangeFrom)),
                new SqlParameter("@changeTo", GetNullableValue(filter.ChangeTo)),
                new SqlParameter("@customerID", GetNullableValue(filter.CustomerID)),
                GetIDListParameter("@changeWorkingGroups", filter.ChangeWorkingGroups),
                new SqlParameter("@registerFrom", GetNullableValue(filter.RegisterFrom)),
                new SqlParameter("@registerTo", GetNullableValue(filter.RegisterTo)),
                new SqlParameter("@closeFrom", GetNullableValue(filter.CloseFrom)),
                new SqlParameter("@closeTo", GetNullableValue(filter.CloseTo)),
                new SqlParameter("@includeCasesWithHistoricalNoWorkingGroup", filter.IncludeHistoricalCasesWithNoWorkingGroup),
                new SqlParameter("@includeCasesWithNoWorkingGroup", filter.IncludeCasesWithNoWorkingGroup),
                GetIDListParameter("@administrators", filter.Administrators),
                GetIDListParameter("@departments", filter.Departments),
                GetIDListParameter("@caseTypes", filter.CaseTypes),
                GetIDListParameter("@productAreas", filter.ProductAreas),
                GetIDListParameter("@workingGroups", filter.WorkingGroups)
            ).ToList();

            return result;
        }

        public IList<ReportedTimeDataResult> GetReportedTimeData(ReportedTimeDataFilter filter)
        {
            var query = DataContext.Logs.AsNoTracking()
                .Where(l => l.Case.Customer_Id == filter.CustomerID);

            if (filter.RegisterFrom.HasValue)
                query = query.Where(l => l.Case.RegTime >= filter.RegisterFrom.Value);

            if (filter.RegisterTo.HasValue)
                query = query.Where(l => l.Case.RegTime <= filter.RegisterTo.Value);

            if (filter.Administrators != null && filter.Administrators.Any())
                query = query.Where(l => l.Case.Performer_User_Id.HasValue && filter.Administrators.Contains(l.Case.Performer_User_Id.Value));

            if (filter.CloseFrom.HasValue)
                query = query.Where(l => l.Case.FinishingDate >= filter.CloseFrom.Value);

            if (filter.CloseTo.HasValue)
                query = query.Where(l => l.Case.FinishingDate <= filter.CloseTo.Value);

            if (filter.Departments != null && filter.Departments.Any())
                query = query.Where(c =>
                    c.Case.Department_Id.HasValue && filter.Departments.Contains(c.Case.Department_Id.Value));

            if ((filter.WorkingGroups == null || !filter.WorkingGroups.Any()) && !filter.IncludeCasesWithNoWorkingGroup)
                // prevent fetch data
                return new List<ReportedTimeDataResult>();

            if (filter.IncludeCasesWithNoWorkingGroup)
                query = query.Where(c => !c.Case.WorkingGroup_Id.HasValue ||
                                         (c.Case.WorkingGroup_Id.HasValue && filter.WorkingGroups.Contains(c.Case.WorkingGroup_Id.Value)));
            else
                query = query.Where(c =>
                    c.Case.WorkingGroup_Id.HasValue && filter.WorkingGroups.Contains(c.Case.WorkingGroup_Id.Value));

            if (filter.CaseTypes != null && filter.CaseTypes.Any())
                query = query.Where(l => filter.CaseTypes.Contains(l.Case.CaseType_Id));

            if (filter.ProductAreas != null && filter.ProductAreas.Any())
                query = query.Where(l => l.Case.ProductArea_Id.HasValue && filter.ProductAreas.Contains(l.Case.ProductArea_Id.Value));

            if (filter.LogNoteFrom.HasValue)
                query = query.Where(l => l.LogDate >= filter.LogNoteFrom.Value);

            if (filter.LogNoteTo.HasValue)
                query = query.Where(l => l.LogDate <= filter.LogNoteTo.Value);

            if (filter.CaseStatus.HasValue)
            {
                var status = (CaseProgressFilterEnum)filter.CaseStatus.Value;
                switch (status)
                {
                    case CaseProgressFilterEnum.None:
                        {
                            break;
                        }
                    case CaseProgressFilterEnum.ClosedCases:
                        {
                            query = query.Where(l => l.Case.FinishingDate.HasValue);
                            break;
                        }
                    case CaseProgressFilterEnum.CasesInProgress:
                        {
                            query = query.Where(l => !l.Case.FinishingDate.HasValue);
                            break;
                        }
                    default:
                        {
                            throw new Exception(string.Format("Unknown CaseStatus value: {0}", filter.CaseStatus.Value));
                        }
                }
            }

            IQueryable<ReportedTimeDataResult> result = null;
            switch (filter.GroupBy)
            {
                case ReportedTimeGroup.CaseType_Id:
                    result = query.GroupBy(l => new { l.Case.CaseType_Id, l.Case.CaseType.Name })
                        .Select(lg => new ReportedTimeDataResult
                        { Id = lg.Key.CaseType_Id, Label = lg.Key.Name, TotalTime = lg.Sum(k => k.WorkingTime + k.OverTime) });
                    break;
                case ReportedTimeGroup.CaseNumber:
                    result = query.GroupBy(l => new { l.Case.Id, l.Case.CaseNumber })
                        .Select(lg => new ReportedTimeDataResult
                        { Id = lg.Key.Id, Label = lg.Key.CaseNumber.ToString(), TotalTime = lg.Sum(k => k.WorkingTime + k.OverTime) });
                    break;
                case ReportedTimeGroup.Department_Id:
                    result = query
                        .Where(l => l.Case.Department_Id.HasValue)
                        .GroupBy(l => new { l.Case.Department_Id, l.Case.Department.DepartmentName })
                        .Select(lg => new ReportedTimeDataResult
                        { Id = lg.Key.Department_Id ?? 0, Label = lg.Key.DepartmentName, TotalTime = lg.Sum(k => k.WorkingTime + k.OverTime) });
                    break;
                case ReportedTimeGroup.Priority_Id:
                    result = query
                        .GroupBy(l => new { l.Case.Priority_Id, l.Case.Priority.Name })
                        .Select(lg => new ReportedTimeDataResult
                        { Id = lg.Key.Priority_Id ?? 0, Label = lg.Key.Name, TotalTime = lg.Sum(k => k.WorkingTime + k.OverTime) });
                    break;
                case ReportedTimeGroup.ProductArea_Id:
                    result = query
                        .GroupBy(l => new { l.Case.ProductArea_Id, l.Case.ProductArea.Name })
                        .Select(lg => new ReportedTimeDataResult
                        { Id = lg.Key.ProductArea_Id ?? 0, Label = lg.Key.Name, TotalTime = lg.Sum(k => k.WorkingTime + k.OverTime) });
                    break;
                case ReportedTimeGroup.LogNoteDate:
                    result = query.GroupBy(l => DbFunctions.TruncateTime(l.LogDate))
                        .Select(lg => new ReportedTimeDataResult
                        { Label = lg.Key.ToString(), DateTime = lg.Key, TotalTime = lg.Sum(k => k.WorkingTime + k.OverTime) });
                    break;
                case ReportedTimeGroup.Performer_User_Id:
                    result = query
                        .GroupBy(l => new { l.Case.Performer_User_Id, l.Case.Administrator.FirstName, l.Case.Administrator.SurName })
                        .Select(lg => new ReportedTimeDataResult
                        { Id = lg.Key.Performer_User_Id ?? 0, Label = lg.Key.FirstName + " " + lg.Key.SurName, TotalTime = lg.Sum(k => k.WorkingTime + k.OverTime) });
                    break;
                case ReportedTimeGroup.WorkingGroup_Id:
                    result = query
                        .GroupBy(l => new { l.Case.WorkingGroup_Id, l.Case.Workinggroup.WorkingGroupName })
                        .Select(lg => new ReportedTimeDataResult
                        { Id = lg.Key.WorkingGroup_Id ?? 0, Label = lg.Key.WorkingGroupName, TotalTime = lg.Sum(k => k.WorkingTime + k.OverTime) });
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return result.ToList();
        }

        public IList<NumberOfCaseDataResult> GetNumberOfCasesData(NumberOfCasesDataFilter filter)
        {
            var query = DataContext.Cases.AsNoTracking()
                .Where(c => c.Customer_Id == filter.CustomerID);

            if (filter.RegisterFrom.HasValue)
                query = query.Where(c => c.RegTime >= filter.RegisterFrom.Value);

            if (filter.RegisterTo.HasValue)
                query = query.Where(c => c.RegTime <= filter.RegisterTo.Value);

            if (filter.Administrators != null && filter.Administrators.Any())
                query = query.Where(c => c.Performer_User_Id.HasValue && filter.Administrators.Contains(c.Performer_User_Id.Value));

            if (filter.CloseFrom.HasValue)
                query = query.Where(c => c.FinishingDate >= filter.CloseFrom.Value);

            if (filter.CloseTo.HasValue)
                query = query.Where(c => c.FinishingDate <= filter.CloseTo.Value);

            if (filter.Departments != null && filter.Departments.Any())
                query = query.Where(c =>
                    c.Department_Id.HasValue && filter.Departments.Contains(c.Department_Id.Value));

            if ((filter.WorkingGroups == null || !filter.WorkingGroups.Any()) && !filter.IncludeCasesWithNoWorkingGroup)
                // prevent fetch data
                return new List<NumberOfCaseDataResult>();

            if (filter.IncludeCasesWithNoWorkingGroup)
                query = query.Where(c => !c.WorkingGroup_Id.HasValue ||
                                         (c.WorkingGroup_Id.HasValue && filter.WorkingGroups.Contains(c.WorkingGroup_Id.Value)));
            else
                query = query.Where(c =>
                    c.WorkingGroup_Id.HasValue && filter.WorkingGroups.Contains(c.WorkingGroup_Id.Value));

            if (filter.CaseTypes != null && filter.CaseTypes.Any())
                query = query.Where(c => filter.CaseTypes.Contains(c.CaseType_Id));

            if (filter.ProductAreas != null && filter.ProductAreas.Any())
                query = query.Where(c => c.ProductArea_Id.HasValue && filter.ProductAreas.Contains(c.ProductArea_Id.Value));

            if (filter.CaseStatus.HasValue)
            {
                var status = (CaseProgressFilterEnum)filter.CaseStatus.Value;
                switch (status)
                {
                    case CaseProgressFilterEnum.None:
                        {
                            break;
                        }
                    case CaseProgressFilterEnum.ClosedCases:
                        {
                            query = query.Where(c => c.FinishingDate.HasValue);
                            break;
                        }
                    case CaseProgressFilterEnum.CasesInProgress:
                        {
                            query = query.Where(c => !c.FinishingDate.HasValue);
                            break;
                        }
                    default:
                        {
                            throw new Exception(string.Format("Unknown CaseStatus value: {0}", filter.CaseStatus.Value));
                        }
                }
            }

            IQueryable<NumberOfCaseDataResult> result = null;
            switch (filter.GroupBy)
            {
                case NumberOfCasesGroup.CaseType_Id:
                    result = query.GroupBy(c => new { c.CaseType_Id, c.CaseType.Name })
                        .Select(cg => new NumberOfCaseDataResult
                        { Id = cg.Key.CaseType_Id, Label = cg.Key.Name, CasesAmount = cg.Count() });
                    break;
                case NumberOfCasesGroup.RegistrationYear:
                    result = query.GroupBy(c => c.RegTime.Year)
                        .Select(cg => new NumberOfCaseDataResult
                        { Id = cg.Key, Label = cg.Key.ToString(), CasesAmount = cg.Count() });
                    break;
                case NumberOfCasesGroup.RegistrationWeekday:
                    result = query.GroupBy(c => SqlFunctions.DatePart("weekday", c.RegTime))
                        .Select(cg => new NumberOfCaseDataResult
                        { Id = cg.Key.Value, Label = cg.Key.ToString(), CasesAmount = cg.Count() });
                    break;
                case NumberOfCasesGroup.RegistrationMonth:
                    result = query.GroupBy(c => c.RegTime.Month)
                        .Select(cg => new NumberOfCaseDataResult
                        { Id = cg.Key, Label = cg.Key.ToString(), CasesAmount = cg.Count() });
                    break;
                case NumberOfCasesGroup.RegistrationDate:
                    result = query.GroupBy(c => DbFunctions.TruncateTime(c.RegTime))
                        .Select(cg => new NumberOfCaseDataResult
                        { Label = cg.Key.ToString(), DateTime = cg.Key, CasesAmount = cg.Count() });
                    break;
                case NumberOfCasesGroup.RegistrationHour:
                    result = query.GroupBy(c => c.RegTime.Hour)
                        .Select(cg => new NumberOfCaseDataResult
                        { Id = cg.Key, Label = cg.Key.ToString(), CasesAmount = cg.Count() });
                    break;
                case NumberOfCasesGroup.RegistrationSourceCustomer:
                    result = query.GroupBy(c => new { c.RegistrationSourceCustomer_Id, c.RegistrationSourceCustomer.SourceName })
                        .Select(cg => new NumberOfCaseDataResult
                        { Id = cg.Key.RegistrationSourceCustomer_Id ?? 0, Label = cg.Key.SourceName.ToString(), CasesAmount = cg.Count() });
                    break;
                case NumberOfCasesGroup.WorkingGroup_Id:
                    result = query.GroupBy(c => new { c.WorkingGroup_Id, c.Workinggroup.WorkingGroupName })
                        .Select(cg => new NumberOfCaseDataResult
                        { Id = cg.Key.WorkingGroup_Id ?? 0, Label = cg.Key.WorkingGroupName.ToString(), CasesAmount = cg.Count() });
                    break;
                case NumberOfCasesGroup.StateSecondary_Id:
                    result = query.GroupBy(c => new { c.StateSecondary_Id, c.StateSecondary.Name })
                        .Select(cg => new NumberOfCaseDataResult
                        { Id = cg.Key.StateSecondary_Id ?? 0, Label = cg.Key.Name.ToString(), CasesAmount = cg.Count() });
                    break;
                case NumberOfCasesGroup.Department_Id:
                    result = query.GroupBy(c => new { c.Department_Id, c.Department.DepartmentName })
                        .Select(cg => new NumberOfCaseDataResult
                        { Id = cg.Key.Department_Id ?? 0, Label = cg.Key.DepartmentName.ToString(), CasesAmount = cg.Count() });
                    break;
                case NumberOfCasesGroup.Priority_Id:
                    result = query.GroupBy(c => new { c.Priority_Id, c.Priority.Name })
                        .Select(cg => new NumberOfCaseDataResult
                        { Id = cg.Key.Priority_Id ?? 0, Label = cg.Key.Name.ToString(), CasesAmount = cg.Count() });
                    break;
                case NumberOfCasesGroup.FinishingDate:
                    result = query.GroupBy(c => DbFunctions.TruncateTime(c.FinishingDate))
                        .Select(cg => new NumberOfCaseDataResult
                        { Label = "", DateTime = cg.Key, CasesAmount = cg.Count() });
                    break;
                case NumberOfCasesGroup.ProductArea_Id:
                    result = query.GroupBy(c => new { c.ProductArea_Id, c.ProductArea.Name })
                        .Select(cg => new NumberOfCaseDataResult
                        { Id = cg.Key.ProductArea_Id ?? 0, Label = cg.Key.Name.ToString(), CasesAmount = cg.Count() });
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return result.ToList();
        }

        public IList<SolvedInTimeDataResult> GetSolvedInTimeData(SolvedInTimeDataFilter filter)
        {
            var query = DataContext.Cases.AsNoTracking()
               .Where(c => c.Customer_Id == filter.CustomerID);

            if (filter.RegisterFrom.HasValue)
                query = query.Where(c => c.RegTime >= filter.RegisterFrom.Value);

            if (filter.RegisterTo.HasValue)
                query = query.Where(c => c.RegTime <= filter.RegisterTo.Value);

            if (filter.Administrators != null && filter.Administrators.Any())
                query = query.Where(c => c.Performer_User_Id.HasValue && filter.Administrators.Contains(c.Performer_User_Id.Value));

            if (filter.CloseFrom.HasValue)
                query = query.Where(c => c.FinishingDate >= filter.CloseFrom.Value);

            if (filter.CloseTo.HasValue)
                query = query.Where(c => c.FinishingDate <= filter.CloseTo.Value);

            if (filter.Departments != null && filter.Departments.Any())
                query = query.Where(c =>
                    c.Department_Id.HasValue && filter.Departments.Contains(c.Department_Id.Value));

            if ((filter.WorkingGroups == null || !filter.WorkingGroups.Any()) && !filter.IncludeCasesWithNoWorkingGroup)
                // prevent fetch data
                return new List<SolvedInTimeDataResult>();

            if (filter.IncludeCasesWithNoWorkingGroup)
                query = query.Where(c => !c.WorkingGroup_Id.HasValue ||
                                         (c.WorkingGroup_Id.HasValue && filter.WorkingGroups.Contains(c.WorkingGroup_Id.Value)));
            else
                query = query.Where(c =>
                    c.WorkingGroup_Id.HasValue && filter.WorkingGroups.Contains(c.WorkingGroup_Id.Value));

            if (filter.CaseTypes != null && filter.CaseTypes.Any())
                query = query.Where(c => filter.CaseTypes.Contains(c.CaseType_Id));

            if (filter.ProductAreas != null && filter.ProductAreas.Any())
                query = query.Where(c => c.ProductArea_Id.HasValue && filter.ProductAreas.Contains(c.ProductArea_Id.Value));

            query = query.Where(c => c.FinishingDate.HasValue);

            IQueryable<SolvedInTimeDataResult> result = null;
            switch (filter.GroupBy)
            {
                case SolvedInTimeGroup.CaseType_Id:
                    result = query.GroupBy(c => new { c.CaseType_Id, c.CaseType.Name })
                        .Select(cg => new SolvedInTimeDataResult
                        {
                            Id = cg.Key.CaseType_Id,
                            Label = cg.Key.Name,
                            Total = cg.Count(),
                            SolvedInTimeTotal = cg.Join(DataContext.CaseStatistics, c => c.Id, cs => cs.CaseId, (c, cs) => cs)
                                    .Count(cs => cs.WasSolvedInTime.HasValue || cs.WasSolvedInTime.Value == 1)
                        });
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            return result.ToList();
        }

        private object GetNullableValue<T>(T value)
        {
            if (value == null)
            {
                return DBNull.Value;
            }
            else
            {
                return value;
            }
        }

        private SqlParameter GetIDListParameter(string name, IList<int> idList)
        {
            const string idTableName = "dbo.IDList";

            var dt = new DataTable();
            dt.TableName = idTableName;
            dt.Columns.Add("ID", typeof(int));

            if (idList != null)
            {
                foreach (var id in idList)
                {
                    dt.Rows.Add(id);
                }
            }

            var parameter = new SqlParameter(name, SqlDbType.Structured);
            parameter.TypeName = idTableName;
            parameter.Value = dt;

            return parameter;
        }

        private List<Tuple<string, string, int>> GetQueriesFor(string reportIdentity, ReportSelectedFilter filters)
        {

            /* TODO: It must change some how takes the query dynamicly from file */
            var ret = new List<Tuple<string, string, int>>();
            var _whereClause = GetWhereClauseBy(filters);
            switch (reportIdentity)
            {
                //case "CasesPerSource":                    
                //    ret.Add(                               
                //          new Tuple<string, string, int>(
                //            "CasesPerDate",
                //            "SELECT COUNT(tblCase.Casenumber) AS Volume, tblCustomer.Name, " +
                //                   "tblDate.CalendarYearMonth, tblCustomer.Id, tblRegistrationSourceCustomer.SourceName " +
                //            "FROM tblCustomer INNER JOIN " +
                //                 "tblCase ON tblCustomer.Id = tblCase.Customer_Id LEFT OUTER JOIN " +
                //                 "tblRegistrationSourceCustomer ON tblCase.RegistrationSourceCustomer_Id = tblRegistrationSourceCustomer.Id " +
                //                 "RIGHT OUTER JOIN tblDate ON CAST(tblCase.RegTime AS Date) = tblDate.FullDate " +
                //            _whereClause +  
                //            "GROUP BY tblCustomer.Name, tblDate.CalendarYearMonth, tblCustomer.Id, tblRegistrationSourceCustomer.SourceName " +
                //            "ORDER BY tblDate.CalendarYearMonth",
                //            (int) QueryType.SQLQUERY)                            
                //            );
                //    break;

                //case "CasesPerDate":
                //    ret.Add(                               
                //          new Tuple<string, string, int>(
                //            "CasesPerDate",
                //            "SELECT COUNT(tblCase.Casenumber) AS Volume, cast(convert(date, cast(tblDate.DateKey as nvarchar),11) as nvarchar) as DateKey, tblCustomer.Id " + 
                //            "FROM  tblCustomer INNER JOIN " + 
                //                "tblCase ON tblCustomer.Id = tblCase.Customer_Id RIGHT OUTER JOIN " +
                //                "tblDate ON CAST(tblCase.RegTime AS Date) = tblDate.FullDate "  +
                //            _whereClause +  
                //            "GROUP BY tblDate.DateKey, tblCustomer.Name, tblCustomer.Id " +
                //            "ORDER BY tblDate.DateKey",(int) QueryType.SQLQUERY)                            
                //            );
                //    break;

                //case "CasesPerCasetype":
                //    ret.Add(
                //          new Tuple<string, string, int>(
                //            "CasesPerDate",
                //            "SELECT COUNT(tblCase.Casenumber) AS Volume, tblDate.CalendarYearMonth, tblCaseType.CaseType " +
                //            "FROM tblCase INNER JOIN " +
                //                 "tblCaseType ON tblCase.CaseType_Id = tblCaseType.Id RIGHT OUTER JOIN "+
                //                 "tblDate ON CAST(tblCase.RegTime AS Date) = tblDate.FullDate " + 
                //            _whereClause +
                //            "GROUP BY tblDate.CalendarYearMonth, tblCaseType.CaseType " +
                //            "ORDER BY tblDate.CalendarYearMonth", (int)QueryType.SQLQUERY)
                //            );
                //    break;

                //case "CasesPerWorkingGroup":
                //    ret.Add(
                //          new Tuple<string, string, int>(
                //            "CasesPerWorkGroup",
                //            "SELECT COUNT(tblCase.Casenumber) AS Volume, tblCustomer.Name, tblDate.DateKey, tblCustomer.Id, " +
                //                   "tblCase.WorkingGroup_Id, tblWorkingGroup.WorkingGroup, tblDate.CalendarYearMonth " +
                //            "FROM tblCustomer INNER JOIN " + 
                //                 "tblCase ON tblCustomer.Id = tblCase.Customer_Id INNER JOIN " +
                //                 "tblWorkingGroup ON tblCustomer.Id = tblWorkingGroup.Customer_Id AND "+
                //                 "tblCase.WorkingGroup_Id = tblWorkingGroup.Id RIGHT OUTER JOIN " +
                //                 "tblDate ON CAST(tblCase.RegTime AS Date) = tblDate.FullDate " +
                //            _whereClause +
                //            "GROUP BY tblDate.DateKey, tblCustomer.Name, tblCustomer.Id, tblCase.WorkingGroup_Id, " + 
                //                     "tblWorkingGroup.WorkingGroup, tblDate.CalendarYearMonth " +
                //            "ORDER BY tblDate.DateKey", (int)QueryType.SQLQUERY)
                //            );                                        
                //    break;

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

                //case "CasesPerDepartment":
                //    ret.Add(
                //          new Tuple<string, string, int>(
                //            "CasesPerDepartment",
                //            "SELECT  tblDepartment.Department AS Name, " +
                //                "COUNT(CASE WHEN tblCase.FinishingDate IS NULL THEN tblCase.Id ELSE NULL END) AS CasesInProgress, " +
                //                "COUNT(CASE WHEN tblCase.FinishingDate IS NULL AND tblCase.StateSecondary_Id IS NOT NULL AND tblStateSecondary.IncludeInCaseStatistics = 0 " +
                //                    "THEN tblCase.Id ELSE NULL END) AS CasesInRest, " +
                //                "COUNT(CASE WHEN tblCase.FinishingDate IS NULL THEN NULL ELSE tblCase.Id END) AS CasesClosed " +
                //            "FROM tblCase " +
                //                "INNER JOIN tblDepartment ON tblCase.Department_Id = tblDepartment.Id " +
                //                "LEFT OUTER JOIN tblStateSecondary ON tblCase.StateSecondary_Id = tblStateSecondary.Id " +
                //            _whereClause +
                //            " AND (tblCase.Deleted = 0) " +
                //            "GROUP BY tblDepartment.Department", (int)QueryType.SQLQUERY)
                //            );
                //    break;

                case "ReportedTime":
                    ret.Add(new Tuple<string, string, int>("VariabelAxel",
                        "SELECT (dbo.tblLog.WorkingTime + dbo.tblLog.OverTime)/60 AS Quantity, " +
                            "dbo.tblcase.casenumber, CONVERT(VARCHAR, dbo.tblcase.casenumber) + ' ' + dbo.tblcase.caption AS[Case], " +
                            "CONVERT(nvarchar(10), dbo.tblLog.LogDate, 121) AS [LogDate], " +
                            "CONVERT(NVARCHAR(10), dbo.tblcase.regtime, 121) AS [Registration Date], " +
                            "dbo.tblusers.firstname + N' ' + dbo.tblusers.surname AS[Registrated By], " +
                            "CONVERT(NVARCHAR(10), dbo.tblcase.finishingdate, 121) AS[Closing Date], " +
                            "CASE Isnull(tblcase.finishingdate, 0) WHEN 0 THEN 'On Going'  ELSE 'Closed' END AS[Case Status], " +
                            "dbo.tbldepartment.department, " +
                            "[dbo].[Gethierarchy](tblcase.casetype_id, 'tblCaseType') AS CaseType, " +
                            "[dbo].[Gethierarchy](tblcase.productarea_id, 'tblProductArea') AS ProductArea, " +
                            "dbo.tblworkinggroup.workinggroup AS[Working group], " +
                            "dbo.tblpriority.priorityname AS [Priority], " +
                            "dbo.tblstatus.statusname AS [Status], " +
                            "Isnull(tblUsers2.firstname, '') + ' ' + Isnull(tblUsers2.surname, '') AS Administrator, " +
                            "[dbo].[Gethierarchy]((SELECT TOP(1) dbo.tblfinishingcause.id FROM dbo.tbllog " +
                            "INNER JOIN dbo.tblfinishingcause ON dbo.tbllog.finishingtype = dbo.tblfinishingcause.id WHERE(dbo.tblcase.id = dbo.tbllog.case_id) " +
                            "ORDER  BY dbo.tbllog.id DESC), 'tblFinishingCause') AS ClosingReason " +
                            "FROM dbo.tblcase INNER JOIN dbo.tblcustomer ON dbo.tblcase.customer_id = dbo.tblcustomer.id " +
                            "INNER JOIN dbo.tblLog ON dbo.tblLog.Case_Id = dbo.tblcase.id " +
                            "LEFT OUTER JOIN dbo.tblusers ON dbo.tblcase.user_id = dbo.tblusers.id " +
                            "LEFT OUTER JOIN dbo.tblstatus ON dbo.tblcase.status_id = dbo.tblstatus.id " +
                            "LEFT OUTER JOIN dbo.tblworkinggroup ON dbo.tblcase.workinggroup_id = dbo.tblworkinggroup.id " +
                            "LEFT OUTER JOIN dbo.tblpriority ON dbo.tblcase.priority_id = dbo.tblpriority.id " +
                            "LEFT OUTER JOIN dbo.tbldepartment ON dbo.tblcase.department_id = dbo.tbldepartment.id " +
                            "LEFT OUTER JOIN dbo.tblcasetype ON dbo.tblcase.casetype_id = dbo.tblcasetype.id " +
                            "LEFT OUTER JOIN dbo.tblusers AS tblUsers2 ON dbo.tblcase.performer_user_id = tblUsers2.id " +
                            _whereClause + "ORDER BY dbo.tblcase.casenumber ", (int)QueryType.SQLQUERY));
                    break;
                case "NumberOfCases":
                    ret.Add(new Tuple<string, string, int>("VariabelAxel",
                            "SELECT 1 AS Quantity, dbo.tblCustomer.Name AS Customer, dbo.tblCase.Casenumber, CONVERT(varchar, dbo.tblCase.Casenumber) + ' ' + dbo.tblCase.Caption AS[Case], " +
                                                    "CONVERT(nvarchar(10), dbo.tblCase.RegTime, 121) AS[Registration Date], " +
                                                    "DATEPART(yy, dbo.tblCase.RegTime) AS[Registration Year], " +
                                                    "DATEPART(mm, dbo.tblCase.RegTime) AS[Registration Month], " +
                                                    "DATEPART(dd, dbo.tblCase.RegTime) AS[Registration Day], " +
                                                    "'FY ' + CASE WHEN DATEPART(mm, dbo.tblCase.RegTime) > 8 THEN SUBSTRING(CONVERT(nvarchar(4), DATEADD(yy, 1, dbo.tblCase.RegTime), 121), 3, 2) " +
                                                        "ELSE SUBSTRING(CONVERT(nvarchar(4), dbo.tblCase.RegTime, 121), 3, 2) END AS[Financial Year], " +
                                                        "dbo.tblUsers.FirstName + N' ' + dbo.tblUsers.SurName AS[Registrated By], " +
                                      "dbo.tblCase.Caption AS Subject, CONVERT(nvarchar(10), " +
                                                        "dbo.tblCase.FinishingDate, 121) AS[Closing Date], " +
                                      "dbo.tblCase.FinishingDate,  " +
                                  "CASE DATEPART(dw, dbo.tblCase.FinishingDate) WHEN 1 THEN 8 ELSE DATEPART(dw, dbo.tblCase.FinishingDate) END AS[ClosingDayNoOfWeek], " +
                                                    "CASE DATEPART(dw, dbo.tblCase.FinishingDate) " +
                                                    "WHEN 2 THEN 'Måndag' " +
                                                    "WHEN 3 THEN 'Tisdag' " +
                                                    "WHEN 4 THEN 'Onsdag' " +
                                                    "WHEN 5 THEN 'Torsdag' " +
                                                    "WHEN 6 THEN 'Fredag' " +
                                                    "WHEN 7 THEN 'Lördag' " +
                                                    "WHEN 1 THEN 'Söndag' " +
                                                    "ELSE 'Saknas' END AS[Closing Weekday], " +
                                                    "CASE isnull(tblcase.FinishingDate, 0) " +
                                                        "WHEN 0 THEN 'On Going' ELSE 'Closed' END AS[Case Status], " +
                                                        "dbo.tblRegion.Region, dbo.tblDepartment.Department, " +
                                                        "ISNULL(dbo.tblOU.OU, '') AS[Org unit], " +
                                                    "[dbo].[GetHierarchy] " +
                                                    " (tblCase.CaseType_id, 'tblCaseType') AS CaseType, " +
                                                    "[dbo].[GetHierarchy] " +
                                                    " (tblCase.ProductArea_id, 'tblProductArea') AS ProductArea, " +
                                                    " ISNULL(dbo.tblRegistrationSourceCustomer.SourceName, '') AS Source, " +
                                                    "dbo.tblWorkingGroup.WorkingGroup AS[Working group], " +
                                                    "dbo.tblPriority.PriorityName AS Priority, " +
                                                    "dbo.tblStatus.StatusName AS Status, " +
                                                    "dbo.tblStateSecondary.StateSecondary AS[SubStatus], " +
                                                    "CASE DATEPART(dw, dbo.tblCase.RegTime) WHEN 1 THEN 8 ELSE DATEPART(dw, dbo.tblCase.RegTime)  END AS RegistrationDayNoOfWeek, " +
                                                    "CASE DATEPART(dw, dbo.tblCase.RegTime) " +
                                                        "WHEN 2 THEN 'Måndag' " +
                                                        "WHEN 3 THEN 'Tisdag' " +
                                                        "WHEN 4 THEN 'Onsdag' " +
                                                        "WHEN 5 THEN 'Torsdag' " +
                                                        "WHEN 6 THEN 'Fredag' " +
                                                        "WHEN 7 THEN 'Lördag' " +
                                                        "WHEN 1 THEN 'Söndag' " +
                                                        "ELSE 'Saknas' END AS[Registration Weekday], " +
                                                    "DATEPART(hh, dbo.tblCase.RegTime) AS[Registration Hour], " +
                                                    "CASE DatePart(m, tblCase.regtime) " +
                                                    "WHEN 1 THEN 'Januari ' WHEN 2 THEN 'Februari ' WHEN 3 THEN 'Mars ' WHEN 4 THEN 'April ' WHEN 5 THEN 'Maj ' WHEN 6 THEN 'Juni ' WHEN 7 THEN 'Juli ' WHEN " +
                                                        "8 THEN 'Augusti ' WHEN 9 THEN 'September ' WHEN 10 THEN 'Oktober ' WHEN 11 THEN 'November ' ELSE 'December ' END + CONVERT(varchar(4), " +
                                                    " DATEPART(yyyy, dbo.tblCase.RegTime)) AS Period, " +
                                                    "CONVERT(varchar(4), DATEPART(yyyy, dbo.tblCase.RegTime)) + '-' + CASE WHEN CONVERT(varchar(2), " +
                                                    "DATEPART(mm, dbo.tblCase.RegTime)) < 10 THEN '0' + CONVERT(varchar(2), DATEPART(mm, dbo.tblCase.RegTime)) ELSE CONVERT(varchar(2), DATEPART(mm, " +
                                                        "dbo.tblCase.RegTime)) END AS YearMonth, " +
                                                        "(SELECT CASE WHEN MAX(id) IS NOT NULL THEN 'Ja' ELSE 'Nej' END AS Expr1 " +
                                                            "FROM dbo.tblCaseHistory " +
                                                        " WHERE(Case_Id = dbo.tblCase.Id) AND(StateSecondary_Id IS NOT NULL)) AS OnHold, " +
                                                        "CASE WHEN tblCase.Watchdate IS NULL THEN CASE WHEN tblCase.LeadTime / 60 > tblPriority.SolutionTime AND " +
                                                        "tblPriority.SolutionTime > 0 THEN 'Nej' ELSE 'Ja' END ELSE CASE WHEN tblCase.LeadTime > 0 THEN 'Nej' ELSE 'Ja' END END AS InTime, " +
                                                       "CASE WHEN tblCase.Watchdate IS NULL THEN CASE WHEN tblCase.LeadTime / 60 > tblPriority.SolutionTime AND " +
                                                        " tblPriority.SolutionTime > 0 THEN 'Nej' ELSE 'Ja' END ELSE CASE WHEN CONVERT(varchar(10), tblCase.WatchDate, 121) < CONVERT(varchar(10), " +
                                                        " tblCase.FinishingDate, 121) THEN 'Nej' ELSE 'Ja' END END AS InTime2, CASE WHEN tblCase.Watchdate IS NULL " +
                                                        "THEN CASE WHEN tblCase.LeadTime > (tblPriority.SolutionTime * 60) AND " +
                                                        "tblPriority.SolutionTime > 0 THEN 'Nej' ELSE 'Ja' END ELSE CASE WHEN CONVERT(varchar(10), tblCase.WatchDate, 121) < CONVERT(varchar(10), " +
                                               "tblCase.FinishingDate, 121) THEN 'Nej' ELSE 'Ja' END END AS InTime3, ISNULL(tblUsers2.FirstName, '') +' ' + ISNULL(tblUsers2.SurName, '') AS Administrator, " +
                                                " [dbo].[GetHierarchy](  " +
                            "(SELECT TOP(1) dbo.tblFinishingCause.Id " +
                                                    "FROM dbo.tblLog INNER JOIN " +
                                                        "dbo.tblFinishingCause ON dbo.tblLog.FinishingType = dbo.tblFinishingCause.Id " +
                                                    " WHERE (dbo.tblCase.Id = dbo.tblLog.Case_Id) " +
                                                   "ORDER BY dbo.tblLog.Id DESC), 'tblFinishingCause') AS ClosingReason, " +
                                                   "dbo.tblCase.WatchDate, " +
                              "dbo.tblCase.LeadTime, " +
                              "CASE WHEN tblCaseStatistics.WasSolvedInTime IS NOT NULL " +
                                                    "THEN CASE WHEN tblCaseStatistics.WasSolvedInTime = 1 THEN 'Ja' ELSE 'Nej' END END AS SolvedInTime " +
                                                "FROM dbo.tblCase INNER JOIN " +
                                                    "dbo.tblCustomer ON dbo.tblCase.Customer_Id = dbo.tblCustomer.Id LEFT OUTER JOIN " +
                                                    "dbo.tblCaseStatistics ON dbo.tblCase.Id = dbo.tblCaseStatistics.Case_Id LEFT OUTER JOIN " +
                                                    "dbo.tblUsers ON dbo.tblCase.User_Id = dbo.tblUsers.Id LEFT OUTER JOIN " +
                                                    "dbo.tblStateSecondary ON dbo.tblCase.StateSecondary_Id = dbo.tblStateSecondary.Id LEFT OUTER JOIN " +
                                                    "dbo.tblStatus ON dbo.tblCase.Status_Id = dbo.tblStatus.Id LEFT OUTER JOIN " +
                                                    "dbo.tblWorkingGroup ON dbo.tblCase.WorkingGroup_Id = dbo.tblWorkingGroup.Id LEFT OUTER JOIN " +
                                                    "dbo.tblPriority ON dbo.tblCase.Priority_Id = dbo.tblPriority.Id LEFT OUTER JOIN " +
                                                    "dbo.tblRegion ON dbo.tblCase.Region_Id = dbo.tblRegion.Id LEFT OUTER JOIN " +
                                                    "dbo.tblDepartment ON dbo.tblCase.Department_Id = dbo.tblDepartment.Id LEFT OUTER JOIN " +
                                                    "dbo.tblCaseType ON dbo.tblCase.CaseType_Id = dbo.tblCaseType.Id LEFT OUTER JOIN " +
                                                    "dbo.tblUsers AS tblUsers2 ON dbo.tblCase.Performer_User_Id = tblUsers2.Id LEFT OUTER JOIN " +
                                                    "dbo.tblRegistrationSourceCustomer ON dbo.tblCase.RegistrationSourceCustomer_Id = dbo.tblRegistrationSourceCustomer.Id LEFT OUTER JOIN " +
                                                    "dbo.tblOU ON dbo.tblCase.OU_Id = dbo.tblOU.Id " +
                            _whereClause + "ORDER BY dbo.tblcase.casenumber ", (int)QueryType.SQLQUERY));
                    break;

                case "AvgResolutionTime":
                    ret.Add(
                          new Tuple<string, string, int>(
                            "ResolutionTime",
                            "SELECT Count(casetype)    AS Volume, " +
                                   "casetype, " +
                                   "Sum(leadtime) / 60 AS LeadTime " +
                            "FROM(SELECT TOP(100) PERCENT dbo.tblcustomer.NAME " +
                                                                  "AS " +
                                                                  "Customer, " +
                                                            "dbo.tblcase.casenumber, " +
                                                            "CONVERT(VARCHAR, dbo.tblcase.casenumber) " +
                                                            "+ ' ' + dbo.tblcase.caption " +
                                                                  "AS[Case], " +
                                                            "CONVERT(NVARCHAR(10), dbo.tblcase.regtime, 121) " +
                                                             "     AS[Registration Date], " +
                                                            "Datepart(yy, dbo.tblcase.regtime) " +
                                                            "      AS[Registration Year], " +
                                                            "Datepart(mm, dbo.tblcase.regtime) " +
                                                            "      AS[Registration Month], " +
                                                            "Datepart(dd, dbo.tblcase.regtime) " +
                                                            "      AS[Registration Day], " +
                                                            "'FY ' + CASE WHEN Datepart(mm, " +
                                                            "dbo.tblcase.regtime) > " +
                                                            "      8 THEN Substring(CONVERT(" +
                                                            "NVARCHAR(4), Dateadd(yy, 1, " +
                                                            "dbo.tblcase.regtime), 121) " +
                                                            "      , 3, 2) ELSE Substring(" +
                                                            "CONVERT(NVARCHAR(4), dbo.tblcase.regtime, 121), " +
                                                            "3, 2) " +
                                                            "      END                 AS[Financial Year], " +
                                                            "dbo.tblusers.firstname + N' ' " +
                                                            "+ dbo.tblusers.surname " +
                                                            "      AS[Registrated By], " +
                                                            "dbo.tblcase.caption " +
                                                            "      AS Subject, " +
                                                            "CONVERT(NVARCHAR(10), dbo.tblcase.finishingdate, " +
                                                            "121) " +
                                                            "      AS[Closing Date], " +
                                                            "CASE Isnull(tblcase.finishingdate, 0) " +
                                                            "  WHEN 0 THEN 'On Going' " +
                                                            "  ELSE 'Closed' " +
                                                            "END " +
                                                            "      AS[Case Status], " +
                                                            "dbo.tblregion.region, " +
                                                            "dbo.tbldepartment.department, " +
                                                            "Isnull(dbo.tblou.ou, '') " +
                                                            "      AS[Org unit], " +
                                                            "[dbo].[Gethierarchy] " +
                                                            "(tblcase.casetype_id, 'tblCaseType') " +
                                                            "      AS CaseType, " +
                                                            "[dbo].[Gethierarchy] " +
                                                            "(tblcase.productarea_id, 'tblProductArea') " +
                                                            "      AS ProductArea, " +
                                                            "Isnull(" +
                                  "dbo.tblregistrationsourcecustomer.sourcename, " +
                                                            "'')                  AS Source, " +
                                                            "dbo.tblworkinggroup.workinggroup " +
                                                            "      AS[Working group], " +
                                                            "dbo.tblpriority.priorityname " +
                                                            "      AS Priority, " +
                                                            "dbo.tblstatus.statusname " +
                                                            "      AS Status, " +
                                                            "dbo.tblstatesecondary.statesecondary " +
                                                            "      AS[Sub State], " +
                                                            "CASE Datepart(dw, dbo.tblcase.regtime) " +
                                                            "  WHEN 2 THEN 'Måndag' " +
                                                            "  WHEN 3 THEN 'Tisdag' " +
                                                            "  WHEN 4 THEN 'Onsdag' " +
                                                            "  WHEN 5 THEN 'Torsdag' " +
                                                            "  WHEN 6 THEN 'Fredag' " +
                                                            "  WHEN 7 THEN 'Lördag' " +
                                                            "  ELSE 'Söndag' " +
                                                            "END " +
                                                            "      AS[Registration Weekday], " +
                                                            "Datepart(hh, dbo.tblcase.regtime) " +
                                                            "      AS[Registration Hour], " +
                                                            "CASE Datepart(m, tblcase.regtime) WHEN 1 THEN " +
                                                            "      'Januari ' WHEN 2 THEN 'Februari ' " +
                                                            "WHEN 3 THEN 'Mars ' WHEN 4 THEN 'April ' WHEN 5 " +
                                                            "THEN " +
                                                            "      'Maj ' WHEN 6 THEN 'Juni ' " +
                                                            "WHEN 7 THEN 'Juli ' WHEN 8 THEN 'Augusti ' WHEN " +
                                                            "9 THEN " +
                                                            "      'September ' WHEN 10 THEN " +
                                                            "'Oktober ' WHEN 11 THEN 'November ' ELSE " +
                                                            "'December ' " +
                                                            "      END " +
                                                            "+ CONVERT(VARCHAR(4), Datepart(yyyy, " +
                                                            "      dbo.tblcase.regtime))                AS " +
                                                            "Period, " +
                                                            "CONVERT(VARCHAR(4), Datepart(yyyy, " +
                                                            "      dbo.tblcase.regtime)) " +
                                                            "+ '-' + CASE WHEN CONVERT(VARCHAR(2), Datepart(" +
                                                            "mm, " +
                                                            "      dbo.tblcase.regtime)) < 10 " +
                                                            "THEN '0' + CONVERT(VARCHAR(2), Datepart(mm, " +
                                                            "      dbo.tblcase.regtime)) ELSE CONVERT(" +
                                                            "VARCHAR(2), Datepart(mm, dbo.tblcase.regtime)) " +
                                                            "END " +
                                                            "      AS YearMonth, " +
                                                            "(SELECT CASE " +
                                                            "          WHEN Max(id) IS NOT NULL THEN 'Ja' " +
                                                            "          ELSE 'Nej' " +
                                                            "        END AS Expr1 " +
                                                            " FROM   dbo.tblcasehistory " +
                                                            " WHERE(case_id = dbo.tblcase.id)" +
                                                            "        AND(statesecondary_id IS NOT NULL)) " +
                                                            "      AS OnHold, " +
                                                            "CASE " +
                                                            "  WHEN tblcase.watchdate IS NULL THEN " +
                                                            "    CASE " +
                                                            "      WHEN tblcase.leadtime / 60 > " +
                                                            "           tblpriority.solutiontime " +
                                                            "           AND tblpriority.solutiontime > 0 THEN " +
                                                            "      'Nej' " +
                                                            "      ELSE 'Ja' " +
                                                            "    END " +
                                                            "  ELSE " +
                                                            "    CASE " +
                                                            "      WHEN tblcase.leadtime > 0 THEN 'Nej' " +
                                                            "      ELSE 'Ja' " +
                                                            "    END " +
                                                            "END " +
                                                            "      AS InTime, " +
                                                            "CASE " +
                                                            "  WHEN tblcase.watchdate IS NULL THEN " +
                                                            "    CASE " +
                                                            "      WHEN tblcase.leadtime / 60 > " +
                                                            "           tblpriority.solutiontime " +
                                                            "           AND tblpriority.solutiontime > 0 THEN " +
                                                            "      'Nej' " +
                                                            "      ELSE 'Ja' " +
                                                            "    END " +
                                                            "  ELSE " +
                                                            "    CASE " +
                                                            "      WHEN CONVERT(VARCHAR(10), " +
                                                            "           tblcase.watchdate, 121 " +
                                                            "           ) < " +
                                                            "           CONVERT(VARCHAR(10), " +
                                                            "           tblcase.finishingdate, " +
                                                            "           121) THEN 'Nej' " +
                                                            "      ELSE 'Ja' " +
                                                            "    END " +
                                                            "END " +
                                                            "      AS InTime2, " +
                                                            "CASE " +
                                                            "  WHEN tblcase.watchdate IS NULL THEN " +
                                                            "    CASE " +
                                                            "      WHEN tblcase.leadtime > (" +
                                                            "           tblpriority.solutiontime * 60) " +
                                                            "           AND tblpriority.solutiontime > 0 THEN " +
                                                            "      'Nej' " +
                                                            "      ELSE 'Ja' " +
                                                            "    END " +
                                                            "  ELSE " +
                                                            "    CASE " +
                                                            "      WHEN CONVERT(VARCHAR(10), " +
                                                            "           tblcase.watchdate, 121 " +
                                                            "           ) < " +
                                                            "           CONVERT(VARCHAR(10), " +
                                                            "           tblcase.finishingdate, " +
                                                            "           121) THEN 'Nej' " +
                                                            "      ELSE 'Ja' " +
                                                            "    END " +
                                                            "END " +
                                                            "      AS InTime3, " +
                                                            "Isnull(tblUsers2.firstname, '') + ' ' " +
                                                            "+ Isnull(tblUsers2.surname, '') " +
                                                            "      AS Administrator, " +
                                                            "[dbo].[Gethierarchy](" +
                                                            "(SELECT TOP(1) dbo.tblfinishingcause.id " +
                                                            " FROM   dbo.tbllog " +
                                                            "        INNER JOIN dbo.tblfinishingcause " +
                                                            "                ON dbo.tbllog.finishingtype = " +
                                                            "                   dbo.tblfinishingcause.id " +
                                                            " WHERE(dbo.tblcase.id = dbo.tbllog.case_id) " +
                                                            " ORDER  BY dbo.tbllog.id DESC), " +
                                                            "'tblFinishingCause') " +
                                                            "      AS " +
                                                            "ClosingReason, " +
                                                            "dbo.tblcase.watchdate, " +
                                                            "dbo.tblcase.leadtime, " +
                                                            "CASE " +
                                                            "  WHEN tblcasestatistics.wassolvedintime IS NOT " +
                                                            "       NULL " +
                                                            "      THEN " +
                                                            "    CASE " +
                                                            "      WHEN tblcasestatistics.wassolvedintime = 1 " +
                                                            "    THEN " +
                                                            "      'Ja' " +
                                                            "      ELSE 'Nej' " +
                                                            "    END " +
                                                            "END " +
                                                            "      AS SolvedInTime " +
                                   "FROM   dbo.tblcase " +
                                          "INNER JOIN dbo.tblcustomer " +
                                          "       ON dbo.tblcase.customer_id = dbo.tblcustomer.id " +
                                          "LEFT OUTER JOIN dbo.tblcasestatistics " +
                                          "             ON dbo.tblcase.id = dbo.tblcasestatistics.case_id " +
                                          "LEFT OUTER JOIN dbo.tblusers " +
                                          "             ON dbo.tblcase.user_id = dbo.tblusers.id " +
                                          "LEFT OUTER JOIN dbo.tblstatesecondary " +
                                          "             ON dbo.tblcase.statesecondary_id = " +
                                          "                dbo.tblstatesecondary.id " +
                                          "LEFT OUTER JOIN dbo.tblstatus " +
                                          "             ON dbo.tblcase.status_id = dbo.tblstatus.id " +
                                          "LEFT OUTER JOIN dbo.tblworkinggroup " +
                                          "             ON dbo.tblcase.workinggroup_id = " +
                                          "                dbo.tblworkinggroup.id " +
                                          "LEFT OUTER JOIN dbo.tblpriority " +
                                          "             ON dbo.tblcase.priority_id = dbo.tblpriority.id " +
                                          "LEFT OUTER JOIN dbo.tblregion " +
                                          "             ON dbo.tblcase.region_id = dbo.tblregion.id " +
                                          "LEFT OUTER JOIN dbo.tbldepartment " +
                                          "             ON dbo.tblcase.department_id = dbo.tbldepartment.id " +
                                          "LEFT OUTER JOIN dbo.tblcasetype " +
                                          "             ON dbo.tblcase.casetype_id = dbo.tblcasetype.id " +
                                          "LEFT OUTER JOIN dbo.tblusers AS tblUsers2 " +
                                          "             ON dbo.tblcase.performer_user_id = tblUsers2.id " +
                                          "LEFT OUTER JOIN dbo.tblregistrationsourcecustomer " +
                                          "             ON dbo.tblcase.registrationsourcecustomer_id = " +
                                          "                dbo.tblregistrationsourcecustomer.id " +
                                          "LEFT OUTER JOIN dbo.tblou " +
                                          "             ON dbo.tblcase.ou_id = dbo.tblou.id " +
                                          _whereClause +
                                          ") AS TimeData " +
                            "Where [case status] = 'Closed' " +
                            "GROUP BY casetype "
                            , (int)QueryType.SQLQUERY)
                            );
                    break;


                case "CaseDetailsList":
                    ret.Add(
                            new Tuple<string, string, int>(
                              "PrintCase",
                              "sp_GetCaseInfo",
                              (int)QueryType.STOREPROCEURE
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