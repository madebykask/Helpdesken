namespace DH.Helpdesk.BusinessData.Models.Invoice
{
    public class InvoiceRowStatistics
    {
        public InvoiceRowStatistics(int caseId, int numOfReady, int numOfInvoiced, int numOfNotInvoiced)
        {
            CaseId = caseId;
            NumOfInvoiced = numOfInvoiced;
            NumOfNotInvoiced = numOfNotInvoiced;
            NumOfReady = numOfReady;
        }   
        
        public int CaseId { get; private set; }
        public int NumOfReady { get; private set; }
        public int NumOfInvoiced { get; private set; }
        public int NumOfNotInvoiced { get; private set; }

    }
}
