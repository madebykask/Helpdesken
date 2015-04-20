namespace DH.Helpdesk.BusinessData.Models.Reports.Data.FinishingCauseCategoryCustomer
{
    public sealed class FinishingCauseCategoryCustomerRow
    {
        public FinishingCauseCategoryCustomerRow(
            string department, 
            int numberOfUsers, 
            int numberOfFinishedCases, 
            double? casesNumberOfUsersPercents, 
            double? casesAllCasesPercents)
        {
            this.CasesAllCasesPercents = casesAllCasesPercents;
            this.CasesNumberOfUsersPercents = casesNumberOfUsersPercents;
            this.NumberOfFinishedCases = numberOfFinishedCases;
            this.NumberOfUsers = numberOfUsers;
            this.Department = department;
        }

        public string Department { get; private set; }

        public int NumberOfUsers { get; private set; }

        public int NumberOfFinishedCases { get; private set; }

        public double? CasesNumberOfUsersPercents { get; private set; }

        public double? CasesAllCasesPercents { get; private set; }
    }
}