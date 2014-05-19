namespace DH.Helpdesk.Web.Infrastructure.Tools
{
    using System.Web.Mvc;

    using DH.Helpdesk.Web.Models.Reports;

    public interface IReportsHelper
    {
        void CreateRegistratedCasesCaseTypeReport(RegistratedCasesCaseTypeModel model, out string cachedReportKey);

        FileContentResult GetReportImageFromCache(string cacheKey);
    }
}