using DH.Helpdesk.BusinessData.Models.Reports;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Services.Services.Reports
{
    using System;
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Reports.Data.CaseSatisfaction;
    using DH.Helpdesk.BusinessData.Models.Reports.Data.CasesInProgressDay;
    using DH.Helpdesk.BusinessData.Models.Reports.Data.CaseTypeArticleNo;
    using DH.Helpdesk.BusinessData.Models.Reports.Data.ClosedCasesDay;
    using DH.Helpdesk.BusinessData.Models.Reports.Data.FinishingCauseCategoryCustomer;
    using DH.Helpdesk.BusinessData.Models.Reports.Data.FinishingCauseCustomer;
    using DH.Helpdesk.BusinessData.Models.Reports.Data.LeadtimeActiveCases;
    using DH.Helpdesk.BusinessData.Models.Reports.Data.LeadtimeFinishedCases;
    using DH.Helpdesk.BusinessData.Models.Reports.Data.RegistratedCasesDay;
    using DH.Helpdesk.BusinessData.Models.Reports.Data.ReportGenerator;
    using DH.Helpdesk.BusinessData.Models.Reports.Enums;
    using DH.Helpdesk.BusinessData.Models.Reports.Options;
    using DH.Helpdesk.BusinessData.Models.Reports.Print;
    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.BusinessData.OldComponents;
    using DH.Helpdesk.Common.Enums;

    public interface IReportService
    {
        List<ReportType> GetAvailableCustomerReports(int customerId);

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
                                    int userId,
                                    int languageId,
                                    List<int> fieldIds,
                                    List<int> departmentIds,
                                    List<int> ouIds,
                                    List<int> workingGroupIds,
                                    List<int> productAreaIds,
                                    List<int> administratorIds,
                                    List<int> caseStatusIds,
                                    List<int> caseTypeIds,
									List<string> extendedCaseFormFieldIds,
									int? extendedCaseFormId,
                                    DateTime? periodFrom,
                                    DateTime? periodUntil,
                                    string text,
                                    SortField sort,
                                    int selectCount,
                                    DateTime? closeFrom,
                                    DateTime? closeTo);

        Dictionary<DateTime, int> GetReportGeneratorAggregation(
            int customerId,
            int userId,
            int languageId,
            List<int> fieldIds,
            List<int> departmentIds,
            List<int> ouIds,
            List<int> workingGroupIds,
            List<int> productAreaIds,
            List<int> administratorIds,
            List<int> caseStatusIds,
            List<int> caseTypeIds,
			List<string> extendedCaseFormFieldIds,
			int? extendedCaseFormFieldId,
            DateTime? periodFrom,
            DateTime? periodUntil,
            string text,
            SortField sort,
            int selectCount,
            DateTime? closeFrom,
            DateTime? closeTo);

        LeadtimeFinishedCasesOptions GetLeadtimeFinishedCasesOptions(int customerId);

        LeadtimeFinishedCasesData GetLeadtimeFinishedCasesData(
                                    int customerId,
                                    List<int> departmentIds,
                                    int? caseTypeId,
                                    List<int> workingGroupIds,
                                    CaseRegistrationSource registrationSource,
                                    DateTime? periodFrom,
                                    DateTime? periodUntil,
                                    int leadTime,
                                    bool isShowDetails);

        LeadtimeActiveCasesOptions GetLeadtimeActiveCasesOptions(int customerId);

        LeadtimeActiveCasesData GetLeadtimeActiveCasesData(
                                    int customerId,
                                    List<int> departmentIds,
                                    int? caseTypeId,
                                    int highHours,
                                    int mediumDays,
                                    int lowDays);

        FinishingCauseCustomerOptions GetFinishingCauseCustomerOptions(int customerId);

        FinishingCauseCustomerData GetFinishingCauseCustomerData(
                                    int customerId,
                                    List<int> departmentIds,
                                    int? workingGroupId,
                                    int? caseTypeId,
                                    int? administratorId,
                                    DateTime? periodFrom,
                                    DateTime? periodUntil);

        FinishingCauseCategoryCustomerOptions GetFinishingCauseCategoryCustomerOptions(int customerId);

        FinishingCauseCategoryCustomerData GetFinishingCauseCategoryCustomerData(
                                    int customerId,
                                    List<int> departmentIds,
                                    List<int> workingGroupIds,
                                    int? caseTypeId,
                                    DateTime? periodFrom,
                                    DateTime? periodUntil);

        ClosedCasesDayOptions GetClosedCasesDayOptions(int customerId);

        ClosedCasesDayData GetClosedCasesDayData(
                                    int customerId,
                                    List<int> departmentIds,
                                    int? workingGroupId,
                                    int? caseTypeId,
                                    int? administratorId,
                                    DateTime period);

        CasesInProgressDayOptions GetCasesInProgressDayOptions(int customerId);

        CasesInProgressDayData GetCasesInProgressDayData(
                                    int customerId,
                                    int? departmentId,
                                    int? workingGroupId,
                                    int? administratorId,
                                    DateTime period);

        List<ReportFavoriteList> GetCustomerReportFavoriteList(int customerId, int? userId);
        ReportFavorite GetCustomerReportFavorite(int reportFavoriteId, int customerId, int? userId);
        int SaveCustomerReportFavorite(ReportFavorite reportFavorite);
        void DeleteCustomerReportFavorite(int reportFavoriteId, int customerId, int? userId);
    }
}