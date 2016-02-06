namespace DH.Helpdesk.Services.Services
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Invoice;
    using System;
    using DH.Helpdesk.BusinessData.Models.Shared;    

    public interface IInvoiceArticleService
    {
        InvoiceArticleUnit[] GetUnits(int customerId);

        InvoiceArticle[] GetArticles(int customerId, int productAreaId);

        InvoiceArticle[] GetArticles(int customerId);

        CaseInvoice[] GetCaseInvoices(int caseId);

        CaseInvoice[] GetCaseInvoicesWithTimeZone(int caseId, TimeZoneInfo userTimeZone);

        DataValidationResult ValidateInvoiceSettings(int customerId);

        void SaveCaseInvoices(IEnumerable<CaseInvoice> invoices, int caseId);

        void DeleteCaseInvoices(int caseId);

        void DoInvoiceWork(CaseInvoice[] caseInvoiceData, int caseId, int customerId, int? orderIdToXML);

        int SaveArticle(InvoiceArticle article);

        int SaveUnit(InvoiceArticleUnit unit);
    }
}