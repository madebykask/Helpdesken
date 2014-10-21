namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Customers
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Case.Output;
    using DH.Helpdesk.BusinessData.Models.Statistics.Output;
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

        public static StatisticsOverview MapToStatistics(this IQueryable<Case> query, int userId)
        {
            var entity = query.Take(1).Select(cs => new
                                                {
                                                    ActiveCases = query.Where(c => c.FinishingDate == null).Count(),
                                                    EndedCases = query.Where(c => c.FinishingDate != null).Count(),
                                                    InRestCases = query.Where(c => c.FinishingDate == null && c.StateSecondary_Id != null).Count(),
                                                    MyCases = query.Where(c => c.FinishingDate == null && c.Performer_User_Id == userId).Count()                                                              
                                                }).SingleOrDefault();
            if (entity == null)
            {
                return new StatisticsOverview();
            }

            return new StatisticsOverview(
                                    entity.ActiveCases,
                                    entity.EndedCases,
                                    entity.InRestCases,
                                    entity.MyCases);
        }
    }
}