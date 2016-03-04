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
        IList<CaseSearchResult> Search(CaseSearchContext context, out CaseRemainingTimeData remainingTime, out CaseAggregateData aggregateData);
    }

    public class CaseSearchRepository : ICaseSearchRepository
    {
        private const string TimeLeftColumn = "_temporary_.LeadTime";
        private const string TimeLeftColumnLower = "_temporary_.leadtime";
        
        private const string Combinator_OR = "OR";

        private const string Combinator_AND = "AND";

        private readonly ICustomerUserRepository _customerUserRepository;

        private readonly IProductAreaRepository _productAreaRepository;

        private readonly ICaseTypeRepository caseTypeRepository;
        
        private readonly ILogRepository logRepository;

        public CaseSearchRepository(
                ICustomerUserRepository customerUserRepository, 
                IProductAreaRepository productAreaRepository, 
                ICaseTypeRepository caseTypeRepository, 
                ILogRepository logRepository)
        {
            this._customerUserRepository = customerUserRepository;
            this._productAreaRepository = productAreaRepository;
            this.caseTypeRepository = caseTypeRepository;
            this.logRepository = logRepository;            
        }    

        public IList<CaseSearchResult> Search(CaseSearchContext context, out CaseRemainingTimeData remainingTime, out CaseAggregateData aggregateData)
        {
            var now = DateTime.UtcNow;
            var dsn = ConfigurationManager.ConnectionStrings["HelpdeskOleDbContext"].ConnectionString;
            
            /// shortcuts for context fields
            var f = context.f;
            var userId = f.UserId;
            var userCaseSettings = context.userCaseSettings;
            var workTimeCalcFactory = context.workTimeCalcFactory;
            var calculateRemainingTime = context.calculateRemainingTime;
            var customerSetting = context.customerSetting;
            var productAreaNamesResolver = context.productAreaNamesResolver;
            var s = context.s;

            var customerUserSetting = this._customerUserRepository.GetCustomerSettings(f.CustomerId, userId);
            IList<ProductAreaEntity> pal = this._productAreaRepository.GetMany(x => x.Customer_Id == f.CustomerId).OrderBy(x => x.Name).ToList(); 
            IList<CaseSearchResult> ret = new List<CaseSearchResult>();
            var caseTypes = this.caseTypeRepository.GetCaseTypeOverviews(f.CustomerId).ToArray();
            var displayLeftTime = userCaseSettings.Any(it => it.Name == TimeLeftColumn);
            remainingTime = new CaseRemainingTimeData();
            aggregateData = new CaseAggregateData();

            var sql = this.ReturnCaseSearchSql(
                                        context.f,
                                        context.customerSetting,
                                        customerUserSetting,
                                        context.isFieldResponsibleVisible,
                                        context.userId,
                                        context.userUserId,
                                        context.showNotAssignedWorkingGroups,
                                        context.userGroupId,
                                        context.gs,
                                        context.s,
                                        context.applicationType,
                                        context.relatedCasesCaseId,
                                        context.relatedCasesUserId,
                                        context.caseIds,
                                        context.userCaseSettings);

            if (string.IsNullOrEmpty(sql))
            {
                return ret;
            }

            var workTimeCalculator = this.InitCalcFromSQL(dsn, sql, workTimeCalcFactory, now);

            IDictionary<int,int> aggregateStatus = new Dictionary<int,int>();
            IDictionary<int,int> aggregateSubStatus = new Dictionary<int,int>();

            using (var con = new OleDbConnection(dsn)) 
            {
                using (var cmd = new OleDbCommand())
                {
                    try
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sql;
                        cmd.CommandTimeout = 300;                        
                        var dr = cmd.ExecuteReader();
                        if (dr != null && dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var doCalcTimeLeft = displayLeftTime || calculateRemainingTime;

                                var row = new CaseSearchResult();  
                                IList<Field> cols = new List<Field>();
                                var toolTip = string.Empty;
                                var sortOrder = string.Empty;
                                DateTime caseRegistrationDate;

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
                                        //// calc time by watching date
                                        if (watchDateDue > now)
                                        {
                                            // #52951 timeOnPause shouldn't calculate when watchdate has value
                                            timeLeft = (workTimeCalculator.CalculateWorkTime(
                                                now,
                                                watchDateDue,
                                                departmentId) ) / 60;                                            
                                        }
                                        else
                                        {
                                            //// for cases that should be closed in the past
                                            // #52951 timeOnPause shouldn't calculate when watchdate has value
                                            timeLeft = (-workTimeCalculator.CalculateWorkTime(
                                            watchDateDue,
                                            now,
                                            departmentId)) / 60;
                                        }                                        
                                    }
                                    else if (SLAtime > 0)
                                    {
                                        //// calc by SLA value
                                        var dtFrom = DatesHelper.Min(caseRegistrationDate, now);
                                        var dtTo = DatesHelper.Max(caseRegistrationDate, now);
                                        var calcTime = workTimeCalculator.CalculateWorkTime(dtFrom, dtTo, departmentId);
                                        timeLeft = (SLAtime * 60 - calcTime + timeOnPause) / 60;
                                        var floatingPoint = (SLAtime * 60 - calcTime + timeOnPause) % 60;
                                        
                                        if (timeLeft <= 0 && floatingPoint < 0)
                                            timeLeft--; 
                                        
                                    }

                                    if (timeLeft.HasValue)
                                    {
                                        int caseId;
                                        if (int.TryParse(dr["Id"].ToString(), out caseId))
                                        {
                                            remainingTime.AddRemainingTime(caseId, timeLeft.Value);
                                        }
                                    }
                                }
                                var isExternalEnv = (context.applicationType.ToLower() == ApplicationTypes.LineManager || context.applicationType.ToLower() == ApplicationTypes.SelfService);
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
                                        field = new Field { StringValue = string.Empty };
                                    }

                                    cols.Add(field);
                                }
                                row.SortOrder = sortOrder; 
                                row.Tooltip = toolTip; 
                                row.CaseIcon = GetCaseIcon(dr);
                                row.Id = dr.SafeGetInteger("Id"); 
                                row.Columns = cols;
                                row.IsUnread = dr.SafeGetInteger("Status") == 1;
                                row.IsUrgent = timeLeft.HasValue && timeLeft < 0;                                
                                if (!row.Ignored)
                                    ret.Add(row); 
                            }
                        }

                        dr.Close(); 
                    }
                    finally
                    {
                        con.Close();
                    }
                }
            }

            if (!string.IsNullOrEmpty(f.CaseClosingReasonFilter))
            {
                int closingReason;
                if (int.TryParse(f.CaseClosingReasonFilter, out closingReason))
                {
                    var closingReasons = new List<int> { closingReason };
                    var filtered = new List<CaseSearchResult>();
                    foreach (var foundedCase in ret)
                    {
                        var log = this.logRepository.GetLastLog(foundedCase.Id);
                        if (log != null && log.FinishingType.HasValue
                            && closingReasons.Contains(log.FinishingType.Value))
                        {
                            filtered.Add(foundedCase);
                        }
                    }

                    return this.SortSearchResult(filtered, s);
                }
            }
            
            return this.SortSearchResult(ret, s);
        }

        /// <summary>
        /// Builds work time calculator based on cases that can be fetched using supplied SQL
        /// </summary>
        /// <param name="dsn"></param>
        /// <param name="sql"></param>
        /// <param name="calculatorFactory"></param>
        /// <param name="utcNow"></param>
        /// <returns></returns>
        private WorkTimeCalculator InitCalcFromSQL(string dsn, string sql, IWorkTimeCalculatorFactory calculatorFactory, DateTime utcNow)
        {
            DateTime fetchRangeBegin = utcNow;
            DateTime fetchRangeEnd = utcNow;
            var deptIds = new HashSet<int>();
            using (var con = new OleDbConnection(dsn))
            {
                using (var cmd = new OleDbCommand())
                {
                    try
                    {
                        con.Open();                        
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sql;
                        cmd.CommandTimeout = 300;

                        var dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    var intTmp = 0;
                                    if (int.TryParse(dr["Department_Id"].ToString(), out intTmp) && !deptIds.Contains(intTmp))
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
                    }
                    finally
                    {
                        con.Close();
                    }
                }
            }

            return calculatorFactory.Build(fetchRangeBegin, fetchRangeEnd, deptIds.ToArray());
        }

        private IList<CaseSearchResult> SortSearchResult(IList<CaseSearchResult> csr, ISearch s)
        {
            //tid kvar samt produktområde kan inte sorteras i databasen
            if (string.Compare(s.SortBy, "ProductArea_Id", true, CultureInfo.InvariantCulture) == 0)
            {
                if (s.Ascending)
                {
                    return csr.OrderBy(x => x.SortOrder).ToList();
                }
                return csr.OrderByDescending(x => x.SortOrder).ToList();
            }
            else if (string.Compare(s.SortBy, TimeLeftColumn, true, CultureInfo.InvariantCulture) == 0)
            {
                /// we have to sort this field on "integer" manner
                var indx = 0;
                var structToSort = csr.Select(
                    it =>
                        {
                            int intVal;
                            int? val = int.TryParse(it.SortOrder, out intVal) ? (int?)intVal : null;
                            return new { index = indx++, val };
                        });
                if (s.Ascending)
                {
                    return structToSort.OrderBy(it => it.val).Select(it => csr[it.index]).ToList();
                }

                return structToSort.OrderByDescending(it => it.val).Select(it => csr[it.index]).ToList();
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
                
                if (parent.ShowOnExternalPage == 0)
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

        private string ReturnCaseSearchSql(
                    CaseSearchFilter f, 
                    Setting customerSetting, 
                    CustomerUser customerUserSetting,
                    bool isFieldResponsibleVisible,
                    int userId, 
                    string userUserId, 
                    int showNotAssignedWorkingGroups, 
                    int userGroupId, 
                    GlobalSetting gs,
                    ISearch s,
                    string applicationType,
                    int? relatedCasesCaseId,
                    string relatedCasesUserId,
                    int[] caseIds,
                    IList<CaseSettings> userCaseSettings)
        {
            var sql = new List<string>();
            var validateUserCaseSettings = new List<CaseSettings>();
            foreach (var us in userCaseSettings)
                if (!validateUserCaseSettings.Select(v => v.Name).Contains(us.Name))
                    validateUserCaseSettings.Add(us);

            var caseSettings = validateUserCaseSettings.ToDictionary(it => it.Name, it => it);
            // fields
            sql.Add("select distinct");

            // vid avslutade ärenden visas bara första 500
            if (f != null && (f.CaseProgress == CaseProgressFilter.ClosedCases || f.CaseProgress == CaseProgressFilter.None))
            {
                sql.Add("top 500");
            }

            var columns = new List<string>();
            #region adding columns in SELECT
            columns.Add("tblCase.Id");
            columns.Add("tblCase.CaseNumber");
            columns.Add("tblCase.Place");
            columns.Add("tblCustomer.Name as Customer_Id");
            columns.Add("tblRegion.Region as Region_Id");
            columns.Add("tblOU.OU as OU_Id");
            columns.Add("tblCase.UserCode");
            columns.Add("tblCase.Department_Id");
            columns.Add("tblCase.Persons_Name");
            columns.Add("tblCase.Persons_EMail");
            columns.Add("tblCase.Persons_Phone");
            columns.Add("tblCase.Persons_CellPhone");
            columns.Add("tblCase.FinishingDate");
            columns.Add("tblCase.FinishingDescription");
            columns.Add("tblCase.Caption");
            columns.Add("Cast(tblCase.[Description] as Nvarchar(Max)) as [Description] ");
            columns.Add("tblCase.Miscellaneous");
            columns.Add("tblCase.[Status] ");
            columns.Add("tblCase.ExternalTime");
            columns.Add("tblCase.RegTime");
            columns.Add("tblCase.ReportedBy");
            columns.Add("tblCase.ProductArea_Id");
            columns.Add("tblCase.InvoiceNumber");
            columns.Add("tblCustomer.Name");
            columns.Add("tblDepartment.Department as DepertmentName");
            columns.Add("tblDepartment.DepartmentId");
            columns.Add("tblDepartment.SearchKey");
            columns.Add("tblCase.ReferenceNumber");
            columns.Add("tblRegistrationSourceCustomer.SourceName as RegistrationSourceCustomer");
            if (customerSetting != null)
            {
                if (customerSetting.IsUserFirstLastNameRepresentation == 1)
                {
                    columns.Add("coalesce(tblUsers.FirstName, '') + ' ' + coalesce(tblUsers.SurName, '') as Performer_User_Id");
                    columns.Add("IsNull(tblCase.RegUserName, coalesce(tblUsers2.FirstName, '') + ' ' + coalesce(tblUsers2.SurName, '')) as User_Id");
                    columns.Add("coalesce(tblUsers3.FirstName, '') + ' ' + coalesce(tblUsers3.SurName, '') as CaseResponsibleUser_Id");
                }
                else
                {
                    columns.Add("coalesce(tblUsers.SurName, '') + ' ' + coalesce(tblUsers.FirstName, '') as Performer_User_Id");
                    columns.Add("IsNull(tblCase.RegUserName, coalesce(tblUsers2.Surname, '') + ' ' + coalesce(tblUsers2.Firstname, '')) as User_Id");
                    columns.Add("coalesce(tblUsers3.Surname, '') + ' ' + coalesce(tblUsers3.Firstname, '') as CaseResponsibleUser_Id");
                }

                columns.Add("coalesce(tblUsers4.Surname, '') + ' ' + coalesce(tblUsers4.Firstname, '') as tblProblem_ResponsibleUser_Id");
            }

            columns.Add("tblStatus.StatusName as Status_Id");
            columns.Add("tblSupplier.Supplier as Supplier_Id");
            string appId = string.Empty;
            try
            {
                appId = ConfigurationManager.AppSettings["InitFromSelfService"];
            }
            catch (Exception)
            {
            }

            if (appId == "true")
            {
                columns.Add("tblStateSecondary.AlternativeStateSecondaryName as StateSecondary_Id");
            }
            else
            {
                columns.Add("tblStateSecondary.StateSecondary as StateSecondary_Id");
            }

            columns.Add("tblCase.Priority_Id");
            columns.Add("tblPriority.PriorityName");
            columns.Add("coalesce(tblPriority.SolutionTime, 0) as SolutionTime");
            columns.Add("tblCase.WatchDate");
            columns.Add("tblCaseType.RequireApproving");
            columns.Add("tblCase.ApprovedDate");
            columns.Add("tblCase.ContactBeforeAction");
            columns.Add("tblCase.SMS");
            columns.Add("tblCase.Available");
            columns.Add("tblCase.Cost");
            columns.Add("tblCase.PlanDate");
            if (customerSetting != null)
            {
                columns.Add("tblWorkingGroup.WorkingGroup as WorkingGroup_Id");
            }

            columns.Add("tblCase.ChangeTime");
            columns.Add("tblCaseType.Id as CaseType_Id");
            columns.Add("tblCase.RegistrationSource");
            columns.Add("tblCase.InventoryNumber");
            columns.Add("tblCase.InventoryType as ComputerType_Id");
            columns.Add("tblCase.InventoryLocation");
            columns.Add("tblCategory.Category as Category_Id");
            columns.Add("tblCase.SolutionRate");
            columns.Add("tblSystem.SystemName as System_Id");
            columns.Add("tblUrgency.Urgency as Urgency_Id");
            columns.Add("tblImpact.Impact as Impact_Id");
            columns.Add("tblCase.Verified");
            columns.Add("tblCase.VerifiedDescription");
            columns.Add("tblCase.LeadTime");
            columns.Add("tblCase.Status_Id as aggregate_Status");
            columns.Add("tblCase.StateSecondary_Id as aggregate_SubStatus");
             
            columns.Add(string.Format("'0' as [{0}]", TimeLeftColumn));
            columns.Add("tblStateSecondary.IncludeInCaseStatistics");
            if (caseSettings.ContainsKey(GlobalEnums.TranslationCaseFields.CausingPart.ToString()))
            {
                columns.Add("tblCausingPart.Name as CausingPart");
            }
            #endregion
            sql.Add(string.Join(",", columns));

            /// tables and joins
            var tables = new List<string>();
            #region adding tables into FROM section
            tables.Add("from tblCase");
            tables.Add("inner join tblCustomer on tblCase.Customer_Id = tblCustomer.Id "); 
            tables.Add("inner join tblCustomerUser on tblCase.Customer_Id = tblCustomerUser.Customer_Id ");  
            tables.Add("left outer join tblDepartment on tblDepartment.Id = tblCase.Department_Id ");  
            tables.Add("left outer join tblRegion on tblCase.Region_Id = tblRegion.Id ");  
            tables.Add("left outer join tblOU on tblCase.OU_Id=tblOU.Id ");            
            tables.Add("left outer join tblSupplier on tblCase.Supplier_Id=tblSupplier.Id "); 
            tables.Add("left outer join tblSystem on tblCase.System_Id = tblSystem.Id ");  
            tables.Add("left outer join tblUrgency on tblCase.Urgency_Id = tblUrgency.Id ");  
            tables.Add("left outer join tblImpact on tblCase.Impact_Id = tblImpact.Id ");
            tables.Add("left outer join tblRegistrationSourceCustomer on tblCase.RegistrationSourceCustomer_Id = tblRegistrationSourceCustomer.Id");
            tables.Add("left outer join tblUsers on tblUsers.Id = tblCase.Performer_User_Id");
            if (customerSetting != null)
            {
                const int SHOW_IF_DEFAULT_USER_GROUP = 0;
                if (customerSetting.CaseWorkingGroupSource == SHOW_IF_DEFAULT_USER_GROUP)
                {
                    tables.Add(string.Format("left join tblUserWorkingGroup as defGroup on defGroup.User_Id = {0} and defGroup.IsDefault = 1 and defGroup.WorkingGroup_Id = tblCase.WorkingGroup_Id", userId));
                    tables.Add("left join tblWorkingGroup on tblWorkingGroup.id = defgroup.WorkingGroup_Id");
                }
                else
                {
                    tables.Add("left outer join tblWorkingGroup on tblCase.WorkingGroup_Id = tblWorkingGroup.Id ");
                }
            }

            tables.Add("left outer join tblStatus on tblCase.Status_Id = tblStatus.Id ");  
            tables.Add("left outer join tblCategory on tblCase.category_Id = tblCategory.Id ");  
            tables.Add("left outer join tblStateSecondary on tblCase.StateSecondary_Id = tblStateSecondary.Id ");  
            tables.Add("left outer join tblPriority on tblCase.Priority_Id = tblPriority.Id ");  
            tables.Add("inner join tblCaseType on tblCase.CaseType_Id = tblCaseType.Id ");  
            tables.Add("left outer join tblUsers as tblUsers2 on tblCase.[User_Id] = tblUsers2.Id ");
            tables.Add("left outer join tblUsers as tblUsers3 on tblCase.CaseResponsibleUser_Id = tblUsers3.Id "); 
            tables.Add("left outer join tblProblem on tblCase.Problem_Id = tblProblem.Id ");
            tables.Add("left outer join tblUsers as tblUsers4 on tblProblem.ResponsibleUser_Id = tblUsers4.Id ");
            if (caseSettings.ContainsKey(GlobalEnums.TranslationCaseFields.CausingPart.ToString()))
            {
                tables.Add("left outer join tblCausingPart on (tblCase.CausingPartId = tblCausingPart.Id)");
            }
            #endregion
            sql.Add(string.Join(" ", tables));

            /// WHERE ..
            if (applicationType.ToLower() == ApplicationTypes.LineManager || applicationType.ToLower() == ApplicationTypes.SelfService)
            {
                sql.Add(this.ReturnCustomCaseSearchWhere(f, userUserId));
            }
            else
            {
                sql.Add(this.ReturnCaseSearchWhere(
                    f, 
                    customerSetting, 
                    customerUserSetting, 
                    isFieldResponsibleVisible, 
                    userId, 
                    userUserId, 
                    showNotAssignedWorkingGroups, 
                    userGroupId, 
                    gs, 
                    relatedCasesCaseId, 
                    caseSettings,
                    relatedCasesUserId, 
                    caseIds));
            }

            // ORDER BY ...
           var orderBy = new List<string> { "order by" };
            string sort = (s != null && !string.IsNullOrEmpty(s.SortBy)) ? s.SortBy.Replace("_temporary_.", string.Empty) : string.Empty;
            if (string.IsNullOrEmpty(sort))
            {
                orderBy.Add(" CaseNumber desc");
            }
            else
            {
                orderBy.Add(sort);
                if (s != null && !s.Ascending)
                {
                    orderBy.Add("desc");
                }

                if (sort.ToLower() != "casenumber")
                orderBy.Add(", CaseNumber desc");
            }

            sql.Add(string.Join(" ", orderBy));

            return string.Join(" ", sql);
        }

        //Used for LineManager & SelfService
        private string ReturnCustomCaseSearchWhere(CaseSearchFilter f, string userUserId)
        {
            if (f == null)
            {
                return string.Empty;
            }

            var sb = new StringBuilder();                        

            // kund 
            sb.Append(" where (tblCase.Customer_Id = " + f.CustomerId + ")");
            sb.Append(" and (tblCase.Deleted = 0)");


            //CaseType
            sb.Append(" and (tblCaseType.ShowOnExternalPage <> 0)");

            //ProductArea
            sb.Append(" and (tblCase.ProductArea_Id Is Null or tblCase.ProductArea_Id in (select id from tblProductArea where ShowOnExternalPage <> 0))");
            

            if (f.ReportedBy.Trim() == string.Empty)
                f.ReportedBy = "''";
            switch (f.CaseListType.ToLower())
            {

                case CaseListTypes.UserCases:
                    sb.Append(" and (tblCase.[RegUserId] = '" + userUserId + "')");
                    break;

                //Manager Cases Only
                case CaseListTypes.ManagerCases:
                    sb.Append(" and (tblCase.[RegUserId] = '" + userUserId + "' or tblCase.[ReportedBy] = " + f.ReportedBy + ")");
                    break;

                //CoWorkers Cases Only
                case CaseListTypes.CoWorkerCases:
                    sb.Append(" and (((tblCase.[ReportedBy] is null or tblCase.[ReportedBy] = '') and tblCase.[RegUserId] = '" + userUserId + "') or tblCase.[ReportedBy] in (" + f.ReportedBy + "))");
                    break;

                //Manager & Coworkers Cases
                case CaseListTypes.ManagerCoWorkerCases:
                    sb.Append(" and (tblCase.[RegUserId] = '" + userUserId + "' or tblCase.[ReportedBy] in (" + f.ReportedBy + "))");                    
                    break;
                
            }

            // ärende progress - iShow i gammal helpdesk
            switch (f.CaseProgress)
            {
                case CaseProgressFilter.None:
                    break;
                case CaseProgressFilter.ClosedCases:
                    sb.Append(" and (tblCase.FinishingDate is not null)");
                    break;
                case CaseProgressFilter.CasesInProgress:
                    sb.Append(" and (tblCase.FinishingDate is null)");
                    break;
                case CaseProgressFilter.CasesInRest:
                    sb.Append(" and (tblCase.FinishingDate is null and tblCase.StateSecondary_Id is not null)");
                    break;
                case CaseProgressFilter.UnreadCases:
                    sb.Append(" and (tblCase.FinishingDate is null and tblCase.Status = 1)");
                    break;
                case CaseProgressFilter.FinishedNotApproved:
                    sb.Append(" and (tblCase.FinishingDate is not null and tblCaseType.RequireApproving = 1 and tblCase.ApprovedDate is null)");
                    break;
                case CaseProgressFilter.InProgressStatusGreater1:
                    sb.Append(" and (tblCase.FinishingDate is null and tblCase.Status > 1)");
                    break;
                case CaseProgressFilter.CasesWithWatchDate:
                    sb.Append(" and (tblCase.FinishingDate is null and tblCase.WatchDate is not null)");
                    break;
                case CaseProgressFilter.FollowUp:
                    sb.Append(" and (tblCase.FollowUpdate is not null)");
                    break;                
                default:
                    sb.Append(" and (tblCase.FinishingDate is null)");
                    break;
            }        

            if (!string.IsNullOrWhiteSpace(f.FreeTextSearch))
            {
                var text = f.FreeTextSearch;
                sb.Append(" AND (");
                sb.Append(this.GetSqlLike("[tblCase].[CaseNumber]", text));
                sb.AppendFormat(" OR {0}", this.GetSqlLike("[tblCase].[ReportedBy]", text));
                sb.AppendFormat(" OR {0}", this.GetSqlLike("[tblCase].[Persons_Name]", text));
                sb.AppendFormat(" OR {0}", this.GetSqlLike("[tblCase].[Persons_EMail]", text));
                sb.AppendFormat(" OR {0}", this.GetSqlLike("[tblCase].[Persons_Phone]", text));
                sb.AppendFormat(" OR {0}", this.GetSqlLike("[tblCase].[Persons_CellPhone]", text));
                sb.AppendFormat(" OR {0}", this.GetSqlLike("[tblCase].[Place]", text));
                sb.AppendFormat(" OR {0}", this.GetSqlLike("[tblCase].[Caption]", text));
                sb.AppendFormat(" OR [tblCase].[Description] LIKE '%{0}%'", text);
                sb.AppendFormat(" OR {0}", this.GetSqlLike("[tblCase].[Miscellaneous]", text));
                sb.AppendFormat(" OR {0}", this.GetSqlLike("[tblDepartment].[Department]", text));
                sb.AppendFormat(" OR {0}", this.GetSqlLike("[tblDepartment].[DepartmentId]", text));
                sb.AppendFormat(" OR ([tblCase].[Id] IN (SELECT [Case_Id] FROM [tblLog] WHERE [tblLog].[Text_Internal] LIKE '%{0}%' OR [tblLog].[Text_External] LIKE '%{0}%'))", text);
                sb.AppendFormat(" OR ([tblCase].[Id] IN (SELECT [Case_Id] FROM [tblFormFieldValue] WHERE {0}))", this.GetSqlLike("FormFieldValue", text));
                sb.Append(") ");
            }
            
            return sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="f"></param>
        /// <param name="customerSetting"></param>
        /// <param name="customerUserSetting"></param>
        /// <param name="isFieldResponsibleVisible"></param>
        /// <param name="userId"></param>
        /// <param name="userUserId"></param>
        /// <param name="showNotAssignedWorkingGroups"></param>
        /// <param name="userGroupId"></param>
        /// <param name="gs"></param>
        /// <param name="relatedCasesCaseId"></param>
        /// <param name="relatedCasesUserId"></param>
        /// <param name="caseIds"></param>
        /// <param name="caseSettings"></param>
        /// <param name="restrictedCasePermission"> User has permission to see own cases only </param>
        /// <returns></returns>
        private string ReturnCaseSearchWhere(CaseSearchFilter f, Setting customerSetting, CustomerUser customerUserSetting, bool isFieldResponsibleVisible, int userId, string userUserId, int showNotAssignedWorkingGroups, int userGroupId, GlobalSetting gs, int? relatedCasesCaseId, Dictionary<string, CaseSettings> caseSettingsMap, string relatedCasesUserId = null, int[] caseIds = null)
        {
            if (f == null || customerSetting == null || gs == null)
            {
                return string.Empty;
            }

            var sb = new StringBuilder();           

            // kund 
            sb.Append(" where (tblCase.Customer_Id = " + f.CustomerId + ")");
            sb.Append(" and (tblCase.Deleted = 0)");

            if (caseIds != null && caseIds.Any())
            {
                sb.AppendFormat(" AND ([tblCase].[Id] IN ({0})) ", string.Join(",", caseIds));
                return sb.ToString();
            }

            // Related cases list http://redmine.fastdev.se/issues/11257
            if (relatedCasesCaseId.HasValue)
            {
                sb.AppendFormat(" AND ([tblCase].[Id] != {0}) AND (LOWER(LTRIM(RTRIM([tblCase].[ReportedBy]))) = LOWER(LTRIM(RTRIM('{1}')))) ", relatedCasesCaseId.Value, relatedCasesUserId);
                
                // http://redmine.fastdev.se/issues/11257
                /*if (restrictedCasePermission == 1)
                {
                    if (userGroupId == (int)UserGroup.Administrator)
                    {
                        sb.AppendFormat(" AND ([tblCase].[Performer_User_Id] = {0} OR [tblCase].[CaseResponsibleUser_Id] = {0}) ", userId);
                    }
                    else if (userGroupId == (int)UserGroup.User)
                    {
                        sb.AppendFormat(" AND (LOWER(LTRIM(RTRIM([tblCase].[ReportedBy]))) = LOWER(LTRIM(RTRIM('{0}')))) ", userUserId);
                    }
                }*/            

                return sb.ToString();
            }

            sb.Append(" and (tblCustomerUser.[User_Id] = " + f.UserId + ")");

            // användaren får bara se avdelningar som den har behörighet till
            sb.Append(" and (tblCase.Department_Id In (select Department_Id from tblDepartmentUser where [User_Id] = " + userId + ")");
            sb.Append(" or not exists (select du.Department_Id from tblDepartmentUser du " +
                                                "Inner join tblDepartment d on (d.Id = du.Department_Id) " +
                                      "where (du.[User_Id] = " + userId + ") and d.customer_id = " + f.CustomerId + ")");
            sb.Append(") ");

            // finns kryssruta på användaren att den bara får se sina egna ärenden
            var restrictedCasePermission = customerUserSetting.User.RestrictedCasePermission;
            if (restrictedCasePermission == 1)
            {
                if (userGroupId == 2)
                    sb.Append(" and (tblCase.Performer_User_Id = " + userId + " or tblcase.CaseResponsibleUser_Id = " + userId + ")");
                else if (userGroupId == 1)
                    sb.Append(" and (lower(tblCase.reportedBy) = lower('" + userUserId + "') or tblcase.User_Id = " + userId + ")");
            }            
            
            // ärende progress - iShow i gammal helpdesk
            switch (f.CaseProgress)
            {
                case CaseProgressFilter.None:
                    break;
                case CaseProgressFilter.ClosedCases:
                    sb.Append(" and (tblCase.FinishingDate is not null)");
                    break;
                case CaseProgressFilter.CasesInProgress:
                    sb.Append(" and (tblCase.FinishingDate is null)");
                    break;
                case CaseProgressFilter.CasesInRest:
                    sb.Append(" and (tblCase.FinishingDate is null and tblCase.StateSecondary_Id is not null and tblStateSecondary.IncludeInCaseStatistics = 0)");
                    break;
                case CaseProgressFilter.UnreadCases:
                    sb.Append(" and (tblCase.FinishingDate is null and tblCase.Status = 1)");
                    break;
                case CaseProgressFilter.FinishedNotApproved:
                    sb.Append(" and (tblCase.FinishingDate is not null and tblCaseType.RequireApproving = 1 and tblCase.ApprovedDate is null)");
                    break;
                case CaseProgressFilter.InProgressStatusGreater1:
                    sb.Append(" and (tblCase.FinishingDate is null and tblCase.Status > 1)");
                    break;
                case CaseProgressFilter.CasesWithWatchDate:
                    sb.Append(" and (tblCase.FinishingDate is null and tblCase.WatchDate is not null)");
                    break;
                case CaseProgressFilter.FollowUp:
                    sb.Append(" and (tblCase.FollowUpdate is not null)");
                    break;
                default:
                    sb.Append(" and (tblCase.FinishingDate is null)");
                    break;
            }

            // working group 
            if (!string.IsNullOrWhiteSpace(f.WorkingGroup))
            {
                if (customerSetting.CaseWorkingGroupSource == 0)
                    sb.Append(" and (tblWorkingGroup.Id in (" + f.WorkingGroup.SafeForSqlInject() + ")) ");
                else
                    sb.Append(" and (coalesce(tblCase.WorkingGroup_Id, 0) in (" + f.WorkingGroup.SafeForSqlInject() + ")) ");
            }

            if (f.SearchInMyCasesOnly)
            {
                var preparedUserIds = userId.ToString(CultureInfo.InvariantCulture).SafeForSqlInject();
                sb.AppendFormat(
                    " AND ([tblCase].[Performer_User_Id] IN ({0}) ", preparedUserIds);
                if (isFieldResponsibleVisible)
                {
                    sb.AppendFormat("OR [tblCase].[CaseResponsibleUser_Id] IN ({0})", preparedUserIds);
                }

                sb.Append(") ");
            }

            // performer/utförare
            if (!string.IsNullOrWhiteSpace(f.UserPerformer))
            {
                var performersDict = f.UserPerformer.Split(',').ToDictionary(it => it, it => true);
                var searchingUnassigned = restrictedCasePermission != 1 && performersDict.ContainsKey(int.MinValue.ToString());
                if (searchingUnassigned)
                {
                    performersDict.Remove(int.MinValue.ToString());
                }

                if (performersDict.Count > 0 || searchingUnassigned)
                {
                    sb.Append(" AND (");
                }
                
                if (performersDict.Count > 0)
                {
                    sb.AppendFormat("tblCase.Performer_User_Id in ({0}) ", string.Join(",", performersDict.Keys).SafeForSqlInject());
                }

                if (searchingUnassigned || customerUserSetting.User.ShowNotAssignedCases == 1)
                {
                    if (performersDict.Count > 0)
                    {
                        sb.AppendFormat(" OR ");
                    }

                    sb.Append("tblCase.Performer_User_Id is NULL");
                }

                if (performersDict.Count > 0 || searchingUnassigned)
                {
                    sb.Append(") ");
                }
            }
           
            // ansvarig
            if (!string.IsNullOrWhiteSpace(f.UserResponsible))
                sb.Append(" and (tblCase.CaseResponsibleUser_Id in (" + f.UserResponsible.SafeForSqlInject() + "))"); 
           
            // case type
            if (f.CaseType != 0)
            {
                var caseTypes = new List<int>();
                caseTypes.Add(f.CaseType);
                var caseTypeChildren = this.caseTypeRepository.GetChildren(f.CaseType);
                if (caseTypeChildren != null)
                {
                    caseTypes.AddRange(caseTypeChildren);
                }

                sb.Append(" and (tblcase.CaseType_Id IN (" + string.Join(",", caseTypes) + "))");
            }
            // Product area 
            if (!string.IsNullOrWhiteSpace(f.ProductArea))
                if (string.Compare(f.ProductArea, "0", true, CultureInfo.InvariantCulture) != 0)  
                    sb.Append(" and (tblcase.ProductArea_Id in (" + f.ProductArea.SafeForSqlInject() + "))");

            // department / avdelning
            if (!string.IsNullOrWhiteSpace(f.Department))
            {
                // organizationUnit
                if (!string.IsNullOrWhiteSpace(f.OrganizationUnit))
                    sb.Append(" and (tblCase.Department_Id in (" + f.Department.SafeForSqlInject() + ") or " +
                                    "tblCase.OU_Id in (" + f.OrganizationUnit.SafeForSqlInject() + "))");
                else
                    sb.Append(" and (tblCase.Department_Id in (" + f.Department.SafeForSqlInject() + "))");
            }
            else
            {
                // organizationUnit
                if (!string.IsNullOrWhiteSpace(f.OrganizationUnit))
                    sb.Append(" and (tblCase.OU_Id in (" + f.OrganizationUnit.SafeForSqlInject() + "))");                                    
            }
            
            // användare / user            
            if (!string.IsNullOrWhiteSpace(f.User))
            {
                sb.Append(" and (tblCase.User_Id in (" + f.User.SafeForSqlInject() + "))");
            }
            
            // region
            if (!string.IsNullOrWhiteSpace(f.Region ))
                sb.Append(" and (tblDepartment.Region_Id in (" + f.Region.SafeForSqlInject() + "))");
            // prio
            if (!string.IsNullOrWhiteSpace(f.Priority))
                sb.Append(" and (tblcase.Priority_Id in (" + f.Priority.SafeForSqlInject() + "))");
            // katagori / category
            if (!string.IsNullOrWhiteSpace(f.Category))
                sb.Append(" and (tblcase.Category_Id in (" + f.Category.SafeForSqlInject() + "))");
            // status
            if (!string.IsNullOrWhiteSpace(f.Status ))
                sb.Append(" and (tblcase.Status_Id in (" + f.Status.SafeForSqlInject() + "))");
            // state secondery
            if (!string.IsNullOrWhiteSpace(f.StateSecondary))
                sb.Append(" and (tblcase.StateSecondary_Id in (" + f.StateSecondary.SafeForSqlInject() + "))");

            if (f.CaseRegistrationDateStartFilter.HasValue)
            {
                sb.AppendFormat(" AND ([tblCase].[RegTime] >= '{0}')", f.CaseRegistrationDateStartFilter);
            }

            if (f.CaseRegistrationDateEndFilter.HasValue)
            {
                sb.AppendFormat(" AND ([tblCase].[RegTime] <= '{0}')", f.CaseRegistrationDateEndFilter);
            }

            if (f.CaseWatchDateStartFilter.HasValue)
            {
                sb.AppendFormat(" AND ([tblCase].[WatchDate] >= '{0}')", f.CaseWatchDateStartFilter);
            }

            if (f.CaseWatchDateEndFilter.HasValue)
            {
                sb.AppendFormat(" AND ([tblCase].[WatchDate] <= '{0}')", f.CaseWatchDateEndFilter);
            }            

            if (f.CaseClosingDateStartFilter.HasValue)
            {
                sb.AppendFormat(" AND ([tblCase].[FinishingDate] >= '{0}')", f.CaseClosingDateStartFilter);
            }

            if (f.CaseClosingDateEndFilter.HasValue)
            {
                sb.AppendFormat(" AND ([tblCase].[FinishingDate] <= '{0}')", f.CaseClosingDateEndFilter);
            }

            if (!string.IsNullOrWhiteSpace(f.FreeTextSearch))
            {
                if (f.FreeTextSearch[0] == '#')
                {
                    var text = f.FreeTextSearch.Substring(1, f.FreeTextSearch.Length-1);
                    int res = 0;
                    if (int.TryParse(text, out res))
                    {
                        sb.Append(" AND (");
                        sb.Append("[tblCase].[CaseNumber] = " + text);
                        sb.Append(") ");
                    }
                    else
                    {
                        sb.Append(" AND (");
                        sb.Append(this.GetSqlLike("[tblCase].[CaseNumber]", text));
                        sb.Append(") ");
                    }
                }
                else
                {
                    var text = f.FreeTextSearch;
                    var safeText = f.FreeTextSearch.Replace("'","''");
                    sb.Append(" AND (");
                    sb.Append(this.GetSqlLike("[tblCase].[CaseNumber]", text));
                    sb.AppendFormat(" OR {0}", this.GetSqlLike("[tblCase].[ReportedBy]", text));
                    sb.AppendFormat(" OR {0}", this.GetSqlLike("[tblCase].[Persons_Name]", text));
                    sb.AppendFormat(" OR {0}", this.GetSqlLike("[tblCase].[RegUserName]", text));
                    sb.AppendFormat(" OR {0}", this.GetSqlLike("[tblCase].[Persons_EMail]", text));
                    sb.AppendFormat(" OR {0}", this.GetSqlLike("[tblCase].[Persons_Phone]", text));
                    sb.AppendFormat(" OR {0}", this.GetSqlLike("[tblCase].[Persons_CellPhone]", text));
                    sb.AppendFormat(" OR {0}", this.GetSqlLike("[tblCase].[Place]", text));
                    sb.AppendFormat(" OR {0}", this.GetSqlLike("[tblCase].[Caption]", text));                    
                    sb.AppendFormat(" OR [tblCase].[Description] LIKE '%{0}%'", safeText);
                    sb.AppendFormat(" OR {0}", this.GetSqlLike("[tblCase].[Miscellaneous]", text));
                    sb.AppendFormat(" OR {0}", this.GetSqlLike("[tblDepartment].[Department]", text));
                    sb.AppendFormat(" OR {0}", this.GetSqlLike("[tblDepartment].[DepartmentId]", text));
                    sb.AppendFormat(" OR ([tblCase].[Id] IN (SELECT [Case_Id] FROM [tblLog] WHERE [tblLog].[Text_Internal] LIKE '%{0}%' OR [tblLog].[Text_External] LIKE '%{0}%'))", safeText);
                    sb.AppendFormat(" OR ([tblCase].[Id] IN (SELECT [Case_Id] FROM [tblFormFieldValue] WHERE {0}))", this.GetSqlLike("FormFieldValue", text));
                    sb.AppendFormat(" OR {0}", this.GetSqlLike("[tblCase].[ReferenceNumber]", text));
                    sb.AppendFormat(" OR {0}", this.GetSqlLike("[tblCase].[InvoiceNumber]", text));
                    sb.AppendFormat(" OR {0}", this.GetSqlLike("[tblCase].[InventoryNumber]", text));                                            
                    
                    // Get CaseNumbers from Indexing Service
                    if (f.SearchThruFiles)
                    {
                        if (!string.IsNullOrEmpty(customerSetting.FileIndexingServerName) && !string.IsNullOrEmpty(customerSetting.FileIndexingCatalogName))
                        {
                            var caseNumbers = GetCasesContainsText(customerSetting.FileIndexingServerName, customerSetting.FileIndexingCatalogName, safeText);
                            if (!string.IsNullOrEmpty(caseNumbers))
                                sb.AppendFormat(" OR [tblCase].[CaseNumber] In ({0}) ", caseNumbers);
                        }
                    }

                    sb.Append(") ");
                }
            }

            // "Caption" Search
            if (!string.IsNullOrEmpty(f.CaptionSearch))
            {
                var text = f.CaptionSearch;
                sb.Append(" AND (");
                sb.AppendFormat("LOWER({0}) LIKE '%{1}%'", "[tblCase].[Caption]", text);
                sb.Append(") ");
            }

            // "Initiator" search field
            if (!string.IsNullOrEmpty(f.Initiator))
            {
                sb.Append(" AND (");
                sb.AppendFormat("{0}", this.GetSqlLike("[tblCase].[ReportedBy]", f.Initiator, Combinator_AND));
                sb.AppendFormat(" OR {0}", this.GetSqlLike("[tblCase].[Persons_Name]", f.Initiator, Combinator_AND));                
                sb.AppendFormat(" OR {0}", this.GetSqlLike("[tblCase].[UserCode]", f.Initiator, Combinator_AND));
                sb.AppendFormat(" OR {0}", this.GetSqlLike("[tblCase].[Persons_Email]", f.Initiator, Combinator_AND));
                sb.AppendFormat(" OR {0}", this.GetSqlLike("[tblCase].[Place]", f.Initiator, Combinator_AND));
                sb.AppendFormat(" OR {0}", this.GetSqlLike("[tblCase].[Persons_CellPhone]", f.Initiator, Combinator_AND));
                sb.AppendFormat(" OR {0}", this.GetSqlLike("[tblCase].[Persons_Phone]", f.Initiator, Combinator_AND));
                sb.Append(") ");
            }

            //LockCaseToWorkingGroup
            if (userGroupId < 3 && gs.LockCaseToWorkingGroup == 1)
            {
                sb.Append(" and (tblCase.WorkingGroup_Id in ");
                sb.Append(" (");
                sb.Append("select WorkingGroup_Id from tblUserWorkingGroup where [User_Id] = " + userId + " ");
                sb.Append("and WorkingGroup_Id in (select Id from tblWorkingGroup where Customer_Id = " + f.CustomerId + ") "); 
                sb.Append(" )");

                if (showNotAssignedWorkingGroups == 1)
                    sb.Append(" or tblCase.WorkingGroup_Id is null ");

                sb.Append(" or not exists (select id from tblWorkingGroup where Customer_Id = " + f.CustomerId + ")");
                sb.Append(") ");
            }

            return sb.ToString();
        }

        private string GetCasesContainsText(string indexingServerName, string catalogName, string searchText)
        {
            var res = string.Empty;
            if (string.IsNullOrEmpty(indexingServerName) ||
                string.IsNullOrEmpty(catalogName) || string.IsNullOrEmpty(searchText))
                return string.Empty;
            else
            {                
                var caseNumbers = FileIndexingRepository.GetCaseNumbersBy(indexingServerName, catalogName, searchText);                
                if (caseNumbers.Any())
                    res = string.Join(",", caseNumbers.ToArray());
            }                        
            return res;
        }

        private string GetSqlLike(string field, string text, string combinator = Combinator_OR)
        {
            var sb = new StringBuilder();
            if (!string.IsNullOrEmpty(field) && 
                !string.IsNullOrEmpty(text))
            {
                sb.Append(" (");
                var words = text
                        .FreeTextSafeForSqlInject()
                        .ToLower()
                        .Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                for (var i = 0; i < words.Length; i++)
                {
                    sb.AppendFormat("(LOWER({0}) LIKE '%{1}%')", field, words[i].Trim());
                    if (words.Length > 1 && i < words.Length - 1)
                    {
                        sb.Append(string.Format(" {0} ", combinator));
                    }
                }

                sb.Append(") ");
            }

            return sb.ToString();
        }
        
        private string InsensitiveSearch(string fieldName)
        {
            var ret = fieldName;
            var dsn = ConfigurationManager.ConnectionStrings["HelpdeskOleDbContext"].ConnectionString;

            if (!dsn.Contains("SQLOLEDB")) 
                ret = "lower(" + fieldName + ")";  

            return ret;
        }
        
    }
}