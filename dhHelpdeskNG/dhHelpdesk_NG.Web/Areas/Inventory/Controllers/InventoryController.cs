﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DH.Helpdesk.BusinessData.Enums.Case;
using DH.Helpdesk.BusinessData.Enums.Inventory;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Web.Areas.Inventory.Models;
using DH.Helpdesk.Web.Areas.Inventory.Models.RelatedCasesModels;
using DH.Helpdesk.Web.Controllers;
using DH.Helpdesk.Web.Infrastructure;
using DH.Helpdesk.Web.Infrastructure.CaseOverview;
using DH.Helpdesk.Web.Infrastructure.Helpers;
using DH.Helpdesk.Web.Infrastructure.ModelFactories.Case;
using DH.Helpdesk.Web.Models.Case;

namespace DH.Helpdesk.Web.Areas.Inventory.Controllers
{
    public class InventoryController : UserInteractionController
    {
        private readonly IInventoryService _inventoryService;
        private readonly ICaseModelFactory _caseModelFactory;
        private readonly CaseOverviewGridSettingsService _caseOverviewSettingsService;
        private readonly ICaseSettingsService _caseSettingService;
        private readonly ICaseFieldSettingService _caseFieldSettingService;
        private readonly ICaseSearchService _caseSearchService;
        private readonly IProductAreaService _productAreaService;
        private readonly IInventorySettingsService _inventorySettingsService;
        private readonly ICustomerUserService _customerUserService;

        public InventoryController(IMasterDataService masterDataService,
            IInventoryService inventoryService,
            ICaseModelFactory caseModelFactory,
            CaseOverviewGridSettingsService caseOverviewSettingsService,
            ICaseSettingsService caseSettingService,
            ICaseFieldSettingService caseFieldSettingService,
            ICaseSearchService caseSearchService,
            IProductAreaService productAreaService,
            IInventorySettingsService inventorySettingsService,
            ICustomerUserService customerUserService) : base(masterDataService)
        {
            _customerUserService = customerUserService;
            _inventoryService = inventoryService;
            _caseModelFactory = caseModelFactory;
            _caseOverviewSettingsService = caseOverviewSettingsService;
            _caseSettingService = caseSettingService;
            _caseFieldSettingService = caseFieldSettingService;
            _caseSearchService = caseSearchService;
            _productAreaService = productAreaService;
            _inventorySettingsService = inventorySettingsService;
        }

        [HttpGet]
        public ActionResult RelatedCases(int inventoryId, CurrentModes inventoryType)
        {
            var tabSettings = _inventorySettingsService.GetWorkstationTabsSettingsForEdit(
                SessionFacade.CurrentCustomer.Id,
                SessionFacade.CurrentLanguageId);

            var sortBy = CaseSortField.CaseNumber;
            var sortByAsc = true;
            if (inventoryType == CurrentModes.Workstations)
            {
                var model = new WorkstationRelatedCasesModel(inventoryId)
                {
                    RelatedCases = GetRelatedCaseModel(inventoryId, sortBy, sortByAsc, inventoryType),
                    TabSettings = tabSettings
                };
                return View("~/Areas/Inventory/Views/Workstation/WorkstationRelatedCases.cshtml", model);
            }
            if (inventoryType == CurrentModes.Servers)
            {
                var model = new ServerRelatedCasesModel(inventoryId)
                {
                    RelatedCases = GetRelatedCaseModel(inventoryId, sortBy, sortByAsc, inventoryType)
                };
                return View("~/Areas/Inventory/Views/Server/ServerRelatedCases.cshtml", model);
            }
            if (inventoryType == CurrentModes.Printers)
            {
                var model = new PrinterRelatedCasesModel(inventoryId)
                {
                    RelatedCases = GetRelatedCaseModel(inventoryId, sortBy, sortByAsc, inventoryType)
                };
                return View("~/Areas/Inventory/Views/Printer/PrinterRelatedCases.cshtml", model);
            }
            if (inventoryType == CurrentModes.Custom)
            {
                var inventory = _inventoryService.GetInventory(inventoryId);
                var inventoryTypeName = "Custom";
                var inventoryTypeId = 0;
                if (inventory != null)
                {
                    inventoryTypeName = inventory.Inventory.InventoryTypeName;
                    inventoryTypeId = inventory.Inventory.InventoryTypeId;
                }
                var model = new CustomInventoryRelatedCasesModel(inventoryId)
                {
                    RelatedCases = GetRelatedCaseModel(inventoryId, sortBy, sortByAsc, inventoryType),
                    InventoryTypeId = inventoryTypeId,
                    InventoryName = inventoryTypeName
                };
                
                return View("~/Areas/Inventory/Views/CustomInventory/CustomInventoryRelatedCases.cshtml", model);
            }
            return RedirectToAction("Index", "Workstation");
        }

