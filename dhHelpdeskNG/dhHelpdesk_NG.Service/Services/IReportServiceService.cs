using DH.Helpdesk.BusinessData.Models.ReportService;
namespace DH.Helpdesk.Services.Services
{
    public interface IReportServiceService
    {
        ReportFilter GetReportFilter(int customerId, int userId, bool addOUsToDepartments = true);

        ReportData GetReportData(string reportIdentity, ReportSelectedFilter filters);        
    }
}