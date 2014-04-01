using DH.Helpdesk.BusinessData.Models.Statistics.Output;

namespace DH.Helpdesk.Services.Services
{
    public interface IStatisticsService
    {
        StatisticsOverview GetStatistics(int[] customers);
    }
}