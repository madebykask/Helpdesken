namespace DH.Helpdesk.Services.Services.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Statistics.Output;
    using DH.Helpdesk.Services.Infrastructure.Cases;

    public sealed class StatisticsService : IStatisticsService
    {
        private readonly ICaseService caseService;

        public StatisticsService(ICaseService caseService)
        {
            this.caseService = caseService;
        }

        public StatisticsOverview GetStatistics(
                        int[] customers, 
                        ICasesCalculator calculator, 
                        int userId)
        {
            var cases = this.caseService.GetCasesByCustomers(customers);
            calculator.CollectCases(cases);

            var statistics = new StatisticsOverview
                                 {
                                     ActiveCases = calculator.CalculateInProgressForCustomers(customers),
                                     EndedCases = calculator.CalculateClosedForCustomers(customers),
                                     InRestCases = calculator.CalculateInRestForCustomers(customers),
                                     MyCases = calculator.CalculateMyForCustomers(customers, userId)
                                 };

            return statistics;
        }
    }
}