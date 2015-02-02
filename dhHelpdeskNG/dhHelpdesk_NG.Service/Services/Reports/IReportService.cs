namespace DH.Helpdesk.Services.Services.Reports
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Reports.Data;
    using DH.Helpdesk.BusinessData.Models.Reports.Options;

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
    }
}