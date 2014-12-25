namespace DH.Helpdesk.Dal.Repositories.Invoice
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Invoice;

    public interface ICaseInvoiceArticleRepository
    {
        CaseInvoice[] GetCaseInvoices(int caseId);

        void SaveCaseInvoices(IEnumerable<CaseInvoice> invoices, int caseId);
    }
}