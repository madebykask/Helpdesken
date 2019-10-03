using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.ExternalInvoice;
using DH.Helpdesk.BusinessData.Models.Invoice;
using DH.Helpdesk.Common.Constants;
using DH.Helpdesk.Domain.Invoice;
using DH.Helpdesk.Services.BusinessLogic.BusinessModelExport;
using DH.Helpdesk.Services.BusinessLogic.BusinessModelExport.ExcelExport;
using DH.Helpdesk.Services.DisplayValues;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Services.Services.Invoice;
using DH.Helpdesk.Web.Areas.Invoices.Models;
using DH.Helpdesk.Web.Areas.OrderAccounts.Models.Order;
using DH.Helpdesk.Web.Enums;
using DH.Helpdesk.Web.Infrastructure;
using DH.Helpdesk.Web.Infrastructure.Attributes;
using DH.Helpdesk.Web.Infrastructure.Extensions;
using DH.Helpdesk.Web.Models.Invoice;
using DH.Helpdesk.Web.Models.Shared;

namespace DH.Helpdesk.Web.Areas.Invoices.Controllers
{
    public class OverviewController : BaseController
    {
        private readonly IDepartmentService _departmentService;
        private readonly IInvoiceService _invoiceService;
        private readonly ISettingService _settingService;
        private readonly IGlobalSettingService _globalSettingService;
        private readonly IExternalInvoiceService _externalInvoiceService;
        private readonly ILogService _logService;

        private readonly IExportFileNameFormatter _exportFileNameFormatter;
        private readonly IExcelFileComposer _excelFileComposer;

        public OverviewController(
            IMasterDataService masterDataService,
            IDepartmentService departmentService,
            IInvoiceService invoiceService,
            ISettingService settingService,
            IGlobalSettingService globalSettingService,
            IExternalInvoiceService externalInvoiceService,
            IExportFileNameFormatter exportFileNameFormatter,
            IExcelFileComposer excelFileComposer,
            ILogService logService)
            : base(masterDataService)
        {
            _departmentService = departmentService;
            _invoiceService = invoiceService;
            _settingService = settingService;
            _globalSettingService = globalSettingService;
            _externalInvoiceService = externalInvoiceService;
            _logService = logService;
            _exportFileNameFormatter = exportFileNameFormatter;
            _excelFileComposer = excelFileComposer;
        }

        // GET: Invoices/Overview
        public ActionResult Index()
        {
            var customerId = SessionFacade.CurrentCustomer.Id;

            ViewBag.Departments = _departmentService.GetChargedDepartments(customerId)
                .Select(x => new SelectListItem
                {
                    Text = x.DepartmentName,
                    Value = x.Id.ToString()
                }).ToList();

            var statuses = InvoiceStatus.No.ToSelectListItems();
            statuses.RemoveAll(x => x.Value == InvoiceStatus.No.ToInt().ToString());
            ViewBag.Statuses = statuses;

            var settings = _settingService.GetCustomerSetting(customerId);

            ViewBag.MinStep = settings.MinRegWorkingTime;

            var model = new InvoiceOverviewViewModel { Filter = new InvoiceOverviewFilterModel { Status = InvoiceStatus.Ready }, ShowFiles = settings.InvoiceType == 2 };
            return View(model);
        }

        public ActionResult Files()
        {
            var customerId = SessionFacade.CurrentCustomer.Id;

            var settings = _settingService.GetCustomerSetting(customerId);

            if (settings.InvoiceType != 2)
                return new HttpNotFoundResult();

            var files = _invoiceService.GetInvoiceHeaders(customerId).Select(x => new InvoiceFileViewModel
            {
                Guid = x.Guid,
                Date = x.Date,
                Name = x.Name
            }).ToList();

            var filesModel = files.GroupBy(x => x.Date.ToString("yyyy"))
                .ToDictionary(x => x.Key.ToString(), x => x.GroupBy(y => y.Date.ToString("MM"))
                    .ToDictionary(y => y.Key, y => y.Select(z => z).ToList()));

            var model = new InvoiceFilesViewModel {ShowFiles = true, Files = filesModel };

            return View(model);
        }

