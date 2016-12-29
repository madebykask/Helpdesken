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

namespace DH.Helpdesk.Web.Areas.Invoices.Controllers
{
    public class OverviewController : BaseController
    {
	    private readonly IDepartmentService _departmentService;
		private readonly IInvoiceService _invoiceService;

		public OverviewController(
		    IMasterDataService masterDataService,
			IDepartmentService departmentService,
			IInvoiceService invoiceService)
		    : base(masterDataService)
		{
			_departmentService = departmentService;
			_invoiceService = invoiceService;
		}

		// GET: Invoices/Overview
		public ActionResult Index()
		{
			ViewBag.Departments = _departmentService.GetChargedDepartments(SessionFacade.CurrentCustomer.Id)
				.Select(x => new SelectListItem
				{
					Text = x.DepartmentName,
					Value = x.Id.ToString()
				}).ToList();
			ViewBag.Statuses = new List<SelectListItem>
			{
				new SelectListItem {Text = Translation.GetCoreTextTranslation("Ej debiterade"), Value = ((int)InvoiceStatus.No).ToString()},
				new SelectListItem
				{
					Text = $"{Translation.GetCoreTextTranslation("Klara")} ({Translation.GetCoreTextTranslation("Debiterade")})",
					Value = ((int)InvoiceStatus.Invoiced).ToString()
				},
				new SelectListItem
				{
					Text = $"{Translation.GetCoreTextTranslation("Klara")} ({Translation.GetCoreTextTranslation("Ej Debiterade")})",
					Value = ((int)InvoiceStatus.NotInvoiced).ToString()
				},
			};

			var model = new InvoiceOverviewFilterModel();
			return View(model);
        }

		[System.Web.Http.HttpGet]
		public ActionResult InvoiceExport(InvoiceOverviewFilterModel filter)
		{
			var sb = new StringBuilder();
			var customerId = SessionFacade.CurrentCustomer.Id;
			var data = _invoiceService.GetInvoiceOverviewList(customerId, filter.DepartmentId, filter.DateFrom, filter.DateTo,
				filter.Status);

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
					caseRow.LogInvoices.Sum(x => x.WorkingTime) * caseRow.WorkingHourRate + caseRow.LogInvoices.Sum(x => x.Overtime) * caseRow.OvertimeHourRate,
					caseRow.LogInvoices.Sum(x => x.Price),
					caseRow.LogInvoices.Sum(x => x.EquipmentPrice),
					caseRow.ExternalInvoices.Sum(x => x.InvoicePrice)
					);
			}

			sb.AppendFormat(@"</table>
						</BODY>
					</HTML>");

			var bytes = Encoding.ASCII.GetBytes(sb.ToString());
			return File(
				   bytes,
				   "application/vnd.ms-excel", "Invoice.xls");
		}
	}
}