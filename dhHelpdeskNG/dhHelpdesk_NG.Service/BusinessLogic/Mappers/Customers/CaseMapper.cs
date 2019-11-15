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
    }
}