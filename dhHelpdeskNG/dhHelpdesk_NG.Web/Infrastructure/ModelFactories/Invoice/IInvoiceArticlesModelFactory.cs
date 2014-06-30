namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Invoice
{
    using DH.Helpdesk.BusinessData.Models.Invoice;
    using DH.Helpdesk.Web.Models.Invoice;

    public interface IInvoiceArticlesModelFactory
    {
        CaseInvoiceArticlesModel CreateCaseInvoiceArticlesModel(CaseInvoiceArticle[] caseArticles);
    }
}