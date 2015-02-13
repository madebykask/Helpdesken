namespace DH.Helpdesk.Web.Areas.Reports.Infrastructure.ModelFactories
{
    using DH.Helpdesk.BusinessData.Models.Reports.Data.ReportGenerator;
    using DH.Helpdesk.BusinessData.Models.Reports.Options;
    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.Web.Areas.Reports.Models.Options.ReportGenerator;
    using DH.Helpdesk.Web.Areas.Reports.Models.Reports.ReportGenerator;

    public interface IReportGeneratorModelFactory
    {
        ReportGeneratorOptionsModel GetReportGeneratorOptionsModel(ReportGeneratorOptions options, ReportGeneratorFilterModel filters);

        ReportGeneratorModel GetReportGeneratorModel(ReportGeneratorData data, SortField sortField);
    }
}