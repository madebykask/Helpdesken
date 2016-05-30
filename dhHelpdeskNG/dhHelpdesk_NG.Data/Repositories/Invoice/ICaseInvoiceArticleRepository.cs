namespace DH.Helpdesk.Dal.Repositories.Invoice
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Invoice;
    using DH.Helpdesk.Common.Enums;

    public interface ICaseInvoiceArticleRepository
    {
        CaseInvoice[] GetCaseInvoices(int caseId);

        CaseInvoiceOrder GetCaseInvoiceOrder(int caseId, int invoiceOrderId);

        CaseInvoiceOrder[] GetOrders(int caseId, InvoiceOrderFetchStatus status);        

        int SaveCaseInvoices(IEnumerable<CaseInvoice> invoices, int caseId);

        void CancelInvoiced(int caseId, int InvoiceOrderId);
    }
}