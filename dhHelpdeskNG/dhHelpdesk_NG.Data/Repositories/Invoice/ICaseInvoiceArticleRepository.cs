namespace DH.Helpdesk.Dal.Repositories.Invoice
{
    using DH.Helpdesk.BusinessData.Models.Invoice;

    public interface ICaseInvoiceArticleRepository
    {
        CaseInvoiceArticle[] GetCaseArticles(int caseId);

        void SaveCaseArticles(int caseId, CaseInvoiceArticle[] articles);
    }
}