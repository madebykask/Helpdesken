// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DailyReportRepository.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the IDailyReportRepository type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.DailyReport.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    #region DAILYREPORT

    /// <summary>
    /// The DailyReportRepository interface.
    /// </summary>
    public interface IDailyReportRepository : IRepository<DailyReport>
    {
        /// <summary>
        /// The get daily report overviews.
        /// </summary>
        /// <param name="customers">
        /// The customers.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
        IEnumerable<DailyReportOverview> GetDailyReportOverviews(int[] customers);
    }

    /// <summary>
    /// The daily report repository.
    /// </summary>
    public class DailyReportRepository : RepositoryBase<DailyReport>, IDailyReportRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DailyReportRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">
        /// The database factory.
        /// </param>
        public DailyReportRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        /// <summary>
        /// The get daily report overviews.
        /// </summary>
        /// <param name="customers">
        /// The customers.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
        public IEnumerable<DailyReportOverview> GetDailyReportOverviews(int[] customers)
        {
            var entities = this.GetSecuredEntities(this.Table
                .Where(d => customers.Contains(d.Customer_Id))
                .Select(d => new 
                {
                    d.Customer_Id,
                    d.CreatedDate,
                    d.DailyReportSubject,
                    d.DailyReportText
                })
                .OrderByDescending(p => p.CreatedDate));

            return entities.Select(d => new DailyReportOverview()
                {
                    Customer_Id = d.Customer_Id,
                    CreatedDate = d.CreatedDate,
                    DailyReportSubject = d.DailyReportSubject,
                    DailyReportText = d.DailyReportText
                });
        }
    }

    #endregion

    #region DAILYREPORTSUBJECT

    /// <summary>
    /// The DailyReportSubjectRepository interface.
    /// </summary>
    public interface IDailyReportSubjectRepository : IRepository<DailyReportSubject>
    {
    }

    /// <summary>
    /// The daily report subject repository.
    /// </summary>
    public class DailyReportSubjectRepository : RepositoryBase<DailyReportSubject>, IDailyReportSubjectRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DailyReportSubjectRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">
        /// The database factory.
        /// </param>
        public DailyReportSubjectRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion
}
