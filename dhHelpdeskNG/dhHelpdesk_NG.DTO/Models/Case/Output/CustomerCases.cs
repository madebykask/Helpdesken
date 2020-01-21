namespace DH.Helpdesk.BusinessData.Models.Case.Output
{
    public sealed class CustomerCases
    {
        public CustomerCases(
                int customerId, 
                string customerName, 
                int casesInProgress, 
                int casesUnreaded, 
                int casesInRest, 
                int casesMy,
                bool active)
        {            
            this.CasesMy = casesMy;
            this.CasesInRest = casesInRest;
            this.CasesUnreaded = casesUnreaded;
            this.CasesInProgress = casesInProgress;
            this.CustomerName = customerName;
            this.CustomerId = customerId;
            this.Active = active;
        }

        public int CustomerId { get; private set; }

        public string CustomerName { get; private set; }

        public int CasesInProgress { get; private set; }

        public int CasesUnreaded { get; private set; }

        public int CasesInRest { get; private set; }

        public int CasesMy { get; private set; }

        public bool Active { get; private set; }
        
    }
}