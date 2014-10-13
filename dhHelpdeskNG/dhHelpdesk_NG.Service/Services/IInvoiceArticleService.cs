namespace DH.Helpdesk.Services.Services
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Invoice;

    public interface IInvoiceArticleService
    {
        InvoiceArticleUnit[] GetUnits(int customerId);

        InvoiceArticle[] GetArticles(int customerId, int productAreaId);

        InvoiceArticle[] GetArticles(int customerId);

        CaseInvoice[] GetCaseInvoices(int caseId);

        void SaveCaseInvoices(IEnumerable<CaseInvoice> invoices, int caseId);

        void DeleteCaseInvoices(int caseId);

        int SaveArticle(InvoiceArticle article);

        int SaveUnit(InvoiceArticleUnit unit);
    }
}