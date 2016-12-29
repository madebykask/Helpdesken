using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DH.Helpdesk.Web.Areas.Invoices.Models
{
	public class LogInvoiceItemViewModel
	{
		public int Id { get; set; }
		public string Text { get; set; }
		public int WorkingTime { get; set; }
		public int Overtime { get; set; }
		public int Material { get; set; }
		public decimal Price { get; set; }
		public bool Charge { get; set; }

		public int WorkingHourRate { get; set; }
		public int OvertimeHourRate { get; set; }

		public InvoiceRowViewModel InvoiceRow { get; set; }

	}
}