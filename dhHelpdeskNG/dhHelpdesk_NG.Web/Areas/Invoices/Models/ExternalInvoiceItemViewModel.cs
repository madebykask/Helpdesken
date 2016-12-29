using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DH.Helpdesk.Web.Areas.Invoices.Models
{
	public class ExternalInvoiceItemViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public decimal Amount { get; set; }
		public bool Charge { get; set; }

		public InvoiceRowViewModel InvoiceRow { get; set; }
	}
}