using System;

namespace DH.Helpdesk.Dal.Repositories.Cases
{
	using System.Collections.Generic;
	using DH.Helpdesk.Dal.Infrastructure;
	using DH.Helpdesk.Domain.ExtendedCaseEntity;
	using DH.Helpdesk.BusinessData.Models.Case;
	using DH.Helpdesk.Common.Enums;
	using Common.Enums.Cases;
	using System.Linq;

	public interface IExtendedCaseReportRepository : IRepository<ExtendedCaseReport>
	{
		IQueryable<ExtendedCaseReport> GetExtendedCaseReportsFromCustomer(int customerId);

	}
}