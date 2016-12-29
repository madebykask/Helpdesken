using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DH.Helpdesk.Domain.Invoice;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Web.Infrastructure;

namespace DH.Helpdesk.Web.Areas.Invoices.Controllers
{
    public class OverviewController : BaseController
    {
	    private readonly IDepartmentService _departmentService;

		public OverviewController(
		    IMasterDataService masterDataService,
			IDepartmentService departmentService)
		    : base(masterDataService)
		{
			_departmentService = departmentService;
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
			return View();
        }
    }
}