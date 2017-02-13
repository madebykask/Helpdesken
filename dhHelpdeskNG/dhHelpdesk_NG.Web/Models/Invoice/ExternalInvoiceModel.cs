namespace DH.Helpdesk.Web.Models.Invoice
{
	public class ExternalInvoiceModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public decimal Amount { get; set; }
		public bool Charge { get; set; }

		public InvoiceRowViewModel InvoiceRow { get; set; }
	}
}