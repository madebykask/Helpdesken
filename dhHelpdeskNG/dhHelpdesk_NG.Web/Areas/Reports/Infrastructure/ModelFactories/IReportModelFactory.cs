namespace DH.Helpdesk.Web.Areas.Reports.Infrastructure.ModelFactories
{
    using DH.Helpdesk.Web.Areas.Reports.Models.Options;

    public interface IReportModelFactory
    {
        ReportsOptions GetReportsOptions();
    }
}