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
using DH.Helpdesk.Models.CasesOverview;

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
        public async Task<IHttpActionResult> Overview([FromBody]SearchOverviewFilterInputModel input)
        {
            var filter = new CaseSearchFilter();
            filter.CustomerId = input.CustomerId;//TODO: 0 check

            var claimsIdentity = (ClaimsIdentity)User.Identity;//TODO: Move Claims to context
            filter.UserId = UserId;//TODO: 0 check
            var userGroupIdStr = claimsIdentity.Claims.First(c => c.Type == ClaimTypes.Role).Value;
            var userGroupId = int.Parse(userGroupIdStr);//TODO: 0 check

            filter.Initiator = input.Initiator ?? string.Empty;//from params - frm.ReturnFormValue(CaseFilterFields.InitiatorNameAttribute);
            if (input.InitiatorSearchScope.HasValue) 
                filter.InitiatorSearchScope = input.InitiatorSearchScope.Value;

            filter.CaseType = input.CaseTypeId ?? 0;//from params - frm.ReturnFormValue(CaseFilterFields.CaseTypeIdNameAttribute).convertStringToInt();
            filter.ProductArea = input.ProductAreaId.HasValue ? input.ProductAreaId.Value.ToString() : string.Empty;// TODO: Check 0//from params - frm.ReturnFormValue(CaseFilterFields.ProductAreaIdNameAttribute).ReturnCustomerUserValue();
            filter.Category = input.CategoryId.HasValue ? input.CategoryId.Value.ToString() : string.Empty;//from params - frm.ReturnFormValue(CaseFilterFields.CategoryNameAttribute);
            filter.Region = input.RegionIds.Any() ? string.Join(",", input.RegionIds) : string.Empty;//from params - frm.ReturnFormValue(CaseFilterFields.RegionNameAttribute);
            filter.User = input.RegisteredByIds.Any() ? string.Join(",", input.RegisteredByIds) : string.Empty;//from params - frm.ReturnFormValue(CaseFilterFields.RegisteredByNameAttribute);
            filter.WorkingGroup = input.WorkingGroupIds.Any() ? string.Join(",", input.WorkingGroupIds) : string.Empty;// from params - frm.ReturnFormValue(CaseFilterFields.WorkingGroupNameAttribute);
            filter.UserResponsible = input.ResponsibleUserIds.Any() ? string.Join(",", input.ResponsibleUserIds) : string.Empty;//from params - frm.ReturnFormValue(CaseFilterFields.ResponsibleNameAttribute);
            filter.UserPerformer = input.PerfomerUserIds.Any() ? string.Join(",", input.PerfomerUserIds) : string.Empty;//from params - frm.ReturnFormValue(CaseFilterFields.PerformerNameAttribute);

            filter.Priority = input.PriorityIds.Any() ? string.Join(",", input.PriorityIds) : string.Empty; //from params - frm.ReturnFormValue(CaseFilterFields.PriorityNameAttribute);
            filter.Status = input.StatusIds.Any() ? string.Join(",", input.StatusIds) : string.Empty;//from params - frm.ReturnFormValue(CaseFilterFields.StatusNameAttribute);
            filter.StateSecondary = input.StateSecondaryIds.Any() ? string.Join(",", input.StateSecondaryIds) : string.Empty;//from params - frm.ReturnFormValue(CaseFilterFields.StateSecondaryNameAttribute);

            filter.CaseRegistrationDateStartFilter = input.CaseRegistrationDateStartFilter;// from params - frm.GetDate(CaseFilterFields.CaseRegistrationDateStartFilterNameAttribute);
            filter.CaseRegistrationDateEndFilter = input.CaseRegistrationDateEndFilter;// from paramsfrm.GetDate(CaseFilterFields.CaseRegistrationDateEndFilterFilterNameAttribute).GetEndOfDay();

            filter.CaseWatchDateStartFilter = input.CaseWatchDateStartFilter;//from params - frm.GetDate(CaseFilterFields.CaseWatchDateStartFilterNameAttribute);
            filter.CaseWatchDateEndFilter = input.CaseWatchDateEndFilter;// from params - frm.GetDate(CaseFilterFields.CaseWatchDateEndFilterNameAttribute).GetEndOfDay();
            filter.CaseClosingDateStartFilter = input.CaseClosingDateStartFilter;// from params - frm.GetDate(CaseFilterFields.CaseClosingDateStartFilterNameAttribute);
            filter.CaseClosingDateEndFilter = input.CaseClosingDateEndFilter;// from params - frm.GetDate(CaseFilterFields.CaseClosingDateEndFilterNameAttribute).GetEndOfDay();
            filter.CaseClosingReasonFilter = input.CaseClosingReasonId.HasValue ? input.CaseClosingReasonId.Value.ToString() : string.Empty; //TODO: Check 0 // from params - frm.ReturnFormValue(CaseFilterFields.ClosingReasonNameAttribute).ReturnCustomerUserValue();
            filter.SearchInMyCasesOnly = input.SearchInMyCasesOnly;//from params - frm.IsFormValueTrue("SearchInMyCasesOnly");

            filter.IsConnectToParent = input.IsConnectToParent;// from params - frm.IsFormValueTrue(CaseFilterFields.IsConnectToParent);
            if (filter.IsConnectToParent)
                filter.CurrentCaseId = input.CurrentCaseId;
            
            filter.CaseProgress = ((int)input.CaseProgress).ToString();// from params - frm.ReturnFormValue(CaseFilterFields.FilterCaseProgressNameAttribute);
            filter.CaseFilterFavorite = input.CaseFilterFavoriteId.HasValue ? input.CaseFilterFavoriteId.Value.ToString() : string.Empty;// from params - frm.ReturnFormValue(CaseFilterFields.CaseFilterFavoriteNameAttribute);
            filter.FreeTextSearch = input.FreeTextSearch;//TODO: remove restricted symbols here. from params - frm.ReturnFormValue(CaseFilterFields.FreeTextSearchNameAttribute);

            filter.Department = input.DepartmentIds.Any() ? string.Join(",", input.DepartmentIds) : string.Empty;// format - GetDepartmentsFrom(departments_OrganizationUnits);
            filter.OrganizationUnit = input.OrganizationUnitIds.Any() ? string.Join(",", input.OrganizationUnitIds) : string.Empty;// format - GetOrganizationUnitsFrom(departments_OrganizationUnits);

            filter.CaseRemainingTime = input.CaseRemainingTime.HasValue ? ((int)input.CaseRemainingTime.Value).ToString() : string.Empty;//from params - frm.ReturnFormValue(CaseFilterFields.CaseRemainingTimeAttribute);
            if (input.CaseRemainingTime.HasValue)//TODO: review if really required
            {
                var timeTable = GetRemainigTimeById(input.CaseRemainingTime.Value);
                if (timeTable != null)
                {
                    filter.CaseRemainingTimeFilter = timeTable.RemaningTime;
                    filter.CaseRemainingTimeUntilFilter = timeTable.RemaningTimeUntil;
                    filter.CaseRemainingTimeMaxFilter = timeTable.MaxRemaningTime;
                    filter.CaseRemainingTimeHoursFilter = timeTable.IsHour;
                }
            }
            if (input.CaseRemainingTimeFilter.HasValue)
                filter.CaseRemainingTimeFilter = input.CaseRemainingTimeFilter.Value;
            if (input.CaseRemainingTimeUntilFilter.HasValue)
                filter.CaseRemainingTimeUntilFilter = input.CaseRemainingTimeUntilFilter.Value;
            if (input.CaseRemainingTimeMaxFilter.HasValue)
                filter.CaseRemainingTimeMaxFilter = input.CaseRemainingTimeMaxFilter.Value;
            if (input.CaseRemainingTimeHoursFilter.HasValue)
                filter.CaseRemainingTimeHoursFilter = input.CaseRemainingTimeHoursFilter.Value;

            var sm = InitCaseSearchModel(filter.CustomerId, filter.UserId);
            sm.CaseSearchFilter = filter;

            //TODO: review if it required
            var caseSettings = _caseSettingService.GetCaseSettingsWithUser(filter.CustomerId, filter.UserId, userGroupId);
            var caseFieldSettings = _caseFieldSettingService.GetCaseFieldSettings(filter.CustomerId).ToArray();
            var showRemainingTime = false; //TODO: SessionFacade.CurrentUser.ShowSolutionTime;

            var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");//TODO: remove hard code TimeZoneInfo.FindSystemTimeZoneById(SessionFacade.CurrentUser.TimeZoneId)
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
                PageSize = input.PageSize ?? 10,
                PageNumber = input.Page ?? 1
            };

            CaseRemainingTimeData remainingTimeData;
            CaseAggregateData aggregateData;

            var searchResult = _caseSearchService.Search(
                filter,
                caseSettings,
                caseFieldSettings,
                filter.UserId,
                User.Identity.Name, 
                0,//TODO: Get real data SessionFacade.CurrentUser.ShowNotAssignedWorkingGroups,
                userGroupId,
                0,//TODO: Get real data SessionFacade.CurrentUser.RestrictedCasePermission,
                sm.Search,
                8,//TODO: Get real data workContext.Customer.WorkingDayStart,
                17,//TODO: Get real data workContext.Customer.WorkingDayEnd,
                userTimeZone,
                ApplicationTypes.Helpdesk,//TODO: remove hardcode
                showRemainingTime,//TODO: Get real data
                out remainingTimeData,
                out aggregateData);


            //searchResults = CommonHelper.TreeTranslate(m.cases, f.CustomerId, _productAreaService);

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
