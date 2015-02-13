namespace DH.Helpdesk.Web.Areas.Reports.Infrastructure.ModelFactories
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Reports.Data.CaseTypeArticleNo;
    using DH.Helpdesk.BusinessData.Models.Reports.Data.LeadtimeFinishedCases;
    using DH.Helpdesk.BusinessData.Models.Reports.Enums;
    using DH.Helpdesk.BusinessData.Models.Reports.Options;
    using DH.Helpdesk.Web.Areas.Reports.Models.Options;
    using DH.Helpdesk.Web.Areas.Reports.Models.Reports;

    public interface IReportModelFactory
    {
        ReportsOptions GetReportsOptions(List<ReportType> reports);

        RegistratedCasesDayOptionsModel GetRegistratedCasesDayOptionsModel(RegistratedCasesDayOptions options);

        CaseTypeArticleNoOptionsModel GetCaseTypeArticleNoOptionsModel(CaseTypeArticleNoOptions options);

        CaseTypeArticleNoModel GetCaseTypeArticleNoModel(
                                CaseTypeArticleNoData data, 
                                bool isShowCaseTypeDetails,
                                bool isShowPercents);

        CaseSatisfactionOptions CreateCaseSatisfactionOptions(OperationContext context);

        CaseSatisfactionReport CreateCaseSatisfactionReport(CaseSatisfactionOptions options, OperationContext context);

        LeadtimeFinishedCasesOptionsModel GetLeadtimeFinishedCasesOptionsModel(LeadtimeFinishedCasesOptions options);

        LeadtimeFinishedCasesModel GetLeadtimeFinishedCasesModel(
                                LeadtimeFinishedCasesData data,
                                bool isShowDetails);
    }
}