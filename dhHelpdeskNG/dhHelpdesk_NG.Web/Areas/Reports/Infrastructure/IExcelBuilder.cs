namespace DH.Helpdesk.Web.Areas.Reports.Infrastructure
{
    using DH.Helpdesk.BusinessData.Models.Reports;
    using DH.Helpdesk.BusinessData.Models.Reports.Data.CaseTypeArticleNo;

    public interface IExcelBuilder
    {
        string GetExcelFileName(ReportType report);

        byte[] GetCaseTypeArticleNoExcel(
                    CaseTypeArticleNoData data,
                    bool isShowCaseTypeDetails,
                    bool isShowPercents);
    }
}