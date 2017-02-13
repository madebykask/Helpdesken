using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DH.Helpdesk.Domain.Invoice;

namespace DH.Helpdesk.Web.Areas.Invoices.Models
{
	public class InvoiceOverviewFilterModel
	{
		public int? DepartmentId { get; set; }
		public DateTime? DateFrom { get; set; }
		public DateTime? DateTo { get; set; }
		public InvoiceStatus? Status { get; set; }
		public int? CaseId { get; set; }
	}
}