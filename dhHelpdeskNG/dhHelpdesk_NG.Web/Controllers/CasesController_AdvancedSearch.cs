using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DH.Helpdesk.BusinessData.Enums.Case;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.Grid;
using DH.Helpdesk.BusinessData.Models.Shared;
using DH.Helpdesk.BusinessData.OldComponents;
using DH.Helpdesk.BusinessData.OldComponents.DH.Helpdesk.BusinessData.Utils;
using DH.Helpdesk.Common.Constants;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Common.Extensions.Integer;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Web.Common.Extensions;
using DH.Helpdesk.Web.Common.Models.CaseSearch;
using DH.Helpdesk.Web.Enums;
using DH.Helpdesk.Web.Infrastructure;
using DH.Helpdesk.Web.Infrastructure.CaseOverview;
using DH.Helpdesk.Web.Infrastructure.Extensions;
using DH.Helpdesk.Web.Infrastructure.Grid;
using DH.Helpdesk.Web.Infrastructure.Helpers;
using DH.Helpdesk.Web.Models.Case;
using DH.Helpdesk.Web.Models.Case.Output;

namespace DH.Helpdesk.Web.Controllers
{
    public partial class CasesController
    {

        public ActionResult AdvancedSearch(bool? clearFilters = false, bool doSearchAtBegining = false,
            bool isExtSearch = false)
        {
            if (SessionFacade.CurrentUser == null)
            {
                return new RedirectResult("~/Error/Unathorized");
            }

            ApplicationFacade.UpdateLoggedInUser(Session.SessionID, string.Empty);

            if (SessionFacade.CurrentCustomer == null)
            {
                return new RedirectResult("~/Error/Unathorized");
            }

            var currentCustomerId = SessionFacade.CurrentCustomer.Id;
            var currentUserId = SessionFacade.CurrentUser.Id;

            var customers = this._userService.GetUserProfileCustomersSettings(SessionFacade.CurrentUser.Id);
            var m = new AdvancedSearchIndexViewModel();
            var availableCustomers = customers.Select(c => new ItemOverview(c.CustomerName, c.CustomerId.ToString()))
                .OrderBy(c => c.Name).ToList();

            m.SelectedCustomers = availableCustomers;

            var extendIncludedCustomerIds = _settingService.GetExtendedSearchIncludedCustomers();
            var extCustomers = _customerService.GetAllCustomers().Where(x => extendIncludedCustomerIds.Contains(x.Id))
                .Select(c => new ItemOverview(c.Name, c.Id.ToString())).OrderBy(c => c.Name).ToList();
            m.ExtendIncludedCustomers = extCustomers;

            CaseSearchModel advancedSearchModel;
            if ((clearFilters != null && clearFilters.Value)
                || SessionFacade.CurrentAdvancedSearch == null)
            {
                SessionFacade.CurrentAdvancedSearch = null;
                advancedSearchModel = this.InitAdvancedSearchModel(currentCustomerId, currentUserId);
                SessionFacade.CurrentAdvancedSearch = advancedSearchModel;
            }
            else
                advancedSearchModel = SessionFacade.CurrentAdvancedSearch;

            m.CaseSearchFilterData = CreateAdvancedSearchFilterData(
                currentCustomerId,
                currentUserId,
                advancedSearchModel,
                availableCustomers);

            m.SpecificSearchFilterData = CreateAdvancedSearchSpecificFilterData(currentUserId);

            m.CaseSetting = GetCaseSettingModel(currentCustomerId, currentUserId);
            m.GridSettings = JsonGridSettingsMapper.GetAdvancedSearchGridSettingsModel(currentCustomerId);

            if (advancedSearchModel.Search != null)
                m.GridSettings.sortOptions = new GridSortOptions
                {
                    sortBy = advancedSearchModel.Search.SortBy,
                    sortDir = advancedSearchModel.Search.Ascending ? SortingDirection.Asc : SortingDirection.Desc
                };

            m.CaseSearchFilterData.IsAboutEnabled =
                m.CaseSetting.ColumnSettingModel.CaseFieldSettings.GetIsAboutEnabled();

            m.DoSearchAtBegining = doSearchAtBegining;
            m.IsExtSearch = isExtSearch;
            return this.View("AdvancedSearch/Index", m);
        }

