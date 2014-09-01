namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Invoice.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Invoice;
    using DH.Helpdesk.Web.Models.Invoice;

    public sealed class InvoiceArticlesModelFactory : IInvoiceArticlesModelFactory
    {
        public CaseInvoiceArticlesModel CreateCaseInvoiceArticlesModel(CaseInvoice[] caseInvoices)
        {
            return new CaseInvoiceArticlesModel(caseInvoices);
        }
    }
}