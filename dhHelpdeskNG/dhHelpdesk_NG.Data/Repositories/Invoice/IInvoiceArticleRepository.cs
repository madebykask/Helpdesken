namespace DH.Helpdesk.Dal.Repositories.Invoice
{
    using DH.Helpdesk.BusinessData.Models.Invoice;

    public interface IInvoiceArticleRepository
    {
        InvoiceArticle[] GetArticles(int customerId, int productAreaId);

        InvoiceArticle[] GetArticles(int customerId);

        int SaveArticle(InvoiceArticle article);
    }
}