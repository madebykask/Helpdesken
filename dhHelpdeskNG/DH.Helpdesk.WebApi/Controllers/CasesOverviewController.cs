using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using DH.Helpdesk.BusinessData.Enums.Case;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.Paging;
using DH.Helpdesk.Common.Extensions.Integer;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Web.Common.Enums.Case;
using DH.Helpdesk.Web.Common.Extensions;
using DH.Helpdesk.Web.Common.Models.Case;
using DH.Helpdesk.Web.Common.Models.CaseSearch;
using DH.Helpdesk.WebApi.Infrastructure;
using DH.Helpdesk.Common.Enums;

namespace DH.Helpdesk.WebApi.Controllers
{
    public class CasesOverviewController : BaseApiController
    {
        private readonly ICaseSearchService _caseSearchService;
        private readonly ICustomerUserService _customerUserService;
        private readonly ICaseSettingsService _caseSettingService;
        private readonly ICaseFieldSettingService _caseFieldSettingService;

        public CasesOverviewController(ICaseSearchService caseSearchService,
            ICustomerUserService customerUserService,
            ICaseSettingsService caseSettingService,
            ICaseFieldSettingService caseFieldSettingService)
        {
            _caseSearchService = caseSearchService;
            _customerUserService = customerUserService;
            _caseSettingService = caseSettingService;
            _caseFieldSettingService = caseFieldSettingService;
        }

