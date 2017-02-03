using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using DH.Helpdesk.Domain.Invoice;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Services.Services.Invoice;
using DH.Helpdesk.Web.Areas.Invoices.Models;
using DH.Helpdesk.Web.Infrastructure;
using DH.Helpdesk.Web.Infrastructure.Extensions;

namespace DH.Helpdesk.Web.Areas.Invoices.Controllers
{
    public class OverviewController : BaseController
    {
	    private readonly IDepartmentService _departmentService;
		private readonly IInvoiceService _invoiceService;
	    private readonly ISettingService _settingService;
		private readonly IGlobalSettingService _globalSettingService;

		public OverviewController(
		    IMasterDataService masterDataService,
			IDepartmentService departmentService,
			IInvoiceService invoiceService,
			ISettingService settingService,
			IGlobalSettingService globalSettingService)
		    : base(masterDataService)
		{
			_departmentService = departmentService;
			_invoiceService = invoiceService;
			_settingService = settingService;
			_globalSettingService = globalSettingService;
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
	}
}