using DH.Helpdesk.Common.Constants;

namespace DH.Helpdesk.Web.Areas.Inventory.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Printer;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Printer;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.PrinterSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.PrinterFieldSettings;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelExport;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelExport.ExcelExport;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.Services.Concrete;
    using DH.Helpdesk.Web.Areas.Inventory.Models;
    using DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Printer;
    using DH.Helpdesk.Web.Areas.Inventory.Models.OptionsAggregates;
    using DH.Helpdesk.Web.Areas.Inventory.Models.SearchModels;
    using DH.Helpdesk.Web.Enums.Inventory;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.ActionFilters;
    using DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Inventory;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Inventory;
    using DH.Helpdesk.Services.BusinessLogic.Admin.Users;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Users;
    using DH.Helpdesk.BusinessData.Enums.Admin.Users;

    public class PrinterController : InventoryBaseController
    {
        private readonly IInventoryService inventoryService;

        private readonly IPrinterViewModelBuilder printerViewModelBuilder;

        private readonly IPrinterBuilder printerBuilder;

        private readonly IInventorySettingsService inventorySettingsService;

        private readonly IUserPermissionsChecker _userPermissionsChecker;

        public PrinterController(
            IMasterDataService masterDataService,
            IExportFileNameFormatter exportFileNameFormatter,
            IExcelFileComposer excelFileComposer,
            IOrganizationService organizationService,
            IPlaceService placeService,
            IInventoryService inventoryService,
            IPrinterViewModelBuilder printerViewModelBuilder,
            IPrinterBuilder printerBuilder,
            IUserPermissionsChecker userPermissionsChecker,
            IInventorySettingsService inventorySettingsService)
            : base(masterDataService, exportFileNameFormatter, excelFileComposer, organizationService, placeService)
        {
            this.inventoryService = inventoryService;
            this.printerViewModelBuilder = printerViewModelBuilder;
            this.printerBuilder = printerBuilder;
            this.inventorySettingsService = inventorySettingsService;
            this._userPermissionsChecker = userPermissionsChecker;
        }

        [HttpGet]
        public PartialViewResult Index()
        {
            var inventoryTypes =
                this.inventoryService.GetInventoryTypes(SessionFacade.CurrentCustomer.Id, true, CreateInventoryTypeSeparatorItem());

            SessionFacade.SavePageFilters(TabName.Inventories, new InventoriesModeFilter((int)CurrentModes.Printers));
            PrinterSearchFilter currentFilter =
                SessionFacade.FindPageFilters<PrinterSearchFilter>(PrinterSearchFilter.CreateFilterId()) ??
                PrinterSearchFilter.CreateDefault();

            var departments = this.OrganizationService.GetDepartments(SessionFacade.CurrentCustomer.Id);
            PrinterFieldsSettingsOverviewForFilter settings =
                this.inventorySettingsService.GetPrinterFieldSettingsOverviewForFilter(
                    SessionFacade.CurrentCustomer.Id,
                    SessionFacade.CurrentLanguageId);

            var userHasInventoryAdminPermission = this._userPermissionsChecker.UserHasPermission(UsersMapper.MapToUser(SessionFacade.CurrentUser), UserPermission.InventoryPermission);

            PrinterSearchViewModel viewModel = PrinterSearchViewModel.BuildViewModel(
                currentFilter,
                departments,
                settings,
                (int)CurrentModes.Printers,
                inventoryTypes);

            viewModel.UserHasInventoryAdminPermission = userHasInventoryAdminPermission;

            return this.PartialView(viewModel);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public PartialViewResult PrintersGrid(PrinterSearchFilter filter)
        {
            SessionFacade.SavePageFilters(PrinterSearchFilter.CreateFilterId(), filter);
            filter.RecordsCount = SearchFilter.RecordsOnPage;
            var viewModel = this.CreateInventoryGridModel(filter);

            return this.PartialView("InventoryGrid", viewModel);
        }

        [HttpGet]
        public ViewResult Edit(int id, bool dialog = false)
        {
            var userHasInventoryAdminPermission = this._userPermissionsChecker.UserHasPermission(UsersMapper.MapToUser(SessionFacade.CurrentUser), UserPermission.InventoryPermission);
            var readOnly = !userHasInventoryAdminPermission && dialog;

            PrinterForRead model = this.inventoryService.GetPrinter(id);
            PrinterEditOptions options = this.GetPrinterEditOptions(SessionFacade.CurrentCustomer.Id);
            PrinterFieldsSettingsForModelEdit settings =
                this.inventorySettingsService.GetPrinterFieldSettingsForModelEdit(
                    SessionFacade.CurrentCustomer.Id,
                    SessionFacade.CurrentLanguageId, readOnly);

            PrinterViewModel printerEditModel = this.printerViewModelBuilder.BuildViewModel(model, options, settings);
            printerEditModel.IsForDialog = dialog;

            var viewModel = new PrinterEditViewModel(id, printerEditModel)
            {
                UserHasInventoryAdminPermission = userHasInventoryAdminPermission,
                IsForDialog = dialog
            };

            return this.View("Edit", viewModel);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public RedirectToRouteResult Edit(PrinterViewModel printerViewModel)
        {
            PrinterForUpdate businessModel = this.printerBuilder.BuildForUpdate(printerViewModel, this.OperationContext);
            this.inventoryService.UpdatePrinter(businessModel, this.OperationContext);

            if (printerViewModel.IsForDialog)
            {
                return RedirectToAction("Edit", new { id = printerViewModel.Id, dialog = printerViewModel.IsForDialog });
            }
            return this.RedirectToAction("Index");
        }

        [HttpGet]
        public ViewResult New()
        {
            PrinterEditOptions options = this.GetPrinterEditOptions(SessionFacade.CurrentCustomer.Id);
            PrinterFieldsSettingsForModelEdit settings =
                this.inventorySettingsService.GetPrinterFieldSettingsForModelEdit(
                    SessionFacade.CurrentCustomer.Id,
                    SessionFacade.CurrentLanguageId);

            PrinterViewModel printerEditModel = this.printerViewModelBuilder.BuildViewModel(
                options,
                settings,
                SessionFacade.CurrentCustomer.Id);

            return this.View("New", printerEditModel);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public RedirectToRouteResult New(PrinterViewModel printerViewModel)
        {
            PrinterForInsert businessModel = this.printerBuilder.BuildForAdd(printerViewModel, this.OperationContext);
            this.inventoryService.AddPrinter(businessModel, this.OperationContext);

            return this.RedirectToAction("Index");
        }

        [HttpPost]
        public RedirectToRouteResult Delete(int id)
        {
            this.inventoryService.DeletePrinter(id);

            return this.RedirectToAction("Index");
        }

        [HttpGet]
        public FileContentResult ExportGridToExcelFile()
        {
            var printerFilter =
                SessionFacade.FindPageFilters<PrinterSearchFilter>(PrinterSearchFilter.CreateFilterId()) ?? 
                PrinterSearchFilter.CreateDefault();

            // do not save to keep records limit for UI
            printerFilter.RecordsCount = null;

            var gridModel = CreateInventoryGridModel(printerFilter);
            
            var file = CreateExcelReport(CurrentModes.Printers.ToString(), gridModel.Headers, gridModel.Inventories);
            return file;
        }

        private PrinterEditOptions GetPrinterEditOptions(int customerId)
        {
            List<ItemOverview> departments =
                this.OrganizationService.GetDepartments(customerId).OrderBy(x => x.Name).ToList();
            List<ItemOverview> buildings = this.PlaceService.GetBuildings(customerId).OrderBy(x => x.Name).ToList();
            List<ItemOverview> floors = this.PlaceService.GetFloors(customerId).OrderBy(x => x.Name).ToList();
            List<ItemOverview> rooms = this.PlaceService.GetRooms(customerId).OrderBy(x => x.Name).ToList();

            return new PrinterEditOptions(departments, buildings, floors, rooms);
        }

        private InventoryGridModel CreateInventoryGridModel(PrinterSearchFilter filter)
        {
            var settings = 
                inventorySettingsService.GetPrinterFieldSettingsOverview(
                    SessionFacade.CurrentCustomer.Id,
                    SessionFacade.CurrentLanguageId);

            var models =
                inventoryService.GetPrinters(filter.CreateRequest(SessionFacade.CurrentCustomer.Id));

            var viewModel = InventoryGridModel.BuildModel(models, settings, filter.SortField);
            return viewModel;
        }
    }
}