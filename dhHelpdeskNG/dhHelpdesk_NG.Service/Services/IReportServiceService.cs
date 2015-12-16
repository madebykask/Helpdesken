using DH.Helpdesk.BusinessData.Models.ReportService;
namespace DH.Helpdesk.Services.Services
{
    public interface IReportServiceService
    {
        ReportFilter GetReportFilter(int defaultCustomerId, int? selectedCustomerId = null);

        ReportData GetReportData(string reportIdentity, ReportSelectedFilter filters);        
    }
}