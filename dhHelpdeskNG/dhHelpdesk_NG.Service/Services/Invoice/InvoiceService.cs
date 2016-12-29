using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.ExternalInvoice;
using DH.Helpdesk.BusinessData.Models.Invoice;
using DH.Helpdesk.Dal.Repositories;
using DH.Helpdesk.Dal.Repositories.Cases;
using DH.Helpdesk.Dal.Repositories.Invoice;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Domain.Invoice;
using InvoiceHeader = DH.Helpdesk.BusinessData.Models.Invoice.InvoiceHeader;
using InvoiceRow = DH.Helpdesk.BusinessData.Models.Invoice.InvoiceRow;

namespace DH.Helpdesk.Services.Services.Invoice
{
	public class InvoiceService : IInvoiceService
	{
		private readonly IInvoiceRepository _invoiceRepository;
		private readonly IInvoiceHeaderRepository _invoiceHeaderRepository;
		private readonly IInvoiceRowRepository _invoiceRowRepository;
		private readonly ILogRepository _logRepository;
		private readonly ICaseInvoiceRowRepository _caseInvoiceRowRepository;

		public InvoiceService(IInvoiceRepository invoiceRepository,
			IInvoiceHeaderRepository invoiceHeaderRepository,
			IInvoiceRowRepository invoiceRowRepository,
			ILogRepository logRepository,
			ICaseInvoiceRowRepository caseInvoiceRowRepository)
		{
			_invoiceRepository = invoiceRepository;
			_invoiceHeaderRepository = invoiceHeaderRepository;
			_invoiceRowRepository = invoiceRowRepository;
			_logRepository = logRepository;
			_caseInvoiceRowRepository = caseInvoiceRowRepository;
		}

		public List<InvoiceOverview> GetInvoiceOverviewList(int customerId, int? departmentId, DateTime? dateFrom, DateTime? dateTo, InvoiceStatus? status)
		{
			var dbModels = _invoiceRepository.GetInvoiceOverviewList(customerId, departmentId, dateFrom, dateTo, status);
			var res = new List<InvoiceOverview>();
			foreach (var dbModel in dbModels)
			{
				var item = new InvoiceOverview();
				item.CaseId = dbModel.Id;
				item.Caption = dbModel.Caption;
				item.CaseNumber = dbModel.CaseNumber;
				item.FinishingDate = dbModel.FinishingDate;
				item.Department = dbModel.Department?.DepartmentName;
				item.Category = dbModel.Category?.Name;
				item.WorkingHourRate = dbModel.Department?.AccountancyAmount ?? 0;
				item.OvertimeHourRate = dbModel.Department?.OverTimeAmount ?? 0;
				item.ExternalInvoices = dbModel.CaseInvoiceRows.Select(x => new ExternalInvoice
				{
					Id	= x.Id,
					CaseId = x.Case_Id,
					Charge = x.Charge == 1,
					InvoiceNumber = x.InvoiceNumber,
					InvoicePrice = x.InvoicePrice,
					CreatedByUserId = x.CreatedByUser_Id,
					CreatedDate = x.CreatedDate,
					InvoiceRow = new InvoiceRow { Status = x.InvoiceRow?.Status }
				}).ToList();
				item.LogInvoices = dbModel.Logs.Select(x => new CaseLog
				{
					LogDate = x.LogDate,
					RegUser = x.RegUser,
					LogType = x.LogType,
					LogGuid = x.LogGUID,
					Id = x.Id,
					CaseId = x.Case_Id,
					UserId = x.User_Id,
					Charge = (x.Charge == 1 ? true : false),
					EquipmentPrice = x.EquipmentPrice,
					Price = x.Price,
					FinishingDate = x.FinishingDate,
					FinishingType = x.FinishingType,
					SendMailAboutCaseToNotifier = x.InformCustomer == 1 ? true : false,
					TextExternal = string.IsNullOrWhiteSpace(x.Text_External) ? string.Empty : x.Text_External,
					TextInternal = string.IsNullOrWhiteSpace(x.Text_Internal) ? string.Empty : x.Text_Internal,
					CaseHistoryId = x.CaseHistory_Id,
					WorkingTime = x.WorkingTime,
					Overtime = x.OverTime,
					InvoiceRow = new InvoiceRow { Status = x.InvoiceRow?.Status}
				}).ToList();

				res.Add(item);
			}

			return res;
		}

		public void SaveInvoiceActions(InvoiceHeader invoiceHeader)
		{
			var now = DateTime.Now;
			var dbInvoiceHeader = new Domain.InvoiceHeader
			{
				CreatedDate = now,
				User_Id = invoiceHeader.CreatedById,
				InvoiceFilename = "test",
				InvoiceHeaderGUID = Guid.NewGuid()
			};
			_invoiceHeaderRepository.Add(dbInvoiceHeader);
			foreach (var invoiceRow in invoiceHeader.InvoiceRows)
			{
				if (!invoiceRow.Status.HasValue)
					continue;
				var dbInvoiceRow = new Domain.InvoiceRow
				{
					InvoiceHeader = dbInvoiceHeader,
					CreatedDate = now,
					ChangedDate = now,
					CreatedByUser_Id = invoiceHeader.CreatedById,
					Status = invoiceRow.Status.Value,
				};

				foreach (var logInvoice in invoiceRow.LogInvoices)
				{
					var dbLog = _logRepository.GetById(logInvoice.Id);
					dbLog.InvoiceRow = dbInvoiceRow;
				}

				foreach (var externalInvoice in invoiceRow.ExternalInvoices)
				{
					var dbcaseInvoiceRow = _caseInvoiceRowRepository.GetById(externalInvoice.Id);
					dbcaseInvoiceRow.InvoiceRow = dbInvoiceRow;
				}

				_invoiceRowRepository.Add(dbInvoiceRow);
			}

			_invoiceHeaderRepository.Commit();
		}
	}

	public interface IInvoiceService
	{
		List<InvoiceOverview> GetInvoiceOverviewList(int customerId, int? departmentId, DateTime? dateFrom, DateTime? dateTo, InvoiceStatus? status);
		void SaveInvoiceActions(InvoiceHeader invoiceHeader);
	}
}
