using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.SessionState;
using DH.Helpdesk.BusinessData.Enums.Case;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.Grid;
using DH.Helpdesk.BusinessData.Models.User.Input;
using DH.Helpdesk.Common.Constants;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Common.Extensions.Integer;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Services.Services.Grid;
using DH.Helpdesk.Web.Common.Extensions;
using DH.Helpdesk.Web.Common.Models.CaseSearch;
using DH.Helpdesk.Web.Infrastructure;
using DH.Helpdesk.Web.Infrastructure.CaseOverview;
using DH.Helpdesk.Web.Infrastructure.Extensions;
using DH.Helpdesk.Web.Infrastructure.Grid;
using DH.Helpdesk.Web.Infrastructure.Grid.Output;
using DH.Helpdesk.Web.Infrastructure.Helpers;
using DH.Helpdesk.Web.Models.Case;

namespace DH.Helpdesk.Web.Controllers
{
    //NOTE: its important to keep search logic in this separate controller with Session readonly attribute to allow ajax requests running in parallel!
    // if session is not readonly requests will be blocked in queue by session lock and executed one by one...
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class AdvancedSearchController : Controller
    {
        private readonly ICaseFieldSettingService _caseFieldSettingService;
        private readonly ICaseSearchService _caseSearchService;
        private readonly IUserService _userService;
        private readonly ISettingService _settingService;
        private readonly IProductAreaService _productAreaService;
        private readonly GridSettingsService _gridSettingsService; 

        public AdvancedSearchController(
            ICaseFieldSettingService caseFieldSettingService, 
            ICaseSearchService caseSearchService,
            ISettingService settingService,
            IProductAreaService productAreaService,
            ICaseSettingsService caseSettingsService,
            IUserService userService, 
            GridSettingsService gridSettingsService)
        {
            _settingService = settingService;
            _userService = userService;
            _gridSettingsService = gridSettingsService;
            _productAreaService = productAreaService;
            _caseSearchService = caseSearchService;
            _caseFieldSettingService = caseFieldSettingService;
        }

        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Search(AdvancedCaseSearchInput inputModel)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.GetErrorsText();
                //todo: return js error
                return CreateErrorResult($"Invalid state. {Environment.NewLine}{errors}"); 
            }

            var customerId = inputModel.CustomerId;
            var searchFilter = MapToCaseSearchFilter(inputModel);

            var gridSortingOptions = new GridSortOptions()
            {
                sortBy = inputModel.SortBy,
                sortDir = (SortingDirection)inputModel.SortDir
            };

            var gridSettings =
                CreateGridSettingsModel(customerId, SessionFacade.CurrentUser, gridSortingOptions);

            //TODO: check if its required
            //SessionFacade.AdvancedSearchOverviewGridSettings = gridSettings;

            var extendedCustomers = _settingService.GetExtendedSearchIncludedCustomers();
            
            var sr = RunAdvancedSearchForCustomer(searchFilter,
                gridSettings,
                customerId,
                customerId,
                SessionFacade.CurrentUser,
                extendedCustomers);

