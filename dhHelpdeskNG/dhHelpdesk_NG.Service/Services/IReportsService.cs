namespace DH.Helpdesk.Services.Services
{
    using System;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Reports;
    using DH.Helpdesk.BusinessData.Models.Reports.Output;

    public interface IReportsService
    {
        SearchData GetSearchData(OperationContext context);

        RegistratedCasesCaseTypeOptionsResponse GetRegistratedCasesCaseTypeOptionsResponse(OperationContext context);

        RegistratedCasesCaseTypeReportResponse GetRegistratedCasesCaseTypeReportResponse(
                                                    OperationContext context,
                                                    int[] workingGroupsIds,
                                                    int[] caseTypesIds,
                                                    int? productAreaId,
                                                    DateTime periodFrom,
                                                    DateTime periodUntil);

        RegistratedCasesDayOptionsResponse GetRegistratedCasesDayOptionsResponse(OperationContext context);

        RegistratedCasesDayReportResponse GetRegistratedCasesDayReportResponse(
                                                    OperationContext context,
                                                    int? departmentId,
                                                    int[] caseTypesIds,
                                                    int? workingGroupId,
                                                    int? administrator,
                                                    DateTime period);
    }
}