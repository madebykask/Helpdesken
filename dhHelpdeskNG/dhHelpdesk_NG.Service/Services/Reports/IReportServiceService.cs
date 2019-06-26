using DH.Helpdesk.BusinessData.Models.ReportService;
using System.Collections.Generic;

namespace DH.Helpdesk.Services.Services.Reports
{
    public interface IReportServiceService
    {
        ReportFilter GetReportFilter(int customerId, int userId, bool addOUsToDepartments = true);

        ReportData GetReportData(string reportIdentity, ReportSelectedFilter filters, int userId, int customerId);

		IList<HistoricalDataResult> GetHistoricalData(HistoricalDataFilter filter, int userId);
        IList<ReportedTimeDataResult> GetReportedTimeData(ReportedTimeDataFilter filter, int userId);
        IList<NumberOfCaseDataResult> GetNumberOfCasesData(NumberOfCasesDataFilter filter, int userId);
    }
}