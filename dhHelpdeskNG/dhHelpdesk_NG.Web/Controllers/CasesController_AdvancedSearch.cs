using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DH.Helpdesk.BusinessData.Models.Grid;
using DH.Helpdesk.BusinessData.Models.Shared;
using DH.Helpdesk.BusinessData.OldComponents;
using DH.Helpdesk.BusinessData.OldComponents.DH.Helpdesk.BusinessData.Utils;
using DH.Helpdesk.Common.Constants;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Common.Extensions.Integer;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Web.Common.Models.CaseSearch;
using DH.Helpdesk.Web.Infrastructure;
using DH.Helpdesk.Web.Infrastructure.Attributes;
using DH.Helpdesk.Web.Infrastructure.Extensions;
using DH.Helpdesk.Web.Infrastructure.Grid;
using DH.Helpdesk.Web.Models.Case;
using DH.Helpdesk.Web.Models.Case.Output;
using static DH.Helpdesk.Dal.Repositories.CaseRepository;

namespace DH.Helpdesk.Web.Controllers
{
    public partial class CasesController
    {
        [System.Web.Http.HttpGet]
        public ActionResult AdvancedSearch(bool? clearFilters = false, bool doSearchAtBegining = false, bool isExtSearch = false, bool currentUserAdmin = false)
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

            var model = new AdvancedSearchIndexViewModel();

            var userCustomers =
                _userService.GetUserProfileCustomersSettings(SessionFacade.CurrentUser.Id)
                    .Select(c => new ItemOverview(c.CustomerName, c.CustomerId.ToString(), c.Active))
                    .OrderBy(c => c.Name).ToList();

            model.UserCustomers = userCustomers;

            //get extended search customers
            var extendIncludedCustomerIds = _settingService.GetExtendedSearchIncludedCustomers();
            var extCustomers = _customerService.GetCustomers(extendIncludedCustomerIds);
            model.ExtendedCustomers = extCustomers.ToList();

            CaseSearchModel advancedSearchModel;
            if ((clearFilters != null && clearFilters.Value) || SessionFacade.CurrentAdvancedSearch == null)
            {
                SessionFacade.CurrentAdvancedSearch = null;
                advancedSearchModel = _advancedSearchBehavior.CreateAdvancedSearchModel(currentCustomerId, currentUserId);
                SessionFacade.CurrentAdvancedSearch = advancedSearchModel;
            }
            else
            {
                advancedSearchModel = SessionFacade.CurrentAdvancedSearch;
            }

            if (currentUserAdmin)
            {
                advancedSearchModel.CaseSearchFilter.UserPerformer = currentUserId.ToString();
                advancedSearchModel.CaseSearchFilter.Customer = "";
            }

            model.CaseSearchFilterData =
                CreateAdvancedSearchFilterData(currentCustomerId, currentUserId, advancedSearchModel, userCustomers);

            model.SpecificSearchFilterData = CreateAdvancedSearchSpecificFilterData(currentUserId);

            model.CaseSetting = GetCaseSettingModel(currentCustomerId, currentUserId);

            model.GridSettings = JsonGridSettingsMapper.GetAdvancedSearchGridSettingsModel(currentCustomerId);

            if (advancedSearchModel.Search != null)
            {
                model.GridSettings.sortOptions = new GridSortOptions
                {
                    sortBy = advancedSearchModel.Search.SortBy,
                    sortDir = advancedSearchModel.Search.Ascending ? SortingDirection.Asc : SortingDirection.Desc
                };
            }

            model.CaseSearchFilterData.IsAboutEnabled =
                model.CaseSetting.ColumnSettingModel.CaseFieldSettings.GetIsAboutEnabled();

            model.DoSearchAtBegining = doSearchAtBegining;
            model.IsExtSearch = isExtSearch;

            var useOldAdvancedSearch = _featureToggleService.Get(FeatureToggleTypes.USE_DEPRICATED_ADVANCED_CASE_SEARCH);
            
            return View(!useOldAdvancedSearch.Active ? "AdvancedSearch" : "AdvancedSearch/Index", model);
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

            var currentUser = SessionFacade.CurrentUser;
            var currentCustomerId = SessionFacade.CurrentCustomer.Id;
            var customers = frm.ReturnFormValue("currentCustomerId").Split(',').Select(x => x.ToInt()).ToList();
            var res = new List<Tuple<List<Dictionary<string, object>>, GridSettingsModel>>();
            var extendedCustomers = new List<int>();

            //create models from request input
            var searchFilter = _advancedSearchBehavior.CreateSearchFilterFromRequest(frm);
            var gridSortingOptions = CreateGridSortOptionsFromRequest(frm);
            
