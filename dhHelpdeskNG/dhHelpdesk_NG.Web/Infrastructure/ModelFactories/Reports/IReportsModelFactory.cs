namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Reports
{
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Reports;
    using DH.Helpdesk.Web.Infrastructure.Filters.Reports;
    using DH.Helpdesk.Web.Models.Reports;

    public interface IReportsModelFactory
    {
        IndexModel CreateIndexModel();

        SearchModel CreateSearchModel(
            ReportsFilter filter, 
            SearchData searchData);

        RegistratedCasesCaseTypeOptions CreateRegistratedCasesCaseTypeOptions(OperationContext context);

        RegistratedCasesCaseTypeReport CreateRegistratedCasesCaseTypeReport(
                                RegistratedCasesCaseTypeOptions options,
                                OperationContext context);

        RegistratedCasesDayOptions CreateRegistratedCasesDayOptions(OperationContext context);

        RegistratedCasesDayReport CreateRegistratedCasesDayReport(
                                RegistratedCasesDayOptions options,
                                OperationContext context);

        AverageSolutionTimeOptions CreateAverageSolutionTimeOptions(OperationContext context);

        AverageSolutionTimeReport CreateAverageSolutionTimeReport(
                                AverageSolutionTimeOptions options,
                                OperationContext context);
    }
}