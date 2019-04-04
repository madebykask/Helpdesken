using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DH.Helpdesk.BusinessData.Enums.Case;
using DH.Helpdesk.BusinessData.Enums.Users;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.Grid;
using DH.Helpdesk.BusinessData.Models.Shared;
using DH.Helpdesk.BusinessData.Models.User.Input;
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
        public ActionResult AdvancedSearch(bool? clearFilters = false, bool doSearchAtBegining = false, bool isExtSearch = false)
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

            
            var m = new AdvancedSearchIndexViewModel();
            var userCustomers =
                _userService.GetUserProfileCustomersSettings(SessionFacade.CurrentUser.Id)
                    .Select(c => new ItemOverview(c.CustomerName, c.CustomerId.ToString())).OrderBy(c => c.Name).ToList();

            m.UserCustomers = userCustomers;

            var extendIncludedCustomerIds = _settingService.GetExtendedSearchIncludedCustomers();
            var extCustomers = 
                _customerService.GetAllCustomers()
                     .Where(x => extendIncludedCustomerIds.Contains(x.Id))
                     .Select(c => new ItemOverview(c.Name, c.Id.ToString())).OrderBy(c => c.Name).ToList();

            m.ExtendedCustomers = extCustomers;

            CaseSearchModel advancedSearchModel;
            if (clearFilters != null && clearFilters.Value || SessionFacade.CurrentAdvancedSearch == null)
            {
                SessionFacade.CurrentAdvancedSearch = null;
                var userCustomerSettings = _userService.GetUser(currentUserId);
                advancedSearchModel = CreateAdvancedSearchModel(currentCustomerId, currentUserId, null, userCustomerSettings.StartPage == (int)StartPage.AdvancedSearch);
                SessionFacade.CurrentAdvancedSearch = advancedSearchModel;
            }
            else
            {
                advancedSearchModel = SessionFacade.CurrentAdvancedSearch;
            }

            m.CaseSearchFilterData = 
                CreateAdvancedSearchFilterData(currentCustomerId, 
                    currentUserId, 
                    advancedSearchModel,
                    userCustomers);

            m.SpecificSearchFilterData = CreateAdvancedSearchSpecificFilterData(currentUserId);

            m.CaseSetting = GetCaseSettingModel(currentCustomerId, currentUserId);
            m.GridSettings = JsonGridSettingsMapper.GetAdvancedSearchGridSettingsModel(currentCustomerId);

            if (advancedSearchModel.Search != null)
            {
                m.GridSettings.sortOptions = new GridSortOptions
                {
                    sortBy = advancedSearchModel.Search.SortBy,
                    sortDir = advancedSearchModel.Search.Ascending ? SortingDirection.Asc : SortingDirection.Desc
                };
            }

            m.CaseSearchFilterData.IsAboutEnabled = 
                m.CaseSetting.ColumnSettingModel.CaseFieldSettings.GetIsAboutEnabled();

            m.DoSearchAtBegining = doSearchAtBegining;
            m.IsExtSearch = isExtSearch;

            return View("AdvancedSearch/Index", m);
        }

        [HttpGet]
        public string LookupLanguage(string customerid, string notifier, string region, string department, string notifierid)
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

            var currentCustomerId = SessionFacade.CurrentCustomer.Id;
            var customers = frm.ReturnFormValue("currentCustomerId").Split(',').Select(x => x.ToInt()).ToList();
            var res = new List<Tuple<List<Dictionary<string, object>>, GridSettingsModel>>();
            var extendedCustomers = new List<int>();

            //create models from request input
            var searchFilter = CreateSearchFilterFromRequest(frm);
            var gridSortingOptions = CreateGridSortOptionsFromRequest(frm);
            var gridSettings = CreateGridSettingsModel(currentCustomerId, gridSortingOptions);

            SessionFacade.AdvancedSearchOverviewGridSettings = gridSettings;

            if (searchFilter.IsExtendedSearch)
            {
                extendedCustomers = _settingService.GetExtendedSearchIncludedCustomers();

                foreach (var searchCustomerId in extendedCustomers)
                {
                    searchFilter.CustomerId = searchCustomerId;
                    gridSettings.CustomerId = searchCustomerId;

                    var sr = 
                        RunAdvancedSearchForCustomer(searchFilter, 
                            gridSettings, 
                            searchCustomerId,
                            currentCustomerId,
                            SessionFacade.CurrentUser,
                            extendedCustomers);

                    res.Add(sr);
                }
            }
            
            //do normal search 
            foreach (var searchCustomerId in customers.Except(extendedCustomers))
            {
                searchFilter.IsExtendedSearch = false;
                searchFilter.CustomerId = searchCustomerId;
                gridSettings.CustomerId = searchCustomerId;

                var sr = 
                    RunAdvancedSearchForCustomer(searchFilter, 
                        gridSettings, 
                        searchCustomerId,
                        currentCustomerId,
                        SessionFacade.CurrentUser);

                res.Add(sr);
            }

            var totalCount = res.Sum(x => x.Item1.Count);

            var data = res.Select(x => new
            {
                data = x.Item1,
                gridSettings = x.Item2
            }).ToList();

            var ret = new
            {
                Items = data,
                TotalCount = totalCount
            };

            return Json(new { result = "success", data = ret });
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

        private Tuple<List<Dictionary<string, object>>, GridSettingsModel> RunAdvancedSearchForCustomer(
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
                    Name = colDef.name
                }).ToList() ?? new List<CaseSettings>()
            };
            
            var caseFieldSettings = _caseFieldSettingService.GetCaseFieldSettings(f.CustomerId).ToArray();
            f.MaxTextCharacters = 0;
            var maxRecords = f.MaxRows.ToInt();
            var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(currentUser.TimeZoneId);

            var normalSearchResultIds = new List<int>();

            if (f.IsExtendedSearch && 
                extendedCustomers != null && 
                !extendedCustomers.Contains(searchCustomerId))
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
                    availableCustomerIds.AddRange(user.Cs.Select(x => x.Id));
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
            var customerSettings = GetCustomerSettings(f.CustomerId);
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

            return new Tuple<List<Dictionary<string, object>>, GridSettingsModel>(data, gridSettings);
        }

        private CaseSearchFilterData CreateAdvancedSearchFilterData(int cusId, int userId, CaseSearchModel sm, List<ItemOverview> customers)
        {
            var fd = new CaseSearchFilterData
            {
                caseSearchFilter = sm.CaseSearchFilter,
                CaseInitiatorFilter = sm.CaseSearchFilter.Initiator,
                InitiatorSearchScope = sm.CaseSearchFilter.InitiatorSearchScope,
                customerSetting = GetCustomerSettings(cusId),
                filterCustomers = customers,
                filterCustomerId = cusId
            };

            // Case #53981
            var userSearch = new UserSearch()
            {
                CustomerId = cusId,
                StatusId = 3
            };

            fd.AvailablePerformersList =
                _userService.SearchSortAndGenerateUsers(userSearch).MapToCustomSelectList(fd.caseSearchFilter.UserPerformer, fd.customerSetting);

            if (!string.IsNullOrEmpty(fd.caseSearchFilter.UserPerformer))
            {
                fd.lstfilterPerformer = fd.caseSearchFilter.UserPerformer.Split(',').Select(int.Parse).ToArray();
            }

            fd.filterCaseProgress = ObjectExtensions.GetFilterForAdvancedSearch();

            var gs = _globalSettingService.GetGlobalSettings().FirstOrDefault();

            //Working group
            if (gs.LockCaseToWorkingGroup == 0)
            {
                fd.filterWorkingGroup = 
                    _workingGroupService.GetAllWorkingGroupsForCustomer(cusId, isTakeOnlyActive: false);
            }
            else
            {
                if (SessionFacade.CurrentUser.UserGroupId == 1 || SessionFacade.CurrentUser.UserGroupId == 2)
                {
                    fd.filterWorkingGroup = _workingGroupService.GetWorkingGroups(cusId, userId, isTakeOnlyActive: false, caseOverviewFilter: true);
                }
                else
                {
                    fd.filterWorkingGroup = _workingGroupService.GetWorkingGroups(cusId, isTakeOnlyActive: false);
                }
            }

            fd.filterWorkingGroup.Insert(0, ObjectExtensions.notAssignedWorkingGroup());

            //Sub status            
            fd.filterStateSecondary = _stateSecondaryService.GetStateSecondaries(cusId);
            fd.filterMaxRows = GetMaxRowsFilter();

            return fd;
        }

        private bool HasField(IList<CaseFieldSetting> fieldSetitngs, GlobalEnums.TranslationCaseFields field)
        {
            return fieldSetitngs.Any(fs => fs.Name == field.ToString() && fs.ShowOnStartPage != 0);
        }

        private AdvancedSearchSpecificFilterData CreateAdvancedSearchSpecificFilterData(int userId, int customerId = 0)
        {
            var caseSearchModel = new CaseSearchModel();

            // While customer is not changed (cusId == 0), should use session values for default filter
            if (customerId == 0)
            {
                caseSearchModel = SessionFacade.CurrentAdvancedSearch;
                if (caseSearchModel != null && caseSearchModel.CaseSearchFilter != null)
                    customerId = caseSearchModel.CaseSearchFilter.CustomerId;
            }

            var customerSetting = GetCustomerSettings(customerId);

            var specificFilter = new AdvancedSearchSpecificFilterData
            {
                CustomerId = customerId,
                CustomerSetting = customerSetting,
                FilteredCaseTypeText = ParentPathDefaultValue,
                FilteredProductAreaText = ParentPathDefaultValue,
                FilteredClosingReasonText = ParentPathDefaultValue,
                NewProductAreaList = GetProductAreasModel(customerId, null).OrderBy(p => p.Text).ToList()
            };

            var customerfieldSettings = _caseFieldSettingService.GetCaseFieldSettings(customerId);
            
            if (HasField(customerfieldSettings, GlobalEnums.TranslationCaseFields.Department_Id))
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

            if (HasField(customerfieldSettings, GlobalEnums.TranslationCaseFields.StateSecondary_Id))
            {
                specificFilter.StateSecondaryList = this._stateSecondaryService.GetStateSecondaries(customerId);
            }

            if (HasField(customerfieldSettings, GlobalEnums.TranslationCaseFields.Priority_Id))
            {
                specificFilter.PriorityList = this._priorityService.GetPriorities(customerId);
            }

            if (HasField(customerfieldSettings, GlobalEnums.TranslationCaseFields.ClosingReason))
            {
                specificFilter.ClosingReasonList = this._finishingCauseService.GetFinishingCausesWithChilds(customerId);
            }

            if (HasField(customerfieldSettings, GlobalEnums.TranslationCaseFields.CaseType_Id))
            {
                specificFilter.CaseTypeList = _caseTypeService.GetCaseTypesOverviewWithChildren(customerId);
            }

            if (HasField(customerfieldSettings, GlobalEnums.TranslationCaseFields.ProductArea_Id))
            {
                const bool isTakeOnlyActive = false;
                specificFilter.ProductAreaList = 
                    _productAreaService.GetTopProductAreasForUser(customerId, SessionFacade.CurrentUser, isTakeOnlyActive);
            }

            if (HasField(customerfieldSettings, GlobalEnums.TranslationCaseFields.WorkingGroup_Id))
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

            if (caseSearchModel != null && caseSearchModel.CaseSearchFilter != null)
            {
                specificFilter.FilteredDepartment = caseSearchModel.CaseSearchFilter.Department;
                specificFilter.FilteredPriority = caseSearchModel.CaseSearchFilter.Priority;
                specificFilter.FilteredStateSecondary = caseSearchModel.CaseSearchFilter.StateSecondary;
                specificFilter.FilteredCaseType = caseSearchModel.CaseSearchFilter.CaseType;
                specificFilter.FilteredWorkingGroup = caseSearchModel.CaseSearchFilter.WorkingGroup;
                if (specificFilter.FilteredCaseType > 0)
                {
                    var c = this._caseTypeService.GetCaseType(specificFilter.FilteredCaseType);
                    if (c != null)
                        specificFilter.FilteredCaseTypeText = c.getCaseTypeParentPath();
                }

                specificFilter.FilteredProductArea = caseSearchModel.CaseSearchFilter.ProductArea;
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

                specificFilter.FilteredClosingReason = caseSearchModel.CaseSearchFilter.CaseClosingReasonFilter;
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
        
        protected CaseSearchModel CreateAdvancedSearchModel(int customerId, int userId, CaseSearchFilter filter = null, bool isStartPage = false)
        {
            var f = filter ?? new CaseSearchFilter
            {
                CustomerId = customerId,
                UserId = userId,
                UserPerformer = isStartPage ? userId.ToString() : string.Empty,
                CaseProgress = isStartPage ? ((int)CaseProgressFilterEnum.CasesInProgress).ToString() : string.Empty,
                WorkingGroup = string.Empty,
                CaseRegistrationDateStartFilter = null,
                CaseRegistrationDateEndFilter = null,
                CaseClosingDateStartFilter = null,
                CaseClosingDateEndFilter = null,
                Customer = isStartPage ? string.Empty : customerId.ToString()
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

        private GridSortOptions CreateGridSortOptionsFromRequest(FormCollection frm)
        {
            var sortBy = frm.ReturnFormValue("sortBy");
            var sortDir = frm.ReturnFormValue("sortDir");

            return new GridSortOptions()
            {
                sortBy = sortBy,
                sortDir = string.IsNullOrEmpty(sortDir)
                    ? SortingDirection.Desc
                    : (SortingDirection)sortDir.ToInt()
            };
        }

        private CaseSearchFilter CreateSearchFilterFromRequest(FormCollection frm)
        {
            var f = new CaseSearchFilter();
            f.IsExtendedSearch = frm.IsFormValueTrue(CaseFilterFields.IsExtendedSearch);
            //f.CustomerId = customerId;//int.Parse(frm.ReturnFormValue("currentCustomerId"));

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

        private GridSettingsModel CreateGridSettingsModel(int customerId, GridSortOptions sortOptions)
        {
            var jsonGridSettings = JsonGridSettingsMapper.GetAdvancedSearchGridSettingsModel(customerId);

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
                    CustomerId = customerId,
                    cls = jsonGridSettings.cls,
                    pageOptions = jsonGridSettings.pageOptions,
                    sortOptions = sortOptions,
                    columnDefs = colDefs
                };
            return gridSettings;
        }


        #endregion
    }
}