namespace DH.Helpdesk.Services.Services
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Invoice;
    using System;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.Enums;    

    public interface IInvoiceArticleService
    {
        InvoiceArticleUnit[] GetUnits(int customerId);

        InvoiceArticle[] GetArticles(int customerId, int productAreaId);

        InvoiceArticle[] GetArticles(int customerId);

        CaseInvoice[] GetCaseInvoices(int caseId);

        CaseInvoice[] GetCaseInvoicesWithTimeZone(int caseId, TimeZoneInfo userTimeZone);

        DataValidationResult ValidateInvoiceSettings(int customerId);

        int SaveCaseInvoices(IEnumerable<CaseInvoice> invoices, int caseId, int userId);

        void DeleteCaseInvoices(int caseId);

        ProcessResult DoInvoiceWork(CaseInvoice[] caseInvoiceData, int caseId, decimal caseNumber, int customerId, int userId, int? orderIdToXML);

        CaseInvoiceOrder[] GetInvoiceOrders(int caseId, InvoiceOrderFetchStatus status);

        int SaveArticle(InvoiceArticle article);

        int SaveUnit(InvoiceArticleUnit unit);

        void DeleteFileByCaseId(int caseId, string fileName);
        void SaveArticleProductArea(InvoiceArticleProductAreaSelectedFilter selectedItems);

        void DeleteArticleProductArea(int articleid, int productareaid);

        void DeactivateArticlesBySyncDate(int customerId, DateTime lastSyncDate);
        void SaveArticles(List<InvoiceArticle> articles);
    }
}