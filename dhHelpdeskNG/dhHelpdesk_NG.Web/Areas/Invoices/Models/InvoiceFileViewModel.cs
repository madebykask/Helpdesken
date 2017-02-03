using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DH.Helpdesk.Web.Areas.Invoices.Models
{
	public class InvoiceFileViewModel
	{
		public Guid Guid { get; set; }
		public string Name { get; set; }
		public DateTime Date { get; set; }
	}
}