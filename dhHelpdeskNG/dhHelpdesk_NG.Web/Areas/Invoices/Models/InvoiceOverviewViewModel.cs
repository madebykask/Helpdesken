
using DH.Helpdesk.Web.Models.Shared;

namespace DH.Helpdesk.Web.Areas.Invoices.Models
{
	public class InvoiceOverviewViewModel : BaseIndexModel
	{
		public InvoiceOverviewFilterModel Filter { get; set; }

        public override IndexModelType Type => IndexModelType.InvoiceOvweview;
	}
}