namespace DH.Helpdesk.Web.Areas.Reports.Infrastructure
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Reports;
    using DH.Helpdesk.BusinessData.Models.Reports.Enums;
    using DH.Helpdesk.BusinessData.Models.Reports.Print;

    public interface IPrintBuilder
    {
        string GetPrintFileName(ReportType report);

        byte[] GetCaseTypeArticleNoPrint(
                CaseTypeArticleNoPrintData data,
                DateTime? periodFrom,
                DateTime? periodUntil,
                ShowCases showCases,
                bool isShowCaseTypeDetails,
                bool isShowPercents);
    }
}