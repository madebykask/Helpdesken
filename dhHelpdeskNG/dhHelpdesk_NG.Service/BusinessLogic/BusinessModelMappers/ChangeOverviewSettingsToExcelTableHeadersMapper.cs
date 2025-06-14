﻿namespace DH.Helpdesk.Services.BusinessLogic.BusinessModelMappers
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Enums.Changes.Fields;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeOverview;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelExport.ExcelExport;
    using DH.Helpdesk.Services.Localization;

    public sealed class ChangeOverviewSettingsToExcelTableHeadersMapper : IBusinessModelsMapper<ChangeOverviewSettings, List<ExcelTableHeader>>
    {
        public List<ExcelTableHeader> Map(ChangeOverviewSettings businessModel)
        {
            var headers = new List<ExcelTableHeader>();

            headers.Add(new ExcelTableHeader(Translator.Translate("Ändring"), "Id"));

            AddHeaderIfNeeded(businessModel.Orderer.Id, OrdererField.Id, headers);
            AddHeaderIfNeeded(businessModel.Orderer.Name, OrdererField.Name, headers);
            AddHeaderIfNeeded(businessModel.Orderer.Phone, OrdererField.Phone, headers);
            AddHeaderIfNeeded(businessModel.Orderer.CellPhone, OrdererField.CellPhone, headers);
            AddHeaderIfNeeded(businessModel.Orderer.Email, OrdererField.Email, headers);
            AddHeaderIfNeeded(businessModel.Orderer.Department, OrdererField.Department, headers);
            
            AddHeaderIfNeeded(businessModel.General.Priority, GeneralField.Priority, headers);
            AddHeaderIfNeeded(businessModel.General.Title, GeneralField.Title, headers);
            AddHeaderIfNeeded(businessModel.General.Status, GeneralField.Status, headers);
            AddHeaderIfNeeded(businessModel.General.System, GeneralField.System, headers);
            AddHeaderIfNeeded(businessModel.General.Object, GeneralField.Object, headers);
            AddHeaderIfNeeded(businessModel.General.Inventory, GeneralField.Inventory, headers);
            AddHeaderIfNeeded(businessModel.General.WorkingGroup, GeneralField.WorkingGroup, headers);
            AddHeaderIfNeeded(businessModel.General.Administrator, GeneralField.Administrator, headers);
            AddHeaderIfNeeded(businessModel.General.FinishingDate, GeneralField.FinishingDate, headers);
            AddHeaderIfNeeded(businessModel.General.Rss, GeneralField.Rss, headers);

//            AddHeaderIfNeeded(businessModel.Registration.Name, RegistrationField.Name, headers);
//            AddHeaderIfNeeded(businessModel.Registration.Phone, RegistrationField.Phone, headers);
//            AddHeaderIfNeeded(businessModel.Registration.Email, RegistrationField.Email, headers);
//            AddHeaderIfNeeded(businessModel.Registration.Company, RegistrationField.Company, headers);
            AddHeaderIfNeeded(businessModel.Registration.Owner, RegistrationField.Owner, headers);
            AddAffectedProcessesHeaderIfNeeded(businessModel.Registration.AffectedProcesses, businessModel.Registration.AffectedProcessesOverviews, headers);
            AddHeaderIfNeeded(businessModel.Registration.AffectedDepartments, RegistrationField.AffectedDepartments, headers);
            AddHeaderIfNeeded(businessModel.Registration.Description, RegistrationField.Description, headers);
            AddHeaderIfNeeded(businessModel.Registration.BusinessBenefits, RegistrationField.BusinessBenefits, headers);
            AddHeaderIfNeeded(businessModel.Registration.Consequence, RegistrationField.Consequence, headers);
            AddHeaderIfNeeded(businessModel.Registration.Impact, RegistrationField.Impact, headers);
            AddHeaderIfNeeded(businessModel.Registration.DesiredDate, RegistrationField.DesiredDate, headers);
            AddHeaderIfNeeded(businessModel.Registration.Verified, RegistrationField.Verified, headers);
//            AddHeaderIfNeeded(businessModel.Registration.AttachedFiles, RegistrationField.AttachedFiles, headers);
            AddHeaderIfNeeded(businessModel.Registration.Approval, RegistrationField.Approval, headers);
            AddHeaderIfNeeded(businessModel.Registration.RejectExplanation, RegistrationField.RejectExplanation, headers);
            
            AddHeaderIfNeeded(businessModel.Analyze.Category, AnalyzeField.Category, headers);
            AddHeaderIfNeeded(businessModel.Analyze.Priority, AnalyzeField.Priority, headers);
            AddHeaderIfNeeded(businessModel.Analyze.Responsible, AnalyzeField.Responsible, headers);
            AddHeaderIfNeeded(businessModel.Analyze.Solution, AnalyzeField.Solution, headers);
            AddHeaderIfNeeded(businessModel.Analyze.Cost, AnalyzeField.Cost, headers);
            AddHeaderIfNeeded(businessModel.Analyze.YearlyCost, AnalyzeField.YearlyCost, headers);
            AddHeaderIfNeeded(businessModel.Analyze.EstimatedTimeInHours, AnalyzeField.EstimatedTimeInHours, headers);
            AddHeaderIfNeeded(businessModel.Analyze.Risk, AnalyzeField.Risk, headers);
            AddHeaderIfNeeded(businessModel.Analyze.StartDate, AnalyzeField.StartDate, headers);
            AddHeaderIfNeeded(businessModel.Analyze.FinishDate, AnalyzeField.FinishDate, headers);
            AddHeaderIfNeeded(businessModel.Analyze.HasImplementationPlan, AnalyzeField.HasImplementationPlan, headers);
            AddHeaderIfNeeded(businessModel.Analyze.HasRecoveryPlan, AnalyzeField.HasRecoveryPlan, headers);
//            AddHeaderIfNeeded(businessModel.Analyze.AttachedFiles, AnalyzeField.AttachedFiles, headers);
//            AddHeaderIfNeeded(businessModel.Analyze.Logs, AnalyzeField.Logs, headers);
            AddHeaderIfNeeded(businessModel.Analyze.Approval, AnalyzeField.Approval, headers);
            AddHeaderIfNeeded(businessModel.Analyze.RejectRecommendation, AnalyzeField.RejectExplanation, headers);

            AddHeaderIfNeeded(businessModel.Implementation.Status, ImplementationField.Status, headers);
            AddHeaderIfNeeded(businessModel.Implementation.RealStartDate, ImplementationField.RealStartDate, headers);
            AddHeaderIfNeeded(businessModel.Implementation.BuildImplemented, ImplementationField.BuildImplemented, headers);
            AddHeaderIfNeeded(businessModel.Implementation.ImplementationPlanUsed, ImplementationField.ImplementationPlanUsed, headers);
            AddHeaderIfNeeded(businessModel.Implementation.Deviation, ImplementationField.Deviation, headers);
            AddHeaderIfNeeded(businessModel.Implementation.RecoveryPlanUsed, ImplementationField.RecoveryPlanUsed, headers);
            AddHeaderIfNeeded(businessModel.Implementation.FinishingDate, ImplementationField.FinishingDate, headers);
//            AddHeaderIfNeeded(businessModel.Implementation.AttachedFiles, ImplementationField.AttachedFiles, headers);
//            AddHeaderIfNeeded(businessModel.Implementation.Logs, ImplementationField.Logs, headers);
            AddHeaderIfNeeded(businessModel.Implementation.ImplementationReady, ImplementationField.ImplementationReady, headers);

            AddHeaderIfNeeded(businessModel.Evaluation.ChangeEvaluation, EvaluationField.ChangeEvaluation, headers);
//            AddHeaderIfNeeded(businessModel.Evaluation.AttachedFiles, EvaluationField.AttachedFiles, headers);
//            AddHeaderIfNeeded(businessModel.Evaluation.Logs, EvaluationField.Logs, headers);
            AddHeaderIfNeeded(businessModel.Evaluation.EvaluationReady, EvaluationField.EvaluationReady, headers);

            return headers;
        }

        private static void AddHeaderIfNeeded(FieldOverviewSetting setting, string fieldName, List<ExcelTableHeader> headers)
        {
            if (!setting.Show)
            {
                return;
            }

            var header = new ExcelTableHeader(setting.Caption, fieldName);
            headers.Add(header);
        }

        private static void AddAffectedProcessesHeaderIfNeeded(
                FieldOverviewSetting setting,
                IEnumerable<ItemOverview> overviews,
                List<ExcelTableHeader> headers)
        {
            if (!setting.Show)
            {
                return;
            }

            foreach (var overview in overviews)
            {
                var header = new ExcelTableHeader(
                            string.Format("{0} - {1}", setting.Caption, overview.Name), 
                            RegistrationField.GetAffectedProcess(overview.Value));
                headers.Add(header);                
            }
        }
    }
}
