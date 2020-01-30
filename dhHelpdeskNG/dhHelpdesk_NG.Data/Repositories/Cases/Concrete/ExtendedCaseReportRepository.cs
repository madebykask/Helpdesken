using DH.Helpdesk.Dal.Infrastructure;
using DH.Helpdesk.Domain.ExtendedCaseEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.Dal.Repositories.Cases.Concrete
{
	public sealed class ExtendedCaseReportRepository : RepositoryBase<ExtendedCaseReport>, IExtendedCaseReportRepository
	{
		public ExtendedCaseReportRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
		{
		}

		public IQueryable<ExtendedCaseReport> GetExtendedCaseReportsFromCustomer(int customerId)
		{
			var reports = Table
				.Where(o => o.Customer_Id == customerId);

			return reports;

		}
	}
}
