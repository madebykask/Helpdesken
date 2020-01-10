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
using DH.Helpdesk.Common.Enums.Cases;

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
        private readonly ICaseTranslationService _caseTranslationService;
		private readonly ICaseService _caseService;

		public CasesOverviewController(ICaseSearchService caseSearchService,
            ICustomerUserService customerUserService,
            ICaseSettingsService caseSettingService,
            ICaseFieldSettingService caseFieldSettingService,
            ICaseTranslationService caseTranslationService,
            IUserService userSerivice,
            ICustomerService customerService,
			ICaseService caseService)
        {
            _caseTranslationService = caseTranslationService;
            _caseSearchService = caseSearchService;
            _customerUserService = customerUserService;
            _caseSettingService = caseSettingService;
            _caseFieldSettingService = caseFieldSettingService;
            _userSerivice = userSerivice;
            _customerService = customerService;
			_caseService = caseService;
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

        /// <summary>
        /// List of filtered cases.
        /// Contains data only for case overview.
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<SearchResult<CaseSearchResult>> Search([FromBody]SearchOverviewFilterInputModel input, int? cid = null)
        {
			SearchResult<CaseSearchResult> searchResult = null;
			if (input.CustomersIds.Count == 1) // TODO: fix? 
			{
				var userGroupId = User.Identity.GetGroupId();
				var userOverview = await _userSerivice.GetUserOverviewAsync(UserId);
				var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(userOverview.TimeZoneId);
				var filter = CreateSearchFilter(input, cid.Value);

				var customerSettings = await _customerService.GetCustomerAsync(filter.CustomerId);

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


				var caseSettings = _caseSettingService.GetCaseSettingsWithUser(filter.CustomerId, filter.UserId, userGroupId);
				AddMissingCaseSettingsForMobile(caseSettings); //TODO: Temporary  - remove after mobile case settings is implemented

				var caseFieldSettings = await _caseFieldSettingService.GetCaseFieldSettingsAsync(filter.CustomerId);
				var customerUserSettings = await _customerUserService.GetCustomerUserSettingsAsync(customerId, currentUserId);

				CaseRemainingTimeData remainingTimeData;
				CaseAggregateData aggregateData;



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
			}
			else
			{
				var userId = UserId;
				var result = _caseSearchService.SearchActiveCustomerUserCases(input.SearchInMyCasesOnly, UserId, null, input.FreeTextSearch, ((input.PageSize??0) * (input.Page ?? 0)), (input.PageSize??10), input.OrderBy, input.Ascending.HasValue ? input.Ascending.Value : true);
				searchResult = new SearchResult<CaseSearchResult>();
				foreach (var r in result)
				{
					var sr = new CaseSearchResult();
					sr.Id = r.Id;
					sr.CaseIcon = r.CaseIcon;
					sr.SortOrder = input.OrderBy;
					sr.IsUnread = r.Unread;
					sr.Columns = new List<Field>();
					sr.Columns.Add(new Field { Key = "CaseNumber", StringValue = r.CaseNumber.ToString(), DateTimeValue = null, FieldType = FieldTypes.String });
					sr.Columns.Add(new Field { Key = "ChangeTime", StringValue = r.ChangedDate.ToString("yyyy-MM-dd"), DateTimeValue = r.ChangedDate, FieldType = FieldTypes.Date });
					sr.Columns.Add(new Field { Key = "Caption", StringValue = r.Subject, FieldType = FieldTypes.String });
					sr.Columns.Add(new Field { Key = "StateSecondary_Id", StringValue = r.StateSecondaryName, FieldType = FieldTypes.String });
					sr.Columns.Add(new Field { Key = "WatchDate", StringValue = r.WatchDate.HasValue ? r.WatchDate.Value.ToString("yyyy-MM-dd") : "", DateTimeValue = r.WatchDate, FieldType = FieldTypes.Date });

					if (!string.IsNullOrEmpty(r.PriorityName))
						sr.Columns.Add(new Field { Key = "Priority_Id", StringValue = r.PriorityName, FieldType = FieldTypes.String, TranslateThis = true });
					if (!string.IsNullOrEmpty(r.WorkingGroupName))
						sr.Columns.Add(new Field { Key = "WorkingGroup_Id", StringValue = r.WorkingGroupName, FieldType = FieldTypes.String });
					if (!string.IsNullOrEmpty(r.PerformerName))
						sr.Columns.Add(new Field { Key = "Performer_User_Id", StringValue = r.PerformerName, FieldType = FieldTypes.String });
					sr.Columns.Add(new Field { Key = "CustomerName", StringValue = r.CustomerName, FieldType = FieldTypes.String });

					sr.Columns.Add(new Field { Key = "_temporary_LeadTime", StringValue = "" /*TODO: ?"*/, FieldType = FieldTypes.NullableHours });

					searchResult.Items.Add(sr);
				}
			}
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

            if (input.CustomersIds != null && input.CustomersIds.Count == 1)
                filter.CustomerId = input.CustomersIds.Single();

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
