using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DH.Helpdesk.BusinessData.Models.Invoice
{
	public class InvoiceHeader
	{
		public InvoiceHeader()
		{
			InvoiceRows = new List<InvoiceRow>();
		}
		public List<InvoiceRow> InvoiceRows { get; set; }
		public int CreatedById { get; set; }
	}
}
