namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Customers
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Case.Output;
    using DH.Helpdesk.BusinessData.Models.Statistics.Output;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Problems;
    using DH.Helpdesk.BusinessData.OldComponents;

    public static class CaseMapper
    {
        public static CustomerCases[] MapToCustomerCases(
            this IQueryable<Customer> customers,
            IQueryable<CaseFieldSetting> caseFieldSettings,
            IQueryable<Problem> problems, //not used
            int userId)
        {
            var entities = customers.Select(customer => new
            {
                CustomerId = customer.Id,
                CustomerName = customer.Name,

                CasesInProgress = customer.Cases.Where(c => c.FinishingDate == null && c.Deleted == 0).Count(),
                CasesUnreaded = customer.Cases.Where(c => c.Unread == 1 && c.Deleted == 0 && c.FinishingDate == null).Count(),
                CasesInRest = customer.Cases.Where(c => c.FinishingDate == null && 
                                                        c.StateSecondary_Id != null && c.StateSecondary.IncludeInCaseStatistics == 0 &&
                                                        c.Deleted == 0).Count(),
                CasesMy = 
                    customer.Cases.Where(c => c.FinishingDate == null &&
                        c.Deleted == 0 &&
                        (c.Performer_User_Id == userId || 
                         (c.CaseResponsibleUser_Id == userId &&
                          caseFieldSettings.Any(cf => cf.Customer_Id == c.Customer_Id && 
                                                      cf.Name == GlobalEnums.TranslationCaseFields.CaseResponsibleUser_Id.ToString() &&
                                                      cf.ShowOnStartPage == 1)))
                    ).Count()
            }).ToArray();

            var overviews = entities.Select(c => new CustomerCases(
                                                c.CustomerId,
                                                c.CustomerName,
                                                c.CasesInProgress,
                                                c.CasesUnreaded,
                                                c.CasesInRest,
                                                c.CasesMy)).ToArray();

            return overviews;
        }

        public static StatisticsOverview MapToStatistics(this IQueryable<Case> query, IQueryable<Problem> problems, int userId)
        {
            var entity = query.Take(1).Select(cs => new
            {
                ActiveCases = query.Where(c => c.FinishingDate == null).Count(),
                UnreadCases = query.Where(c => c.Unread == 1 && c.Deleted == 0).Count(),
                EndedCases = query.Where(c => c.FinishingDate != null).Count(),
                InRestCases = query.Where(c => c.FinishingDate == null && 
                                               c.StateSecondary_Id != null && c.StateSecondary.IncludeInCaseStatistics == 0 && 
                                               c.Deleted == 0).Count(),
                MyCases = (from c in query
                           join p in problems on c.Problem equals p into gj
                           from cp in gj.DefaultIfEmpty()
                           where c.FinishingDate == null &&
                                 (c.Performer_User_Id == userId ||
                                  c.CaseResponsibleUser_Id == userId ||
                                  c.Problem.ResponsibleUser_Id == userId)
                           select c).Count()                                                             
            }).SingleOrDefault();

            if (entity == null)
            {
                return new StatisticsOverview();
            }

            return new StatisticsOverview(
                        entity.ActiveCases,
                        entity.EndedCases,
                        entity.InRestCases,
                        entity.MyCases,
                        entity.UnreadCases);
        }
    }
}