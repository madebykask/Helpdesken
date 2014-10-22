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
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    #region DAILYREPORT

    /// <summary>
    /// The DailyReportRepository interface.
    /// </summary>
    public interface IDailyReportRepository : IRepository<DailyReport>
    {        
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
