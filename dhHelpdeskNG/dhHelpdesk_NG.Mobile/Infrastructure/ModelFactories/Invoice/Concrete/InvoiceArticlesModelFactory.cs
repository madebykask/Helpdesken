namespace DH.Helpdesk.Mobile.Infrastructure.ModelFactories.Invoice.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Invoice;
    using DH.Helpdesk.Mobile.Models.Invoice;

    public sealed class InvoiceArticlesModelFactory : IInvoiceArticlesModelFactory
    {
        public CaseInvoiceArticlesModel CreateCaseInvoiceArticlesModel(CaseInvoice[] caseInvoices)
        {
            return new CaseInvoiceArticlesModel(caseInvoices);
        }
    }
}