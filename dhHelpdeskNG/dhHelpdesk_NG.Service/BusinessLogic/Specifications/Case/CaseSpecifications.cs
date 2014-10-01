namespace DH.Helpdesk.Services.BusinessLogic.Specifications.Case
{
    using System.Linq;

    using DH.Helpdesk.Domain;

    public static class CaseSpecifications
    {
        public static IQueryable<Case> GetAvaliableCases(this IQueryable<Case> query)
        {
            const int MinEmailLength = 3;
            const int IsDeleted = 0;

            query =
                query.Where(
                    c => c.Deleted == IsDeleted && c.FinishingDate != null && c.PersonsEmail.Length > MinEmailLength);

            return query;
        }

        public static IQueryable<Case> GetAvaliableCustomerCases(this IQueryable<Case> query, int customerId)
        {
            query = query.GetByCustomer(customerId).GetAvaliableCases();

            return query;
        }
    }
}