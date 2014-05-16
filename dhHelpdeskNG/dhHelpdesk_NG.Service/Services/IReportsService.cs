namespace DH.Helpdesk.Services.Services
{
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Reports;
    using DH.Helpdesk.BusinessData.Models.Reports.Output;

    public interface IReportsService
    {
        SearchData GetSearchData(OperationContext context);

        GetRegistratedCasesCaseTypeResponse GetRegistratedCasesCaseTypeResponse(OperationContext context);
    }
}