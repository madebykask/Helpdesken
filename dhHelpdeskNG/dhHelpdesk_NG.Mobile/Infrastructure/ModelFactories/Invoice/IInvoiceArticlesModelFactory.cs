namespace DH.Helpdesk.Mobile.Infrastructure.ModelFactories.Invoice
{
    using DH.Helpdesk.BusinessData.Models.Invoice;
    using DH.Helpdesk.Mobile.Models.Invoice;

    public interface IInvoiceArticlesModelFactory
    {
        CaseInvoiceArticlesModel CreateCaseInvoiceArticlesModel(CaseInvoice[] caseInvoices);
    }
}