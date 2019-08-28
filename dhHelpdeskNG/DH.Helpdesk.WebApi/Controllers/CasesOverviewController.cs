using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using DH.Helpdesk.BusinessData.Enums.Case;
using DH.Helpdesk.BusinessData.Enums.Case.Fields;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.Paging;
using DH.Helpdesk.BusinessData.Models.Shared;
using DH.Helpdesk.BusinessData.Models.Shared.Output;
using DH.Helpdesk.BusinessData.OldComponents;
using DH.Helpdesk.Common.Extensions.Integer;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Web.Common.Enums.Case;
using DH.Helpdesk.Web.Common.Extensions;
using DH.Helpdesk.Web.Common.Models.Case;
using DH.Helpdesk.Web.Common.Models.CaseSearch;
using DH.Helpdesk.WebApi.Infrastructure;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Common.Extensions.Lists;
using DH.Helpdesk.Models.CasesOverview;
using DH.Helpdesk.WebApi.Infrastructure.Authentication;
using DH.Helpdesk.WebApi.Logic.Case;
using DH.Helpdesk.WebApi.Models.Output;

namespace DH.Helpdesk.WebApi.Controllers
{
    [RoutePrefix("api/CasesOverview")]
    public class CasesOverviewController : BaseApiController
    {
        private readonly ICaseSearchService _caseSearchService;
        private readonly ICustomerUserService _customerUserService;
        private readonly ICaseSettingsService _caseSettingService;
        private readonly ICaseFieldSettingService _caseFieldSettingService;
        private readonly IUserService _userSerivice;
        private readonly ICustomerService _customerService;
        private readonly ICaseService _caseService;
        private readonly ICaseTranslationService _caseTranslationService;

        public CasesOverviewController(ICaseSearchService caseSearchService,
            ICaseService caseService,
            ICustomerUserService customerUserService,
            ICaseSettingsService caseSettingService,
            ICaseFieldSettingService caseFieldSettingService,
            ICaseTranslationService caseTranslationService,
            IUserService userSerivice,
            ICustomerService customerService)
        {
            _caseTranslationService = caseTranslationService;
            _caseService = caseService;
            _caseSearchService = caseSearchService;
            _customerUserService = customerUserService;
            _caseSettingService = caseSettingService;
            _caseFieldSettingService = caseFieldSettingService;
            _userSerivice = userSerivice;
            _customerService = customerService;
        }

        [HttpGet]
        [Route("sortingfields")]
        public IList<CaseSortFieldModel> GetSortingFields([FromUri]int langId, [FromUri]int cid)
        {
            var sortingFields = new List<GlobalEnums.TranslationCaseFields>()
            {
                GlobalEnums.TranslationCaseFields.CaseNumber,
                GlobalEnums.TranslationCaseFields.ChangeTime,
                GlobalEnums.TranslationCaseFields.Performer_User_Id,
                GlobalEnums.TranslationCaseFields.WorkingGroup_Id,
                GlobalEnums.TranslationCaseFields.Priority_Id,
                GlobalEnums.TranslationCaseFields.StateSecondary_Id,
                GlobalEnums.TranslationCaseFields.WatchDate,
                GlobalEnums.TranslationCaseFields._temporary_LeadTime // Time Left - virtual field
            };

            var res =
                sortingFields.Select(f => new CaseSortFieldModel(_caseTranslationService.GetCaseTranslation(f.ToString(), langId, cid), f.ToString()))
                .OrderBy(f => f.Text)
                .ToList();

            return res;
        }

