using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.ExternalInvoice;
using DH.Helpdesk.BusinessData.Models.Invoice;
using DH.Helpdesk.Common.Extensions.DateTime;
using DH.Helpdesk.Domain.Invoice;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Services.Services.Invoice;
using DH.Helpdesk.Web.Areas.Invoices.Models;
using DH.Helpdesk.Web.Infrastructure;

namespace DH.Helpdesk.Web.Areas.Invoices.Controllers.WebApi
{
    public class InvoicesApiController : BaseApiController
	{
		private readonly IInvoiceService _invoiceService;
		private readonly IExternalInvoiceService _externalInvoiceService;
		private readonly ILogService _logService;

		public InvoicesApiController(IInvoiceService invoiceService,
			IExternalInvoiceService externalInvoiceService,
			ILogService logService)
		{
			_invoiceService = invoiceService;
			_externalInvoiceService = externalInvoiceService;
			_logService = logService;
		}

		[HttpGet]
		public object GetInvoicesOverviewList([FromUri]InvoiceOverviewFilterModel filter)
		{
			var customerId = SessionFacade.CurrentCustomer.Id;
			var srvModels = _invoiceService.GetInvoiceOverviewList(customerId, filter.DepartmentId, filter.DateFrom, filter.DateTo, filter.Status);

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
					Charge = y.Charge,
					Material = y.Price,
					Price = y.EquipmentPrice,
					Text = y.TextInternal,
					Overtime = y.Overtime,
					WorkingTime = y.WorkingTime,
					OvertimeHourRate = x.OvertimeHourRate,
					WorkingHourRate = x.WorkingHourRate,
					InvoiceRow = new InvoiceRowViewModel { Status = y.InvoiceRow.Status }
				}).ToList(),
				ExternalInvoices = x.ExternalInvoices.Select(y => new ExternalInvoiceItemViewModel
				{
					Id = y.Id,
					Name = y.InvoiceNumber,
					Amount = y.InvoicePrice,
					Charge = y.Charge,
					InvoiceRow = new InvoiceRowViewModel { Status = y.InvoiceRow.Status }
				}).ToList()
			}).ToList();

			return res;
		}

		[HttpPost]
		public object SaveInvoiceValues([FromBody]InvoiceValuesParams invoiceParams)
		{
			if (invoiceParams.ExternalInvoices != null && invoiceParams.ExternalInvoices.Any())
			{
				_externalInvoiceService.UpdateExternalInvoiceValues(invoiceParams.ExternalInvoices.Select(x => new ExternalInvoice
				{
					Id = x.Id,
					InvoicePrice = x.Value,
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

			return true;
		}

		[HttpPost]
		public object InvoiceAction([FromBody]InvoiceActionParams actionParams)
		{
			var invoiceHeader = new InvoiceHeader();
			invoiceHeader.CreatedById = SessionFacade.CurrentUser.Id;

			var invoiceLogActions = actionParams.LogInvoices.Where(x => x.Status == InvoiceStatus.Invoiced).ToList();
			var invoiceExternalActions = actionParams.ExternalInvoices.Where(x => x.Status == InvoiceStatus.Invoiced).ToList();
			if (invoiceLogActions.Any() || invoiceExternalActions.Any())
			{
				var invoiceRow = new InvoiceRow {Status = InvoiceStatus.Invoiced};
				invoiceRow.LogInvoices.AddRange(invoiceLogActions.Select(x => new CaseLog {Id = x.Id}));
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

			_invoiceService.SaveInvoiceActions(invoiceHeader);
			return true;
		}
	}
}
