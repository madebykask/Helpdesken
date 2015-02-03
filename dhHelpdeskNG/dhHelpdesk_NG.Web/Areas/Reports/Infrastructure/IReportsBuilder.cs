namespace DH.Helpdesk.Web.Areas.Reports.Infrastructure
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Reports.Data;

    public interface IReportsBuilder
    {
        byte[] GetRegistratedCasesDayReport(RegistratedCasesDayData data, DateTime period, ReportTheme theme = null);
    }
}