using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DH.Helpdesk.Web.Areas.Invoices.Models
{
	public class InvoiceOverviewViewModel : BaseIndexModel
	{
		public InvoiceOverviewFilterModel Filter { get; set; }

		public override IndexModelType Type => IndexModelType.InvoiceOvweview;
	}
}