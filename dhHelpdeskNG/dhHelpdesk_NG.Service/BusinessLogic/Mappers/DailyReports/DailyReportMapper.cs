using System.Linq;
namespace DH.Helpdesk.Services.BusinessLogic.Mappers.DailyReports
{
    

    using DH.Helpdesk.BusinessData.Models.DailyReport.Output;
    using DH.Helpdesk.Domain;

    public static class DailyReportMapper
    {
        public static DailyReportOverview[] MapToOverviews(this IQueryable<DailyReport> query)
        {
            var entities = query.Select(d => new
                                        {
                                            d.Id,
                                            d.MailSent,
                                            d.User.UserID,
                                            d.CreatedDate,
                                            d.DailyReportSubject,
                                            d.DailyReportText,
                                            d.User.FirstName,
                                            d.User.SurName
                                        }).ToArray();

            return entities.Select(d => new DailyReportOverview(
                                            d.Id,
                                            d.MailSent,
                                            d.UserID,
                                            d.CreatedDate,
                                            d.DailyReportSubject,
                                            d.DailyReportText,
                                            d.FirstName,
                                            d.SurName)).ToArray();
        }
    }
}