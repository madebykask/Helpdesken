using System.Diagnostics;
using DH.Helpdesk.BusinessData.Models.Shared.Output;
using DH.Helpdesk.Dal.DbQueryExecutor;

namespace DH.Helpdesk.Dal.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.OleDb;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    using DH.Helpdesk.BusinessData.Enums.Case;
    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.BusinessData.Models.Case.CaseSearch;
    using DH.Helpdesk.BusinessData.Models.Case.Output;
    using DH.Helpdesk.BusinessData.Models.Grid;
    using DH.Helpdesk.BusinessData.Models.ProductArea;
    using DH.Helpdesk.BusinessData.Models.WorktimeCalculator;
    using DH.Helpdesk.BusinessData.OldComponents;
    using DH.Helpdesk.BusinessData.OldComponents.DH.Helpdesk.BusinessData.Utils;
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.Common.Enums.Cases;
    using DH.Helpdesk.Common.Tools;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Dal.Repositories;

    using ProductAreaEntity = DH.Helpdesk.Domain.ProductArea;
    using DH.Helpdesk.Dal.Enums;

    /// <summary>
    /// The CaseSearchRepository interface.
    /// </summary>
    public interface ICaseSearchRepository
    {
        SearchResult<CaseSearchResult> Search(CaseSearchContext context, int workingHours, out CaseRemainingTimeData remainingTime, out CaseAggregateData aggregateData);
    }

    public partial class CaseSearchRepository : ICaseSearchRepository
    {
        private const string TimeLeftColumn = "_temporary_LeadTime";
        private const string TimeLeftColumnLower = "_temporary_leadtime";

        private readonly ICustomerUserRepository _customerUserRepository;

        private readonly IProductAreaRepository _productAreaRepository;

        private readonly ICaseTypeRepository _caseTypeRepository;

        private readonly ILogRepository _logRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IDbQueryExecutorFactory _queryExecutorFactory;

        public CaseSearchRepository(
                ICustomerUserRepository customerUserRepository,
                IProductAreaRepository productAreaRepository,
                ICaseTypeRepository caseTypeRepository,
                ILogRepository logRepository,
                IDepartmentRepository departmentRepository,
                IDbQueryExecutorFactory queryExecutorFactory)
        {
            _customerUserRepository = customerUserRepository;
            _productAreaRepository = productAreaRepository;
            _caseTypeRepository = caseTypeRepository;
            _logRepository = logRepository;
            _departmentRepository = departmentRepository;
            _queryExecutorFactory = queryExecutorFactory;
        }

        public SearchResult<CaseSearchResult> Search(CaseSearchContext context, 
                                                     int workingHours,
                                                     out CaseRemainingTimeData remainingTime, 
                                                     out CaseAggregateData aggregateData)
        {
            var now = DateTime.UtcNow;
            var dsn = ConfigurationManager.ConnectionStrings["HelpdeskOleDbContext"].ConnectionString;

            // shortcuts for context fields
            var f = context.f;
            var userCaseSettings = context.userCaseSettings;
            var workTimeCalcFactory = context.workTimeCalcFactory;
            var calculateRemainingTime = context.calculateRemainingTime;
            var customerSetting = context.customerSetting;
            var productAreaNamesResolver = context.productAreaNamesResolver;
            var s = context.s;

            //check if freetext is searchable
            var freetext = context.f.FreeTextSearch;
            if (freetext != null)
            {
                if (freetext.Contains("#") && freetext.Length == 1)
                {
                    context.f.FreeTextSearch = String.Empty;
                }
            }
            
            var searchResult = new SearchResult<CaseSearchResult>();
            remainingTime = new CaseRemainingTimeData();
            aggregateData = new CaseAggregateData();

            
            IList<ProductAreaEntity> pal = this._productAreaRepository.GetMany(x => x.Customer_Id == f.CustomerId).OrderBy(x => x.Name).ToList();
            var caseTypes = this._caseTypeRepository.GetCaseTypeOverviews(f.CustomerId).ToArray();
            var displayLeftTime = userCaseSettings.Any(it => it.Name == TimeLeftColumn);
            
            var queryBuilderContext = BuildSearchQueryContext(context);
            
            //build search query
            var searchQueryBuilder = new CaseSearchQueryBuilder();
            var sql = searchQueryBuilder.BuildCaseSearchSql(queryBuilderContext);

            if (string.IsNullOrEmpty(sql))
            {
                return searchResult;
            }

            #region Run Search 

            var queryExecutor = _queryExecutorFactory.Create();
            var dataTable = queryExecutor.ExecuteTable(sql, commandType: CommandType.Text, timeout: 300);

            #endregion

            #region Calculate work time 

            var workTimeCalculator = this.InitCalcFromSQL(dataTable, workTimeCalcFactory, now);

            #endregion

            #region Prepare Search Result

            var secSortOrder = string.Empty;

            using (var dr = dataTable.CreateDataReader())
            {
                if (dr != null && dr.HasRows)
                {
                    int? closingReasonFilter = null;
                    if (!string.IsNullOrEmpty(f.CaseClosingReasonFilter))
                    {
                        int closingReason;
                        if (int.TryParse(f.CaseClosingReasonFilter, out closingReason))
                        {
                            closingReasonFilter = closingReason;
                        }
                    }

                    while (dr.Read())
                    {
                        var caseId = int.Parse(dr["Id"].ToString());

                        if (closingReasonFilter.HasValue)
                        {
                            var log = this._logRepository.GetLastLog(caseId);
                            if (log?.FinishingType == null || closingReasonFilter != log.FinishingType)
                            {
                                continue;
                            }
                        }

                        var doCalcTimeLeft = displayLeftTime || calculateRemainingTime;

                        var row = new CaseSearchResult();
                        IList<Field> cols = new List<Field>();
                        var toolTip = string.Empty;
                        var sortOrder = string.Empty;
                        DateTime caseRegistrationDate;

                        DateTime.TryParse(dr["RegTime"].ToString(), out caseRegistrationDate);
                        DateTime dtTmp;
                        DateTime? caseFinishingDate = null;
                        if (DateTime.TryParse(dr["FinishingDate"].ToString(), out dtTmp))
                        {
                            caseFinishingDate = dtTmp;
                            doCalcTimeLeft = false;
                        }

                        int intTmp;
                        if (int.TryParse(dr["IncludeInCaseStatistics"].ToString(), out intTmp))
                        {
                            doCalcTimeLeft = doCalcTimeLeft && intTmp == 1;
                        }

                        DateTime? watchDate = null;
                        if (DateTime.TryParse(dr["WatchDate"].ToString(), out dtTmp))
                        {
                            watchDate = dtTmp;
                        }

                        int SLAtime;
                        int.TryParse(dr["SolutionTime"].ToString(), out SLAtime);
                        int timeOnPause;
                        int.TryParse(dr["ExternalTime"].ToString(), out timeOnPause);
                        int? departmentId = null;
                        if (int.TryParse(dr["Department_Id"].ToString(), out intTmp))
                        {
                            departmentId = intTmp;
                        }

                        int? timeLeft = null;
                        if (doCalcTimeLeft)
                        {
                            if (watchDate.HasValue)
                            {
                                var watchDateDue = watchDate.Value.MakeTomorrow();
                                int workTime = 0;
                                //// calc time by watching date
                                if (watchDateDue > now)
                                {
                                    // #52951 timeOnPause shouldn't calculate when watchdate has value
                                    workTime = workTimeCalculator.CalculateWorkTime(
                                        now,
                                        watchDateDue,
                                        departmentId);
                                }
                                else
                                {
                                    //// for cases that should be closed in the past
                                    // #52951 timeOnPause shouldn't calculate when watchdate has value
                                    workTime = -workTimeCalculator.CalculateWorkTime(
                                        watchDateDue,
                                        now,
                                        departmentId);
                                }

                                timeLeft = workTime/60;
                                var floatingPoint = workTime%60;
                                secSortOrder = floatingPoint.ToString();

                                if (timeLeft <= 0 && floatingPoint < 0)
                                    timeLeft--;
                            }
                            else if (SLAtime > 0)
                            {
                                //// calc by SLA value
                                var dtFrom = DatesHelper.Min(caseRegistrationDate, now);
                                var dtTo = DatesHelper.Max(caseRegistrationDate, now);
                                var calcTime = workTimeCalculator.CalculateWorkTime(dtFrom, dtTo, departmentId);
                                timeLeft = (SLAtime*60 - calcTime + timeOnPause)/60;
                                var floatingPoint = (SLAtime*60 - calcTime + timeOnPause)%60;
                                secSortOrder = floatingPoint.ToString();

                                if (timeLeft <= 0 && floatingPoint < 0)
                                    timeLeft--;
                            }
                        }

                        if (!CheckRemainingTimeFilter(timeLeft, workingHours, f))
                            continue;

                        if (timeLeft.HasValue)
                        {
                            remainingTime.AddRemainingTime(caseId, timeLeft.Value);
                        }

                        var isExternalEnv = (context.applicationType.ToLower() == ApplicationTypes.LineManager ||
                                                context.applicationType.ToLower() == ApplicationTypes.SelfService);
                        row.Ignored = false;

                        foreach (var c in userCaseSettings)
                        {
                            Field field = null;
                            for (var i = 0; i < dr.FieldCount; i++)
                            {
                                if (string.Compare(dr.GetName(i), GetCaseFieldName(c.Name), true, CultureInfo.InvariantCulture) == 0)
                                {
                                    if (!dr.IsDBNull(i))
                                    {
                                        FieldTypes fieldType;
                                        DateTime? dateValue;
                                        bool translateField;
                                        bool treeTranslation;
                                        int baseId = 0;
                                        bool preventToAddRecord = false;

                                        var value = GetDatareaderValue(
                                            dr,
                                            i,
                                            c.Name,
                                            customerSetting,
                                            pal,
                                            timeLeft,
                                            caseTypes,
                                            productAreaNamesResolver,
                                            isExternalEnv,
                                            out translateField,
                                            out treeTranslation,
                                            out dateValue,
                                            out fieldType,
                                            out baseId,
                                            out preventToAddRecord);

                                        // according to some rules it can/canot be shown
                                        if (!row.Ignored)
                                            row.Ignored = preventToAddRecord;

                                        field = new Field
                                        {
                                            Key = c.Name,
                                            Id = baseId,
                                            StringValue = value,
                                            TranslateThis = translateField,
                                            TreeTranslation = treeTranslation,
                                            DateTimeValue = dateValue,
                                            FieldType = fieldType
                                        };

                                        if (string.Compare(
                                            s.SortBy,
                                            c.Name,
                                            true,
                                            CultureInfo.InvariantCulture) == 0)
                                        {
                                            sortOrder = value;
                                        }
                                    }

                                    break;
                                }
                            }

                            if (field == null)
                            {
                                field = new Field {StringValue = string.Empty};
                            }

                            cols.Add(field);
                        }
                        row.SortOrder = sortOrder;
                        row.SecSortOrder = secSortOrder;
                        row.Tooltip = toolTip;
                        row.CaseIcon = GetCaseIcon(dr);
                        row.Id = dr.SafeGetInteger("Id");
                        row.Columns = cols;
                        row.IsUnread = dr.SafeGetInteger("Status") == 1;
                        row.IsUrgent = timeLeft.HasValue && timeLeft < 0;
                        row.IsClosed = caseFinishingDate.HasValue;

                        if (!row.Ignored)
                        {
                                searchResult.Items.Add(row);

                            int? curStatus = dr.SafeGetNullableInteger("aggregate_Status");
                            if (curStatus.HasValue)
                            {
                                if (aggregateData.Status.Keys.Contains(curStatus.Value))
                                    aggregateData.Status[curStatus.Value] += 1;
                                else
                                    aggregateData.Status.Add(curStatus.Value, 1);
                            }

                            int? curSubStatus = dr.SafeGetNullableInteger("aggregate_SubStatus");
                            if (curSubStatus.HasValue)
                            {
                                if (aggregateData.SubStatus.Keys.Contains(curSubStatus.Value))
                                    aggregateData.SubStatus[curSubStatus.Value] += 1;
                                else
                                    aggregateData.SubStatus.Add(curSubStatus.Value, 1);
                            }
                        }
                    }
                }

                dr.Close();
            }
                    
                    
                
            

            #endregion

            var result = searchResult;

            result = SortSearchResult(result, s);

            //TODO: refactor when true server paging will be implemented
            result.Count = result.Items.Count;
            if (f.PageInfo != null && f.PageInfo.PageSize > 0)
            {
                result.Items = result.Items.Skip(f.PageInfo.PageNumber * f.PageInfo.PageSize).Take(f.PageInfo.PageSize).ToList();
            }

            return result;
        }

        private SearchQueryBuildContext BuildSearchQueryContext(CaseSearchContext context)
        {
            var f = context.f;
            var userId = f.UserId;

            var customerUserSettings = this._customerUserRepository.GetCustomerSettings(f.CustomerId, userId);
            var userDepartments = _departmentRepository.GetDepartmentsByUserPermissions(userId, f.CustomerId).ToList();
            
            var validateUserCaseSettings = new List<CaseSettings>();
            foreach (var us in context.userCaseSettings)
            {
                if (!validateUserCaseSettings.Select(v => v.Name).Contains(us.Name))
                    validateUserCaseSettings.Add(us);
            }

            var caseSettings = validateUserCaseSettings.ToDictionary(it => it.Name, it => it);

            var caseTypes = new List<int>();
            if (context.f.CaseType != 0)
            {
                caseTypes.Add(context.f.CaseType);
                var caseTypeChildren = this._caseTypeRepository.GetChildren(context.f.CaseType);
                if (caseTypeChildren != null)
                {
                    caseTypes.AddRange(caseTypeChildren);
                }
            }

            var searchQueryBuildContext = 
                new SearchQueryBuildContext(context, customerUserSettings, caseSettings, userDepartments, caseTypes);

            return searchQueryBuildContext;
        }

        private bool CheckRemainingTimeFilter(int? timeLeft, int workingHours, CaseSearchFilter f)
        {
            if (!f.CaseRemainingTimeFilter.HasValue)
                return true;

            if (!timeLeft.HasValue)
                return false;

            if (f.CaseRemainingTimeFilter < 0)
            {
                //filteredCaseRemainigTimes = remainingTime.CaseRemainingTimes.Where(t => t.RemainingTime < 0);
                return timeLeft < 0;
            }
            else if (f.CaseRemainingTimeHoursFilter)
            {
                if (f.CaseRemainingTimeUntilFilter.HasValue)
                {
                    //filteredCaseRemainigTimes = remainingTime.CaseRemainingTimes.Where(t => t.RemainingTime >= f.CaseRemainingTimeFilter.Value && t.RemainingTime < f.CaseRemainingTimeUntilFilter);
                    return timeLeft >= f.CaseRemainingTimeFilter.Value && timeLeft < f.CaseRemainingTimeUntilFilter;
                }
                else
                {
                    //filteredCaseRemainigTimes = remainingTime.CaseRemainingTimes.Where(t => t.RemainingTime == f.CaseRemainingTimeFilter.Value - 1);
                    return timeLeft == f.CaseRemainingTimeFilter.Value - 1;
                }
            }
            else if (f.CaseRemainingTimeFilter == int.MaxValue && f.CaseRemainingTimeMaxFilter.HasValue)
            {
                //filteredCaseRemainigTimes = remainingTime.CaseRemainingTimes.Where(t => t.RemainingTime.IsHoursGreaterEqualDays(f.CaseRemainingTimeMaxFilter.Value, workingHours));
                return timeLeft.Value.IsHoursGreaterEqualDays(f.CaseRemainingTimeMaxFilter.Value, workingHours);
            }
            else
            {
                //filteredCaseRemainigTimes = remainingTime.CaseRemainingTimes.Where(t => t.RemainingTime.IsHoursEqualDays(f.CaseRemainingTimeFilter.Value - 1, workingHours));
                return timeLeft.Value.IsHoursEqualDays(f.CaseRemainingTimeFilter.Value - 1, workingHours);
            }
        }

        /// <summary>
        /// Builds work time calculator based on cases that can be fetched using supplied SQL
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="calculatorFactory"></param>
        /// <param name="utcNow"></param>
        /// <returns></returns>
        private WorkTimeCalculator InitCalcFromSQL(DataTable dataTable, IWorkTimeCalculatorFactory calculatorFactory, DateTime utcNow)
        {
            DateTime fetchRangeBegin = utcNow;
            DateTime fetchRangeEnd = utcNow;
            var deptIds = new HashSet<int>();
                      
            using (var dr = dataTable.CreateDataReader())
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        var intTmp = 0;
                        if (int.TryParse(dr["Department_Id"].ToString(), out intTmp) &&
                            !deptIds.Contains(intTmp))
                        {
                            deptIds.Add(intTmp);
                        }

                        DateTime caseRegistrationDate;
                        DateTime.TryParse(dr["RegTime"].ToString(), out caseRegistrationDate);

                        int SLAtime;
                        int.TryParse(dr["SolutionTime"].ToString(), out SLAtime);

                        DateTime watchDate;
                        if (DateTime.TryParse(dr["WatchDate"].ToString(), out watchDate))
                        {
                            //// calc time by watching date
                            if (watchDate > utcNow)
                            {
                                fetchRangeEnd = DatesHelper.Max(fetchRangeEnd, watchDate);
                            }
                            else
                            {
                                //// for cases that should be closed in the past
                                fetchRangeBegin = DatesHelper.Min(fetchRangeBegin, watchDate);
                            }
                        }
                        else if (SLAtime > 0)
                        {
                            //// calc by SLA value
                            fetchRangeBegin = DatesHelper.Min(fetchRangeBegin, caseRegistrationDate);
                        }
                    }
                }

                dr.Close();
            }
                    
            return calculatorFactory.Build(fetchRangeBegin, fetchRangeEnd, deptIds.ToArray());
        }

        private SearchResult<CaseSearchResult> SortSearchResult(SearchResult<CaseSearchResult> csr, ISearch s)
        {
            //tid kvar samt produktområde kan inte sorteras i databasen
            if (string.Compare(s.SortBy, "ProductArea_Id", true, CultureInfo.InvariantCulture) == 0)
            {
                if (s.Ascending)
                {
                    csr.Items = csr.Items.OrderBy(x => x.SortOrder).ToList();
                    return csr;
                }
                csr.Items = csr.Items.OrderByDescending(x => x.SortOrder).ToList();
                return csr;
            }
            else if (string.Compare(s.SortBy, TimeLeftColumn, true, CultureInfo.InvariantCulture) == 0)
            {
                /// we have to sort this field on "integer" manner
                var indx = 0;
                var structToSort = csr.Items.Select(
                    it =>
                        {
                            int intVal;
                            int? val = int.TryParse(it.SortOrder, out intVal) ? (int?)intVal : null;

                            double dVal;
                            double? floatingPoint = double.TryParse(it.SecSortOrder, out dVal) ? (double?)dVal : null;
                            return new { index = indx++, val, dVal };
                        });
                if (s.Ascending)
                {
                    csr.Items = structToSort.OrderBy(it => it.val).ThenBy(it => it.dVal).Select(it => csr.Items[it.index]).ToList();
                    return csr;
                }
                csr.Items = structToSort.OrderByDescending(it => it.val).ThenBy(it => it.dVal).Select(it => csr.Items[it.index]).ToList();
                return csr;
            }

            return csr;
        }

        private static GlobalEnums.CaseIcon GetCaseIcon(IDataReader dr)
        {
            var ret = GlobalEnums.CaseIcon.Normal;

            // TODO Hantera icon för urgent
            if (dr.SafeGetNullableDateTime("FinishingDate") != null)
                if (dr.SafeGetNullableDateTime("ApprovedDate") == null && string.Compare("1", dr.SafeGetString("RequireApproving"), true, CultureInfo.InvariantCulture) == 0)
                    ret = GlobalEnums.CaseIcon.FinishedNotApproved;
                else
                    ret = GlobalEnums.CaseIcon.Finished;

            return ret;
        }

        private static string GetCaseFieldName(string value)
        {
            return value.Replace("tblProblem.", "tblProblem_");
        }

        private static string GetCaseTypeFullPath(
                            CaseTypeOverview[] caseTypes,
                            int caseTypeId)
        {
            var caseType = caseTypes.FirstOrDefault(c => c.Id == caseTypeId);
            if (caseType == null)
            {
                return string.Empty;
            }

            var list = new List<CaseTypeOverview>();
            var parent = caseType;
            do
            {
                list.Add(parent);
                parent = caseTypes.FirstOrDefault(c => c.Id == parent.ParentId);
            }
            while (parent != null);

            return string.Join(" - ", list.Select(c => c.Name).Reverse());
        }

        private static string GetCaseTypeFullPathOnExternal(
                            CaseTypeOverview[] caseTypes,
                            int caseTypeId,
                            out bool checkShowOnExternal)
        {
            checkShowOnExternal = true;
            var caseType = caseTypes.FirstOrDefault(c => c.Id == caseTypeId);
            if (caseType == null)
            {
                return string.Empty;
            }

            var list = new List<CaseTypeOverview>();
            var parent = caseType;
            do
            {
                list.Add(parent);
                //if (parent.ShowOnExternalPage == 0)
                //Hide this for next version #57742
                if (parent.ShowOnExtPageCases == 0)
                    checkShowOnExternal = false;

                parent = caseTypes.FirstOrDefault(c => c.Id == parent.ParentId);
            }
            while (parent != null);

            return string.Join(" - ", list.Select(c => c.Name).Reverse());
        }

        private static string GetDatareaderValue(
                                IDataReader dr,
                                int col,
                                string fieldName,
                                Setting customerSetting,
                                IList<ProductAreaEntity> pal,
                                int? timeLeft,
                                IEnumerable<CaseTypeOverview> caseTypes,
                                IProductAreaNameResolver productAreaNamesResolver,
                                bool isExternalEnv,
                                out bool translateField,
                                out bool treeTranslation,
                                out DateTime? dateValue,
                                out FieldTypes fieldType,
                                out int baseId,
                                out bool preventToAddRecord)
        {
            var ret = string.Empty;
            var sep = " - ";
            translateField = false;
            treeTranslation = false;
            fieldType = FieldTypes.String;
            dateValue = null;
            baseId = 0;
            preventToAddRecord = false;

            switch (fieldName.ToLower())
            {
                case "regtime":
                    ret = dr[col].ToString();
                    dateValue = dr.SafeGetDate(col);
                    fieldType = FieldTypes.Time;
                    break;
                case "department_id":
                    if (customerSetting.DepartmentFormat == 1)
                        ret = dr.SafeGetString("DepertmentName") + sep + dr.SafeGetString("SearchKey") + sep + dr.SafeGetString("DepartmentId");
                    else
                        ret = dr.SafeGetString("DepertmentName");
                    break;
                case "priority_id":
                    ret = dr.SafeGetString("PriorityName");
                    translateField = true;
                    break;
                case "casetype_id":
                    if (isExternalEnv)
                    {
                        bool cOut = true;
                        ret = GetCaseTypeFullPathOnExternal(
                                    caseTypes.ToArray(),
                                    int.Parse(dr[col].ToString()), out cOut);
                        preventToAddRecord = !cOut;
                    }
                    else
                    {
                        ret = GetCaseTypeFullPath(
                                    caseTypes.ToArray(),
                                    int.Parse(dr[col].ToString()));
                    }
                    translateField = true;
                    break;

                case "status_id":
                    ret = dr[col].ToString();
                    translateField = true;
                    break;

                case "statesecondary_id":
                    ret = dr[col].ToString();
                    translateField = true;
                    break;

                case "plandate":
                    if (customerSetting.PlanDateFormat == 0)
                    {
                        ret = dr.SafeGetFormatedDateTime(col);
                        dateValue = dr.SafeGetDate(col);
                        fieldType = FieldTypes.Date;
                    }
                    else
                        ret = dr.SafeGetDateTimeWithWeek(col);
                    break;
                case "sms":
                case "contactbeforeaction":
                    ret = dr.SafeGetIntegerAsYesNo(col);
                    translateField = true;
                    break;
                case "verified":
                    ret = dr.SafeGetIntegerAsYesNo(col, true);
                    translateField = true;
                    break;
                case TimeLeftColumnLower:
                    fieldType = FieldTypes.NullableHours;
                    ret = timeLeft.ToString();
                    break;
                case "productarea_id":
                    ProductAreaEntity p = dr.SafeGetInteger("ProductArea_Id").getProductAreaItem(pal);
                    if (p != null)
                    {
                        // SelfService/LM 
                        if (isExternalEnv)
                        {
                            bool pOut = true;
                            var pRes = productAreaNamesResolver.GetParentPathOnExternalPage(p.Id, customerSetting.Customer_Id, out pOut);
                            preventToAddRecord = !pOut;

                            if (ConfigurationManager.AppSettings["InitFromSelfService"] == "true")
                            {
                                ret = p.Name;
                            }
                            else
                            {
                                ret = string.Join(" - ", pRes);
                            }
                        }
                        else
                        {
                            if (ConfigurationManager.AppSettings["InitFromSelfService"] == "true")
                            {
                                ret = p.Name;
                            }
                            else
                            {
                                ret = string.Join(" - ", productAreaNamesResolver.GetParentPath(p.Id, customerSetting.Customer_Id));
                            }
                        }

                        baseId = p.Id;
                    }
                    treeTranslation = true;
                    break;
                default:
                    if (string.Compare(dr[col].GetType().FullName, "System.DateTime", true, CultureInfo.InvariantCulture) == 0)
                    {
                        ret = dr.SafeGetFormatedDateTime(col);
                        dateValue = dr.SafeGetDate(col);
                        fieldType = FieldTypes.Date;
                    }
                    else
                        ret = dr[col].ToString();
                    break;
            }

            return ret;
        }
    }
}