        [HttpGet]
        public string LookupLanguage(string customerid, string notifier, string region, string department,
            string notifierid)
        {
            //return string.Empty;

            customerid = customerid.Replace("'", "");
            notifier = notifier.Replace("'", "");
            region = region.Replace("'", "");
            department = department.Replace("'", "");
            notifierid = notifierid.Replace("'", "");

            if ((notifier != string.Empty && notifierid != string.Empty) | customerid != string.Empty |
                region != string.Empty | department != string.Empty)
            {
                int depid = 0;
                int regid = 0;

                int custid = 0;

                if (customerid != string.Empty)
                {
                    custid = Convert.ToInt32(customerid);
                }

                if (region != string.Empty)
                {
                    regid = Convert.ToInt32(region);
                }

                if (department != string.Empty)
                {
                    depid = Convert.ToInt32(department);
                }

                int languageId = this._caseService.LookupLanguage(custid, notifier, regid, depid, notifierid);
                return languageId.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        [HttpGet]
        public PartialViewResult GetCustomerSpecificFilter(int selectedCustomerId, bool resetFilter = false)
        {
            if (SessionFacade.CurrentUser == null || SessionFacade.CurrentCustomer == null)
            {
                return PartialView("AdvancedSearch/_SpecificSearchTab", null);
            }

            CaseSearchModel csm = null;

            if (!resetFilter)
                selectedCustomerId = 0;

            var model = CreateAdvancedSearchSpecificFilterData(SessionFacade.CurrentUser.Id, selectedCustomerId);
            return PartialView("AdvancedSearch/_SpecificSearchTab", model);
        }

        [ValidateInput(false)]
        public ActionResult DoAdvancedSearch(FormCollection frm)
        {
            if (SessionFacade.CurrentUser == null || SessionFacade.CurrentCustomer == null)
            {
                return new RedirectResult("~/Error/Unathorized");
            }

            var customers = frm.ReturnFormValue("currentCustomerId").Split(',').Select(x => Int32.Parse(x)).ToList();
            var isExtendedSearch = frm.IsFormValueTrue(CaseFilterFields.IsExtendedSearch);
            var res = new List<Tuple<List<Dictionary<string, object>>, GridSettingsModel>>();
            var extendedCustomers = new List<int>();

            if (isExtendedSearch)
            {
                extendedCustomers = _settingService.GetExtendedSearchIncludedCustomers();
                foreach (var includedCustomer in extendedCustomers)
                {
                    res.Add(AdvancedSearchForCustomer(frm, includedCustomer, isExtendedSearch, extendedCustomers));
                }
            }

            foreach (var customer in customers.Except(extendedCustomers))
            {
                res.Add(AdvancedSearchForCustomer(frm, customer, false, extendedCustomers));
            }

            var totalCount = res.Sum(x => x.Item1.Count);
            var data = res.Select(x => new { data = x.Item1, gridSettings = x.Item2 }).ToList();
            var ret = new { Items = data, TotalCount = totalCount };

            return this.Json(new { result = "success", data = ret });
        }

        [ValidateInput(false)]
        public ActionResult QuickOpen(FormCollection frm)
        {
            if (SessionFacade.CurrentUser == null || SessionFacade.CurrentCustomer == null)
            {
                return new RedirectResult("~/Error/Unathorized");
            }

            string searchFor = frm.ReturnFormValue("txtQuickOpen");

            string notFoundText = Translation.GetCoreTextTranslation("Inget ärende tillgängligt");

            int caseId = _caseService.GetCaseQuickOpen(SessionFacade.CurrentUser, searchFor);

            if (caseId > 0)
            {

                if (_userService.VerifyUserCasePermissions(SessionFacade.CurrentUser, caseId))
                {
                    return this.Json(new { result = "success", data = "/Cases/Edit/" + caseId });
                }
                else
                {
                    notFoundText = Translation.GetCoreTextTranslation("Åtkomst nekad");
                }
            }

            return this.Json(new { result = "error", data = notFoundText });
        }

        #region private

        private CaseSearchFilterData CreateAdvancedSearchFilterData(int cusId, int userId, CaseSearchModel sm, List<ItemOverview> customers)
        {
            var fd = new CaseSearchFilterData();

            fd.caseSearchFilter = sm.CaseSearchFilter;
            fd.CaseInitiatorFilter = sm.CaseSearchFilter.Initiator;
            fd.InitiatorSearchScope = sm.CaseSearchFilter.InitiatorSearchScope;
            fd.customerSetting = GetCustomerSettings(cusId);
            fd.filterCustomers = customers;
            fd.filterCustomerId = cusId;
            // Case #53981
            var userSearch = new UserSearch() { CustomerId = cusId, StatusId = 3 };
            fd.AvailablePerformersList =
                this._userService.SearchSortAndGenerateUsers(userSearch).MapToCustomSelectList(fd.caseSearchFilter.UserPerformer, fd.customerSetting);

            if (!string.IsNullOrEmpty(fd.caseSearchFilter.UserPerformer))
            {
                fd.lstfilterPerformer = fd.caseSearchFilter.UserPerformer.Split(',').Select(int.Parse).ToArray();
            }

            fd.filterCaseProgress = ObjectExtensions.GetFilterForAdvancedSearch();

            //Working group            
            var gs = _globalSettingService.GetGlobalSettings().FirstOrDefault();
            const bool IsTakeOnlyActive = false;
            if (gs.LockCaseToWorkingGroup == 0)
            {
                fd.filterWorkingGroup = this._workingGroupService.GetAllWorkingGroupsForCustomer(cusId, IsTakeOnlyActive);
            }
            else
            {
                if (SessionFacade.CurrentUser.UserGroupId == 1 || SessionFacade.CurrentUser.UserGroupId == 2)
                {
                    fd.filterWorkingGroup = this._workingGroupService.GetWorkingGroups(cusId, userId, IsTakeOnlyActive, true);
                }
                else
                {
                    fd.filterWorkingGroup = this._workingGroupService.GetWorkingGroups(cusId, IsTakeOnlyActive);
                }
            }

            fd.filterWorkingGroup.Insert(0, ObjectExtensions.notAssignedWorkingGroup());

            //Sub status            
            fd.filterStateSecondary = this._stateSecondaryService.GetStateSecondaries(cusId);
            fd.filterMaxRows = GetMaxRowsFilter();

            return fd;
        }

        private AdvancedSearchSpecificFilterData CreateAdvancedSearchSpecificFilterData(int userId, int customerId = 0)
        {
            var csm = new CaseSearchModel();

            // While customer is not changed (cusId == 0), should use session values for default filter
            if (customerId == 0)
            {
                csm = SessionFacade.CurrentAdvancedSearch;
                if (csm != null && csm.CaseSearchFilter != null)
                    customerId = csm.CaseSearchFilter.CustomerId;
            }

            var customerSetting = GetCustomerSettings(customerId);

            var specificFilter = new AdvancedSearchSpecificFilterData();

            specificFilter.CustomerId = customerId;
            specificFilter.CustomerSetting = customerSetting;

            specificFilter.FilteredCaseTypeText = ParentPathDefaultValue;
            specificFilter.FilteredProductAreaText = ParentPathDefaultValue;
            specificFilter.FilteredClosingReasonText = ParentPathDefaultValue;


            specificFilter.NewProductAreaList = GetProductAreasModel(customerId, null).OrderBy(p => p.Text).ToList();

            var customerfieldSettings = this._caseFieldSettingService.GetCaseFieldSettings(customerId);

            if (customerfieldSettings.Where(fs => fs.Name == GlobalEnums.TranslationCaseFields.Department_Id.ToString() &&
                                                  fs.ShowOnStartPage != 0).Any())
            {
                const bool IsTakeOnlyActive = false;
                specificFilter.DepartmentList = this._departmentService.GetDepartmentsByUserPermissions(
                    userId,
                    customerId,
                    IsTakeOnlyActive);
                if (!specificFilter.DepartmentList.Any())
                {
                    specificFilter.DepartmentList =
                        this._departmentService.GetDepartments(customerId, ActivationStatus.All)
                        .ToList();
                }

                if (customerSetting != null && customerSetting.ShowOUsOnDepartmentFilter != 0)
                    specificFilter.DepartmentList = AddOrganizationUnitsToDepartments(specificFilter.DepartmentList);
            }

            if (customerfieldSettings.Where(fs => fs.Name == GlobalEnums.TranslationCaseFields.StateSecondary_Id.ToString() &&
                                                  fs.ShowOnStartPage != 0).Any())
            {
                specificFilter.StateSecondaryList = this._stateSecondaryService.GetStateSecondaries(customerId);
            }

            if (customerfieldSettings.Where(fs => fs.Name == GlobalEnums.TranslationCaseFields.Priority_Id.ToString() &&
                                                  fs.ShowOnStartPage != 0).Any())
            {
                specificFilter.PriorityList = this._priorityService.GetPriorities(customerId);
            }

            if (customerfieldSettings.Where(fs => fs.Name == GlobalEnums.TranslationCaseFields.ClosingReason.ToString() &&
                                                  fs.ShowOnStartPage != 0).Any())
            {
                specificFilter.ClosingReasonList = this._finishingCauseService.GetFinishingCausesWithChilds(customerId);
            }

            if (customerfieldSettings.Where(fs => fs.Name == GlobalEnums.TranslationCaseFields.CaseType_Id.ToString() &&
                                                  fs.ShowOnStartPage != 0).Any())
            {

                specificFilter.CaseTypeList = this._caseTypeService.GetCaseTypesOverviewWithChildren(customerId);
            }

            if (customerfieldSettings.Where(fs => fs.Name == GlobalEnums.TranslationCaseFields.ProductArea_Id.ToString() &&
                                                  fs.ShowOnStartPage != 0).Any())
            {
                const bool isTakeOnlyActive = false;
                specificFilter.ProductAreaList = this._productAreaService.GetTopProductAreasForUser(
                    customerId,
                    SessionFacade.CurrentUser,
                    isTakeOnlyActive);
            }

            if (customerfieldSettings.Where(fs => fs.Name == GlobalEnums.TranslationCaseFields.WorkingGroup_Id.ToString() &&
                                                  fs.ShowOnStartPage != 0).Any())
            {
                var gs = _globalSettingService.GetGlobalSettings().FirstOrDefault();
                const bool IsTakeOnlyActive = false;
                if (gs.LockCaseToWorkingGroup == 0)
                {
                    specificFilter.WorkingGroupList = this._workingGroupService.GetAllWorkingGroupsForCustomer(customerId, IsTakeOnlyActive);
                }
                else
                {
                    if (SessionFacade.CurrentUser.UserGroupId == 1 || SessionFacade.CurrentUser.UserGroupId == 2)
                    {
                        specificFilter.WorkingGroupList = this._workingGroupService.GetWorkingGroups(customerId, userId, IsTakeOnlyActive, true);
                    }
                    else
                    {
                        specificFilter.WorkingGroupList = this._workingGroupService.GetWorkingGroups(customerId, IsTakeOnlyActive);
                    }

                }

                specificFilter.WorkingGroupList.Insert(0, ObjectExtensions.notAssignedWorkingGroup());
            }

            if (csm != null && csm.CaseSearchFilter != null)
            {
                specificFilter.FilteredDepartment = csm.CaseSearchFilter.Department;
                specificFilter.FilteredPriority = csm.CaseSearchFilter.Priority;
                specificFilter.FilteredStateSecondary = csm.CaseSearchFilter.StateSecondary;
                specificFilter.FilteredCaseType = csm.CaseSearchFilter.CaseType;
                specificFilter.FilteredWorkingGroup = csm.CaseSearchFilter.WorkingGroup;
                if (specificFilter.FilteredCaseType > 0)
                {
                    var c = this._caseTypeService.GetCaseType(specificFilter.FilteredCaseType);
                    if (c != null)
                        specificFilter.FilteredCaseTypeText = c.getCaseTypeParentPath();
                }

                specificFilter.FilteredProductArea = csm.CaseSearchFilter.ProductArea;
                if (!string.IsNullOrWhiteSpace(specificFilter.FilteredProductArea))
                {
                    if (specificFilter.FilteredProductArea != "0")
                    {
                        var p = this._productAreaService.GetProductArea(specificFilter.FilteredProductArea.ToInt());
                        if (p != null)
                        {
                            specificFilter.FilteredProductAreaText = string.Join(
                                " - ",
                                this._productAreaService.GetParentPath(p.Id, customerId));
                        }
                    }
                }

                specificFilter.FilteredClosingReason = csm.CaseSearchFilter.CaseClosingReasonFilter;
                if (!string.IsNullOrWhiteSpace(specificFilter.FilteredClosingReason))
                {
                    if (specificFilter.FilteredClosingReason != "0")
                    {
                        var fc =
                            this._finishingCauseService.GetFinishingCause(
                                specificFilter.FilteredClosingReason.ToInt());
                        if (fc != null)
                        {
                            specificFilter.FilteredClosingReasonText = fc.GetFinishingCauseParentPath();
                        }
                    }
                }

            }

            return specificFilter;
        }

        private CaseSearchModel InitAdvancedSearchModel(int customerId, int userId)
        {
            Domain.ISearch s = new Domain.Search();
            var f = new CaseSearchFilter
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

            s.SortBy = "CaseNumber";
            s.Ascending = false;

            return new CaseSearchModel() { CaseSearchFilter = f, Search = s };
        }

        private Tuple<List<Dictionary<string, object>>, GridSettingsModel> AdvancedSearchForCustomer(FormCollection frm, int customerId, bool isExtendedSearch = false, List<int> extendedCustomers = null)
        {
            var currentCustomerId = SessionFacade.CurrentCustomer.Id;

            #region Code from old method. TODO: code review wanted
            var f = new CaseSearchFilter();

            var m = new CaseSearchResultModel();

            f.IsExtendedSearch = isExtendedSearch;
            f.CustomerId = customerId;//int.Parse(frm.ReturnFormValue("currentCustomerId"));
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
                f.WorkingGroup = frm.ReturnFormValue("lstFilterWorkingGroup");
                f.Department = frm.ReturnFormValue("lstfilterDepartment");
                f.Priority = frm.ReturnFormValue("lstfilterPriority");
                f.StateSecondary = frm.ReturnFormValue("lstfilterStateSecondary");
                f.CaseType = frm.ReturnFormValue("hid_CaseTypeDropDown").ToInt();
                f.ProductArea = f.ProductArea = frm.ReturnFormValue("hid_ProductAreaDropDown").ReturnCustomerUserValue();
                f.CaseClosingReasonFilter = frm.ReturnFormValue("hid_ClosingReasonDropDown").ReturnCustomerUserValue();


                var departments_OrganizationUnits = frm.ReturnFormValue(CaseFilterFields.DepartmentNameAttribute);

                f.Department = GetDepartmentsFrom(departments_OrganizationUnits);
                f.OrganizationUnit = GetOrganizationUnitsFrom(departments_OrganizationUnits);
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

            if (!string.IsNullOrEmpty(frm.ReturnFormValue("txtCaseNumberSearch")))
            {
                f.FreeTextSearch = string.Format("#{0}", frm.ReturnFormValue("txtCaseNumberSearch"));
                f.CaseNumber = frm.ReturnFormValue("txtCaseNumberSearch");
            }
            else
            {
                if (!string.IsNullOrEmpty(frm.ReturnFormValue("txtCaptionSearch")))
                    f.CaptionSearch = frm.ReturnFormValue("txtCaptionSearch");
                else
                {
                    f.FreeTextSearch = frm.ReturnFormValue("txtFreeTextSearch");
                    f.SearchThruFiles = frm.IsFormValueTrue("searchThruFiles");
                }
            }

            var maxRecords = this._defaultMaxRows;
            int.TryParse(frm.ReturnFormValue("lstfilterMaxRows"), out maxRecords);
            f.MaxRows = maxRecords.ToString();


            if (string.IsNullOrEmpty(f.CaseProgress))
                f.CaseProgress = CaseProgressFilter.None;

            CaseSearchModel sm;
            sm = this.InitAdvancedSearchModel(f.CustomerId, f.UserId);
            sm.CaseSearchFilter = f;

            var jsonGridSettings = JsonGridSettingsMapper.GetAdvancedSearchGridSettingsModel(SessionFacade.CurrentCustomer.Id);

            // Convert Json Model to Business Model
            var colDefs = jsonGridSettings.columnDefs.Select(c => new GridColumnDef
            {
                id = GridColumnsDefinition.GetFieldId(c.field),
                cls = c.cls,
                name = c.field,
                isExpandable = c.isExpandable,
                width = c.width
            }).ToList();

            var gridSettings =
                new GridSettingsModel()
                {
                    CustomerId = f.CustomerId,
                    cls = jsonGridSettings.cls,
                    pageOptions = jsonGridSettings.pageOptions,
                    sortOptions = jsonGridSettings.sortOptions,
                    columnDefs = colDefs
                };

            gridSettings.sortOptions.sortBy = frm.ReturnFormValue("sortBy");
            var sortDir = 0;
            gridSettings.sortOptions.sortDir = (!string.IsNullOrEmpty(frm.ReturnFormValue("sortDir"))
                               && int.TryParse(frm.ReturnFormValue("sortDir"), out sortDir)
                               && sortDir == (int)SortingDirection.Asc) ? SortingDirection.Asc : SortingDirection.Desc;

            SessionFacade.AdvancedSearchOverviewGridSettings = gridSettings;

            sm.Search.SortBy = gridSettings.sortOptions.sortBy;
            sm.Search.Ascending = gridSettings.sortOptions.sortDir == SortingDirection.Asc;
            m.caseSettings = new List<Domain.CaseSettings>();
            foreach (var col in colDefs)
            {
                var curSetting = new Domain.CaseSettings()
                {
                    Id = col.id,
                    Name = col.name
                };
                m.caseSettings.Add(curSetting);
            }

            var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(SessionFacade.CurrentUser.TimeZoneId);
            var caseFieldSettings = this._caseFieldSettingService.GetCaseFieldSettings(f.CustomerId).ToArray();
            f.MaxTextCharacters = 0;

            var normalSearchResultIds = new List<int>();
            if (f.IsExtendedSearch)
            {
                if (!extendedCustomers.Contains(customerId))
                {
                    f.IsExtendedSearch = false;
                    normalSearchResultIds = _caseSearchService.Search(
                        f,
                        m.caseSettings,
                        caseFieldSettings,
                        SessionFacade.CurrentUser.Id,
                        SessionFacade.CurrentUser.UserId,
                        SessionFacade.CurrentUser.ShowNotAssignedWorkingGroups,
                        SessionFacade.CurrentUser.UserGroupId,
                        SessionFacade.CurrentUser.RestrictedCasePermission,
                        sm.Search,
                        0,
                        0,
                        userTimeZone,
                        ApplicationTypes.Helpdesk
                        ).Items.Select(x => x.Id).ToList();
                    f.IsExtendedSearch = true;
                }

                m.cases = _caseSearchService.Search(
                f,
                m.caseSettings,
                caseFieldSettings,
                SessionFacade.CurrentUser.Id,
                SessionFacade.CurrentUser.UserId,
                SessionFacade.CurrentUser.ShowNotAssignedWorkingGroups,
                SessionFacade.CurrentUser.UserGroupId,
                SessionFacade.CurrentUser.RestrictedCasePermission,
                sm.Search,
                0,
                0,
                userTimeZone,
                ApplicationTypes.Helpdesk
                ).Items.Take(maxRecords).ToList();
            }
            else
            {
                m.cases = _caseSearchService.Search(
                f,
                m.caseSettings,
                caseFieldSettings,
                SessionFacade.CurrentUser.Id,
                SessionFacade.CurrentUser.UserId,
                SessionFacade.CurrentUser.ShowNotAssignedWorkingGroups,
                SessionFacade.CurrentUser.UserGroupId,
                SessionFacade.CurrentUser.RestrictedCasePermission,
                sm.Search,
                0,
                0,
                userTimeZone,
                ApplicationTypes.Helpdesk
                ).Items.Take(maxRecords).ToList();
            }

            m.cases = CommonHelper.TreeTranslate(m.cases, currentCustomerId, _productAreaService);
            sm.Search.IdsForLastSearch = this.GetIdsFromSearchResult(m.cases);

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
                        if (!string.IsNullOrEmpty(id))
                        {
                            if (string.IsNullOrEmpty(sm.CaseSearchFilter.Department))
                                sm.CaseSearchFilter.Department += string.Format("-{0}", id);
                            else
                                sm.CaseSearchFilter.Department += string.Format(",-{0}", id);
                        }
                }
            }

            SessionFacade.CurrentAdvancedSearch = sm;
            #endregion

            var availableDepIds = new List<int>();
            bool accessToAllDepartments = false;
            var availableWgIds = new List<int>();
            var availableCustomerIds = new List<int> { 0 };
            if (isExtendedSearch)
            {
                var user = _userService.GetUser(SessionFacade.CurrentUser.Id);
                if (user != null)
                {
                    availableCustomerIds.AddRange(user.Cs.Select(x => x.Id));
                    availableDepIds.AddRange(user.Departments.Where(x => x.Customer_Id == customerId).Select(x => x.Id));
                    availableWgIds.AddRange(user.UserWorkingGroups.Select(x => x.WorkingGroup_Id));

                    //Department, If 0 selected you should have access to all departments
                    if (availableDepIds.Count() == 0)
                    {
                        availableDepIds.Add(0);
                        accessToAllDepartments = true;
                    }

                    //ShowNotAssignedWorkingGroups
                    if (SessionFacade.CurrentUser.ShowNotAssignedWorkingGroups == 1)
                    {
                        availableWgIds.Add(0);
                    }
                }
            }

            var data = new List<Dictionary<string, object>>();
            var customerSettings = GetCustomerSettings(f.CustomerId);
            var outputFormatter = new OutputFormatter(customerSettings.IsUserFirstLastNameRepresentation == 1, userTimeZone);
            foreach (var searchRow in m.cases)
            {
                var jsRow = new Dictionary<string, object>
                                {
                                    { "case_id", searchRow.Id },
                                    { "caseIconTitle", searchRow.CaseIcon.CaseIconTitle() },
                                    {
                                        "caseIconUrl",
                                        string.Format(
                                            "/Content/icons/{0}",
                                            searchRow.CaseIcon.CaseIconSrc())
                                    },
                                    { "isUnread", searchRow.IsUnread },
                                    { "isUrgent", searchRow.IsUrgent },
                                    { "isClosed", searchRow.IsClosed},
                                };
                if (isExtendedSearch)
                {
                    var infoAvailableInExtended = false;
                    if (normalSearchResultIds.Contains(searchRow.Id))
                    {
                        infoAvailableInExtended = true;
                    }
                    else
                    {
                        if (SessionFacade.CurrentUser.UserGroupId == UserGroups.User || SessionFacade.CurrentUser.UserGroupId == UserGroups.Administrator)
                        {


                            // finns kryssruta pa anvandaren att den bara far se sina egna arenden
                            //Note, this is also checked in where clause  in ReturnCaseSearchWhere(SearchQueryBuildContext ctx)
                            //Check for access Department
                            //Check for access WorkingGroups
                            if (SessionFacade.CurrentUser.RestrictedCasePermission == 1 && (availableDepIds.Contains(searchRow.ExtendedSearchInfo.DepartmentId) || accessToAllDepartments)
                                && availableWgIds.Contains(searchRow.ExtendedSearchInfo.WorkingGroupId))
                            {
                                //Use functionality from VerifyCase
                                infoAvailableInExtended = _userService.VerifyUserCasePermissions(SessionFacade.CurrentUser, searchRow.Id);
                            }

                            if (infoAvailableInExtended == true && availableDepIds.Contains(searchRow.ExtendedSearchInfo.DepartmentId)
                               && availableWgIds.Contains(searchRow.ExtendedSearchInfo.WorkingGroupId)
                               && availableCustomerIds.Contains(searchRow.ExtendedSearchInfo.CustomerId))
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

            return new Tuple<List<Dictionary<string, object>>, GridSettingsModel>(data, gridSettings);
        }


        #endregion
    }
}