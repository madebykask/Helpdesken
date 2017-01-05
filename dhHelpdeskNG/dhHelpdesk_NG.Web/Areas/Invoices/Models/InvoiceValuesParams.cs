using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DH.Helpdesk.Web.Models.Case;
using DH.Helpdesk.Web.Models.Invoice;

namespace DH.Helpdesk.Web.Areas.Invoices.Models
{
	public class InvoiceValuesParams
	{
		public InvoiceValuesParams()
		{
			ExternalInvoices = new List<ExternalInvoiceModel>();
			LogInvoices = new List<LogInvoiceParams>();	
		}

		public List<ExternalInvoiceModel> ExternalInvoices { get; set; }
		public List<LogInvoiceParams> LogInvoices { get; set; }
	}
}