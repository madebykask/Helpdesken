namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    #region DAILYREPORT

    public interface IDailyReportRepository : IRepository<DailyReport>
    {
    }

    public class DailyReportRepository : RepositoryBase<DailyReport>, IDailyReportRepository
    {
        public DailyReportRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region DAILYREPORTSUBJECT

    public interface IDailyReportSubjectRepository : IRepository<DailyReportSubject>
    {
    }

    public class DailyReportSubjectRepository : RepositoryBase<DailyReportSubject>, IDailyReportSubjectRepository
    {
        public DailyReportSubjectRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion
}
