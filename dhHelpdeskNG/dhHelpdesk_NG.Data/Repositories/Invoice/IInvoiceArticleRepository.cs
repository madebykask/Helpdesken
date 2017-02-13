using System;
using System.Collections.Generic;

namespace DH.Helpdesk.Dal.Repositories.Invoice
{
    using DH.Helpdesk.BusinessData.Models.Invoice;

    public interface IInvoiceArticleRepository
    {
        InvoiceArticle[] GetArticles(int customerId, int productAreaId);

        InvoiceArticle[] GetArticles(int customerId);

	    List<InvoiceArticle> GetActiveArticles(int customerId);


		int SaveArticle(InvoiceArticle article);

        void SaveArticleProductArea(InvoiceArticleProductAreaSelectedFilter selectedItems);

        void DeleteArticleProductArea(int articleid, int productareaid);
        void DeactivateArticlesBySyncDate(int customerId, DateTime lastSyncDate);
        void SaveArticles(List<InvoiceArticle> articles);
    }
}