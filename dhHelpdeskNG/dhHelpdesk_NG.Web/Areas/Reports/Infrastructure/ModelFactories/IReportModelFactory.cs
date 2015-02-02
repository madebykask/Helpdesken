namespace DH.Helpdesk.Web.Areas.Reports.Infrastructure.ModelFactories
{
    using DH.Helpdesk.BusinessData.Models.Reports.Options;
    using DH.Helpdesk.Web.Areas.Reports.Models.Options;

    public interface IReportModelFactory
    {
        ReportsOptions GetReportsOptions();

        RegistratedCasesDayOptionsModel GetRegistratedCasesDayOptionsModel(RegistratedCasesDayOptions options);
    }
}