        [HttpGet]
        [Route("favoritefilters")]
        public async Task<List<CaseFavoriteFilterModel>> FavoriteFilters(int cid)
        {
            var favorites = await _caseService.GetMyFavoritesWithFieldsAsync(cid, UserId);

            //#71782: skip filters with ClosingReason and Closing date since mobile app supports only cases in progress only
            favorites = 
                favorites.Where(f => f.Fields != null && 
                                     (f.Fields.ClosingReasonFilter == null || !f.Fields.ClosingReasonFilter.Any()) && 
                                     (f.Fields.ClosingDateFilter == null || !f.Fields.ClosingDateFilter.HasValues)).ToList();

            var model = favorites.Any()
                ? favorites.Select(f => new CaseFavoriteFilterModel()
                {
                    Id = f.Id,
                    Name = f.Name,
                    Fields = f.Fields.ToDictionary().Select(kv => new ItemOverview(kv.Key, kv.Value)).ToList()
                }).OrderBy(f => f.Name).ToList()
                : null;

            return model;
        }

        /// <summary>
        /// List of filtered cases.
        /// Contains data only for case overview.
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<SearchResult<CaseSearchResult>> Search([FromUri]int cid, [FromBody]SearchOverviewFilterInputModel input)
        {
            var userGroupId = User.Identity.GetGroupId();
            var userOverview = await _userSerivice.GetUserOverviewAsync(UserId);
            var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(userOverview.TimeZoneId);
            var customerSettings = await _customerService.GetCustomerAsync(cid);

            var filter = CreateSearchFilter(input, cid);

            var currentUserId = filter.UserId;
            var customerId = filter.CustomerId;

            //var sm = await InitCaseSearchModel(filter.CustomerId, filter.UserId, filter);
            var sm = new CaseSearchModel()
            {
                CaseSearchFilter = filter,
                Search = new Search()
                {
                    SortBy = GlobalEnums.TranslationCaseFields.ChangeTime.ToString(),
                    Ascending = false
                }
            };

            if (!string.IsNullOrWhiteSpace(input.OrderBy))
                sm.Search.SortBy = input.OrderBy;

            if (input.Ascending.HasValue)
                sm.Search.Ascending = input.Ascending.Value;

            // TODO: review if it required
            var caseSettings = _caseSettingService.GetCaseSettingsWithUser(filter.CustomerId, filter.UserId, userGroupId);
            AddMissingCaseSettingsForMobile(caseSettings); //TODO: Temporary  - remove after mobile case settings is implemented

            var caseFieldSettings = await _caseFieldSettingService.GetCaseFieldSettingsAsync(filter.CustomerId);
            var customerUserSettings = await _customerUserService.GetCustomerUserSettingsAsync(customerId, currentUserId);

            CaseRemainingTimeData remainingTimeData;
            CaseAggregateData aggregateData;

            SearchResult<CaseSearchResult> searchResult = null;

            if (input.CountOnly)
            {
                // run count only query 
                searchResult = _caseSearchService.Search(
                    filter,
                    caseSettings,
                    caseFieldSettings.ToArray(),
                    filter.UserId,
                    UserName,
                    userOverview.ShowNotAssignedWorkingGroups,
                    userGroupId,
                    customerUserSettings.RestrictedCasePermission,
                    sm.Search,
                    customerSettings.WorkingDayStart,
                    customerSettings.WorkingDayEnd,
                    userTimeZone,
                    ApplicationTypes.HelpdeskMobile,
                    userOverview.ShowSolutionTime,
                    out remainingTimeData,
                    out aggregateData,
                    null,
                    null,
                    null,
                    countOnly: true);
            }
            else
            {
                // run normal search request
                searchResult = _caseSearchService.Search(
                    filter,
                    caseSettings,
                    caseFieldSettings.ToArray(),
                    filter.UserId,
                    UserName,
                    userOverview.ShowNotAssignedWorkingGroups,
                    userGroupId,
                    customerUserSettings.RestrictedCasePermission,
                    sm.Search,
                    customerSettings.WorkingDayStart,
                    customerSettings.WorkingDayEnd,
                    userTimeZone,
                    ApplicationTypes.HelpdeskMobile,
                    userOverview.ShowSolutionTime,
                    out remainingTimeData,
                    out aggregateData);
            }

            //searchResults = CommonHelper.TreeTranslate(m.cases, f.CustomerId, _productAreaService);

            //var results = _caseSearchService.Search();
            return searchResult;
        }

