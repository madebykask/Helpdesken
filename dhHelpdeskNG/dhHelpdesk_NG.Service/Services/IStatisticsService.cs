namespace DH.Helpdesk.Services.Services
{
    using DH.Helpdesk.BusinessData.Models.Statistics.Output;
    using DH.Helpdesk.Services.Infrastructure.Cases;

    public interface IStatisticsService
    {
        StatisticsOverview GetStatistics(
                        int[] customers, 
                        ICasesCalculator calculator,
                        int userId);
    }
}