namespace DH.Helpdesk.Services.Infrastructure.Cases
{
    using System.Collections.Generic;

    using DH.Helpdesk.Domain;

    public interface ICasesCalculator
    {
        void CollectCases(IEnumerable<Case> cases);

        int CalculateInProgressForCustomer(int customerId);

        int CalculateClosedForCustomer(int customerId);

        int CalculateInRestForCustomer(int customerId);

        int CalculateMyForCustomer(int customerId, int userId);
    }
}