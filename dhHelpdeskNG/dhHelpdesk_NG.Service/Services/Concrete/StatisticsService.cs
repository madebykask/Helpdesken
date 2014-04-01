using System.Linq;
using DH.Helpdesk.BusinessData.Models.Statistics.Output;
using DH.Helpdesk.Dal.Repositories;

namespace DH.Helpdesk.Services.Services.Concrete
{
    public sealed class StatisticsService : IStatisticsService
    {
        private readonly ICaseRepository _caseRepository;

        public StatisticsService(ICaseRepository caseRepository)
        {
            _caseRepository = caseRepository;
        }

        public StatisticsOverview GetStatistics(int[] customers)
        {
            var cases = _caseRepository.GetCaseOverviews(customers);

            var statistics = new StatisticsOverview();

            statistics.ActiveCases = cases.Count(c => !c.FinishingDate.HasValue 
                                                    && c.Deleted == 0);
            statistics.EndedCases = cases.Count(c => c.FinishingDate.HasValue 
                                                    && c.Deleted == 0);
            statistics.WarningCases = cases.Count(c => !c.FinishingDate.HasValue 
                                                    && (c.Status_Id == 1 || c.Status_Id == 3) 
                                                    && c.Deleted == 0);

            return statistics;
        }
    }
}