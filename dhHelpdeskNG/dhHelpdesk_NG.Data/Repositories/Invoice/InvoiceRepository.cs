using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using DH.Helpdesk.BusinessData.Models.Case.CaseLock;
using DH.Helpdesk.Dal.Infrastructure;
using DH.Helpdesk.Dal.Mappers;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Domain.Cases;
using DH.Helpdesk.Domain.Invoice;
using DH.Helpdesk.BusinessData.Models.Invoice;

namespace DH.Helpdesk.Dal.Repositories.Invoice
{
	public class InvoiceRepository : RepositoryBase<Case>, IInvoiceRepository
	{


		public InvoiceRepository(
				IDatabaseFactory databaseFactory
				//IEntityToBusinessModelMapper<CaseLockEntity, CaseLock> caseLockToBusinessModelMapper,
				//IBusinessModelToEntityMapper<CaseLock, CaseLockEntity> caseLockToEntityMapper
			)
                : base(databaseFactory)
            {
			//this._caseLockToBusinessModelMapper = caseLockToBusinessModelMapper;
			//this._caseLockToEntityMapper = caseLockToEntityMapper;
		}

		public Tuple<List<Case>, IEnumerable<InvoiceRowStatistics>> GetInvoiceOverviewList(int customerId, int? departmentId, DateTime? dateFrom, DateTime? dateTo, InvoiceStatus? status, int? caseId, bool departmentCharge = true)
		{

			var query = DataContext.Cases
				.Include(x => x.Logs)
				.Include(x => x.CaseInvoiceRows)
				.Include(x => x.Logs.Select(y => y.InvoiceRow))
				.Include(x => x.CaseInvoiceRows.Select(y => y.InvoiceRow))
				.Include(x => x.Department)
				.Include(x => x.Category)
				.Where(x =>
					(x.Logs.Any(y => y.WorkingTime > 0 || y.OverTime > 0 || y.EquipmentPrice > 0 || y.Price > 0)
					 || x.CaseInvoiceRows.Any())
					&& x.Deleted == 0 && x.Customer_Id == customerId);

			if (dateFrom.HasValue)
				query = query.Where(x => x.FinishingDate >= dateFrom);
			if (dateTo.HasValue)
				query = query.Where(x => x.FinishingDate <= dateTo);
			if (departmentId.HasValue)
				query = query.Where(x => x.Department_Id == departmentId);

			if (status.HasValue && status.Value != InvoiceStatus.No && status.Value != InvoiceStatus.Ready)
				query = query.Where(x => x.Logs.Any(y => (y.WorkingTime > 0 || y.OverTime > 0 || y.EquipmentPrice > 0 || y.Price > 0) && y.InvoiceRow.Status == status) 
					|| x.CaseInvoiceRows.Any(y => y.InvoiceRow.Status == status));
			if (status.HasValue && status.Value == InvoiceStatus.Ready)
				query = query.Where(x => x.Logs.Any(y => (y.WorkingTime > 0 || y.OverTime > 0 || y.EquipmentPrice > 0 || y.Price > 0) && (y.InvoiceRow.Status != InvoiceStatus.Invoiced && y.InvoiceRow.Status != InvoiceStatus.NotInvoiced))
					|| x.CaseInvoiceRows.Any(y => y.InvoiceRow.Status != InvoiceStatus.Invoiced && y.InvoiceRow.Status != InvoiceStatus.NotInvoiced));

			if (caseId.HasValue)
			{
				query = query.Where(x => x.Id == caseId.Value);
			}
            if (departmentCharge)
            {
                query = query.Where(x => x.Department.Charge == 1);
            }

            var res = query.ToList();
            var statictics = new List<InvoiceRowStatistics>();
			foreach (var item in res)
			{
				var logQuery = item.Logs.Where(y => y.WorkingTime > 0 || y.OverTime > 0 || y.EquipmentPrice > 0 || y.Price > 0);
				var externalQuery = item.CaseInvoiceRows.Where(x => true);

                var _readyRows = 0;
                var _invoicedRows = 0;
                var _notInvoicedRows = 0;

                if (logQuery != null)
                {
                    _readyRows += logQuery.Count(l => l.InvoiceRow == null || l.InvoiceRow.Status == InvoiceStatus.No);
                    _invoicedRows += logQuery.Count(l => l.InvoiceRow != null && l.InvoiceRow.Status == InvoiceStatus.Invoiced);
                    _notInvoicedRows += logQuery.Count(l => l.InvoiceRow != null && l.InvoiceRow.Status == InvoiceStatus.NotInvoiced);
                }

                if (externalQuery != null)
                {
                    _readyRows += externalQuery.Count(l => l.InvoiceRow == null || l.InvoiceRow.Status == InvoiceStatus.No);
                    _invoicedRows += externalQuery.Count(l => l.InvoiceRow != null && l.InvoiceRow.Status == InvoiceStatus.Invoiced);
                    _notInvoicedRows += externalQuery.Count(l => l.InvoiceRow != null && l.InvoiceRow.Status == InvoiceStatus.NotInvoiced);
                }

                statictics.Add(new InvoiceRowStatistics(item.Id, _readyRows, _invoicedRows, _notInvoicedRows));                                    

                if (status.HasValue && status.Value != InvoiceStatus.No && status.Value != InvoiceStatus.Ready)
				{
					logQuery = logQuery.Where(y => y.InvoiceRow?.Status == status);
					externalQuery = externalQuery.Where(y => y.InvoiceRow?.Status == status);
				}
				if (status.HasValue && status.Value == InvoiceStatus.Ready)
				{
					logQuery = logQuery.Where(y => y.InvoiceRow == null 
						|| (y.InvoiceRow.Status != InvoiceStatus.Invoiced && y.InvoiceRow.Status != InvoiceStatus.NotInvoiced));
					externalQuery = externalQuery.Where(y => y.InvoiceRow == null 
						|| (y.InvoiceRow.Status != InvoiceStatus.Invoiced && y.InvoiceRow.Status != InvoiceStatus.NotInvoiced));
				}

				item.Logs = logQuery.ToList();
				item.CaseInvoiceRows = externalQuery.ToList();
			}

            return new Tuple<List<Case>, IEnumerable<InvoiceRowStatistics>> (res, statictics);

			//var q1 = DataContext.Logs
			//	.Include(x => x.Case)
			//	.Include(x => x.Case.Department)
			//	.Include(x => x.Case.Category)
			//	.Where(x =>
			//		(x.WorkingTime > 0 || x.OverTime > 0 || x.EquipmentPrice > 0 || x.Price > 0)
			//		&& x.Case.Deleted == 0 && x.Case.Customer_Id == customerId
			//		&& x.Case.Department.Charge == 1)
			//	.Select(x => new { Case = x.Case, Log = x, ExternalInvoice = (CaseInvoiceRow)null });

			//var q2 = DataContext.CaseInvoiceRows
			//	.Include(x => x.Case).
			//	.Include(x => x.Case.Department)
			//	.Include(x => x.Case.Category)
			//	.Where(x =>
			//		x.Case.Deleted == 0 && x.Case.Customer_Id == customerId
			//		&& x.Case.Department.Charge == 1)
			//	.Select(x => new { Case = x.Case, Log = (Log)null, ExternalInvoice = x });

			//var q3 = q1.Concat(q2);
			////var q4 = q3.GroupBy(x => x.Case).Select(x => x.)
			//var q3r = q3.ToList();

			


			//	var sSQL = "SELECT tblCase.Id, tblCase.Casenumber, SUM(tblLog.WorkingTime) AS WorkingTime, SUM(tblLog.OverTime) " +
			//	           "AS OverTime, SUM(CASE tblLog.Charge WHEN 1 THEN tblLog.EquipmentPrice ELSE 0 END) AS EquipmentPrice, " +
			//	           "SUM(tblLog.Price) AS Price, tblDepartment.AccountancyAmount, tblDepartment.OverTimeAmount,  " & _
			//	   "tblCase.Caption, tblDepartment.Department, tblCase.FinishingDate, IsNull(tblInvoiceRow.Status, 0) AS Status, IsNull(tblCategory.Category, '') AS Category " & _
			//"FROM tblLog " & _
			//	"INNER JOIN tblCase ON tblLog.Case_Id = tblCase.Id " & _
			//	"LEFT JOIN tblDepartment ON tblCase.Department_Id = tblDepartment.Id " & _

			//	"LEFT JOIN tblCategory ON tblCase.Category_Id = tblCategory.Id " & _
			//	"LEFT OUTER JOIN tblInvoiceHeader " & _
			//	"RIGHT OUTER JOIN tblInvoiceRow ON tblInvoiceHeader.Id = tblInvoiceRow.InvoiceHeader_Id ON tblCase.Id = tblInvoiceRow.Case_Id " & _
			//"WHERE tblCase.Deleted = 0 AND tblCase.Customer_Id=" & iCustomer_Id & _
			//	" AND (tblDepartment.Charge = 1) " & _

			//	" AND ((tblLog.WorkingTime > 0 OR tblLog.EquipmentPrice > 0 OR tblLog.OverTime > 0 OR tblLog.Price > 0) Or tblLog.EquipmentPrice > 0 OR tblCase.Id " +
			//		"IN (SELECT Case_Id FROM tblCaseInvoiceRow)) "

			//	'" AND (tblLog.Charge = 1 Or tblLog.EquipmentPrice > 0 OR tblCase.Id IN (SELECT Case_Id FROM tblCaseInvoiceRow)) "
		}
		//private Func<Log, bool>
	}

	public interface IInvoiceRepository
	{
        Tuple<List<Case>, IEnumerable<InvoiceRowStatistics>> GetInvoiceOverviewList(int customerId, int? departmentId, DateTime? dateFrom, DateTime? dateTo, InvoiceStatus? status, int? caseId, bool departmentCharge = true);
	}
}
