namespace DH.Helpdesk.Web.Areas.Reports.Infrastructure
{
    using DH.Helpdesk.BusinessData.Models.Reports.Data.CaseTypeArticleNo;
    using DH.Helpdesk.BusinessData.Models.Reports.Enums;
    using DH.Helpdesk.Web.Areas.Reports.Models.Reports;
    using DH.Helpdesk.Web.Areas.Reports.Models.Reports.ReportGenerator;

    public interface IExcelBuilder
    {
        string GetExcelFileName(ReportType report);

        byte[] GetCaseTypeArticleNoExcel(
                    CaseTypeArticleNoData data,
                    bool isShowCaseTypeDetails,
                    bool isShowPercents);

        byte[] GetReportGeneratorExcel(ReportGeneratorModel data);

        byte[] GetFinishingCauseCustomerExcel(FinishingCauseCustomerModel data);
    }
}