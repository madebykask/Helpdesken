namespace DH.Helpdesk.Web.Models.Invoice
{
    using DH.Helpdesk.BusinessData.Models.Invoice;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class CaseInvoiceArticlesModel
    {
        public CaseInvoiceArticlesModel(CaseInvoice[] invoices)
        {
            this.Invoices = invoices;
        }

        [NotNull]
        public CaseInvoice[] Invoices { get; private set; }
    }
}