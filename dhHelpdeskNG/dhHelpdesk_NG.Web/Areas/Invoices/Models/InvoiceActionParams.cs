using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DH.Helpdesk.Web.Areas.Invoices.Models
{
	public class InvoiceActionParams
	{
		public InvoiceActionParams()
		{
			LogInvoices = new List<InvoiceActionParam>();
			ExternalInvoices = new List<InvoiceActionParam>();
		}
		public List<InvoiceActionParam> LogInvoices { get; set; }
		public List<InvoiceActionParam> ExternalInvoices { get; set; }
	}
}