using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DH.Helpdesk.Web.Models.Invoice;

namespace DH.Helpdesk.Web.Areas.Invoices.Models
{
	public class LogInvoiceItemViewModel
	{
		public int Id { get; set; }
		public DateTime Date { get; set; }
		public string Text { get; set; }
		public int WorkingTime { get; set; }
		public int Overtime { get; set; }
		public decimal Material { get; set; }
		public int Price { get; set; }
		public bool Charge { get; set; }

		public int WorkingHourRate { get; set; }
		public int OvertimeHourRate { get; set; }

		public InvoiceRowViewModel InvoiceRow { get; set; }

	}
}