        [System.Web.Http.HttpGet]
        public ActionResult InvoiceExport(InvoiceOverviewFilterModel filter)
        {
            const string name = "Invoice";
            const string sheetName = "dhHelpdesk";

            var customerId = SessionFacade.CurrentCustomer.Id;
            var data = _invoiceService.GetInvoiceOverviewList(customerId, filter.DepartmentId, filter.DateFrom, filter.DateTo, filter.Status, null);
            data = data.OrderBy(x => x.CaseNumber).ToList();

            var headers = new List<GridColumnHeaderModel>
            {
                new GridColumnHeaderModel("2", Translation.GetCoreTextTranslation("Ärende")),
                new GridColumnHeaderModel ("3", Translation.GetCoreTextTranslation("Text")),
                new GridColumnHeaderModel ("4", Translation.GetCoreTextTranslation("Kategori")),
                new GridColumnHeaderModel ("5", Translation.GetCoreTextTranslation("Avslutsdatum")),
                new GridColumnHeaderModel ("6", Translation.GetCoreTextTranslation("Avdelning")),
                new GridColumnHeaderModel ("7", Translation.GetCoreTextTranslation("Arbete")),
                new GridColumnHeaderModel ("8", Translation.GetCoreTextTranslation("Övertid")),
                new GridColumnHeaderModel ("9", $"{Translation.GetCoreTextTranslation("Arbete")} {Translation.GetCoreTextTranslation("belopp")}"),
                new GridColumnHeaderModel ("10", Translation.GetCoreTextTranslation("Material")),
                new GridColumnHeaderModel ("11", Translation.GetCoreTextTranslation("Pris")),
                new GridColumnHeaderModel ("12", Translation.GetCoreTextTranslation("Fakturor"))
            };

            var rows = new List<RowModel>();
            var i = 1;
            foreach (var inv in data)
            {
                var fields = new List<NewGridRowCellValueModel>
                {
                    new NewGridRowCellValueModel("2", new StringDisplayValue(inv.CaseNumber.ToString("F0"))),
                    new NewGridRowCellValueModel("3", new StringDisplayValue(inv.Caption)),
                    new NewGridRowCellValueModel("4", new StringDisplayValue(inv.Category)),
                    new NewGridRowCellValueModel("5", new StringDisplayValue(inv.FinishingDate?.ToString(DateFormats.Date) ?? "")),
                    new NewGridRowCellValueModel("6", new StringDisplayValue(inv.Department)),
                    new NewGridRowCellValueModel("7", new StringDisplayValue($"{inv.LogInvoices.Sum(x => x.WorkingTime)}")),
                    new NewGridRowCellValueModel("8", new StringDisplayValue($"{inv.LogInvoices.Sum(x => x.Overtime)}")),
                    new NewGridRowCellValueModel("9", new StringDisplayValue(((decimal)inv.LogInvoices.Sum(x => x.WorkingTime) / 60 * inv.WorkingHourRate + (decimal)inv.LogInvoices.Sum(x => x.Overtime) / 60 * inv.OvertimeHourRate).ToString("F"))),
                    new NewGridRowCellValueModel("10", new StringDisplayValue(inv.LogInvoices.Sum(x => x.Price).ToString("F0"))),
                    new NewGridRowCellValueModel("11", new StringDisplayValue(inv.LogInvoices.Sum(x => x.EquipmentPrice).ToString("F0"))),
                    new NewGridRowCellValueModel("12", new StringDisplayValue(inv.ExternalInvoices.Sum(x => x.InvoicePrice).ToString("F0")))
                };
                rows.Add(new RowModel(i, fields));
                i++;
            }
            var content = _excelFileComposer.Compose(headers, rows, sheetName);

            var fileName = _exportFileNameFormatter.Format(name, "xlsx");
            return File(content, MimeType.ExcelFile, fileName);
        }

        [System.Web.Http.HttpGet]
        public ActionResult InvoiceFile(Guid id)
        {
            var file = _invoiceService.GetInvoiceHeader(id);

            if (file == null)
                return HttpNotFound();

            var globalSetting = _globalSettingService.GetGlobalSettings().First();

            return File(Path.Combine(globalSetting.InvoiceFileFolder, file.Name), "text/plain", file.Name);
        }