        private CaseSearchFilter CreateSearchFilter(SearchOverviewFilterInputModel input, int customerId)
        {
            const int maxTextCharCount = 200;

            var filter = new CaseSearchFilter
            {
                CustomerId = customerId,
                UserId = UserId,
                Initiator = input.Initiator ?? string.Empty, //from params - frm.ReturnFormValue(CaseFilterFields.InitiatorNameAttribute);
                InitiatorSearchScope = input.InitiatorSearchScope ?? CaseInitiatorSearchScope.UserAndIsAbout,
                CaseType = input.CaseTypeId ?? 0,
                ProductArea = input.ProductAreaId?.ToString() ?? string.Empty,
                Category = input.CategoryId?.ToString() ?? string.Empty,
                Region = input.RegionIds.JoinToString() ?? string.Empty,
                User = input.RegisteredByIds.JoinToString() ?? string.Empty,
                WorkingGroup = input.WorkingGroupIds.JoinToString() ?? string.Empty,
                UserResponsible = input.ResponsibleUserIds.JoinToString() ?? string.Empty,
                UserPerformer = input.PerfomerUserIds.JoinToString() ?? string.Empty,
                Priority = input.PriorityIds.JoinToString() ?? string.Empty,
                Status = input.StatusIds.JoinToString() ?? string.Empty,
                StateSecondary = input.StateSecondaryIds.JoinToString() ?? string.Empty,
                CaseRegistrationDateStartFilter = input.CaseRegistrationDateStartFilter,
                CaseRegistrationDateEndFilter = input.CaseRegistrationDateEndFilter,
                CaseWatchDateStartFilter = input.CaseWatchDateStartFilter,
                CaseWatchDateEndFilter = input.CaseWatchDateEndFilter,
                CaseClosingDateStartFilter = input.CaseClosingDateStartFilter,
                CaseClosingDateEndFilter = input.CaseClosingDateEndFilter,
                CaseClosingReasonFilter = input.CaseClosingReasonId?.ToString() ?? string.Empty,
                SearchInMyCasesOnly = input.SearchInMyCasesOnly,
                IsConnectToParent = input.IsConnectToParent,
                CurrentCaseId = input.IsConnectToParent ? input.CurrentCaseId : null,

                CaseProgress = ((int)input.CaseProgress).ToString(), // from params - frm.ReturnFormValue(CaseFilterFields.FilterCaseProgressNameAttribute);
                CaseFilterFavorite = input.CaseFilterFavoriteId?.ToString() ?? string.Empty, // from params - frm.ReturnFormValue(CaseFilterFields.CaseFilterFavoriteNameAttribute);
                FreeTextSearch = input.FreeTextSearch, //TODO: remove restricted symbols here. from params - frm.ReturnFormValue(CaseFilterFields.FreeTextSearchNameAttribute);

                Department = GetDepartmentsFrom(input.DepartmentIds), 
                OrganizationUnit = GetOrganizationUnitsFrom(input.DepartmentIds),

                MaxTextCharacters = maxTextCharCount,

                //Show Parent/child icons with hint on Case overview
                FetchInfoAboutParentChild = true,

                PageInfo = new PageInfo
                {
                    PageSize = input.PageSize ?? 10,
                    PageNumber = input.Page ?? 1
                }
            };

            #region Set Case remaining time filter

            filter.CaseRemainingTime = input.CaseRemainingTime.HasValue ? ((int)input.CaseRemainingTime.Value).ToString() : string.Empty;//from params - frm.ReturnFormValue(CaseFilterFields.CaseRemainingTimeAttribute);
            if (input.CaseRemainingTime.HasValue) //TODO: review if really required
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

            #endregion

            // TODO: review if required
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

            //from params - frm.ReturnFormValue(CaseFilterFields.CaseTypeIdNameAttribute).convertStringToInt();
            // TODO: Check 0//from params - frm.ReturnFormValue(CaseFilterFields.ProductAreaIdNameAttribute).ReturnCustomerUserValue();
            //from params - frm.ReturnFormValue(CaseFilterFields.CategoryNameAttribute);
            //from params - frm.ReturnFormValue(CaseFilterFields.RegionNameAttribute);
            //from params - frm.ReturnFormValue(CaseFilterFields.RegisteredByNameAttribute);
            // from params - frm.ReturnFormValue(CaseFilterFields.WorkingGroupNameAttribute);
            //from params - frm.ReturnFormValue(CaseFilterFields.ResponsibleNameAttribute);
            //from params - frm.ReturnFormValue(CaseFilterFields.PerformerNameAttribute);

            //from params - frm.ReturnFormValue(CaseFilterFields.PriorityNameAttribute);
            //from params - frm.ReturnFormValue(CaseFilterFields.StatusNameAttribute);
            //from params - frm.ReturnFormValue(CaseFilterFields.StateSecondaryNameAttribute);

            // from params - frm.GetDate(CaseFilterFields.CaseRegistrationDateStartFilterNameAttribute);
            // from paramsfrm.GetDate(CaseFilterFields.CaseRegistrationDateEndFilterFilterNameAttribute).GetEndOfDay();

            //from params - frm.GetDate(CaseFilterFields.CaseWatchDateStartFilterNameAttribute);
            // from params - frm.GetDate(CaseFilterFields.CaseWatchDateEndFilterNameAttribute).GetEndOfDay();
            // from params - frm.GetDate(CaseFilterFields.CaseClosingDateStartFilterNameAttribute);
            // from params - frm.GetDate(CaseFilterFields.CaseClosingDateEndFilterNameAttribute).GetEndOfDay();
            //TODO: Check 0 // from params - frm.ReturnFormValue(CaseFilterFields.ClosingReasonNameAttribute).ReturnCustomerUserValue();
            //from params - frm.IsFormValueTrue("SearchInMyCasesOnly");

            // from params - frm.IsFormValueTrue(CaseFilterFields.IsConnectToParent);

            return filter;
        }

