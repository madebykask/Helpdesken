using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DH.Helpdesk.BusinessData.Models.Invoice
{
	public class InvoiceFile
	{
		public Guid Guid { get; set; }
		public string Name { get; set; }
		public DateTime Date { get; set; }
	}
}
