namespace DH.Helpdesk.Dal.Repositories.Invoice
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Invoice;

    public interface ICaseInvoiceArticleRepository
    {
        CaseInvoice[] GetCaseInvoices(int caseId);

        CaseInvoiceOrder GetCaseInvoiceOrder(int caseId, int invoiceOrderId);

        void SaveCaseInvoices(IEnumerable<CaseInvoice> invoices, int caseId);
    }
}