        [HttpPost]
        public async Task<IHttpActionResult> Overview()
        {
            var filter = new CaseSearchFilter();
            filter.CustomerId = 1;//TODO: from params

            var claimsIdentity = (ClaimsIdentity)User.Identity;//TODO: Move Claims to context
            var userId = claimsIdentity.Claims.First(c => c.Type == ClaimTypes.Sid).Value;
            var userGroupId = 31;//TODO: get real data
            filter.UserId = int.Parse(userId);

            filter.Initiator = "";//TODO: from params - frm.ReturnFormValue(CaseFilterFields.InitiatorNameAttribute);
            //CaseInitiatorSearchScope initiatorSearchScope; //TODO:
            //if (Enum.TryParse(frm.ReturnFormValue(CaseFilterFields.InitiatorSearchScopeAttribute), out initiatorSearchScope))
            //{
            //    filter.InitiatorSearchScope = initiatorSearchScope;
            //}

            filter.CaseType = 0;//TODO: from params - frm.ReturnFormValue(CaseFilterFields.CaseTypeIdNameAttribute).convertStringToInt();
            filter.ProductArea = "";//TODO: from params - frm.ReturnFormValue(CaseFilterFields.ProductAreaIdNameAttribute).ReturnCustomerUserValue();
            filter.Region = "";//TODO: from params - frm.ReturnFormValue(CaseFilterFields.RegionNameAttribute);
            filter.User = "";//TODO: from params - frm.ReturnFormValue(CaseFilterFields.RegisteredByNameAttribute);
            filter.Category = "";//TODO: from params - frm.ReturnFormValue(CaseFilterFields.CategoryNameAttribute);
            filter.WorkingGroup = "";//TODO: from params - frm.ReturnFormValue(CaseFilterFields.WorkingGroupNameAttribute);
            filter.UserResponsible = "";//TODO: from params - frm.ReturnFormValue(CaseFilterFields.ResponsibleNameAttribute);
            filter.UserPerformer = "";//TODO: from params - frm.ReturnFormValue(CaseFilterFields.PerformerNameAttribute);

            filter.Priority = "";//TODO: from params - frm.ReturnFormValue(CaseFilterFields.PriorityNameAttribute);
            filter.Status = "";//TODO: from params - frm.ReturnFormValue(CaseFilterFields.StatusNameAttribute);
            filter.StateSecondary = "";//TODO: from params - frm.ReturnFormValue(CaseFilterFields.StateSecondaryNameAttribute);

            filter.CaseRegistrationDateStartFilter = null;//TODO: from params - frm.GetDate(CaseFilterFields.CaseRegistrationDateStartFilterNameAttribute);
            filter.CaseRegistrationDateEndFilter = null;//TODO: from paramsfrm.GetDate(CaseFilterFields.CaseRegistrationDateEndFilterFilterNameAttribute).GetEndOfDay();

            filter.CaseWatchDateStartFilter = null;//TODO: from params - frm.GetDate(CaseFilterFields.CaseWatchDateStartFilterNameAttribute);
            filter.CaseWatchDateEndFilter = null;//TODO: from params - frm.GetDate(CaseFilterFields.CaseWatchDateEndFilterNameAttribute).GetEndOfDay();
            filter.CaseClosingDateStartFilter = null;//TODO: from params - frm.GetDate(CaseFilterFields.CaseClosingDateStartFilterNameAttribute);
            filter.CaseClosingDateEndFilter = null;//TODO: from params - frm.GetDate(CaseFilterFields.CaseClosingDateEndFilterNameAttribute).GetEndOfDay();
            filter.CaseClosingReasonFilter = null;//TODO: from params - frm.ReturnFormValue(CaseFilterFields.ClosingReasonNameAttribute).ReturnCustomerUserValue();
            filter.SearchInMyCasesOnly = false;//TODO: from params - frm.IsFormValueTrue("SearchInMyCasesOnly");

            filter.IsConnectToParent = false;//TODO: from params - frm.IsFormValueTrue(CaseFilterFields.IsConnectToParent);
            if (filter.IsConnectToParent)
            {
                var id = "";//TODO: from params - frm.ReturnFormValue(CaseFilterFields.CurrentCaseId);
                if (!string.IsNullOrEmpty(id) && int.TryParse(id, out var currentCaseId))
                {
                    filter.CurrentCaseId = currentCaseId;
                }
            }
            
            filter.CaseProgress = "";//TODO: from params - frm.ReturnFormValue(CaseFilterFields.FilterCaseProgressNameAttribute);
            filter.CaseFilterFavorite = "";//TODO: from params - frm.ReturnFormValue(CaseFilterFields.CaseFilterFavoriteNameAttribute);
            filter.FreeTextSearch = "";//TODO: from params - frm.ReturnFormValue(CaseFilterFields.FreeTextSearchNameAttribute);

            var departments_OrganizationUnits = "";//TODO: from params - frm.ReturnFormValue(CaseFilterFields.DepartmentNameAttribute);

            filter.Department = "";//TODO: format - GetDepartmentsFrom(departments_OrganizationUnits);
            filter.OrganizationUnit = "";//TODO: format - GetOrganizationUnitsFrom(departments_OrganizationUnits);

            filter.CaseRemainingTime = "";//TODO: from params - frm.ReturnFormValue(CaseFilterFields.CaseRemainingTimeAttribute);
            if (!string.IsNullOrEmpty(filter.CaseRemainingTime))
            {
                if (int.TryParse(filter.CaseRemainingTime, out var remainingTimeId))
                {
                    var timeTable = GetRemainigTimeById((RemainingTimes)remainingTimeId);
                    if (timeTable != null)
                    {
                        filter.CaseRemainingTimeFilter = timeTable.RemaningTime;
                        filter.CaseRemainingTimeUntilFilter = timeTable.RemaningTimeUntil;
                        filter.CaseRemainingTimeMaxFilter = timeTable.MaxRemaningTime;
                        filter.CaseRemainingTimeHoursFilter = timeTable.IsHour;
                    }
                }
            }

            //int caseRemainingTimeFilter;//TODO: from params - 
            //if (int.TryParse(frm.ReturnFormValue("CaseRemainingTime"), out caseRemainingTimeFilter))
            //{
            //    f.CaseRemainingTimeFilter = caseRemainingTimeFilter;
            //}

            //int caseRemainingTimeUntilFilter;//TODO: from params - 
            //if (int.TryParse(frm.ReturnFormValue("CaseRemainingTimeUntil"), out caseRemainingTimeUntilFilter))
            //{
            //    f.CaseRemainingTimeUntilFilter = caseRemainingTimeUntilFilter;
            //}

            //int caseRemainingTimeMaxFilter;//TODO: from params - 
            //if (int.TryParse(frm.ReturnFormValue("CaseRemainingTimeMax"), out caseRemainingTimeMaxFilter))
            //{
            //    f.CaseRemainingTimeMaxFilter = caseRemainingTimeMaxFilter;
            //}

            //bool caseRemainingTimeHoursFilter;//TODO: from params - 
            //if (bool.TryParse(frm.ReturnFormValue("CaseRemainingTimeHours"), out caseRemainingTimeHoursFilter))
            //{
            //    f.CaseRemainingTimeHoursFilter = caseRemainingTimeHoursFilter;
            //}

            var sm = InitCaseSearchModel(filter.CustomerId, filter.UserId);
            sm.CaseSearchFilter = filter;

            //TODO: review if it required
            var caseSettings = _caseSettingService.GetCaseSettingsWithUser(filter.CustomerId, filter.UserId, userGroupId);
            var caseFieldSettings = _caseFieldSettingService.GetCaseFieldSettings(filter.CustomerId).ToArray();
            //var showRemainingTime = SessionFacade.CurrentUser.ShowSolutionTime;
            CaseRemainingTimeData remainingTimeData;
            CaseAggregateData aggregateData;
            var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");//TODO: remove hard code
            const int maxTextCharCount = 200;
            filter.MaxTextCharacters = maxTextCharCount;

            //Show Parent/child icons with hint on Case overview
            filter.FetchInfoAboutParentChild = true;

            //TODO: review if required
            //int recPerPage;
            //int pageStart;
            //if (int.TryParse(frm.ReturnFormValue(CaseFilterFields.PageSize), out recPerPage) && int.TryParse(frm.ReturnFormValue(CaseFilterFields.PageStart), out pageStart))
            //{
            //    f.PageInfo = new PageInfo
            //    {
            //        PageSize = recPerPage,
            //        PageNumber = recPerPage != 0 ? pageStart / recPerPage : 0
            //    };
            //    if (!f.IsConnectToParent)
            //        SessionFacade.CaseOverviewGridSettings.pageOptions.recPerPage = recPerPage;
            //}
            filter.PageInfo = new PageInfo
            {
                PageSize = 10,
                PageNumber = 1
            };

            var searchResult = _caseSearchService.Search(
                filter,
                caseSettings,
                caseFieldSettings,
                filter.UserId,
                User.Identity.Name,
                0,//TODO: Get real data
                userGroupId,
                0,//TODO: Get real data
                sm.Search,
                8,//TODO: Get real data
                17,//TODO: Get real data
                userTimeZone,
                ApplicationTypes.Helpdesk,//TODO: remove hardcode
                false,////TODO: Get real data
                out remainingTimeData,
                out aggregateData);

            //var results = _caseSearchService.Search();
            return await Task.FromResult(Json(searchResult));
        }

