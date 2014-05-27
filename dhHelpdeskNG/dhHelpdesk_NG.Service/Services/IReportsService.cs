namespace DH.Helpdesk.Services.Services
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Reports;
    using DH.Helpdesk.BusinessData.Models.Reports.Output;

    public interface IReportsService
    {
        SearchData GetSearchData(OperationContext context);

        RegistratedCasesCaseTypeOptionsResponse GetRegistratedCasesCaseTypeOptionsResponse(OperationContext context);

        RegistratedCasesCaseTypeReportResponse GetRegistratedCasesCaseTypeReportResponse(
                                                            OperationContext context,
                                                            IEnumerable<int> workingGroupsIds,
                                                            IEnumerable<int> caseTypesIds,
                                                            int? productAreaId);
    }
}