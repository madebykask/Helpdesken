namespace DH.Helpdesk.Web.Infrastructure.Tools
{
    using DH.Helpdesk.Web.Models.Reports;

    public interface IReportsHelper
    {
        void CreateRegistratedCasesCaseTypeReport(
                            RegistratedCasesCaseTypeModel model, 
                            out string objectId,
                            out string fileName);

        byte[] GetReportImageFromCache(string objectId, string fileName);

        string GetReportPathFromCache(string objectId, string fileName);
    }
}