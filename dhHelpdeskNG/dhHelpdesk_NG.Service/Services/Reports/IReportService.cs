namespace DH.Helpdesk.Services.Services.Reports
{
    using System;
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Reports.Data.CaseSatisfaction;
    using DH.Helpdesk.BusinessData.Models.Reports.Data.CaseTypeArticleNo;
    using DH.Helpdesk.BusinessData.Models.Reports.Data.RegistratedCasesDay;
    using DH.Helpdesk.BusinessData.Models.Reports.Data.ReportGenerator;
    using DH.Helpdesk.BusinessData.Models.Reports.Enums;
    using DH.Helpdesk.BusinessData.Models.Reports.Options;
    using DH.Helpdesk.BusinessData.Models.Reports.Print;
    using DH.Helpdesk.BusinessData.Models.Shared.Input;

    public interface IReportService
    {
        RegistratedCasesDayOptions GetRegistratedCasesDayOptions(int customerId);

        RegistratedCasesDayData GetRegistratedCasesDayData(
                                    int customerId,
                                    int? departmentId,
                                    int[] caseTypeIds,
                                    int? workingGroupId,
                                    int? administratorId,
                                    DateTime period);

        CaseTypeArticleNoOptions GetCaseTypeArticleNoOptions(int customerId);

        CaseTypeArticleNoData GetCaseTypeArticleNoData(
                                    int customerId,
                                    List<int> departmentIds,
                                    List<int> workingGroupIds,
                                    List<int> caseTypeIds,
                                    int? productAreaId,
                                    DateTime? periodFrom,
                                    DateTime? periodUntil,
                                    ShowCases showCases);

        CaseTypeArticleNoPrintData GetCaseTypeArticleNoPrintData(
                                    int customerId,
                                    List<int> departmentIds,
                                    List<int> workingGroupIds,
                                    List<int> caseTypeIds,
                                    int? productAreaId,
                                    DateTime? periodFrom,
                                    DateTime? periodUntil,
                                    ShowCases showCases);

        CaseSatisfactionOptionsResponse GetCaseSatisfactionOptionsResponse(OperationContext context);

        CaseSatisfactionReportResponse GetCaseSatisfactionResponse(
            int customerId,
            DateTime finishingDateFrom,
            DateTime finishingDateTo,
            int[] selectedCaseTypes,
            int? selectedProductArea,
            int[] selectedWorkingGroups);

        RegistratedCasesCaseTypeOptionsResponse GetRegistratedCasesCaseTypeOptionsResponse(OperationContext context);

        ReportGeneratorOptions GetReportGeneratorOptions(int customerId, int languageId);

        ReportGeneratorData GetReportGeneratorData(
                                    int customerId,
                                    int languageId,
                                    List<int> fieldIds,
                                    List<int> departmentIds,
                                    List<int> workingGroupIds,
                                    int? caseTypeId,
                                    DateTime? periodFrom,
                                    DateTime? periodUntil,
                                    string text,
                                    SortField sort,
                                    int selectCount);
    }
}