namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Customers
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Case.Output;
    using DH.Helpdesk.Domain;

    public static class CaseMapper
    {
        public static CustomerCases[] MapToCustomerCases(
                                            this IQueryable<Customer> query, 
                                            int userId)
        {
            var entities = query.Select(cus => new
                                            {
                                                CustomerId = cus.Id,
                                                CustomerName = cus.Name,
                                                CasesInProgress = cus.Cases.Where(c => c.FinishingDate == null).Count(),
                                                CasesClosed = cus.Cases.Where(c => c.FinishingDate != null).Count(),
                                                CasesInRest = cus.Cases.Where(c => c.FinishingDate == null && c.StateSecondary_Id != null).Count(),
                                                CasesMy = cus.Cases.Where(c => c.FinishingDate == null && c.Performer_User_Id == userId).Count()
                                            }).ToArray();

            var overviews = entities.Select(c => new CustomerCases(
                                                c.CustomerId,
                                                c.CustomerName,
                                                c.CasesInProgress,
                                                c.CasesClosed,
                                                c.CasesInRest,
                                                c.CasesMy)).ToArray();

            return overviews;
        }
    }
}