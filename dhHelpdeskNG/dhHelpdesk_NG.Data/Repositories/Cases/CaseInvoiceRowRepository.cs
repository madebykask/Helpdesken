using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using DH.Helpdesk.Dal.Infrastructure;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Dal.Repositories.Cases
{
    public class CaseInvoiceRowRepository : RepositoryBase<CaseInvoiceRow>, ICaseInvoiceRowRepository
    {
        public CaseInvoiceRowRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

	    public List<CaseInvoiceRow> GetCaseInvoiceRows(int caseId)
	    {
		    return Table
					.Include(x => x.InvoiceRow)
					.Where(x => x.Case_Id == caseId).ToList();
	    }

		public void SaveCaseInvoiceRows(int caseId, List<CaseInvoiceRow> rows)
		{
			foreach (var row in rows)
			{
				row.Case_Id = caseId;
			}
			MergeList(x => x.Case_Id == caseId
					, rows
					, (a, b) => a.Id == b.Id
					, (a, b) =>
						{
							if (a.InvoiceNumber != b.InvoiceNumber || a.InvoicePrice != b.InvoicePrice)
							{
								a.InvoiceNumber = b.InvoiceNumber;
								a.InvoicePrice = b.InvoicePrice;
							}
						});

		    Commit();
        }

	    public void UpdateExternalInvoiceValues(List<CaseInvoiceRow> rows)
	    {
		    foreach (var row in rows)
		    {
			    var oldRow = GetById(row.Id);
			    if (oldRow != null)
			    {
				    oldRow.InvoicePrice = row.InvoicePrice;
				    oldRow.Charge = row.Charge;
			    }
		    }

			Commit();
	    }
	}

	public interface ICaseInvoiceRowRepository : IRepository<CaseInvoiceRow>
	{
		List<CaseInvoiceRow> GetCaseInvoiceRows(int caseId);
		void SaveCaseInvoiceRows(int caseId, List<CaseInvoiceRow> rows);
		void UpdateExternalInvoiceValues(List<CaseInvoiceRow> rows);
	}

}
