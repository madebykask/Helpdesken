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
using DH.Helpdesk.Domain.Invoice;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Services.Services.Invoice;
using DH.Helpdesk.Web.Areas.Invoices.Models;
using DH.Helpdesk.Web.Infrastructure;
using DH.Helpdesk.Web.Infrastructure.Extensions;
using DH.Helpdesk.Web.Models.Invoice;

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

        public OverviewController(
		    IMasterDataService masterDataService,
			IDepartmentService departmentService,
			IInvoiceService invoiceService,
			ISettingService settingService,
			IGlobalSettingService globalSettingService,
            IExternalInvoiceService externalInvoiceService,
            ILogService logService)
		    : base(masterDataService)
		{
			_departmentService = departmentService;
			_invoiceService = invoiceService;
			_settingService = settingService;
			_globalSettingService = globalSettingService;
		    _externalInvoiceService = externalInvoiceService;
		    _logService = logService;
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
			var sb = new StringBuilder();
			var customerId = SessionFacade.CurrentCustomer.Id;
			var data = _invoiceService.GetInvoiceOverviewList(customerId, filter.DepartmentId, filter.DateFrom, filter.DateTo,
				filter.Status, null);

			sb.AppendFormat(@"<HTML xmlns:x=""urn:schemas-microsoft-com:office:excel"">
							<HEAD>
								<meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8"" />
								<style>
									<!--table
										@page {{
											mso-header-data:""&dhHeldesk\000ADate\: &D\000APage &P"";
											mso-page-orientation:landscape;
										}}
										br
										{{mso-data-placement:same-cell;}}
									-->
								</style>
								<!--[if gte mso 9]>
									<xml>
										<x:ExcelWorkbook>
											<x:ExcelWorksheets>
												<x:ExcelWorksheet>
													<x:Name>dhHeldesk</x:Name>
													<x:WorksheetOptions>
														<x:Print>
															<x:ValidPrinterInfo/>
														</x:Print>
													</x:WorksheetOptions>
												</x:ExcelWorksheet>
											</x:ExcelWorksheets>
										</x:ExcelWorkbook>
									 </xml>
								<![endif]-->
							</HEAD>
							<BODY>
							   <table Width=""100%"" Border=""1"" Cellspacing=""1"" cellpadding=""2"">
										  <tr>
											<td Width=""60""><b>{0}</b></td>
											<td Width=""300""><b>{1}</b></td>
											<td><b>{2}</b></td>
											<td><b>{3}</b></td>
											<td><b>{4}</b></td>
											<td><b>{5}</b></td>
											<td><b>{6}</b></td>
											<td><b>{7}</b></td>
											<td><b>{8}</b></td>
											<td><b>{9}</b></td>
										</tr>",
					Translation.GetCoreTextTranslation("Ärende"),
					Translation.GetCoreTextTranslation("Text"),
					Translation.GetCoreTextTranslation("Cat"),
					Translation.GetCoreTextTranslation("Avslutsdatum"),
					Translation.GetCoreTextTranslation("Dept"),
					Translation.GetCoreTextTranslation("Arbete"),
					$"{Translation.GetCoreTextTranslation("Arbete")} {Translation.GetCoreTextTranslation("belopp")}",
					Translation.GetCoreTextTranslation("Material"),
					Translation.GetCoreTextTranslation("Pris"),
					Translation.GetCoreTextTranslation("Fakturor")
					);

			var odd = true;
			foreach (var caseRow in data)
			{
				odd = !odd;
				sb.AppendFormat(@"
					<tr>
						<td valign=""top"" BgColor={0}>{1}</td>
						<td valign=""top"" BgColor={0}>{2}</td>
						<td valign=""top"" BgColor={0}>{3}</td>
						<td valign=""top"" BgColor={0}>{4}</td>
						<td valign=""top"" BgColor={0}>{5}</td>
						<td valign=""top"" BgColor={0}>{6} / {7}</td>
						<td valign=""top"" BgColor={0}>{8}</td>
						<td valign=""top"" BgColor={0}>{9}</td>
						<td valign=""top"" BgColor={0}>{10}</td>
						<td valign=""top"" BgColor={0}>{11}</td>
					</tr>",
					odd ? "#FFFFFF" : "#E6EDF7",
					caseRow.CaseNumber,
					caseRow.Caption,
					caseRow.Category,
					caseRow.FinishingDate?.ToString("yyyy-MM-dd") ?? "",
					caseRow.Department,
					caseRow.LogInvoices.Sum(x => x.WorkingTime),
					caseRow.LogInvoices.Sum(x => x.Overtime),
					Math.Round(((decimal)caseRow.LogInvoices.Sum(x => x.WorkingTime)) / 60 * caseRow.WorkingHourRate + ((decimal)caseRow.LogInvoices.Sum(x => x.Overtime)) / 60 * caseRow.OvertimeHourRate, 2),
					caseRow.LogInvoices.Sum(x => x.Price),
					caseRow.LogInvoices.Sum(x => x.EquipmentPrice),
					caseRow.ExternalInvoices.Sum(x => x.InvoicePrice)
					);
			}

			sb.AppendFormat(@"</table>
						</BODY>
					</HTML>");

			var bytes = Encoding.ASCII.GetBytes(sb.ToString());
			return File(bytes, "application/vnd.ms-excel", "Invoice.xls");
		}

	    [System.Web.Http.HttpGet]
	    public ActionResult InvoiceFile(Guid id)
	    {
		    var file = _invoiceService.GetInvoiceHeader(id);

			if (file == null)
				return HttpNotFound();

			var globalSetting = _globalSettingService.GetGlobalSettings().First();

			return File(Path.Combine(globalSetting.InvoiceFileFolder, file.Guid.ToString()), "text/plain", file.Name);
	    }

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
                    EquipmentPrice = x.Price,
                    Price = x.Material,
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