        [HttpGet]
        public PartialViewResult RelatedCasesRows(int inventoryId, CurrentModes inventoryType, string sortBy = CaseSortField.CaseNumber, bool sortByAsc = true)
        {
            var model = GetRelatedCaseModel(inventoryId, sortBy, sortByAsc, inventoryType);
            return PartialView("_CaseRows", model.SearchResult);
        }

        private RelatedCasesModel GetRelatedCaseModel(int inventoryId, string sortBy, bool sortByAsc, CurrentModes inventoryType)
        {
            var relatedCaseIds = _inventoryService.GetRelatedCaseIds(inventoryType, inventoryId, SessionFacade.CurrentCustomer.Id).ToArray();

            var searchResult = new CaseSearchResultModel
            {
                GridSettings =
                    _caseOverviewSettingsService.GetSettings(
                        SessionFacade.CurrentCustomer.Id,
                        SessionFacade.CurrentUser.UserGroupId,
                        SessionFacade.CurrentUser.Id),
                caseSettings = this._caseSettingService.GetCaseSettingsWithUser(SessionFacade.CurrentCustomer.Id, SessionFacade.CurrentUser.Id, SessionFacade.CurrentUser.UserGroupId)
            };

            var customerId = SessionFacade.CurrentCustomer.Id;
            var currentUserId = SessionFacade.CurrentUser.Id;

            var search = _caseModelFactory.InitEmptySearchModel(SessionFacade.CurrentCustomer.Id, SessionFacade.CurrentUser.Id);
            search.Search.SortBy = sortBy;
            search.Search.Ascending = sortByAsc;
            search.CaseSearchFilter.CaseProgress = CaseProgressFilter.None;

            var showRemainingTime = SessionFacade.CurrentUser.ShowSolutionTime;

            var caseFieldSettings = _caseFieldSettingService.GetCaseFieldSettings(SessionFacade.CurrentCustomer.Id).ToArray();
            var customerSettings = _customerUserService.GetCustomerUserSettings(customerId, currentUserId);

            if (relatedCaseIds.Any())
            {
                CaseRemainingTimeData remainingTime;
                CaseAggregateData aggregateData;

                searchResult.cases = _caseSearchService.Search(
                    search.CaseSearchFilter,
                    searchResult.caseSettings,
                    caseFieldSettings,
                    SessionFacade.CurrentUser.Id,
                    SessionFacade.CurrentUser.UserId,
                    SessionFacade.CurrentUser.ShowNotAssignedWorkingGroups,
                    SessionFacade.CurrentUser.UserGroupId,
                    customerSettings.RestrictedCasePermission,
                    search.Search,
                    SessionFacade.CurrentCustomer.WorkingDayStart,
                    SessionFacade.CurrentCustomer.WorkingDayEnd,
                    TimeZoneInfo.FindSystemTimeZoneById(SessionFacade.CurrentUser.TimeZoneId),
                    ApplicationTypes.Helpdesk,
                    showRemainingTime,
                    out remainingTime,
                    out aggregateData,
                    null,
                    null,
                    relatedCaseIds).Items;
                searchResult.cases = CommonHelper.TreeTranslate(searchResult.cases, SessionFacade.CurrentCustomer.Id, _productAreaService);
            }
            else
            {
                searchResult.cases = new List<CaseSearchResult>();
            }
            var model = new RelatedCasesModel
            {
                InventoryType = inventoryType,
                SortBy = sortBy,
                SearchResult = searchResult,
                InventoryId = inventoryId,
                SortByAsc = sortByAsc
            };
            return model;
        }
    }
}