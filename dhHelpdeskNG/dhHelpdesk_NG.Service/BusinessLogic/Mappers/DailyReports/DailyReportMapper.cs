namespace DH.Helpdesk.Services.BusinessLogic.Mappers.DailyReports
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.DailyReport.Output;
    using DH.Helpdesk.Domain;

    public static class DailyReportMapper
    {
        public static DailyReportOverview[] MapToOverviews(this IQueryable<DailyReport> query)
        {
            var entities = query.Select(d => new
                                        {
                                            d.CreatedDate,
                                            d.DailyReportSubject,
                                            d.DailyReportText
                                        }).ToArray();

            return entities.Select(d => new DailyReportOverview(
                                            d.CreatedDate,
                                            d.DailyReportSubject,
                                            d.DailyReportText)).ToArray();
        }
    }
}