namespace DH.Helpdesk.BusinessData.Models.Case.Output
{
    public sealed class CustomerCases
    {
        public CustomerCases(
                int customerId, 
                string customerName, 
                int casesInProgress, 
                int casesClosed, 
                int casesInRest, 
                int casesMy)
        {
            this.CasesMy = casesMy;
            this.CasesInRest = casesInRest;
            this.CasesClosed = casesClosed;
            this.CasesInProgress = casesInProgress;
            this.CustomerName = customerName;
            this.CustomerId = customerId;
        }

        public int CustomerId { get; private set; }

        public string CustomerName { get; private set; }

        public int CasesInProgress { get; private set; }

        public int CasesClosed { get; private set; }

        public int CasesInRest { get; private set; }

        public int CasesMy { get; private set; }
    }
}