namespace DH.Helpdesk.BusinessData.Models.Case
{
    public sealed class CaseAdvancedSearchParams
    {
        public CaseAdvancedSearchParams(string customers)
        {
            this.Customers = customers;
        }

        public string Customers { get; private set; }        
    }
}