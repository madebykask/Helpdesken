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
    using DH.Helpdesk.BusinessData.Models.Case.Output;
    using DH.Helpdesk.BusinessData.OldComponents;
    using DH.Helpdesk.BusinessData.OldComponents.DH.Helpdesk.BusinessData.Utils;
    using DH.Helpdesk.Common.Enums.Cases;
    using DH.Helpdesk.Dal.Utils;
    using DH.Helpdesk.Domain;

    /// <summary>
    /// The CaseSearchRepository interface.
    /// </summary>
    public interface ICaseSearchRepository
    {
        IList<CaseSearchResult> Search(
            CaseSearchFilter f,
            IList<CaseSettings> csl,
            int userId,
            string userUserId,
            int showNotAssignedWorkingGroups,
            int userGroupId,
            int restrictedCasePermission,
            GlobalSetting gs,
            Setting customerSetting,
            ISearch s,
            WorkTimeCalculator workTimeCalculator,
            string applicationId);
    }

    public class CaseSearchRepository : ICaseSearchRepository
    {
        private const string TimeLeftColumn = "_temporary_.LeadTime";
        private const string TimeLeftColumnLower = "_temporary_.leadtime";

        private readonly ICustomerUserRepository _customerUserRepository;

        private readonly IProductAreaRepository _productAreaRepository;

        private readonly ICaseTypeRepository caseTypeRepository;

        private readonly IFinishingCauseRepository finishingCauseRepository;

        private readonly ILogRepository logRepository;

        public CaseSearchRepository(
                ICustomerUserRepository customerUserRepository, 
                IProductAreaRepository productAreaRepository, 
                ICaseTypeRepository caseTypeRepository, 
                IFinishingCauseRepository finishingCauseRepository, 
                ILogRepository logRepository)
        {
            this._customerUserRepository = customerUserRepository;
            this._productAreaRepository = productAreaRepository;
            this.caseTypeRepository = caseTypeRepository;
            this.finishingCauseRepository = finishingCauseRepository;
            this.logRepository = logRepository;
        }

        public IList<CaseSearchResult> Search(
                                    CaseSearchFilter f, 
                                    IList<CaseSettings> csl, 
                                    int userId, 
                                    string userUserId, 
                                    int showNotAssignedWorkingGroups, 
                                    int userGroupId, 
                                    int restrictedCasePermission, 
                                    GlobalSetting gs, 
                                    Setting customerSetting, 
                                    ISearch s,
                                    WorkTimeCalculator workTimeCalculator,
                                    string applicationId)
        {
            var now = DateTime.UtcNow;
            var dsn = ConfigurationManager.ConnectionStrings["HelpdeskOleDbContext"].ConnectionString;
            var customerUserSetting = this._customerUserRepository.GetCustomerSettings(f.CustomerId, userId);
            IList<ProductArea> pal = this._productAreaRepository.GetMany(x => x.Customer_Id == f.CustomerId).OrderBy(x => x.Name).ToList(); 
            IList<CaseSearchResult> ret = new List<CaseSearchResult>();
            var caseTypes = this.caseTypeRepository.GetCaseTypeOverviews(f.CustomerId).ToArray();

            var sql = this.ReturnCaseSearchSql(
                                        f, 
                                        customerSetting, 
                                        customerUserSetting, 
                                        userId, 
                                        userUserId, 
                                        showNotAssignedWorkingGroups, 
                                        userGroupId, 
                                        restrictedCasePermission, 
                                        gs, 
                                        s,
                                        applicationId);

            if (string.IsNullOrEmpty(sql))
            {
                return ret;
            }
            
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

                        var dr = cmd.ExecuteReader();
                        if (dr != null && dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var row = new CaseSearchResult();  
                                IList<Field> cols = new List<Field>();
                                var toolTip = string.Empty;
                                var sortOrder = string.Empty;
                                var displayLeftTime = true;
                                DateTime caseRegistrationDate;

                                DateTime.TryParse(dr["RegTime"].ToString(), out caseRegistrationDate);
                                DateTime dtTmp;
                                DateTime? caseFinishingDate = null;
                                if (DateTime.TryParse(dr["FinishingDate"].ToString(), out dtTmp))
                                {
                                    caseFinishingDate = dtTmp;
                                    displayLeftTime = false;
                                }

                                int intTmp;
                                if (int.TryParse(dr["IncludeInCaseStatistics"].ToString(), out intTmp))
                                {
                                    displayLeftTime = displayLeftTime && intTmp == 1;
                                }

                                DateTime? caseShouldBeFinishedInDate = null;
                                if (DateTime.TryParse(dr["WatchDate"].ToString(), out dtTmp))
                                {
                                    caseShouldBeFinishedInDate = dtTmp;
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
                                if (displayLeftTime)
                                {
                                    if (caseShouldBeFinishedInDate.HasValue)
                                    {
                                        //// calc time by watching date
                                        if (caseShouldBeFinishedInDate.Value > now)
                                        {
                                            timeLeft = (int)Math.Floor((decimal)workTimeCalculator.CalcWorkTimeMinutes(
                                                departmentId,
                                                now,
                                                caseShouldBeFinishedInDate.Value) / 60);
                                        }
                                        else
                                        {
                                            //// for cases that should be closed in the past
                                            timeLeft = -(int)Math.Floor((decimal)workTimeCalculator.CalcWorkTimeMinutes(
                                            departmentId,
                                            caseShouldBeFinishedInDate.Value,
                                            now) / 60);
                                        }
                                        
                                    }
                                    else if (SLAtime > 0)
                                    {
                                        //// calc by SLA value
                                        timeLeft = (int)Math.Floor((SLAtime * 60 - (decimal)workTimeCalculator.CalcWorkTimeMinutes(departmentId, caseRegistrationDate, now)) / 60);
                                    }
                                }

                                foreach (var c in csl)
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
                                                //if (c.Line == 1)
                                                //{
                                                var value = GetDatareaderValue(
                                                                                dr,
                                                                                i,
                                                                                c.Name,
                                                                                customerSetting,
                                                                                pal,
                                                                                timeLeft,
                                                                                caseTypes,
                                                                                out translateField,
                                                                                out dateValue,
                                                                                out fieldType);
                                                field = new Field
                                                            {
                                                                StringValue = value,
                                                                TranslateThis = translateField,
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
                                                //}
                                                //else
                                                //{
                                                //    toolTip += GetDatareaderValue(dr, i, c.Name, customerSetting, pal, timeLeft, caseTypes, out translateField, out dateValue, out fieldType) + Environment.NewLine;
                                                //}
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
                                row.IsUrgent = timeLeft.HasValue && timeLeft <= 0;
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

        private static string GetDatareaderValue(
                                IDataReader dr, 
                                int col, 
                                string fieldName, 
                                Setting customerSetting, 
                                IList<ProductArea> pal,
                                int? timeLeft,
                                IEnumerable<CaseTypeOverview> caseTypes,
                                out bool translateField, 
                                out DateTime? dateValue, 
                                out FieldTypes fieldType) 
        {
            var ret = string.Empty;
            var sep = " - ";
            translateField = false;

            fieldType = FieldTypes.String;
            dateValue = null;
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
                    ret = GetCaseTypeFullPath(
                                caseTypes.ToArray(),
                                int.Parse(dr[col].ToString()));
                    translateField = true;
                    break;

                case "status_id":
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
                    ProductArea p = dr.SafeGetInteger("ProductArea_Id").getProductAreaItem(pal);
                    if (p != null)
                        if (ConfigurationManager.AppSettings["InitFromSelfService"] == "true")
                            ret = p.Name;
                        else                    
                            ret = p.getProductAreaParentPath();
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
                    int userId, 
                    string userUserId, 
                    int showNotAssignedWorkingGroups, 
                    int userGroupId, 
                    int restrictedCasePermission, 
                    GlobalSetting gs, 
                    ISearch s,
                    string applicationId)
        {
            var sql = new List<string>();

            // fields
            sql.Add("select distinct");

            // vid avslutade ärenden visas bara första 500, TODO fungerar inte i Oracle 
            if (f != null && f.CaseProgress == "1")
            {
                sql.Add("top 500");
            }

            var columns = new List<string>();
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
            if (customerSetting != null)
            {
                columns.Add("coalesce(tblUsers.SurName, '') + ' ' + coalesce(tblUsers.FirstName, '') as Performer_User_Id");
                columns.Add("coalesce(tblUsers2.Surname, '') + ' ' + coalesce(tblUsers2.Firstname, '') as User_Id");
                columns.Add("coalesce(tblUsers3.Surname, '') + ' ' + coalesce(tblUsers3.Firstname, '') as CaseResponsibleUser_Id");
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
            columns.Add(string.Format("'0' as [{0}]", TimeLeftColumn));
            columns.Add("tblStateSecondary.IncludeInCaseStatistics");
            sql.Add(string.Join(",", columns));

            /// tables and joins
            var tables = new List<string>();
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

            if (customerSetting != null)
            {
                if (customerSetting.CaseWorkingGroupSource == 0)
                {
                    tables.Add("left outer join tblUsers ");
                    tables.Add(
                        "left outer join tblWorkingGroup on tblUsers.Default_WorkingGroup_Id = tblWorkingGroup.Id on tblCase.Performer_User_Id = tblUsers.Id ");
                }
                else
                {
                    tables.Add("left outer join tblUsers on tblCase.Performer_user_Id = tblUsers.Id ");
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
            sql.Add(string.Join(" ", tables));

            /// WHERE ..
            if (applicationId == "Line Manager")
            {
                sql.Add(this.ReturnCustomCaseSearchWhere(f, userUserId));
            }
            else
            {
                sql.Add(this.ReturnCaseSearchWhere(f, customerSetting, userId, userUserId, showNotAssignedWorkingGroups, userGroupId, restrictedCasePermission, gs));
            }

            // ORDER BY ...
            var orderBy = new List<string> { "order by" };
            string sort = s != null ? s.SortBy.Replace("_temporary_.", string.Empty) : string.Empty;
            if (string.IsNullOrEmpty(sort))
            {
                orderBy.Add(" CaseNumber ");
            }
            else
            {
                orderBy.Add(sort);
            }
            
            if (s != null && !s.Ascending)
            {
                orderBy.Add("desc");
            }

            sql.Add(string.Join(" ", orderBy));

            return string.Join(" ", sql);
        }

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
            //sb.Append(" and (tblCase.[RegUserId] = '" + userUserId + "')");
            switch (f.LMCaseList)
            {

                case "0":
                    sb.Append(" and (tblCase.[RegUserId] = '" + userUserId + "')");
                    break;

                //Manager Cases Only
                case "1":
                    sb.Append(" and (tblCase.[RegUserId] = '" + userUserId + "' or tblCase.[ReportedBy] = " + f.ReportedBy + ")");
                    break;

                //CoWorkers Cases Only
                case "2":
                    sb.Append(" and (tblCase.[ReportedBy] in (" + f.ReportedBy + "))");
                    break;

                //Manager & Coworkers Cases
                case "12":
                    sb.Append(" and (tblCase.[RegUserId] = '" + userUserId + "' or tblCase.[ReportedBy] in (" + f.ReportedBy + ") )");                    
                    break;
                
            }

            // ärende progress - iShow i gammal helpdesk
            switch (f.CaseProgress)
            {
                case "-1":
                    break;
                case "1":
                    sb.Append(" and (tblCase.FinishingDate is not null)");
                    break;
                case "2":
                    sb.Append(" and (tblCase.FinishingDate is null)");
                    break;
                case "3":
                    sb.Append(" and (tblCase.FinishingDate is null and tblCase.StateSecondary_Id is not null)");
                    break;
                case "4":
                    sb.Append(" and (tblCase.FinishingDate is null and tblCase.Status = 1)");
                    break;
                case "5":
                    sb.Append(" and (tblCase.FinishingDate is not null and tblCaseType.RequireApproving = 1 and tblCase.ApprovedDate is null)");
                    break;
                case "6":
                    sb.Append(" and (tblCase.FinishingDate is null and tblCase.Status > 1)");
                    break;
                case "7":
                    sb.Append(" and (tblCase.FinishingDate is null and tblCase.WatchDate is not null)");
                    break;
                case "8":
                    sb.Append(" and (tblCase.FollowUpdate is not null)");
                    break;
                case "1,2":
                    sb.Append(" ");
                    break;
                default:
                    sb.Append(" and (tblCase.FinishingDate is null)");
                    break;
            }

            //if (f.CaseRegistrationDateStartFilter.HasValue)
            //{
            //    sb.AppendFormat(" AND ([tblCase].[RegTime] >= '{0}')", f.CaseRegistrationDateStartFilter);
            //}

            //if (f.CaseRegistrationDateEndFilter.HasValue)
            //{
            //    sb.AppendFormat(" AND ([tblCase].[RegTime] <= '{0}')", f.CaseRegistrationDateEndFilter);
            //}

            //if (f.CaseWatchDateStartFilter.HasValue)
            //{
            //    sb.AppendFormat(" AND ([tblCase].[WatchDate] >= '{0}')", f.CaseWatchDateStartFilter);
            //}

            //if (f.CaseWatchDateEndFilter.HasValue)
            //{
            //    sb.AppendFormat(" AND ([tblCase].[WatchDate] <= '{0}')", f.CaseWatchDateEndFilter);
            //}

            //if (f.CaseClosingDateStartFilter.HasValue)
            //{
            //    sb.AppendFormat(" AND ([tblCase].[FinishingDate] >= '{0}')", f.CaseClosingDateStartFilter);
            //}

            //if (f.CaseClosingDateEndFilter.HasValue)
            //{
            //    sb.AppendFormat(" AND ([tblCase].[FinishingDate] <= '{0}')", f.CaseClosingDateEndFilter);
            //}

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

        private string ReturnCaseSearchWhere(CaseSearchFilter f, Setting customerSetting, int userId, string userUserId, int showNotAssignedWorkingGroups, int userGroupId, int restrictedCasePermission, GlobalSetting gs)
        {
            if (f == null || customerSetting == null || gs == null)
            {
                return string.Empty;
            }

            var sb = new StringBuilder();

            // kund 
            sb.Append(" where (tblCase.Customer_Id = " + f.CustomerId + ")");
            sb.Append(" and (tblCase.Deleted = 0)");
            sb.Append(" and (tblCustomerUser.[User_Id] = " + f.UserId + ")");

            // användaren får bara se avdelningar som den har behörighet till
            sb.Append(" and (tblCase.Department_Id In (select Department_Id from tblDepartmentUser where [User_Id] = " + userId + ")");
            sb.Append(" or not exists (select Department_Id from tblDepartmentUser where ([User_Id] = " + userId + "))");
            sb.Append(") ");

            // finns kryssruta på användaren att den bara får se sina egna ärenden
            if (restrictedCasePermission == 1)
            {
                if (userGroupId == 2)
                    sb.Append(" and (tblCase.Performer_User_Id = " + userId + " or tblcase.CaseResponsibleUser_Id = " + userId + ")");
                else if (userGroupId == 1)
                    sb.Append(" and (lower(tblCase.reportedBy) = lower(" + userUserId + ") or tblcase.UserId = " + userId + ")");
            }            
            
            // ärende progress - iShow i gammal helpdesk
            switch (f.CaseProgress)
            {
                case "-1":
                    break;
                case "1":
                    sb.Append(" and (tblCase.FinishingDate is not null)");
                    break;
                case "2":
                    sb.Append(" and (tblCase.FinishingDate is null)");
                    break;
                case "3":
                    sb.Append(" and (tblCase.FinishingDate is null and tblCase.StateSecondary_Id is not null and tblStateSecondary.IncludeInCaseStatistics = 0)");
                    break;
                case "4":
                    sb.Append(" and (tblCase.FinishingDate is null and tblCase.Status = 1)");
                    break;
                case "5":
                    sb.Append(" and (tblCase.FinishingDate is not null and tblCaseType.RequireApproving = 1 and tblCase.ApprovedDate is null)");
                    break;
                case "6":
                    sb.Append(" and (tblCase.FinishingDate is null and tblCase.Status > 1)");
                    break;
                case "7":
                    sb.Append(" and (tblCase.FinishingDate is null and tblCase.WatchDate is not null)");
                    break;
                case "8":
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
                    sb.Append(" and (tblUsers.Default_WorkingGroup_Id in (" + f.WorkingGroup.SafeForSqlInject() + ")) ");
                else
                    sb.Append(" and (coalesce(tblCase.WorkingGroup_Id, 0) in (" + f.WorkingGroup.SafeForSqlInject() + ")) ");
            }

            // http://redmine.fastdev.se/issues/10422
            if (f.CustomFilter == CasesCustomFilter.MyCases)
            {
                sb.AppendFormat(
                    " AND ([tblCase].[Performer_User_Id] IN ({0}) OR [tblCase].[CaseResponsibleUser_Id] IN ({0}) OR [tblProblem].[ResponsibleUser_Id] IN ({0})) ", 
                    userId.ToString(CultureInfo.InvariantCulture).SafeForSqlInject());
            }

            // performer/utförare
            if (!string.IsNullOrWhiteSpace(f.UserPerformer))
            {
                sb.Append(" and (tblCase.Performer_User_Id in (" + f.UserPerformer.SafeForSqlInject() + ")) ");
            }

            // ansvarig
            if (!string.IsNullOrWhiteSpace(f.UserResponsible))
                sb.Append(" and (tblCase.CaseResponsibleUser_Id in (" + f.UserResponsible.SafeForSqlInject() + "))"); 
            // land/country
            if (!string.IsNullOrWhiteSpace(f.Country))
                sb.Append(" and (tblDepartment.Country_Id in (" + f.Country.SafeForSqlInject() + "))"); 
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
                sb.Append(" and (tblCase.Department_Id in (" + f.Department.SafeForSqlInject() + "))");
            
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

        private string GetSqlLike(string field, string text)
        {
            var sb = new StringBuilder();
            if (!string.IsNullOrEmpty(field) && 
                !string.IsNullOrEmpty(text))
            {
                sb.Append(" (");
                var words = text
                        .SafeForSqlInject()
                        .ToLower()
                        .Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                for (var i = 0; i < words.Length; i++)
                {
                    sb.AppendFormat("(LOWER({0}) LIKE '%{1}%')", field, words[i].Trim());
                    if (words.Length > 1 && i < words.Length - 1)
                    {
                        sb.Append(" OR ");
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