using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using DH.Helpdesk.BusinessData.Enums.Case;
using DH.Helpdesk.BusinessData.Enums.Case.Fields;
using DH.Helpdesk.BusinessData.Models.Case.CaseIntLog;
using DH.Helpdesk.BusinessData.Models.Case.Output;
using DH.Helpdesk.BusinessData.Models.Email;
using DH.Helpdesk.BusinessData.Models.ProductArea;
using DH.Helpdesk.BusinessData.Models.Shared.Output;
using DH.Helpdesk.BusinessData.Models.WorktimeCalculator;
using DH.Helpdesk.BusinessData.OldComponents.DH.Helpdesk.BusinessData.Utils;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Common.Enums.Cases;

namespace DH.Helpdesk.Services.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using DH.Helpdesk.BusinessData.Models.Case;
	using DH.Helpdesk.BusinessData.Models.Case.CaseSearch;
	using DH.Helpdesk.BusinessData.Models.Grid;
	using DH.Helpdesk.BusinessData.OldComponents;
	using DH.Helpdesk.Common.Tools;
	using DH.Helpdesk.Dal.Repositories;
	using DH.Helpdesk.Domain;
	using DH.Helpdesk.Services.Utils;

	using ProductAreaEntity = DH.Helpdesk.Domain.ProductArea;
	using BusinessLogic.Admin.Users;
	using BusinessData.Enums.Admin.Users;
	using BusinessLogic.Mappers.Users;

	public interface ICaseSearchService
    {
        SearchResult<CaseSearchResult> Search(
            CaseSearchFilter f,
            IList<CaseSettings> caseSettings,
            CaseFieldSetting[] customerCaseFieldsSettings,
            int userId,
            string userUserId,
            int showNotAssignedWorkingGroups,
            int userGroupId,
            bool restrictedCasePermission,
            ISearch s,
            int workingDayStart,
            int workingDayEnd,
            TimeZoneInfo userTimeZone,
            string applicationType);

        SearchResult<CaseSearchResult> Search(
            CaseSearchFilter f,
            IList<CaseSettings> caseSettings,
            CaseFieldSetting[] customerCaseFieldsSettings,
            int userId,
            string userUserId,
            int showNotAssignedWorkingGroups,
            int userGroupId,
            bool restrictedCasePermission,// TODO: review parameter, seems that it is not used any more
            ISearch s,
            int workingDayStart,
            int workingDayEnd,
            TimeZoneInfo userTimeZone,
            string applicationType,
            bool calculateRemainingTime,
            out CaseRemainingTimeData remainingTime,
            out CaseAggregateData aggregateData,
            int? relatedCasesCaseId = null,
            string relatedCasesUserId = null,
            int[] caseIds = null,
            bool countOnly = false);

		CustomerUserCase[] SearchActiveCustomerUserCases(bool myCases, int currentUserId, int? customerId, string freeText, int from, int count, string orderby = null, bool orderAscending = true);
	}

    public class CaseSearchService : ICaseSearchService
    {
        private const string TimeLeftColumn = "_temporary_LeadTime";
        private const string TimeLeftColumnLower = "_temporary_leadtime";

        private readonly ICaseSearchRepository _caseSearchRepository;
		private readonly ICaseRepository _caseRepository;
		private readonly IProductAreaRepository _productAreaRepository;
        private readonly ICaseTypeRepository _caseTypeRepository;
        private readonly ILogRepository _logRepository;
        private readonly IProductAreaService _productAreaService;
        private readonly IGlobalSettingService _globalSettingService;
        private readonly ISettingService _settingService;
        private readonly IHolidayService _holidayService;
        private readonly ICustomerService _customerService;
		private readonly IUserPermissionsChecker _userPermissionsChecker;
		private readonly IUserService _userService;

		#region ctor()

		public CaseSearchService(
            ICaseSearchRepository caseSearchRepository,
			ICaseRepository caseRepository,
            IProductAreaRepository productAreaRepository,
            ICaseTypeRepository caseTypeRepository,
            ILogRepository logRepository,
            IProductAreaService productAreaService, 
            IGlobalSettingService globalSettingService, 
            ISettingService settingService,
            IHolidayService holidayService,
            ICustomerService customerService, 
			IUserPermissionsChecker userPermissionsChecker,
			IUserService userService)
		{
            _caseSearchRepository = caseSearchRepository;
			_caseRepository = caseRepository;
			_productAreaRepository = productAreaRepository;
            _caseTypeRepository = caseTypeRepository;
            _logRepository = logRepository;
            _globalSettingService = globalSettingService;
            _settingService = settingService;
            _holidayService = holidayService;
            _productAreaService = productAreaService;
            _customerService = customerService;
			_userPermissionsChecker = userPermissionsChecker;
			_userService = userService;

		}

        #endregion
      

        #region Search Methods

        public SearchResult<CaseSearchResult> Search(
            CaseSearchFilter f,
            IList<CaseSettings> caseSettings,
            CaseFieldSetting[] customerCaseFieldsSettings,
            int userId,
            string userUserId,
            int showNotAssignedWorkingGroups,
            int userGroupId,
            bool restrictedCasePermission,
            ISearch s,
            int workingDayStart,
            int workingDayEnd,
            TimeZoneInfo userTimeZone,
            string applicationType)
        {
            CaseRemainingTimeData remainingTime;
            CaseAggregateData aggregateData;

            return this.Search(
                f,
                caseSettings,
                customerCaseFieldsSettings,
                userId,
                userUserId,
                showNotAssignedWorkingGroups,
                userGroupId,
                restrictedCasePermission,
                s,
                workingDayStart,
                workingDayEnd,
                userTimeZone,
                applicationType,
                false,
                out remainingTime,
                out aggregateData);
        }

        public SearchResult<CaseSearchResult> Search(
            CaseSearchFilter f, 
            IList<CaseSettings> caseSettings, 
            CaseFieldSetting[] customerCaseFieldsSettings,
            int userId, 
            string userUserId, 
            int showNotAssignedWorkingGroups, 
            int userGroupId, 
            bool restrictedCasePermission, 
            ISearch s,
            int workingDayStart,
            int workingDayEnd,
            TimeZoneInfo userTimeZone,
            string applicationType,
            bool calculateRemainingTime,
            out CaseRemainingTimeData remainingTime,
            out CaseAggregateData aggregateData,
            int? relatedCasesCaseId = null,
            string relatedCasesUserId = null,
            int[] caseIds = null, 
            bool countOnly = false)
        {
            var now = DateTime.UtcNow;

            var csf = DoFilterValidation(f);

            var customer = _customerService.GetCustomer(f.CustomerId);
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(customer.TimeZoneId);

            var workTimeFactory = new WorkTimeCalculatorFactory(_holidayService, workingDayStart, workingDayEnd, timeZone);
            var responisbleFieldSettings = customerCaseFieldsSettings.Where(it => it.Name == GlobalEnums.TranslationCaseFields.CaseResponsibleUser_Id.ToString()).FirstOrDefault();
            var isFieldResponsibleVisible = responisbleFieldSettings != null && responisbleFieldSettings.ShowOnStartPage == 1;

			#region Prepare SearchContext

			bool hasInternalLogAccess;
			if (userId > 0)
			{
				var user = _userService.GetUserOverview(userId);
				hasInternalLogAccess = _userPermissionsChecker.UserHasPermission(UsersMapper.MapToUser(user), UserPermission.CaseInternalLogPermission);
			}
			else
			{
				hasInternalLogAccess = false;
			}


			var context = new CaseSearchContext
            {
                f = csf,
                userCaseSettings = caseSettings,
                customerSetting = _settingService.GetCustomerSetting(f.CustomerId),
                globalSettings = _globalSettingService.GetGlobalSettings().FirstOrDefault(),
                
                //use customerCaseFieldSettings
                isFieldResponsibleVisible = isFieldResponsibleVisible,
                userId = userId,
                userUserId = userUserId,
                WorkingHours = workingDayEnd - workingDayStart,
                showNotAssignedWorkingGroups = showNotAssignedWorkingGroups,
                userGroupId = userGroupId,
                restrictedCasePermission = restrictedCasePermission,
                s = s,
                workTimeCalcFactory = workTimeFactory,
                applicationType = applicationType,
                calculateRemainingTime = calculateRemainingTime,
                productAreaNamesResolver = this._productAreaService,
                relatedCasesCaseId = relatedCasesCaseId,
                relatedCasesUserId = relatedCasesUserId,
                caseIds = caseIds,
                now = now,
                useFullTextSearch = _globalSettingService.GetGlobalSettings().First().FullTextSearch != 0,
				hasAccessToInternalLogNotes = hasInternalLogAccess,
                includeExtendedCaseValues = f.IncludeExtendedCaseValues
			};

            #endregion

            SearchResult<CaseSearchResult> result;
            if (countOnly)
            {
                remainingTime = null;
                aggregateData = null;
                var rowsCount = _caseSearchRepository.SearchCount(context);
                
                // set only rows count 
                result = new SearchResult<CaseSearchResult>
                {
                    Count = rowsCount
                };
            }
            else
            {
                //run search
                var searchResults = _caseSearchRepository.Search(context);

                //calc work time    
                var workTimeCalculator = InitCalcFromSQL(searchResults, context.workTimeCalcFactory, now);

                //process results per customer settings 
                result = ProcessSearchResults(context, searchResults, workTimeCalculator, out remainingTime, out aggregateData);

                result = SortSearchResult(result, s);
                if (f.ToBeMerged)
                {
                    //- What to show:
                    //- Not myself and Not Merge Children
                    result.Items = result.Items.Where(x => x.IsMergeChild != true && x.Id != f.CurrentCaseId).ToList();
                }

                //TODO: refactor when true server paging will be implemented
                result.Count = result.Items.Count;
                if (f.PageInfo != null && f.PageInfo.PageSize > 0)
                {
                    result.Items = result.Items.Skip(f.PageInfo.PageNumber * f.PageInfo.PageSize).Take(f.PageInfo.PageSize).ToList();
                }
            }
            

            return result;
        }

        #region Process SearchResults

        private SearchResult<CaseSearchResult> ProcessSearchResults(
            CaseSearchContext context,
            DataTable searchData,
            WorkTimeCalculator workTimeCalculator,
            out CaseRemainingTimeData remainingTime,
            out CaseAggregateData aggregateData)
        {
            // shortcuts for context fields
            var now = context.now;
            var searchFilter = context.f;
            var userCaseSettings = context.userCaseSettings;
            var calculateRemainingTime = context.calculateRemainingTime;
            var customerSetting = context.customerSetting;
            var productAreaNamesResolver = context.productAreaNamesResolver;
            var searchInfo = context.s;
            var secSortOrder = string.Empty;
            int workingHours = context.WorkingHours;

            var fieldNames = userCaseSettings.Select(c => c.Name).ToList();
            if (context.applicationType.ToLower() == ApplicationTypes.HelpdeskMobile)
            {
                fieldNames = GetMobileCaseOverviewFieldNames();
            }

            var productAreas =
                _productAreaRepository.GetMany(x => x.Customer_Id == searchFilter.CustomerId).OrderBy(x => x.Name).ToList();

            var caseTypes = _caseTypeRepository.GetCaseTypeOverviews(searchFilter.CustomerId).ToArray();
            var displayLeftTime = fieldNames.Any(n => n == TimeLeftColumn);

            var searchResult = new SearchResult<CaseSearchResult>();
            remainingTime = new CaseRemainingTimeData();
            aggregateData = new CaseAggregateData();

            using (var dr = searchData.CreateDataReader())
            {
                if (dr != null && dr.HasRows)
                {
                    int? closingReasonFilter = null;
                    if (!string.IsNullOrEmpty(searchFilter.CaseClosingReasonFilter))
                    {
                        int closingReason;
                        if (int.TryParse(searchFilter.CaseClosingReasonFilter, out closingReason))
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
                                    workTime = workTimeCalculator.CalculateWorkTime(now, watchDateDue, departmentId);
                                }
                                else
                                {
                                    //// for cases that should be closed in the past
                                    // #52951 timeOnPause shouldn't calculate when watchdate has value
                                    workTime = -workTimeCalculator.CalculateWorkTime(watchDateDue, now, departmentId);
                                }

                                timeLeft = workTime / 60;
                                var floatingPoint = workTime % 60;
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
                                timeLeft = (SLAtime * 60 - calcTime + timeOnPause) / 60;
                                var floatingPoint = (SLAtime * 60 - calcTime + timeOnPause) % 60;
                                secSortOrder = floatingPoint.ToString();

                                if (timeLeft <= 0 && floatingPoint < 0)
                                    timeLeft--;
                            }
                        }

                        if (!CheckRemainingTimeFilter(timeLeft, workingHours, searchFilter))
                            continue;

                        if (timeLeft.HasValue)
                        {
                            remainingTime.AddRemainingTime(caseId, timeLeft.Value);
                        }

                        var isExternalEnv = (context.applicationType.ToLower() == ApplicationTypes.LineManager ||
                                             context.applicationType.ToLower() == ApplicationTypes.SelfService);
                        row.Ignored = false;

                        foreach (var fieldName in fieldNames)
                        {
                            Field field = null;
                            for (var i = 0; i < dr.FieldCount; i++)
                            {
                                if (dr.IsDBNull(i))
                                    continue;

                                if (string.Equals(dr.GetName(i), GetCaseFieldName(fieldName), StringComparison.OrdinalIgnoreCase))
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
                                            fieldName,
                                            customerSetting,
                                            productAreas,
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
                                            Key = fieldName,
                                            Id = baseId,
                                            StringValue = value,
                                            TranslateThis = translateField,
                                            TreeTranslation = treeTranslation,
                                            DateTimeValue = dateValue,
                                            FieldType = fieldType
                                        };

                                        if (string.Equals(searchInfo.SortBy, fieldName, StringComparison.OrdinalIgnoreCase))
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
                        row.SecSortOrder = secSortOrder;
                        row.Tooltip = toolTip;
                        row.CaseIcon = GetCaseIcon(dr);
                        row.Id = dr.SafeGetInteger("Id");
                        row.Columns = cols;
                        row.IsUnread = dr.SafeGetInteger("Status") == 1;
                        row.IsUrgent = timeLeft.HasValue && timeLeft < 0;
                        row.IsClosed = caseFinishingDate.HasValue;
                        row.IsParent = (context.f.FetchInfoAboutParentChild && dr.SafeGetInteger("IsParent") > 0);
                        row.ParentId = context.f.FetchInfoAboutParentChild ? dr.SafeGetInteger("ParentCaseId") : 0;
                        row.IsMergeParent = (context.f.FetchInfoAboutParentChild && dr.SafeGetInteger("IsMergeParent") > 0);
                        row.IsMergeChild = (context.f.FetchInfoAboutParentChild && dr.SafeGetInteger("IsMergeChild") > 0);
                        row.CaseCaption = dr.SafeGetString("Caption");

                        row.ExtendedSearchInfo = new ExtendedSearchInfo
                        {
                            CustomerId = dr.SafeGetInteger("CaseCustomerId"),
                            DepartmentId = dr.SafeGetInteger("Department_Id"),
                            WorkingGroupId = dr.SafeGetInteger("CaseWorkingGroupId"),
                            Performer_User_Id = dr.SafeGetInteger("CasePerformerUserId"),
                            CaseResponsibleUser_Id = dr.SafeGetInteger("CaseResponsibleUserId"),
                            ReportedBy = dr.SafeGetString("ReportedBy"),
                            User_Id = dr.SafeGetInteger("CaseUserId")
                        };

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

            return searchResult;
        }

        private static List<string> GetMobileCaseOverviewFieldNames()
        {
            return new List<string>
            {
                CaseInfoFields.Case,
                CaseInfoFields.ChangeDate,
                CaseInfoFields.Caption,
                UserFields.Department,
                UserFields.Notifier,
                OtherFields.Priority,
                //OtherFields.State,
                OtherFields.SubState,
                OtherFields.Administrator,
                OtherFields.WorkingGroup,
                OtherFields.WatchDate,
                TimeLeftColumn,
                UserFields.Customer
            };
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
                        ret = GetCaseTypeFullPathOnExternal(caseTypes.ToArray(), int.Parse(dr[col].ToString()), out cOut);
                        preventToAddRecord = !cOut;
                    }
                    else
                    {
                        ret = GetCaseTypeFullPath(caseTypes.ToArray(), int.Parse(dr[col].ToString()));
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

        #endregion

        #region InitCalcFromSQL

        /// <summary>
        /// Builds work time calculator based on cases that can be fetched using supplied SQL
        /// </summary>
        /// <param name="searchData"></param>
        /// <param name="calculatorFactory"></param>
        /// <param name="utcNow"></param>
        /// <returns></returns>
        private WorkTimeCalculator InitCalcFromSQL(DataTable searchData, IWorkTimeCalculatorFactory calculatorFactory, DateTime utcNow)
        {
            DateTime fetchRangeBegin = utcNow;
            DateTime fetchRangeEnd = utcNow;
            var deptIds = new HashSet<int>();

            using (var dr = searchData.CreateDataReader())
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

        #endregion

        #region SortSearchResult

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


		public CustomerUserCase[] SearchActiveCustomerUserCases(bool myCases, int currentUserId, int? customerId, string freeText, int from, int count, string orderby = null, bool orderbyAscending = true)
		{
			// Internal log access
			var user = _userService.GetUser(currentUserId);
			var searchInternalLog = _userPermissionsChecker.UserHasPermission(user, UserPermission.CaseInternalLogPermission);

			var query = _caseRepository.GetActiveCustomerUserCases(myCases, currentUserId, customerId, freeText, from, count, orderby, orderbyAscending, searchInternalLog);

			return query.ToArray();
		}


		#endregion

		#region Helpder Methods

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

        private static string GetCaseTypeFullPath(CaseTypeOverview[] caseTypes, int caseTypeId)
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

        private static string GetCaseTypeFullPathOnExternal(CaseTypeOverview[] caseTypes, int caseTypeId, out bool checkShowOnExternal)
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

        private CaseSearchFilter DoFilterValidation(CaseSearchFilter filter)
        {
            var filterValidate = filter.Copy(filter);


            //Applied in FreeTextSearchSafeForSQLInject
            //if (!string.IsNullOrEmpty(filterValidate.FreeTextSearch))
            //    filterValidate.FreeTextSearch = filterValidate.FreeTextSearch.Replace("'", "''");

            //if (!string.IsNullOrEmpty(filterValidate.CaptionSearch))
            //    filterValidate.CaptionSearch = filterValidate.CaptionSearch.Replace("'", "''");

            // ärenden som tillhör barn till föräldrer ska visas om vi filtrerar på föräldern
            int productAreaId;
            if (int.TryParse(filter.ProductArea, out productAreaId))
            {
                filterValidate.ProductArea = this._productAreaService.GetProductAreaWithChildren(productAreaId, ", ", "Id");
            }
            return filterValidate;
        }

        #endregion

        #endregion
    }
}