namespace DH.Helpdesk.BusinessData.Models.Case
{
    public sealed class CaseAdvancedSearchParams
    {
        public CaseAdvancedSearchParams(
            string customers, 
            string caseNumber)
        {
            this.CaseNumber = caseNumber;
            this.Customers = customers;
        }

        public string Customers { get; private set; }    
    
        public string CaseNumber { get; private set; }
    }
}