namespace DH.Helpdesk.Web.Infrastructure.Tools
{
    using DH.Helpdesk.Web.Models.Reports;

    public interface IReportsHelper
    {
        void CreateRegistratedCasesCaseTypeReport(RegistratedCasesCaseTypeModel model, out string cachedReportKey);

        byte[] GetReportImageFromCache(string cacheKey);
    }
}