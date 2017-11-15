using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.ExternalInvoice;
using DH.Helpdesk.BusinessData.Models.Invoice;
using DH.Helpdesk.Common.Extensions.Boolean;
using DH.Helpdesk.Common.Extensions.Integer;
using DH.Helpdesk.Dal.Repositories;
using DH.Helpdesk.Dal.Repositories.Cases;
using DH.Helpdesk.Dal.Repositories.Invoice;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Domain.Invoice;
using InvoiceHeader = DH.Helpdesk.BusinessData.Models.Invoice.InvoiceHeader;
using InvoiceRow = DH.Helpdesk.BusinessData.Models.Invoice.InvoiceRow;
using System = DH.Helpdesk.Domain.System;

namespace DH.Helpdesk.Services.Services.Invoice
{
	public class InvoiceService : IInvoiceService
	{
		private readonly IInvoiceRepository _invoiceRepository;
		private readonly IInvoiceHeaderRepository _invoiceHeaderRepository;
		private readonly IInvoiceRowRepository _invoiceRowRepository;
		private readonly ILogRepository _logRepository;
		private readonly ICaseInvoiceRowRepository _caseInvoiceRowRepository;
		private readonly ISettingService _settingService;
		private readonly IGlobalSettingService _globalSettingService;
		private readonly ICaseRepository _caseRepository;
		private readonly IOrderRepository _orderRepository;
		private readonly IAccountRepository _accountRepository;

		public InvoiceService(IInvoiceRepository invoiceRepository,
			IInvoiceHeaderRepository invoiceHeaderRepository,
			IInvoiceRowRepository invoiceRowRepository,
			ILogRepository logRepository,
			ICaseInvoiceRowRepository caseInvoiceRowRepository,
			ISettingService settingService,
			IGlobalSettingService globalSettingService,
			ICaseRepository caseRepository,
			IOrderRepository orderRepository,
			IAccountRepository accountRepository)
		{
			_invoiceRepository = invoiceRepository;
			_invoiceHeaderRepository = invoiceHeaderRepository;
			_invoiceRowRepository = invoiceRowRepository;
			_logRepository = logRepository;
			_caseInvoiceRowRepository = caseInvoiceRowRepository;
			_settingService = settingService;
			_globalSettingService = globalSettingService;
			_caseRepository = caseRepository;
			_orderRepository = orderRepository;
			_accountRepository = accountRepository;
		}

		public List<InvoiceOverview> GetInvoiceOverviewList(int customerId, int? departmentId, DateTime? dateFrom, DateTime? dateTo, InvoiceStatus? status, int? caseId, bool departmentCharge = true)
		{
			var dbModels = _invoiceRepository.GetInvoiceOverviewList(customerId, departmentId, dateFrom, dateTo, status, caseId, departmentCharge);
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

		
		public void SaveInvoiceActions(int customerId, InvoiceHeader invoiceHeader, List<string> translations)
		{
			var now = DateTime.Now;

			var fileCases = new Dictionary<int, Tuple<List<Log>, List<CaseInvoiceRow>>>();

			var dbInvoiceHeader = new Domain.InvoiceHeader
			{
				CreatedDate = now,
				User_Id = invoiceHeader.CreatedById,
				InvoiceFilename = "",
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

					if (dbInvoiceRow.Status == InvoiceStatus.Invoiced)
					{
						if (!fileCases.ContainsKey(dbLog.Case_Id))
						{
							fileCases.Add(dbLog.Case_Id, new Tuple<List<Log>, List<CaseInvoiceRow>>(new List<Log>(), new List<CaseInvoiceRow>()));
							fileCases[dbLog.Case_Id].Item1.Add(dbLog);
						}
					}
				}

				foreach (var externalInvoice in invoiceRow.ExternalInvoices)
				{
					var dbcaseInvoiceRow = _caseInvoiceRowRepository.GetById(externalInvoice.Id);
					dbcaseInvoiceRow.InvoiceRow = dbInvoiceRow;

					if (dbInvoiceRow.Status == InvoiceStatus.Invoiced)
					{
						if (!fileCases.ContainsKey(dbcaseInvoiceRow.Case_Id))
						{
							fileCases.Add(dbcaseInvoiceRow.Case_Id, new Tuple<List<Log>, List<CaseInvoiceRow>>(new List<Log>(), new List<CaseInvoiceRow>()));
						}
						fileCases[dbcaseInvoiceRow.Case_Id].Item2.Add(dbcaseInvoiceRow);
					}
				}

				_invoiceRowRepository.Add(dbInvoiceRow);
			}
            
			var setting = _settingService.GetCustomerSetting(customerId);
            if (setting.InvoiceType == 2)
            {
                var fileName = now.ToString("yyyyMMddHHmmss.eko");
				dbInvoiceHeader.InvoiceFilename = fileName;
            }

            _invoiceHeaderRepository.Commit();            

			if (setting.InvoiceType == 2)
			{
				var globalSetting = _globalSettingService.GetGlobalSettings().First();
				CreateInvoiceFile(dbInvoiceHeader.Id, 
                                  Path.Combine(globalSetting.InvoiceFileFolder, dbInvoiceHeader.InvoiceHeaderGUID.ToString()), 
                                  fileCases, translations);
			}			
		}

