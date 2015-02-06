namespace DH.Helpdesk.Web.Areas.Reports.Infrastructure.ModelFactories
{
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Reports.Data.CaseTypeArticleNo;
    using DH.Helpdesk.BusinessData.Models.Reports.Data.ReportGenerator;
    using DH.Helpdesk.BusinessData.Models.Reports.Options;
    using DH.Helpdesk.Web.Areas.Reports.Models.Options;
    using DH.Helpdesk.Web.Areas.Reports.Models.Reports;

    public interface IReportModelFactory
    {
        ReportsOptions GetReportsOptions();

        RegistratedCasesDayOptionsModel GetRegistratedCasesDayOptionsModel(RegistratedCasesDayOptions options);

        CaseTypeArticleNoOptionsModel GetCaseTypeArticleNoOptionsModel(CaseTypeArticleNoOptions options);

        CaseTypeArticleNoModel GetCaseTypeArticleNoModel(
                                CaseTypeArticleNoData data, 
                                bool isShowCaseTypeDetails,
                                bool isShowPercents);

        CaseSatisfactionOptions CreateCaseSatisfactionOptions(OperationContext context);

        CaseSatisfactionReport CreateCaseSatisfactionReport(CaseSatisfactionOptions options, OperationContext context);

        ReportGeneratorOptionsModel GetReportGeneratorOptionsModel(ReportGeneratorOptions options);

        ReportGeneratorModel GetReportGeneratorModel(ReportGeneratorData data);
    }
}