        //1,-2,-3
        private string GetDepartmentsFrom(IList<int> ids)
        {
            var ret = string.Empty;
            if (ids == null)
                return ret;

            var depIds = ids.Where(x => x > 0).ToList();
            if (depIds.Any())
                ret = string.Join(",", depIds);
            return ret;
        }

        private string GetOrganizationUnitsFrom(IList<int> ids)
        {
            var ret = string.Empty;
            if (ids == null)
                return ret;

            var ouIds = ids.Where(x => x < 0).Select(x => -1 * x).ToList();
            if (ouIds.Any())
                ret = string.Join(",", ouIds);
            return ret;
        }

        /// <summary>
        /// This is a temporary methhod. Adds required fields to Mobile Case Overview page. Remove after Case Mobile settings mage added
        /// </summary>
        /// <param name="caseSettings"></param>
        private void AddMissingCaseSettingsForMobile(IList<CaseSettings> caseSettings)
        {
            if (caseSettings.All(cs => cs.Name != CaseInfoFields.Case))
            {
                caseSettings.Add(new CaseSettings
                {
                    Name = CaseInfoFields.Case,
                });
            }
            if (caseSettings.All(cs => cs.Name != CaseInfoFields.RegistrationDate))
            {
                caseSettings.Add(new CaseSettings
                {
                    Name = CaseInfoFields.RegistrationDate,
                });
            }
            if (caseSettings.All(cs => cs.Name != CaseInfoFields.Caption))
            {
                caseSettings.Add(new CaseSettings
                {
                    Name = CaseInfoFields.Caption,
                });
            }
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

        //TODO: check if required?
        private async Task<CaseSearchModel> InitCaseSearchModel(int customerId, int userId)
        {
            var search = new Search();
            var filter = new CaseSearchFilter();

            var cu = await _customerUserService.GetCustomerUserSettingsAsync(customerId, userId);
            if (cu == null)
                throw new Exception($"Customers settings is empty or not valid for customer id {customerId}");
            
            filter.CustomerId = customerId;
            filter.UserId = userId;
            filter.CaseType = cu.CaseCaseTypeFilter.ReturnCustomerUserValue().ToInt();
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
