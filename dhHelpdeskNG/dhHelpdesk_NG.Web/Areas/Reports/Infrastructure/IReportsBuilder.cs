namespace DH.Helpdesk.Web.Areas.Reports.Infrastructure
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Reports.Data;
    using DH.Helpdesk.BusinessData.Models.Reports.Data.RegistratedCasesDay;

    public interface IReportsBuilder
    {
        byte[] GetRegistratedCasesDayReport(RegistratedCasesDayData data, DateTime period, ReportTheme theme = null);
    }
}