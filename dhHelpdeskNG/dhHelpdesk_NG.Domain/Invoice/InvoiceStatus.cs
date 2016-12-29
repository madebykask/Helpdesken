using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DH.Helpdesk.Domain.Invoice
{
	public enum InvoiceStatus
	{
		No = 0,
		Ready = 1,
		Invoiced = 2,
		NotInvoiced = 3
	}
}
