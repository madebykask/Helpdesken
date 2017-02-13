using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DH.Helpdesk.BusinessData.Models.Invoice;

namespace DH.Helpdesk.BusinessData.Models.ExternalInvoice
{
	public class ExternalInvoice
	{
		public int Id { get; set; }
		public decimal InvoicePrice { get; set; }
		public int CaseId { get; set; }
		public bool Charge { get; set; }
		public int CreatedByUserId { get; set; }
		public string InvoiceNumber { get; set; }
		public DateTime CreatedDate { get; set; }
		public InvoiceRow InvoiceRow { get; set; }
	}
}
