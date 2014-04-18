// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StatisticsService.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the StatisticsService type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------



namespace DH.Helpdesk.Services.Services.Concrete
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Statistics.Output;
    using DH.Helpdesk.Dal.Repositories;

    /// <summary>
    /// The statistics service.
    /// </summary>
    public sealed class StatisticsService : IStatisticsService
    {
        /// <summary>
        /// The _case repository.
        /// </summary>
        private readonly ICaseRepository caseRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatisticsService"/> class.
        /// </summary>
        /// <param name="caseRepository">
        /// The case repository.
        /// </param>
        public StatisticsService(ICaseRepository caseRepository)
        {
            this.caseRepository = caseRepository;
        }

        /// <summary>
        /// The get statistics.
        /// </summary>
        /// <param name="customers">
        /// The customers.
        /// </param>
        /// <returns>
        /// The <see cref="StatisticsOverview"/>.
        /// </returns>
        public StatisticsOverview GetStatistics(int[] customers)
        {
            var cases = this.caseRepository.GetCaseOverviews(customers);

            var statistics = new StatisticsOverview();

            statistics.ActiveCases = cases.Count(c => !c.FinishingDate.HasValue 
                                                    && c.Deleted == 0);
            statistics.EndedCases = cases.Count(c => c.FinishingDate.HasValue 
                                                    && c.Deleted == 0);
            statistics.WarningCases = cases.Count(c => !c.FinishingDate.HasValue 
                                                    && (c.StatusId == 1 || c.StatusId == 3) 
                                                    && c.Deleted == 0);

            return statistics;
        }
    }
}