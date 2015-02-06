namespace DH.Helpdesk.Web.Areas.Reports.Infrastructure
{
    using DH.Helpdesk.BusinessData.Models.Reports.Data.CaseTypeArticleNo;
    using DH.Helpdesk.BusinessData.Models.Reports.Enums;

    public interface IExcelBuilder
    {
        string GetExcelFileName(ReportType report);

        byte[] GetCaseTypeArticleNoExcel(
                    CaseTypeArticleNoData data,
                    bool isShowCaseTypeDetails,
                    bool isShowPercents);
    }
}