        [OutputCache(NoStore = true, Duration = 0)]
        [System.Web.Mvc.HttpGet]
        public ActionResult GetInvoicesOverviewList(InvoiceOverviewFilterModel filter)
        {
            var customerId = SessionFacade.CurrentCustomer.Id;
            var srvModels = _invoiceService.GetInvoiceOverviewList(customerId, filter.DepartmentId, filter.DateFrom, filter.DateTo, filter.Status, filter.CaseId, filter.DepartmentCharge);

            var res = srvModels.Select(x => new InvoiceListItemViewModel
            {
                CaseId = x.CaseId,
                CaseNumber = x.CaseNumber.ToString(),
                Caption = x.Caption,
                Department = x.Department,
                Category = x.Category,
                FinishingDate = x.FinishingDate?.Date,
                Statistics = x.Statistics,
                LogInvoices = x.LogInvoices.Select(y => new LogInvoiceItemViewModel
                {
                    Id = y.Id,
                    Date = y.LogDate,
                    Charge = y.Charge,
                    Material = y.EquipmentPrice,
                    Price = y.Price,
                    Text = y.TextInternal,
                    Overtime = y.Overtime,
                    WorkingTime = y.WorkingTime,
                    OvertimeHourRate = x.OvertimeHourRate,
                    WorkingHourRate = x.WorkingHourRate,
                    InvoiceRow = new InvoiceRowViewModel { Status = y.InvoiceRow.Status }
                }).ToList(),
                ExternalInvoices = x.ExternalInvoices.Select(y => new ExternalInvoiceModel
                {
                    Id = y.Id,
                    Name = y.InvoiceNumber,
                    Amount = y.InvoicePrice,
                    Charge = y.Charge,
                    InvoiceRow = new InvoiceRowViewModel { Status = y.InvoiceRow.Status }
                }).ToList()
            }).ToList();

            return JsonDefault(res);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult SaveInvoiceValues(InvoiceValuesParams invoiceParams)
        {
            if (invoiceParams.ExternalInvoices != null && invoiceParams.ExternalInvoices.Any())
            {
                _externalInvoiceService.UpdateExternalInvoiceValues(invoiceParams.ExternalInvoices.Select(x => new ExternalInvoice
                {
                    Id = x.Id,
                    InvoicePrice = x.Amount,
                    Charge = x.Charge
                }).ToList());
            }

            if (invoiceParams.LogInvoices != null && invoiceParams.LogInvoices.Any())
            {
                _logService.UpdateLogInvoices(invoiceParams.LogInvoices.Select(x => new CaseLog
                {
                    Id = x.Id,
                    WorkingTime = x.WorkingTime,
                    Overtime = x.Overtime,
                    EquipmentPrice = x.Material,
                    Price = x.Price,
                    Charge = x.Charge
                }).ToList());
            }

            return JsonDefault(true);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult InvoiceAction(InvoiceActionParams actionParams)
        {
            var customerId = SessionFacade.CurrentCustomer.Id;
            var invoiceHeader = new InvoiceHeader();
            invoiceHeader.CreatedById = SessionFacade.CurrentUser.Id;

            var invoiceLogActions = actionParams.LogInvoices.Where(x => x.Status == InvoiceStatus.Invoiced).ToList();
            var invoiceExternalActions = actionParams.ExternalInvoices.Where(x => x.Status == InvoiceStatus.Invoiced).ToList();
            if (invoiceLogActions.Any() || invoiceExternalActions.Any())
            {
                var invoiceRow = new InvoiceRow { Status = InvoiceStatus.Invoiced };
                invoiceRow.LogInvoices.AddRange(invoiceLogActions.Select(x => new CaseLog { Id = x.Id }));
                invoiceRow.ExternalInvoices.AddRange(invoiceExternalActions.Select(x => new ExternalInvoice { Id = x.Id }));
                invoiceHeader.InvoiceRows.Add(invoiceRow);
            }

            var notInvoiceLogActions = actionParams.LogInvoices.Where(x => x.Status == InvoiceStatus.NotInvoiced).ToList();
            var notInvoiceExternalActions = actionParams.ExternalInvoices.Where(x => x.Status == InvoiceStatus.NotInvoiced).ToList();
            if (notInvoiceLogActions.Any() || notInvoiceExternalActions.Any())
            {
                var invoiceRow = new InvoiceRow { Status = InvoiceStatus.NotInvoiced };
                invoiceRow.LogInvoices.AddRange(notInvoiceLogActions.Select(x => new CaseLog { Id = x.Id }));
                invoiceRow.ExternalInvoices.AddRange(notInvoiceExternalActions.Select(x => new ExternalInvoice { Id = x.Id }));
                invoiceHeader.InvoiceRows.Add(invoiceRow);
            }

            var translations = new List<string>
            {
                Translation.GetCoreTextTranslation("ServiceDesk (direktdebitering)"),
                Translation.GetCoreTextTranslation("ÄrNr"),
                Translation.GetCoreTextTranslation("FakturaNr"),
                Translation.GetCoreTextTranslation("RefNr"),
                Translation.GetCoreTextTranslation("DH"),
            };
            _invoiceService.SaveInvoiceActions(customerId, invoiceHeader, translations);

            return JsonDefault(true);
        }
    }
}