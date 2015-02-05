namespace DH.Helpdesk.Services.Services.Reports
{
    using System;
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Reports.Data.CaseTypeArticleNo;
    using DH.Helpdesk.BusinessData.Models.Reports.Data.RegistratedCasesDay;
    using DH.Helpdesk.BusinessData.Models.Reports.Enums;
    using DH.Helpdesk.BusinessData.Models.Reports.Options;
    using DH.Helpdesk.BusinessData.Models.Reports.Print;

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
    }
}