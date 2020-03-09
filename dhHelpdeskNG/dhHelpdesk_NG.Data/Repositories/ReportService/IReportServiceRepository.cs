namespace DH.Helpdesk.Dal.Repositories.ReportService
{
	using DH.Helpdesk.BusinessData.Models.ReportService;
	using System.Collections.Generic;

	public interface IReportServiceRepository
    {
        ReportData GetReportData(string reportIdentity, ReportSelectedFilter filters);
		IList<HistoricalDataResult> GetHistoricalData(HistoricalDataFilter filter);
        IList<ReportedTimeDataResult> GetReportedTimeData(ReportedTimeDataFilter filter);
        IList<NumberOfCaseDataResult> GetNumberOfCasesData(NumberOfCasesDataFilter filter);
        IList<SolvedInTimeDataResult> GetSolvedInTimeData(SolvedInTimeDataFilter filter);
    }
}