		public List<InvoiceFile> GetInvoiceHeaders(int customerId)
		{
			var res = _invoiceHeaderRepository.GetAll()
				.Where(x => !String.IsNullOrWhiteSpace(x.InvoiceFilename))
				.Where(x => x.InvoiceRows.Any(y => y.CaseInvoiceRows.Any(z => z.Case.Customer_Id == customerId) || y.Logs.Any(z => z.Case.Customer_Id == customerId)))
				.ToList();

			return res.Select(x => new InvoiceFile
			{
				Guid = x.InvoiceHeaderGUID,
				Date = x.CreatedDate,
				Name = x.InvoiceFilename
			}).ToList();
		}

		public InvoiceFile GetInvoiceHeader(Guid guid)
		{
			var res = _invoiceHeaderRepository.GetAll()
				.Where(x => x.InvoiceHeaderGUID == guid)
				.FirstOrDefault();

			return res == null ? null : new InvoiceFile
			{
				Guid = res.InvoiceHeaderGUID,
				Date = res.CreatedDate,
				Name = res.InvoiceFilename
			};
		}

		private void CreateInvoiceFile(int invoiceHeader_Id, string path, Dictionary<int, Tuple<List<Log>, List<CaseInvoiceRow>>> fileCases, List<string> translations)
		{
			var sb = new StringBuilder();
			foreach (var fileCase in fileCases)
			{
				var caseInfo = _caseRepository.GetCaseIncluding(fileCase.Key);
				var orderInfo = _orderRepository.GetOrder(fileCase.Key);
				var accountInfo = _accountRepository.GetAccount(fileCase.Key);

				var externalInvoices = String.Join(",", fileCase.Value.Item2.Select(x => x.InvoiceNumber));
				var referenceNumber = accountInfo?.ReferenceNumber ?? orderInfo?.ReferenceNumber;
				var amount = fileCase.Value.Item1
					             .Where(x => x.Charge.ToBool())
					             .Sum(x => ((decimal)x.WorkingTime / 60) * caseInfo.Department.AccountancyAmount +
							             ((decimal)x.OverTime / 60) * caseInfo.Department.OverTimeAmount + 
										 x.Price + 
                                         x.EquipmentPrice) +
				             fileCase.Value.Item2
					             .Where(x => x.Charge.ToBool()).Sum(x => x.InvoicePrice);

				sb.AppendLine($"{caseInfo.CaseNumber}-{invoiceHeader_Id}\t{translations[0]}\t{caseInfo.Department.DepartmentName}" +
                              $"\t{caseInfo.Workinggroup?.Code}\t{translations[1]}:{caseInfo.CaseNumber}, {caseInfo.Caption}" +
				              $"{(String.IsNullOrWhiteSpace(externalInvoices) ? "" : $", {translations[2]}: " + externalInvoices)}" +
				              $"{(referenceNumber == null ? "" : $", {translations[3]}: " + referenceNumber)}\t{translations[4]}\t{caseInfo.FinishingDate?.ToString("yyyy-MM-dd")}\t\t{amount}");

				using (var file = new StreamWriter(path))
				{
					file.Write(sb.ToString());
				}
			}
		}
	}

	public interface IInvoiceService
	{
		List<InvoiceOverview> GetInvoiceOverviewList(int customerId, int? departmentId, DateTime? dateFrom, DateTime? dateTo, InvoiceStatus? status, int? caseId, bool departmentCharge = true);

		void SaveInvoiceActions(int customerId, InvoiceHeader invoiceHeader, List<string> translations);

		List<InvoiceFile> GetInvoiceHeaders(int customerId);

		InvoiceFile GetInvoiceHeader(Guid guid);
	}
}
