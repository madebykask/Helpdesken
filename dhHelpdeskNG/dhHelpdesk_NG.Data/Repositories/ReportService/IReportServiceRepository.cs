namespace DH.Helpdesk.Dal.Repositories.ReportService
{
    using DH.Helpdesk.BusinessData.Models.ReportService;

    public interface IReportServiceRepository
    {
        ReportData GetReportData(string reportIdentity, ReportSelectedFilter filters);
    }
}