using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace DH.Helpdesk.Domain.Invoice
{
	public enum InvoiceStatus
	{
		[Display(Name = "-")]
		No = 0,
		[Display(Name = "Ej fakturerade")]
		Ready = 1,
		[Display(Name = "Klara (fakturerade)")]
		Invoiced = 2,
		[Display(Name = "Klara (ej fakturerade)")]
		NotInvoiced = 3
	}
}
