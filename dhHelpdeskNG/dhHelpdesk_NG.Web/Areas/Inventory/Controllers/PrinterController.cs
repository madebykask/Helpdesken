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

    public class PrinterController : InventoryBaseController
    {
        private readonly IInventoryService inventoryService;

        private readonly IPrinterViewModelBuilder printerViewModelBuilder;

        private readonly IPrinterBuilder printerBuilder;

        private readonly IInventorySettingsService inventorySettingsService;

        public PrinterController(
            IMasterDataService masterDataService,
            IExportFileNameFormatter exportFileNameFormatter,
            IExcelFileComposer excelFileComposer,
            IOrganizationService organizationService,
            IPlaceService placeService,
            IInventoryService inventoryService,
            IPrinterViewModelBuilder printerViewModelBuilder,
            IPrinterBuilder printerBuilder,
            IInventorySettingsService inventorySettingsService)
            : base(masterDataService, exportFileNameFormatter, excelFileComposer, organizationService, placeService)
        {
            this.inventoryService = inventoryService;
            this.printerViewModelBuilder = printerViewModelBuilder;
            this.printerBuilder = printerBuilder;
            this.inventorySettingsService = inventorySettingsService;
        }

        [HttpGet]
        public PartialViewResult Index()
        {
            List<ItemOverview> inventoryTypes = this.inventoryService.GetInventoryTypes(
                SessionFacade.CurrentCustomer.Id);

            SessionFacade.SavePageFilters(TabName.Inventories, new InventoriesModeFilter((int)CurrentModes.Printers));
            PrinterSearchFilter currentFilter =
                SessionFacade.FindPageFilters<PrinterSearchFilter>(
                    this.CreateFilterId(TabName.Inventories, InventoryFilterMode.Printer.ToString()))
                ?? PrinterSearchFilter.CreateDefault();

            List<ItemOverview> departments = this.OrganizationService.GetDepartments(SessionFacade.CurrentCustomer.Id);
            PrinterFieldsSettingsOverviewForFilter settings =
                this.inventorySettingsService.GetPrinterFieldSettingsOverviewForFilter(
                    SessionFacade.CurrentCustomer.Id,
                    SessionFacade.CurrentLanguageId);

            PrinterSearchViewModel viewModel = PrinterSearchViewModel.BuildViewModel(
                currentFilter,
                departments,
                settings,
                (int)CurrentModes.Printers,
                inventoryTypes);

            return this.PartialView(viewModel);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public PartialViewResult PrintersGrid(PrinterSearchFilter filter)
        {
            SessionFacade.SavePageFilters(
                this.CreateFilterId(TabName.Inventories, InventoryFilterMode.Printer.ToString()),
                filter);

            InventoryGridModel viewModel = this.CreateInventoryGridModel(filter);

            return this.PartialView("InventoryGrid", viewModel);
        }

        [HttpGet]
        public ViewResult Edit(int id)
        {
            PrinterForRead model = this.inventoryService.GetPrinter(id);
            PrinterEditOptions options = this.GetPrinterEditOptions(SessionFacade.CurrentCustomer.Id);
            PrinterFieldsSettingsForModelEdit settings =
                this.inventorySettingsService.GetPrinterFieldSettingsForModelEdit(
                    SessionFacade.CurrentCustomer.Id,
                    SessionFacade.CurrentLanguageId);

            PrinterViewModel printerEditModel = this.printerViewModelBuilder.BuildViewModel(model, options, settings);

            return this.View("Edit", printerEditModel);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public RedirectToRouteResult Edit(PrinterViewModel printerViewModel)
        {
            PrinterForUpdate businessModel = this.printerBuilder.BuildForUpdate(printerViewModel, this.OperationContext);
            this.inventoryService.UpdatePrinter(businessModel, this.OperationContext);

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

        [HttpGet]
        public RedirectToRouteResult Delete(int id)
        {
            this.inventoryService.DeletePrinter(id);

            return this.RedirectToAction("Index");
        }

        [HttpGet]
        public FileContentResult ExportGridToExcelFile()
        {
            var printerFilter =
                SessionFacade.FindPageFilters<PrinterSearchFilter>(
                    this.CreateFilterId(TabName.Inventories, InventoryFilterMode.Printer.ToString()))
                ?? PrinterSearchFilter.CreateDefault();
            var gridModel = this.CreateInventoryGridModel(printerFilter);
            string workSheetName = CurrentModes.Printers.ToString();
            FileContentResult file = this.CreateExcelReport(workSheetName, gridModel.Headers, gridModel.Inventories);

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
            PrinterFieldsSettingsOverview settings =
                this.inventorySettingsService.GetPrinterFieldSettingsOverview(
                    SessionFacade.CurrentCustomer.Id,
                    SessionFacade.CurrentLanguageId);
            List<PrinterOverview> models =
                this.inventoryService.GetPrinters(filter.CreateRequest(SessionFacade.CurrentCustomer.Id));

            InventoryGridModel viewModel = InventoryGridModel.BuildModel(models, settings, filter.SortField);
            return viewModel;
        }
    }
}