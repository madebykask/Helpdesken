namespace DH.Helpdesk.Services.Services
{
    using DH.Helpdesk.BusinessData.Models.Invoice;

    public interface IInvoiceArticleService
    {
        InvoiceArticleUnit[] GetUnits(int customerId);

        InvoiceArticle[] GetArticles(int customerId, int productAreaId);

        CaseInvoiceArticle[] GetCaseArticles(int caseId);
    }
}