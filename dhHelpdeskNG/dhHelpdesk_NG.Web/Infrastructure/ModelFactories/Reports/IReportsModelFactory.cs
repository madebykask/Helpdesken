namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Reports
{
    using DH.Helpdesk.BusinessData.Models.Reports;
    using DH.Helpdesk.BusinessData.Models.Reports.Output;
    using DH.Helpdesk.Web.Infrastructure.Filters.Reports;
    using DH.Helpdesk.Web.Models.Reports;

    public interface IReportsModelFactory
    {
        IndexModel CreateIndexModel();

        SearchModel CreateSearchModel(
            ReportsFilter filter, 
            SearchData searchData);

        RegistratedCasesCaseTypeModel CreateRegistratedCasesCaseTypeModel(GetRegistratedCasesCaseTypeResponse response);
    }
}