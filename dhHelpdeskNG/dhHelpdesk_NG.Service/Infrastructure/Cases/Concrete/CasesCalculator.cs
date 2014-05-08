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
            return this.cases
                .Count(x => x.FinishingDate == null &&
                    x.Customer_Id == customerId);
        }

        public int CalculateClosedForCustomer(int customerId)
        {
            return this.cases
                    .Count(x => x.FinishingDate != null &&
                        x.Customer_Id == customerId);
        }

        public int CalculateInRestForCustomer(int customerId)
        {
            return this.cases
                    .Count(x => x.FinishingDate == null &&
                        x.StateSecondary_Id != null &&
                        x.Customer_Id == customerId);
        }

        public int CalculateMyForCustomer(
                        int customerId,
                        int userId)
        {
            return this.cases
                    .Count(x => x.FinishingDate == null &&
                        x.Customer_Id == customerId &&
                        x.Performer_User_Id == userId);
        }
    }
}