using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DH.Helpdesk.Web.Models.Case
{
	public class ExternalInvoiceModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public decimal Value { get; set; }
		public bool Charge { get; set; }
	}
}