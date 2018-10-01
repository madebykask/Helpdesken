using DH.Helpdesk.Common.Enums;

namespace DH.Helpdesk.Web.Areas.Reports.Infrastructure.ModelFactories.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Enums.Case.Fields;
    using DH.Helpdesk.BusinessData.Models.Case.CaseOverview;
    using DH.Helpdesk.BusinessData.Models.Case.CaseSettingsOverview;
    using DH.Helpdesk.BusinessData.Models.Reports.Data.ReportGenerator;
    using DH.Helpdesk.BusinessData.Models.Reports.Options;
    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.Web.Areas.Reports.Models.Options.ReportGenerator;
    using DH.Helpdesk.Web.Areas.Reports.Models.Reports.ReportGenerator;
    using DH.Helpdesk.Web.Infrastructure.Extensions;
    using DH.Helpdesk.Web.Infrastructure.Tools;
    using DH.Helpdesk.Web.Models.Shared;

    public sealed class ReportGeneratorModelFactory : IReportGeneratorModelFactory
    {
        public ReportGeneratorOptionsModel GetReportGeneratorOptionsModel(ReportGeneratorOptions options, ReportGeneratorFilterModel filters)
        {
            var fields = WebMvcHelper.CreateMultiSelectField(options.Fields, filters.FieldIds);
            var departments = WebMvcHelper.CreateMultiSelectField(options.Departments, filters.DepartmentIds);
            var workingGroups = WebMvcHelper.CreateMultiSelectField(options.WorkingGroups, filters.WorkingGroupIds);
            var caseTypes = options.CaseTypes;
            var caseTypeIds = filters.CaseTypeIds;
            var periodFrom = filters.PeriodFrom.HasValue ? filters.PeriodFrom.Value : DateTime.Today;
            var periodUntil = filters.PeriodUntil.HasValue ? filters.PeriodUntil.Value : DateTime.Today;
            var closeFrom = filters.CloseFrom.HasValue ? filters.CloseFrom.Value : (DateTime?)null;
            var closeTo = filters.CloseTo.HasValue ? filters.CloseTo.Value : (DateTime?)null;

            return new ReportGeneratorOptionsModel(
                                    fields,
                                    departments,
                                    workingGroups,
                                    caseTypes,
                                    caseTypeIds, 
                                    periodFrom,
                                    periodUntil,
                                    filters.RecordsOnPage,
                                    filters.SortField == null ? "" : filters.SortField.Name,
                                    filters.SortField == null ? (SortBy?)null : filters.SortField.SortBy,
                                    closeFrom,
                                    closeTo);
        }

        public ReportGeneratorModel GetReportGeneratorModel(ReportGeneratorData data, SortField sortField)
        {
            var headers = new List<GridColumnHeaderModel>();
            CreateFullHeaders(data.Settings, headers);

            var cases = data.Cases.Select(c => CreateFullValues(data.Settings, c)).ToList();

            return new ReportGeneratorModel(headers, cases, cases.Count, sortField);
        }

        #region Create headers

        private static void CreateFullHeaders(FullCaseSettings settings, List<GridColumnHeaderModel> headers)
        {
            CreateUserHeaders(settings.User, headers);
            CreateComputerHeaders(settings.Computer, headers);
            CreateCaseInfoHeaders(settings.CaseInfo, headers);
            CreateOtherHeaders(settings.Other, headers);
            CreateLogHeaders(settings.Log, headers);
        }

        private static void CreateUserHeaders(UserSettings settings, List<GridColumnHeaderModel> headers)
        {
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.User, UserFields.User, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.Notifier, UserFields.Notifier, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.Email, UserFields.Email, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.Phone, UserFields.Phone, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.CellPhone, UserFields.CellPhone, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.Customer, UserFields.Customer, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.Region, UserFields.Region, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.Department, UserFields.Department, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.Unit, UserFields.Unit, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.Place, UserFields.Place, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.OrdererCode, UserFields.OrdererCode, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.CostCentre, UserFields.CostCentre, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.IsAbout_User, UserFields.IsAbout_User, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.IsAbout_Persons_Name, UserFields.IsAbout_Persons_Name, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.IsAbout_Persons_Phone, UserFields.IsAbout_Persons_Phone, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.IsAbout_UserCode, UserFields.IsAbout_UserCode, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.IsAbout_Persons_Email, UserFields.IsAbout_Persons_Email, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.IsAbout_Persons_CellPhone, UserFields.IsAbout_Persons_CellPhone, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.IsAbout_CostCentre, UserFields.IsAbout_CostCentre, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.IsAbout_Place, UserFields.IsAbout_Place, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.IsAbout_Department, UserFields.IsAbout_Department, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.IsAbout_OU, UserFields.IsAbout_OU, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.IsAbout_Region, UserFields.IsAbout_Region, headers);

        }

        private static void CreateComputerHeaders(ComputerSettings settings, List<GridColumnHeaderModel> headers)
        {
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.PcNumber, ComputerFields.PcNumber, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.ComputerType, ComputerFields.ComputerType, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.Place, ComputerFields.Place, headers);
        }

        private static void CreateCaseInfoHeaders(CaseInfoSettings settings, List<GridColumnHeaderModel> headers)
        {
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.Case, CaseInfoFields.Case, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.RegistrationDate, CaseInfoFields.RegistrationDate, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.ChangeDate, CaseInfoFields.ChangeDate, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.RegistratedBy, CaseInfoFields.RegistratedBy, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.CaseType, CaseInfoFields.CaseType, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.ProductArea, CaseInfoFields.ProductArea, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.System, CaseInfoFields.System, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.UrgentDegree, CaseInfoFields.UrgentDegree, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.Impact, CaseInfoFields.Impact, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.Category, CaseInfoFields.Category, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.Supplier, CaseInfoFields.Supplier, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.InvoiceNumber, CaseInfoFields.InvoiceNumber, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.ReferenceNumber, CaseInfoFields.ReferenceNumber, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.Caption, CaseInfoFields.Caption, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.Description, CaseInfoFields.Description, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.Other, CaseInfoFields.Other, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.PhoneContact, CaseInfoFields.PhoneContact, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.Sms, CaseInfoFields.Sms, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.AgreedDate, CaseInfoFields.AgreedDate, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.Available, CaseInfoFields.Available, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.Cost, CaseInfoFields.Cost, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.AttachedFile, CaseInfoFields.AttachedFile, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.RegistrationSource, CaseInfoFields.RegistrationSourceCustomer, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.LeadTime, CaseInfoFields.LeadTime, headers);
        }

        private static void CreateOtherHeaders(OtherSettings settings, List<GridColumnHeaderModel> headers)
        {
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.WorkingGroup, OtherFields.WorkingGroup, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.Responsible, OtherFields.Responsible, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.Administrator, OtherFields.Administrator, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.Priority, OtherFields.Priority, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.State, OtherFields.State, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.SubState, OtherFields.SubState, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.PlannedActionDate, OtherFields.PlannedActionDate, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.WatchDate, OtherFields.WatchDate, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.Verified, OtherFields.Verified, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.VerifiedDescription, OtherFields.VerifiedDescription, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.SolutionRate, OtherFields.SolutionRate, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.CausingPart, OtherFields.CausingPart, headers);
        }

        private static void CreateLogHeaders(LogSettings settings, List<GridColumnHeaderModel> headers)
        {
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.InternalLogNote, LogFields.InternalLogNote, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.ExternalLogNote, LogFields.ExternalLogNote, headers);
            //FieldSettingsHelper.CreateHeaderIfNeeded(settings.Debiting, LogFields.Debiting, headers);
            //FieldSettingsHelper.CreateHeaderIfNeeded(settings.AttachedFile, LogFields.AttachedFile, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.FinishingDescription, LogFields.FinishingDescription, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.FinishingDate, LogFields.FinishingDate, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.FinishingCause, LogFields.FinishingCause, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.TotalWork, LogFields.TotalWork, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.TotalOverTime, LogFields.TotalOverTime, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.TotalMaterial, LogFields.TotalMaterial, headers);
            FieldSettingsHelper.CreateHeaderIfNeeded(settings.TotalPrice, LogFields.TotalPrice, headers);
        }

        #endregion

        #region Create values

        private static CaseOverviewModel CreateFullValues(FullCaseSettings settings, FullCaseOverview overview)
        {
            var values = new List<NewGridRowCellValueModel>();
            CreateUserValues(settings.User, overview.User, values);
            CreateComputerValues(settings.Computer, overview.Computer, values);
            CreateCaseInfoValues(settings.CaseInfo, overview.CaseInfo, values);
            CreateOtherValues(settings.Other, overview.Other, values);
            CreateLogValues(settings.Log, overview.Log, values);

            return new CaseOverviewModel(overview.Id, values);
        }

        private static void CreateUserValues(UserSettings settings, UserOverview fields, List<NewGridRowCellValueModel> values)
        {
            FieldSettingsHelper.CreateValueIfNeeded(settings.User, UserFields.User, fields.User, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.Notifier, UserFields.Notifier, fields.Notifier, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.Email, UserFields.Email, fields.Email, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.Phone, UserFields.Phone, fields.Phone, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.CellPhone, UserFields.CellPhone, fields.CellPhone, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.Customer, UserFields.Customer, fields.Customer, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.Region, UserFields.Region, fields.Region, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.Department, UserFields.Department, fields.Department, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.Unit, UserFields.Unit, fields.Unit, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.Place, UserFields.Place, fields.Place, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.OrdererCode, UserFields.OrdererCode, fields.OrdererCode, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.CostCentre, UserFields.CostCentre, fields.CostCentre, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.IsAbout_User, UserFields.IsAbout_User, fields.IsAbout_User, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.IsAbout_Persons_Name, UserFields.IsAbout_Persons_Name, fields.IsAbout_Persons_Name, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.IsAbout_Persons_Phone, UserFields.IsAbout_Persons_Phone, fields.IsAbout_Persons_Phone, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.IsAbout_UserCode, UserFields.IsAbout_UserCode, fields.IsAbout_UserCode, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.IsAbout_Persons_Email, UserFields.IsAbout_Persons_Email, fields.IsAbout_Persons_Email, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.IsAbout_Persons_CellPhone, UserFields.IsAbout_Persons_CellPhone, fields.IsAbout_Persons_CellPhone, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.IsAbout_CostCentre, UserFields.IsAbout_CostCentre, fields.IsAbout_ConstCentre, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.IsAbout_Place, UserFields.IsAbout_Place, fields.IsAbout_Place, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.IsAbout_Department, UserFields.IsAbout_Department, fields.IsAbout_Department, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.IsAbout_OU, UserFields.IsAbout_OU, fields.IsAbout_OU, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.IsAbout_Region, UserFields.IsAbout_Region, fields.IsAbout_Region, values);
        }

        private static void CreateComputerValues(ComputerSettings settings, ComputerOverview fields, List<NewGridRowCellValueModel> values)
        {
            FieldSettingsHelper.CreateValueIfNeeded(settings.PcNumber, ComputerFields.PcNumber, fields.PcNumber, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.ComputerType, ComputerFields.ComputerType, fields.ComputerType, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.Place, ComputerFields.Place, fields.Place, values);
        }

        private static void CreateCaseInfoValues(CaseInfoSettings settings, CaseInfoOverview fields, List<NewGridRowCellValueModel> values)
        {
            FieldSettingsHelper.CreateValueIfNeeded(settings.Case, CaseInfoFields.Case, fields.Case, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.RegistrationDate, CaseInfoFields.RegistrationDate, fields.RegistrationDate, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.ChangeDate, CaseInfoFields.ChangeDate, fields.ChangeDate, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.RegistratedBy, CaseInfoFields.RegistratedBy, fields.RegistratedBy, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.CaseType, CaseInfoFields.CaseType, fields.CaseType, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.ProductArea, CaseInfoFields.ProductArea, fields.ProductArea, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.System, CaseInfoFields.System, fields.System, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.UrgentDegree, CaseInfoFields.UrgentDegree, fields.UrgentDegree, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.Impact, CaseInfoFields.Impact, fields.Impact, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.Category, CaseInfoFields.Category, fields.Category, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.Supplier, CaseInfoFields.Supplier, fields.Supplier, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.InvoiceNumber, CaseInfoFields.InvoiceNumber, fields.InvoiceNumber, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.ReferenceNumber, CaseInfoFields.ReferenceNumber, fields.ReferenceNumber, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.Caption, CaseInfoFields.Caption, fields.Caption, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.Description, CaseInfoFields.Description, fields.Description, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.Other, CaseInfoFields.Other, fields.Other, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.PhoneContact, CaseInfoFields.PhoneContact, fields.PhoneContact, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.Sms, CaseInfoFields.Sms, fields.Sms, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.AgreedDate, CaseInfoFields.AgreedDate, fields.AgreedDate, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.Available, CaseInfoFields.Available, fields.Available, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.Cost, CaseInfoFields.Cost, fields.Cost, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.AttachedFile, CaseInfoFields.AttachedFile, fields.AttachedFile, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.RegistrationSource, CaseInfoFields.RegistrationSourceCustomer, fields.RegistrationSource, values);
            FieldSettingsHelper.CreateSolvedTimeValueIfNeeded(settings.LeadTime, CaseInfoFields.LeadTime, fields.SolvedInTime, values);

        }

        private static void CreateOtherValues(OtherSettings settings, OtherOverview fields, List<NewGridRowCellValueModel> values)
        {
            FieldSettingsHelper.CreateValueIfNeeded(settings.WorkingGroup, OtherFields.WorkingGroup, fields.WorkingGroup, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.Responsible, OtherFields.Responsible, fields.Responsible, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.Administrator, OtherFields.Administrator, fields.Administrator, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.Priority, OtherFields.Priority, fields.Priority, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.State, OtherFields.State, fields.State, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.SubState, OtherFields.SubState, fields.SubState, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.PlannedActionDate, OtherFields.PlannedActionDate, fields.PlannedActionDate, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.WatchDate, OtherFields.WatchDate, fields.WatchDate, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.Verified, OtherFields.Verified, fields.Verified, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.VerifiedDescription, OtherFields.VerifiedDescription, fields.VerifiedDescription, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.SolutionRate, OtherFields.SolutionRate, fields.SolutionRate, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.CausingPart, OtherFields.CausingPart, fields.CausingPart, values);
        }

        private static void CreateLogValues(LogSettings settings, LogsOverview fields, List<NewGridRowCellValueModel> values)
        {            
            FieldSettingsHelper.CreateValueIfNeeded(settings.InternalLogNote, LogFields.InternalLogNote, fields.IntenalLogNote, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.ExternalLogNote, LogFields.ExternalLogNote, fields.ExternalLogNote, values);
            //FieldSettingsHelper.CreateValueIfNeeded(settings.Debiting, LogFields.Debiting, debiting, values);
            //FieldSettingsHelper.CreateValueIfNeeded(settings.AttachedFile, LogFields.AttachedFile, attachedFiles, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.FinishingDescription, LogFields.FinishingDescription, fields.FinishingDescription, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.FinishingDate, LogFields.FinishingDate, fields.ClosingDate, values);
            FieldSettingsHelper.CreateValueIfNeeded(settings.FinishingCause, LogFields.FinishingCause, fields.FinishingCause, values);
            FieldSettingsHelper.CreateTotalTimeValueIfNeeded(settings.TotalWork, LogFields.TotalWork, fields.TotalWork, values);
            FieldSettingsHelper.CreateTotalTimeValueIfNeeded(settings.TotalOverTime, LogFields.TotalOverTime, fields.TotalOverTime, values);
            FieldSettingsHelper.CreateTotalValueIfNeeded(settings.TotalMaterial, LogFields.TotalMaterial, fields.TotalMaterial, values);
            FieldSettingsHelper.CreateTotalValueIfNeeded(settings.TotalPrice, LogFields.TotalPrice, fields.TotalPrice, values);
        }

        #endregion
    }
}