        private CaseRemainingTimeTable GetRemainigTimeById(RemainingTimes remainigTimeId)
        {
            var ret = new CaseRemainingTimeTable();

            switch (remainigTimeId)
            {
                case RemainingTimes.Delayed:
                    ret = new CaseRemainingTimeTable((int)remainigTimeId, null, 5, true);
                    break;

                case RemainingTimes.OneHour:
                    ret = new CaseRemainingTimeTable(1, null, 5, true);
                    break;

                case RemainingTimes.TwoHours:
                    ret = new CaseRemainingTimeTable(2, null, 5, true);
                    break;

                case RemainingTimes.FourHours:
                    ret = new CaseRemainingTimeTable(2, 4, 5, true);
                    break;

                case RemainingTimes.EightHours:
                    ret = new CaseRemainingTimeTable(4, 8, 5, true);
                    break;

                case RemainingTimes.OneDay:
                    ret = new CaseRemainingTimeTable(1, null, 5, false);
                    break;

                case RemainingTimes.TwoDays:
                    ret = new CaseRemainingTimeTable(2, null, 5, false);
                    break;

                case RemainingTimes.ThreeDays:
                    ret = new CaseRemainingTimeTable(3, null, 5, false);
                    break;

                case RemainingTimes.FourDays:
                    ret = new CaseRemainingTimeTable(4, null, 5, false);
                    break;

                case RemainingTimes.FiveDays:
                    ret = new CaseRemainingTimeTable(5, null, 5, false);
                    break;

                case RemainingTimes.MaxDays:
                    ret = new CaseRemainingTimeTable((int)remainigTimeId, null, 5, false);
                    break;
                default:
                    ret = null;
                    break;
            }

            return ret;

        }

        private CaseSearchModel InitCaseSearchModel(int customerId, int userId)
        {
            var search = new Search();
            var filter = new CaseSearchFilter();
            var cu = _customerUserService.GetCustomerUserSettings(customerId, userId);
            if (cu == null)
            {
                throw new Exception($"Customers settings is empty or not valid for customer id {customerId}");
            }

            filter.CustomerId = customerId;
            filter.UserId = userId;
            filter.CaseType = cu.CaseCaseTypeFilter.ReturnCustomerUserValue().ConvertStringToInt();
            filter.Category = cu.CaseCategoryFilter.ReturnCustomerUserValue();
            filter.Priority = cu.CasePriorityFilter.ReturnCustomerUserValue();
            filter.ProductArea = cu.CaseProductAreaFilter.ReturnCustomerUserValue();
            filter.Region = cu.CaseRegionFilter.ReturnCustomerUserValue();
            filter.Department = cu.CaseDepartmentFilter.ReturnCustomerUserValue();
            filter.StateSecondary = cu.CaseStateSecondaryFilter.ReturnCustomerUserValue();
            filter.Status = cu.CaseStatusFilter.ReturnCustomerUserValue();
            filter.User = cu.CaseUserFilter.ReturnCustomerUserValue();
            filter.UserPerformer = cu.CasePerformerFilter.ReturnCustomerUserValue();
            filter.UserResponsible = cu.CaseResponsibleFilter.ReturnCustomerUserValue();
            filter.WorkingGroup = cu.CaseWorkingGroupFilter.ReturnCustomerUserValue();
            filter.CaseProgress = CaseSearchFilter.InProgressCases;
            filter.CaseRegistrationDateStartFilter = cu.CaseRegistrationDateStartFilter;
            filter.CaseRegistrationDateEndFilter = cu.CaseRegistrationDateEndFilter;
            filter.CaseWatchDateStartFilter = cu.CaseWatchDateStartFilter;
            filter.CaseWatchDateEndFilter = cu.CaseWatchDateEndFilter;
            filter.CaseClosingDateStartFilter = cu.CaseClosingDateStartFilter;
            filter.CaseClosingDateEndFilter = cu.CaseClosingDateEndFilter;
            filter.CaseClosingReasonFilter = cu.CaseClosingReasonFilter.ReturnCustomerUserValue();
            filter.CaseRemainingTime = cu.CaseRemainingTimeFilter.ReturnCustomerUserValue();
            filter.PageInfo = new PageInfo();
            //ResolveParentPathesForFilter(filter);//TODO: Review, it seems that this logic should be on UI

            search.SortBy = "CaseNumber";
            search.Ascending = true;

            return new CaseSearchModel { CaseSearchFilter = filter, Search = search };
        }
    }
}
