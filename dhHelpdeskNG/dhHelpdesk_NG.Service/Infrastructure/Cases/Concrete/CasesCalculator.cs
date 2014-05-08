namespace DH.Helpdesk.Services.Infrastructure.Cases.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Domain;

    public sealed class CasesCalculator : ICasesCalculator
    {
        private IEnumerable<Case> cases;

        public void CollectCases(IEnumerable<Case> casesToCalculate)
        {
            this.cases = casesToCalculate;
        }

        public int CalculateInProgressForCustomer(int customerId)
        {
            return this.CalculateInProgressForCustomers(new[] { customerId });
        }

        public int CalculateClosedForCustomer(int customerId)
        {
            return this.CalculateClosedForCustomers(new[] { customerId });
        }

        public int CalculateInRestForCustomer(int customerId)
        {
            return this.CalculateInRestForCustomers(new[] { customerId });
        }

        public int CalculateMyForCustomer(int customerId, int userId)
        {
            return this.CalculateMyForCustomers(new[] { customerId }, userId);
        }

        public int CalculateInProgressForCustomers(IEnumerable<int> customersIds)
        {
            return this.cases
                .Count(x => x.FinishingDate == null &&
                    customersIds.Contains(x.Customer_Id));
        }

        public int CalculateClosedForCustomers(IEnumerable<int> customersIds)
        {
            return this.cases
                    .Count(x => x.FinishingDate != null &&
                        customersIds.Contains(x.Customer_Id));
        }

        public int CalculateInRestForCustomers(IEnumerable<int> customersIds)
        {
            return this.cases
                    .Count(x => x.FinishingDate == null &&
                        x.StateSecondary_Id != null &&
                        customersIds.Contains(x.Customer_Id));
        }

        public int CalculateMyForCustomers(
                        IEnumerable<int> customersIds,
                        int userId)
        {
            return this.cases
                    .Count(x => x.FinishingDate == null &&
                        customersIds.Contains(x.Customer_Id) &&
                        x.Performer_User_Id == userId);
        }
    }
}