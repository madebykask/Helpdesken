using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DH.Helpdesk.BusinessData.Enums.Case;
using DH.Helpdesk.BusinessData.Enums.Users;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.Grid;
using DH.Helpdesk.BusinessData.Models.User.Input;
using DH.Helpdesk.BusinessData.OldComponents;
using DH.Helpdesk.Common.Constants;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Common.Extensions.Integer;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Web.Common.Extensions;
using DH.Helpdesk.Web.Common.Models.CaseSearch;
using DH.Helpdesk.Web.Enums;
using DH.Helpdesk.Web.Infrastructure.CaseOverview;
using DH.Helpdesk.Web.Infrastructure.Extensions;
using DH.Helpdesk.Web.Infrastructure.Helpers;
using DH.Helpdesk.Web.Models.Case;

namespace DH.Helpdesk.Web.Infrastructure.Behaviors
{
    public class AdvancedSearchBehavior
    {
        private readonly ICaseFieldSettingService _caseFieldSettingService;
        private readonly ICaseSearchService _caseSearchService;
        private readonly IUserService _userService;
        private readonly ISettingService _settingService;
        private readonly IProductAreaService _productAreaService;
        private readonly ICustomerUserService _customerUserService;
        private readonly IGlobalSettingService _globalSettingService;
        private readonly ICaseLockService _caseLockService;

        private readonly int DefaultCaseLockBufferTime = 30; // Seconds
        private readonly int DefaultMaxRows = 10;

        public AdvancedSearchBehavior(ICaseFieldSettingService caseFieldSettingService,
            ICaseSearchService caseSearchService, 
            IUserService userService, 
            ISettingService settingService, 
            IProductAreaService productAreaService,
            ICustomerUserService customerUserService, 
            IGlobalSettingService globalSettingService,
            ICaseLockService caseLockService)
        {
            _customerUserService = customerUserService;
            _globalSettingService = globalSettingService;
            _caseLockService = caseLockService;
            _caseFieldSettingService = caseFieldSettingService;
            _caseSearchService = caseSearchService;
            _userService = userService;
            _settingService = settingService;
            _productAreaService = productAreaService;
        }
       
        public AdvancedSearchDataModel RunAdvancedSearchForCustomer(
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
            var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(currentUser.TimeZoneId);

            var currentUserId = SessionFacade.CurrentUser.Id;
            var customerId = SessionFacade.CurrentCustomer.Id;
            var customerUserSettings = _customerUserService.GetCustomerUserSettings(customerId, currentUserId);

            var normalSearchResultIds = new List<int>();

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
                        customerUserSettings.RestrictedCasePermission,
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
                    customerUserSettings.RestrictedCasePermission,
                    sm.Search,
                    0,
                    0,
                    userTimeZone,
                    ApplicationTypes.Helpdesk
                ).Items.Take(maxRecords).ToList();

            CaseRemainingTimeData remainingTimeData;
            CaseAggregateData aggregateData;
            var casesCount = _caseSearchService.Search(
                    f,
                    m.caseSettings,
                    caseFieldSettings,
                    currentUser.Id,
                    currentUser.UserId,
                    currentUser.ShowNotAssignedWorkingGroups,
                    currentUser.UserGroupId,
                    customerUserSettings.RestrictedCasePermission,
                    sm.Search,
                    0,
                    0,
                    userTimeZone,
                    ApplicationTypes.Helpdesk,
                    false,
                    out remainingTimeData,
                    out aggregateData,
                    null,
                    null,
                    null,
                    true).Count;

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
                    availableCustomerIds.AddRange(user.Cs.Where(c => c.Status ==1).Select(x => x.Id)); //todo: refactor to use correct query
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
            var globalSettings = _globalSettingService.GetGlobalSettings().FirstOrDefault();
            var ids = m.cases.Select(o => o.Id).ToArray();
            var casesLocks = _caseLockService.GetLockedCasesToOverView(ids, globalSettings, this.DefaultCaseLockBufferTime).ToList();

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

