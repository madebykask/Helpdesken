using DH.Helpdesk.Web.Models.Invoice;
namespace DH.Helpdesk.Web.Models.Case
{    

    public sealed class CaseInvoiceModel
    {
        public CaseInvoiceModel(int customerId, int caseId, CaseInvoiceArticlesModel invoiceArticles, 
                                string caseInvoiceArticles, string caseKey, string logKey)
        {
            this.CustomerId = customerId;
            this.CaseId = caseId;
            this.InvoiceArticles = invoiceArticles;
            this.CaseInvoiceArticles = caseInvoiceArticles;
            this.CaseKey = caseKey;
            this.LogKey = logKey;
        }

        public int CustomerId { get; private set; }

        public int CaseId { get; private set; }

        public CaseInvoiceArticlesModel InvoiceArticles { get; private set; }

        public string CaseInvoiceArticles { get; private set; }

        public string CaseKey { get; private set; }

        public string LogKey { get; private set; }
    }
}