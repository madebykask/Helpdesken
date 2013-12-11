using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.Repositories
{
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
