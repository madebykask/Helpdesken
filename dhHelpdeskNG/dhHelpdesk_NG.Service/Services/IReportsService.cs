namespace DH.Helpdesk.Services.Services
{
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Reports;

    public interface IReportsService
    {
        SearchData GetSearchData(OperationContext context);
    }
}