            if (searchFilter.IsExtendedSearch)
            {
                extendedCustomers = _settingService.GetExtendedSearchIncludedCustomers();

                foreach (var searchCustomerId in extendedCustomers)
                {
                    searchFilter.CustomerId = searchCustomerId;
                    var gridSettings = CreateGridSettingsModel(currentCustomerId, searchCustomerId, gridSortingOptions);

                    var sr = 
                        _advancedSearchBehavior.RunAdvancedSearchForCustomer(searchFilter, 
                            gridSettings, 
                            searchCustomerId,
                            currentCustomerId,
                            currentUser,
                            extendedCustomers).Data;

                    res.Add(new Tuple<List<Dictionary<string, object>>, GridSettingsModel>(sr, gridSettings));
                }
            }
            
            //do normal search 
            foreach (var searchCustomerId in customers.Except(extendedCustomers))
            {
                searchFilter.IsExtendedSearch = false;
                searchFilter.CustomerId = searchCustomerId;
                var gridSettings = CreateGridSettingsModel(currentCustomerId, searchCustomerId, gridSortingOptions);

                var sr = 
                    _advancedSearchBehavior.RunAdvancedSearchForCustomer(searchFilter, 
                        gridSettings, 
                        searchCustomerId,
                        currentCustomerId,
                        SessionFacade.CurrentUser).Data;

                res.Add(new Tuple<List<Dictionary<string, object>>, GridSettingsModel>(sr, gridSettings));
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

            int caseId = _caseService.GetCaseQuickOpen(SessionFacade.CurrentUser, SessionFacade.CurrentCustomer.Id, searchFor);
            //Get the case
            var theCaseToCheck = _caseService.GetCaseById(caseId);
            if (caseId > 0)
            {
                var userDeps = _departmentService.GetDepartmentsByUserPermissions(SessionFacade.CurrentUser.Id, SessionFacade.CurrentCustomer.Id);
                if(userDeps != null && userDeps.Count > 0) {
                    //Check if the user has access to the case
                    if (!userDeps.Any(x => x.Id == theCaseToCheck.Department_Id))
                    {
                        notFoundText = Translation.GetCoreTextTranslation("Åtkomst nekad");
                        return this.Json(new { result = "error", data = notFoundText });
                    }
                }
                
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

        #region Private Methods

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
			var performers = _userService.GetAllPerformers(cusId);
			var notAssigned = ObjectExtensions.notAssignedPerformer();
			performers.Insert(0, new Domain.User { Id = notAssigned.Id, FirstName = notAssigned.FirstName, SurName = notAssigned.SurName });

			fd.AvailablePerformersList = performers.MapToCustomSelectList(fd.caseSearchFilter.UserPerformer, fd.customerSetting);


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
			var stateSecondaries = _stateSecondaryService.GetStateSecondaries(cusId);
			stateSecondaries.Insert(0, ObjectExtensions.notAssignedStateSecondary());
			fd.filterStateSecondary = stateSecondaries;
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

				var departments = this._departmentService.GetDepartmentsByUserPermissions(
					userId,
					customerId,
					IsTakeOnlyActive);
                if (!departments.Any())
                {
                    departments =
                        this._departmentService.GetDepartments(customerId, ActivationStatus.All)
                            .ToList();
                }

                if (customerSetting != null && customerSetting.ShowOUsOnDepartmentFilter != 0)
                    departments = AddOrganizationUnitsToDepartments(departments);

				departments.Insert(0, ObjectExtensions.notAssignedDepartment());
				specificFilter.DepartmentList = departments;

			}

            if (HasField(customerfieldSettings, GlobalEnums.TranslationCaseFields.StateSecondary_Id))
            {
				var stateSecondaries = this._stateSecondaryService.GetStateSecondaries(customerId);
				stateSecondaries.Insert(0, ObjectExtensions.notAssignedStateSecondary());
				specificFilter.StateSecondaryList = stateSecondaries;
            }

            if (HasField(customerfieldSettings, GlobalEnums.TranslationCaseFields.Priority_Id))
            {
				var priorities = this._priorityService.GetPriorities(customerId);
				priorities.Insert(0, ObjectExtensions.notAssignedPriority());
				specificFilter.PriorityList = priorities;
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
                    _productAreaService.GetTopProductAreasWithChilds(customerId, isTakeOnlyActive);
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

        private GridSettingsModel CreateGridSettingsModel(int currentCustomerId, int searchCustomerId, GridSortOptions sortOptions)
        {
            var jsonGridSettings = JsonGridSettingsMapper.GetAdvancedSearchGridSettingsModel(currentCustomerId);

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
                    CustomerId = searchCustomerId,
                    cls = jsonGridSettings.cls,
                    pageOptions = jsonGridSettings.pageOptions,
                    sortOptions = sortOptions,
                    columnDefs = colDefs
                };

            SessionFacade.AdvancedSearchOverviewGridSettings = gridSettings;

            return gridSettings;
        }
        
        #endregion
    }
}