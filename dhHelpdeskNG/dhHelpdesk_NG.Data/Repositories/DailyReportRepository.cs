using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DH.Helpdesk.BusinessData.Models.DailyReport.Output;

namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    #region DAILYREPORT

    public interface IDailyReportRepository : IRepository<DailyReport>
    {
        IEnumerable<DailyReportOverview> GetDailyReportOverviews(int[] customers);
    }

    public class DailyReportRepository : RepositoryBase<DailyReport>, IDailyReportRepository
    {
        public DailyReportRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public IEnumerable<DailyReportOverview> GetDailyReportOverviews(int[] customers)
        {
            return DataContext.DailyReports
                .Where(d => customers.Contains(d.Customer_Id))
                .Select(d => new DailyReportOverview()
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