                var caseLock = casesLocks.FirstOrDefault(x => x.CaseId == searchRow.Id);
                if (caseLock != null)
                {
                    jsRow.Add("isCaseLocked", true);
                    jsRow.Add("caseLockedIconTitle",
                        $"{caseLock.User.FirstName} {caseLock.User.LastName} ({caseLock.User.UserId})");
                    jsRow.Add("caseLockedIconUrl", $"/Content/icons/{GlobalEnums.CaseIcon.Locked.CaseIconSrc()}");
                }

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
                            if (customerUserSettings.RestrictedCasePermission &&
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
                        if (searchCol.Key == "Description")
                        {
                            jsRow.Add(col.name, outputFormatter.StripHTML(searchCol.StringValue));
                        }
                        else
                        {
                            jsRow.Add(col.name, outputFormatter.FormatField(searchCol));
                        }
                    }
                    else
                    {
                        jsRow.Add(col.name, string.Empty);
                    }
                }

                data.Add(jsRow);
            }

            return new AdvancedSearchDataModel
            {
                Data = data,
                CasesCount = casesCount
            };
        }

        public CaseSearchFilter MapToCaseSearchFilter(AdvancedCaseSearchInput input)
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
                IncludeExtendedCaseValues = input.IncludeExtendedCaseValues,
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

        public CaseSearchFilter CreateSearchFilterFromRequest(FormCollection frm)
        {
            var f = new CaseSearchFilter();
            f.IsExtendedSearch = frm.IsFormValueTrue(CaseFilterFields.IsExtendedSearch);
            //f.CustomerId = customerId;//int.Parse(frm.ReturnFormValue("currentCustomerId"));
            f.IncludeExtendedCaseValues = frm.IsFormValueTrue("IncludeExtendedCaseValues");
            f.Customer = frm.ReturnFormValue("lstfilterCustomers");
            f.CaseProgress = frm.ReturnFormValue("lstFilterCaseProgress");
            f.UserPerformer = frm.ReturnFormValue("lstFilterPerformer");
            f.Initiator = frm.ReturnFormValue("CaseInitiatorFilter");

            CaseInitiatorSearchScope initiatorSearchScope;
            if (Enum.TryParse(frm.ReturnFormValue("CaseSearchFilterData.InitiatorSearchScope"), out initiatorSearchScope))
            {
                f.InitiatorSearchScope = initiatorSearchScope;
            }

            f.CaseRegistrationDateStartFilter = frm.GetDate("CaseRegistrationDateStartFilter");
            f.CaseRegistrationDateEndFilter = frm.GetDate("CaseRegistrationDateEndFilter");
            f.CaseClosingDateStartFilter = frm.GetDate("CaseClosingDateStartFilter");
            f.CaseClosingDateEndFilter = frm.GetDate("CaseClosingDateEndFilter");

            if (f.CaseRegistrationDateEndFilter != null)
                f.CaseRegistrationDateEndFilter = f.CaseRegistrationDateEndFilter.Value.AddDays(1);

            if (f.CaseClosingDateEndFilter != null)
                f.CaseClosingDateEndFilter = f.CaseClosingDateEndFilter.Value.AddDays(1);

            //Apply & save specific filters only when user has selected one customer 
            if (!string.IsNullOrEmpty(f.Customer) && !f.Customer.Contains(","))
            {
                f.WorkingGroup = frm.ReturnFormValue("lstfilterWorkingGroup");
                f.Department = frm.ReturnFormValue("lstfilterDepartment");
                f.Priority = frm.ReturnFormValue("lstfilterPriority");
                f.StateSecondary = frm.ReturnFormValue("lstfilterStateSecondary");
                f.CaseType = frm.ReturnFormValue("hid_CaseTypeDropDown").ToInt();
                f.ProductArea = f.ProductArea = frm.ReturnFormValue("hid_ProductAreaDropDown").ReturnCustomerUserValue();
                f.CaseClosingReasonFilter = frm.ReturnFormValue("hid_ClosingReasonDropDown").ReturnCustomerUserValue();


                var departmentsOrganizationUnits = frm.ReturnFormValue(CaseFilterFields.DepartmentNameAttribute);

                f.Department = GetDepartmentsFrom(departmentsOrganizationUnits);
                f.OrganizationUnit = GetOrganizationUnitsFrom(departmentsOrganizationUnits);
            }
            else
            {
                f.Department = string.Empty;
                f.WorkingGroup = string.Empty;
                f.Priority = string.Empty;
                f.StateSecondary = string.Empty;
                f.CaseType = 0;
                f.ProductArea = string.Empty;
                f.CaseClosingReasonFilter = string.Empty;
            }

            f.UserId = SessionFacade.CurrentUser.Id;

            var caseNumber = frm.ReturnFormValue("txtCaseNumberSearch");
            if (!string.IsNullOrEmpty(caseNumber))
            {
                f.FreeTextSearch = $"#{caseNumber}";
                f.CaseNumber = caseNumber;
            }
            else
            {
                if (!string.IsNullOrEmpty(frm.ReturnFormValue("txtCaptionSearch")))
                {
                    f.CaptionSearch = frm.ReturnFormValue("txtCaptionSearch");
                }
                else
                {
                    f.FreeTextSearch = frm.ReturnFormValue("txtFreeTextSearch");
                    f.SearchThruFiles = frm.IsFormValueTrue("searchThruFiles");
                }
            }

            var maxRecords = DefaultMaxRows;
            int.TryParse(frm.ReturnFormValue("lstfilterMaxRows"), out maxRecords);
            f.MaxRows = maxRecords.ToString();

            if (string.IsNullOrEmpty(f.CaseProgress))
                f.CaseProgress = CaseProgressFilter.None;

            return f;
        }

        public CaseSearchModel CreateAdvancedSearchModel(int customerId, int userId, CaseSearchFilter filter = null)
        {
            if (filter == null)
            {
                var userCustomerSettings = _userService.GetUser(userId);
                var isStartPage = userCustomerSettings.StartPage == (int) StartPage.AdvancedSearch;

                filter = new CaseSearchFilter
                {
                    CustomerId = customerId,
                    UserId = userId,
                    UserPerformer = isStartPage ? userId.ToString() : string.Empty,
                    CaseProgress = isStartPage
                        ? ((int) CaseProgressFilterEnum.CasesInProgress).ToString()
                        : string.Empty,
                    WorkingGroup = string.Empty,
                    CaseRegistrationDateStartFilter = null,
                    CaseRegistrationDateEndFilter = null,
                    CaseClosingDateStartFilter = null,
                    CaseClosingDateEndFilter = null,
                    Customer = isStartPage ? string.Empty : customerId.ToString()
                };
            }

            var s = !string.IsNullOrEmpty(filter?.SortBy)
                ? new Search
                {
                    SortBy = filter.SortBy,
                    Ascending = filter.Ascending
                }
                : new Search()
                {
                    SortBy = "CaseNumber",
                    Ascending = false
                };

            var model = new CaseSearchModel
            {
                CaseSearchFilter = filter,
                Search = s
            };

            return model;
        }

        private string GetIdsFromSearchResult(IList<CaseSearchResult> cases)
        {
            if (cases == null)
                return string.Empty;

            return string.Join(",", cases.Select(c => c.Id));
        }

        private string GetDepartmentsFrom(string departmentsOrganizationUnits)
        {
            var ret = string.Empty;
            var ids = departmentsOrganizationUnits.Split(',');
            var depIds = new List<int>();

            if (ids.Length > 0)
            {
                foreach (var id in ids)
                {
                    if (!string.IsNullOrEmpty(id.Trim()) && (int.Parse(id.Trim()) > 0 || int.Parse(id.Trim()) == int.MinValue))
                    {
                        depIds.Add(int.Parse(id));
                    }
                }
            }

            if (depIds.Any())
            {
                ret = string.Join(",", depIds);
            }

            return ret;
        }

        private string GetOrganizationUnitsFrom(string departmentsOrganizationUnits)
        {
            var ret = string.Empty;
            var ids = departmentsOrganizationUnits.Split(',');
            var ouIds = new List<int>();

            if (ids.Length > 0)
            {
                foreach (var id in ids)
                {
                    if (!string.IsNullOrEmpty(id.Trim()) && int.Parse(id.Trim()) < 0 && int.Parse(id.Trim()) != int.MinValue)
                    {
                        ouIds.Add(-int.Parse(id));
                    }
                }
            }

            if (ouIds.Any())
            {
                ret = string.Join(",", ouIds);
            }

            return ret;
        }
    }

    public class AdvancedSearchDataModel
    {
        public List<Dictionary<string, object>> Data { get; set; }
        public int CasesCount { get; set; }
    }
}