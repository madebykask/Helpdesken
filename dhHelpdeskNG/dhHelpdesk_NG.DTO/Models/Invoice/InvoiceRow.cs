using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.Domain.Invoice;

namespace DH.Helpdesk.BusinessData.Models.Invoice
{
	public class InvoiceRow
	{
		public InvoiceRow()
		{
			ExternalInvoices = new List<ExternalInvoice.ExternalInvoice>();
			LogInvoices = new List<CaseLog>();
		}

		public InvoiceStatus? Status { get; set; }

		public List<ExternalInvoice.ExternalInvoice> ExternalInvoices { get; set; }
		public List<CaseLog> LogInvoices { get; set; }
	}
}
