namespace DH.Helpdesk.Web.Areas.Reports.Infrastructure
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Reports.Data.ClosedCasesDay;
    using DH.Helpdesk.BusinessData.Models.Reports.Data.RegistratedCasesDay;

    public interface IReportsBuilder
    {
        byte[] GetRegistratedCasesDayReport(RegistratedCasesDayData data, DateTime period, ReportTheme theme = null);

        void CreateCaseSatisfactionReport(int goodVotes, int normalVotes, int badVotes, int count, out ReportFile file);

        byte[] GetReportImageFromCache(string objectId, string fileName);

        string GetReportPathFromCache(string objectId, string fileName);

        byte[] GetClosedCasesDayReport(ClosedCasesDayData data, DateTime period, ReportTheme theme = null);
    }
}