            var data = new
            {
                searchResults = sr.Item1, //search results
                gridSettings = sr.Item2 // grid settings
            };
            return Json(new { result = "success", data });
        }

        #region Shared Methods - Move to share behavior!

        private Tuple<List<Dictionary<string, object>>, JsonGridSettingsModel> RunAdvancedSearchForCustomer(
            CaseSearchFilter f,
            GridSettingsModel gridSettings,
            int searchCustomerId,
            int currentCustomerId,
            UserOverview currentUser,
            IList<int> extendedCustomers = null)
        {
            #region Code from old method. TODO: code review wanted

            var sm = CreateAdvancedSearchModel(searchCustomerId, f.UserId, f);
            sm.Search.SortBy = gridSettings.sortOptions.sortBy;
            sm.Search.Ascending = gridSettings.sortOptions.sortDir == SortingDirection.Asc;

            var m = new CaseSearchResultModel()
            {
                caseSettings = gridSettings.columnDefs?.Select(colDef => new CaseSettings()
                {
                    Id = colDef.id,
                    Name = colDef.name //field
                }).ToList() ?? new List<CaseSettings>()
            };

            var caseFieldSettings = _caseFieldSettingService.GetCaseFieldSettings(f.CustomerId).ToArray();
            f.MaxTextCharacters = 0;

            var maxRecords = f.MaxRows.ToInt();
            var normalSearchResultIds = new List<int>();

            var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(currentUser.TimeZoneId);

            if (f.IsExtendedSearch && extendedCustomers != null && !extendedCustomers.Contains(searchCustomerId))
            {
                f.IsExtendedSearch = false;

                //RUN Extended customers search
                normalSearchResultIds =
                    _caseSearchService.Search(
                        f,
                        m.caseSettings,
                        caseFieldSettings,
                        currentUser.Id,
                        currentUser.UserId,
                        currentUser.ShowNotAssignedWorkingGroups,
                        currentUser.UserGroupId,
                        currentUser.RestrictedCasePermission,
                        sm.Search,
                        0,
                        0,
                        userTimeZone,
                        ApplicationTypes.Helpdesk
                    ).Items.Select(x => x.Id).ToList();

                f.IsExtendedSearch = true;
            }

            //RUN normal customers search
            m.cases =
                _caseSearchService.Search(
                    f,
                    m.caseSettings,
                    caseFieldSettings,
                    currentUser.Id,
                    currentUser.UserId,
                    currentUser.ShowNotAssignedWorkingGroups,
                    currentUser.UserGroupId,
                    currentUser.RestrictedCasePermission,
                    sm.Search,
                    0,
                    0,
                    userTimeZone,
                    ApplicationTypes.Helpdesk
                ).Items.Take(maxRecords).ToList();


            m.cases = CommonHelper.TreeTranslate(m.cases, currentCustomerId, _productAreaService);
            sm.Search.IdsForLastSearch = GetIdsFromSearchResult(m.cases);

            if (!string.IsNullOrWhiteSpace(sm.CaseSearchFilter.FreeTextSearch))
            {
                if (sm.CaseSearchFilter.FreeTextSearch[0] == CaseSearchConstants.CaseNumberSearchPrefix)
                    sm.CaseSearchFilter.FreeTextSearch = string.Empty;
            }

            if (!string.IsNullOrEmpty(f.OrganizationUnit))
            {
                var ouIds = f.OrganizationUnit.Split(',');
                if (ouIds.Any())
                {
                    foreach (var id in ouIds)
                    {
                        if (!string.IsNullOrEmpty(id))
                        {
                            if (string.IsNullOrEmpty(sm.CaseSearchFilter.Department))
                                sm.CaseSearchFilter.Department += $"-{id}";
                            else
                                sm.CaseSearchFilter.Department += $",-{id}";
                        }
                    }
                }
            }

            SessionFacade.CurrentAdvancedSearch = sm;

            #endregion

            var availableDepIds = new List<int>();
            var accessToAllDepartments = false;
            var availableWgIds = new List<int>();
            var availableCustomerIds = new List<int> { 0 };

            if (f.IsExtendedSearch)
            {
                var user = _userService.GetUser(currentUser.Id);
                if (user != null)
                {
                    availableCustomerIds.AddRange(user.Cs.Select(x => x.Id)); //todo: refactor to use correct query
                    availableDepIds.AddRange(user.Departments.Where(x => x.Customer_Id == searchCustomerId).Select(x => x.Id));
                    availableWgIds.AddRange(user.UserWorkingGroups.Select(x => x.WorkingGroup_Id));

                    //Department, If 0 selected you should have access to all departments
                    if (!availableDepIds.Any())
                    {
                        availableDepIds.Add(0);
                        accessToAllDepartments = true;
                    }

                    //ShowNotAssignedWorkingGroups
                    if (currentUser.ShowNotAssignedWorkingGroups == 1)
                    {
                        availableWgIds.Add(0);
                    }
                }
            }

            var data = new List<Dictionary<string, object>>();
            var customerSettings = _settingService.GetCustomerSetting(currentCustomerId);
            var outputFormatter = new OutputFormatter(customerSettings.IsUserFirstLastNameRepresentation == 1, userTimeZone);

            foreach (var searchRow in m.cases)
            {
                var jsRow = new Dictionary<string, object>
                {
                    { "case_id", searchRow.Id },
                    { "caseIconTitle", searchRow.CaseIcon.CaseIconTitle() },
                    { "caseIconUrl", $"/Content/icons/{searchRow.CaseIcon.CaseIconSrc()}" },
                    { "isUnread", searchRow.IsUnread },
                    { "isUrgent", searchRow.IsUrgent },
                    { "isClosed", searchRow.IsClosed},
                };

                if (f.IsExtendedSearch)
                {
                    var infoAvailableInExtended = false;
                    if (normalSearchResultIds.Contains(searchRow.Id))
                    {
                        infoAvailableInExtended = true;
                    }
                    else
                    {
                        if (currentUser.UserGroupId == UserGroups.User || currentUser.UserGroupId == UserGroups.Administrator)
                        {
                            // finns kryssruta pa anvandaren att den bara far se sina egna arenden
                            //Note, this is also checked in where clause  in ReturnCaseSearchWhere(SearchQueryBuildContext ctx)
                            //Check for access Department
                            //Check for access WorkingGroups
                            if (currentUser.RestrictedCasePermission == 1 &&
                                (availableDepIds.Contains(searchRow.ExtendedSearchInfo.DepartmentId) || accessToAllDepartments) &&
                                availableWgIds.Contains(searchRow.ExtendedSearchInfo.WorkingGroupId))
                            {
                                //Use functionality from VerifyCase
                                infoAvailableInExtended = _userService.VerifyUserCasePermissions(SessionFacade.CurrentUser, searchRow.Id);
                            }

                            if (infoAvailableInExtended && availableDepIds.Contains(searchRow.ExtendedSearchInfo.DepartmentId) &&
                                availableWgIds.Contains(searchRow.ExtendedSearchInfo.WorkingGroupId) &&
                                availableCustomerIds.Contains(searchRow.ExtendedSearchInfo.CustomerId))
                            {
                                infoAvailableInExtended = true;
                            }
                        }
                        else
                        {
                            if (availableCustomerIds.Contains(searchRow.ExtendedSearchInfo.CustomerId))
                            {
                                infoAvailableInExtended = true;
                            }
                        }

                    }
                    jsRow.Add("ExtendedAvailable", infoAvailableInExtended);
                }
                else
                {
                    jsRow.Add("ExtendedAvailable", true);
                }

                foreach (var col in gridSettings.columnDefs)
                {
                    var searchCol = searchRow.Columns.FirstOrDefault(it => it.Key == col.name);
                    if (searchCol != null)
                    {
                        jsRow.Add(col.name, outputFormatter.FormatField(searchCol));
                    }
                    else
                    {
                        jsRow.Add(col.name, string.Empty);
                    }
                }

                data.Add(jsRow);
            }

            var jsonGridSettings = 
                JsonGridSettingsMapper.ToJsonGridSettingsModel(
                    gridSettings, // todo: check if SessionFacade.CaseOverviewGridSettings should be used
                    searchCustomerId, 
                    1/*availableColCount?*/, 
                    CaseColumnsSettingsModel.PageSizes.Select(x => x.Value).ToArray());

            return new Tuple<List<Dictionary<string, object>>, JsonGridSettingsModel>(data, jsonGridSettings);
        }

        private GridSettingsModel CreateGridSettingsModel(int searchCustomerId, UserOverview currentUser, GridSortOptions sortOptions)
        {
            var gridSettings =
                _gridSettingsService.GetForCustomerUserGrid(
                    searchCustomerId,
                    currentUser.UserGroupId, 
                    currentUser.Id,
                    GridSettingsService.CASE_ADVANCED_SEARCH_GRID_ID);

            if (sortOptions != null)
                gridSettings.sortOptions = sortOptions;

            return gridSettings;
        }

        private CaseSearchModel CreateAdvancedSearchModel(int customerId, int userId, CaseSearchFilter filter = null)
        {
            var f = filter ?? new CaseSearchFilter
            {
                CustomerId = customerId,
                UserId = userId,
                UserPerformer = string.Empty,
                CaseProgress = string.Empty,
                WorkingGroup = string.Empty,
                CaseRegistrationDateStartFilter = null,
                CaseRegistrationDateEndFilter = null,
                CaseClosingDateStartFilter = null,
                CaseClosingDateEndFilter = null,
                Customer = customerId.ToString()
            };

            var s = new Search
            {
                SortBy = "CaseNumber",
                Ascending = false
            };

            return new CaseSearchModel
            {
                CaseSearchFilter = f,
                Search = s
            };
        }

        private string GetIdsFromSearchResult(IList<CaseSearchResult> cases)
        {
            if (cases == null)
                return string.Empty;

            return string.Join(",", cases.Select(c => c.Id));
        }

        private CaseSearchFilter MapToCaseSearchFilter(AdvancedCaseSearchInput input)
        {
            var caseNumber = input.CaseNumber;
            var freeTextSearch = input.FreeTextSearch;
            var captionSearch = input.CaptionSearch;

            //todo: check
            //var maxRecords = DefaultMaxRows; //todo: check value is passed to page model

            var f = new CaseSearchFilter()
            {
                IsExtendedSearch = input.IsExtendedSearch, //also run for 
                CustomerId = input.CustomerId,
                Customer = input.Customers,
                CaseProgress = input.CaseProgress,
                UserPerformer = input.UserPerformer,
                Initiator = input.Initiator,
                InitiatorSearchScope = input.InitiatorSearchScope,
                CaseRegistrationDateStartFilter = input.CaseRegistrationDateStartFilter,
                CaseRegistrationDateEndFilter = input.CaseRegistrationDateEndFilter,
                CaseClosingDateStartFilter = input.CaseClosingDateStartFilter,
                CaseClosingDateEndFilter = input.CaseClosingDateEndFilter,
                UserId = SessionFacade.CurrentUser.Id,
                WorkingGroup = string.Empty,
                Department = string.Empty,
                Priority = string.Empty,
                StateSecondary = string.Empty,
                CaseType = 0,
                ProductArea = string.Empty,
                CaseClosingReasonFilter = string.Empty,

                //tood: check if correct value is passed from client
                MaxRows = input.MaxRows.ToString(),
            };

            if (!string.IsNullOrEmpty(caseNumber))
            {
                f.FreeTextSearch = $"#{caseNumber}";
                f.CaseNumber = caseNumber;
            }
            else
            {
                if (!string.IsNullOrEmpty(captionSearch))
                {
                    f.CaptionSearch = captionSearch;
                }
                else
                {
                    f.FreeTextSearch = freeTextSearch;
                    f.SearchThruFiles = input.SearchThruFiles;
                }
            }

            if (string.IsNullOrEmpty(f.CaseProgress))
                f.CaseProgress = CaseProgressFilter.None;

            if (f.CaseRegistrationDateEndFilter != null)
                f.CaseRegistrationDateEndFilter = f.CaseRegistrationDateEndFilter.Value.AddDays(1);

            if (f.CaseClosingDateEndFilter != null)
                f.CaseClosingDateEndFilter = f.CaseClosingDateEndFilter.Value.AddDays(1);

            //Apply & save specific filters only when user has selected one customer 
            if (!string.IsNullOrEmpty(f.Customer) && !f.Customer.Contains(","))
            {
                f.WorkingGroup = input.WorkingGroup;
                f.Priority = input.Priority;
                f.StateSecondary = input.StateSecondary;
                f.CaseType = input.CaseType ?? 0;
                f.ProductArea = input.ProductArea.ReturnCustomerUserValue();
                f.CaseClosingReasonFilter = input.CaseClosingReasonFilter.ReturnCustomerUserValue();

                //todo: review
                var departmentsOrganizationUnits = input.Department;
                if (!string.IsNullOrEmpty(departmentsOrganizationUnits))
                {
                    f.Department = GetDepartmentsFrom(departmentsOrganizationUnits);
                    f.OrganizationUnit = GetOrganizationUnitsFrom(departmentsOrganizationUnits);
                }
            }

            return f;
        }

        private string GetDepartmentsFrom(string departments_OrganizationUnits)
        {
            string ret = string.Empty;
            var ids = departments_OrganizationUnits.Split(',');
            var depIds = new List<int>();
            if (ids.Length > 0)
            {
                for (int i = 0; i < ids.Length; i++)
                    if (!string.IsNullOrEmpty(ids[i].Trim()) && int.Parse(ids[i].Trim()) > 0)
                        depIds.Add(int.Parse(ids[i]));
            }
            if (depIds.Any())
                ret = string.Join(",", depIds);
            return ret;
        }

        private string GetOrganizationUnitsFrom(string departments_OrganizationUnits)
        {
            string ret = string.Empty;
            var ids = departments_OrganizationUnits.Split(',');
            var ouIds = new List<int>();
            if (ids.Length > 0)
            {
                for (int i = 0; i < ids.Length; i++)
                    if (!string.IsNullOrEmpty(ids[i].Trim()) && int.Parse(ids[i].Trim()) < 0)
                        ouIds.Add(-int.Parse(ids[i]));
            }
            if (ouIds.Any())
                ret = string.Join(",", ouIds);
            return ret;
        }
        
        #endregion

        #region Helper Methods

        private PartialViewResult CreateErrorResult(string err)
        {
            ControllerContext.HttpContext.Response.TrySkipIisCustomErrors = true;
            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return PartialView("~/Views/Cases/AdvancedSearch/_SearchError.cshtml", err);
        }

        #endregion
    }
}