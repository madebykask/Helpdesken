using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DH.Helpdesk.Domain.Invoice;

namespace DH.Helpdesk.Web.Models.Invoice
{
	public class InvoiceRowViewModel
	{
		public InvoiceStatus? Status { get; set; }
	}
}