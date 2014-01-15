namespace dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Changes.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using dhHelpdesk_NG.DTO.DTOs;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.Data;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings;
    using dhHelpdesk_NG.DTO.Enums.Changes;
    using dhHelpdesk_NG.Web.Models;
    using dhHelpdesk_NG.Web.Models.Changes;

    public sealed class ChangesGridModelFactory : IChangesGridModelFactory
    {
        public ChangesGridModel Create(SearchResultDto searchResult, FieldOverviewSettingsDto fieldSettings)
        {
            var headers = new List<GridColumnHeaderModel>();

            CreateHeaderIfNeeded(fieldSettings.Orderer.Id, OrdererField.Id, headers);
            CreateHeaderIfNeeded(fieldSettings.Orderer.Name, OrdererField.Name, headers);
            CreateHeaderIfNeeded(fieldSettings.Orderer.Phone, OrdererField.Phone, headers);
            CreateHeaderIfNeeded(fieldSettings.Orderer.CellPhone, OrdererField.CellPhone, headers);
            CreateHeaderIfNeeded(fieldSettings.Orderer.Email, OrdererField.Email, headers);
            CreateHeaderIfNeeded(fieldSettings.Orderer.Department, OrdererField.Department, headers);

            CreateHeaderIfNeeded(fieldSettings.General.Priority, GeneralField.Priority, headers);
            CreateHeaderIfNeeded(fieldSettings.General.Title, GeneralField.Title, headers);
            CreateHeaderIfNeeded(fieldSettings.General.State, GeneralField.State, headers);
            CreateHeaderIfNeeded(fieldSettings.General.System, GeneralField.System, headers);
            CreateHeaderIfNeeded(fieldSettings.General.Object, GeneralField.Object, headers);
            CreateHeaderIfNeeded(fieldSettings.General.Inventory, GeneralField.Inventory, headers);
            CreateHeaderIfNeeded(fieldSettings.General.Owner, GeneralField.Owner, headers);
            CreateHeaderIfNeeded(fieldSettings.General.WorkingGroup, GeneralField.WorkingGroup, headers);
            CreateHeaderIfNeeded(fieldSettings.General.Administrator, GeneralField.Administrator, headers);
            CreateHeaderIfNeeded(fieldSettings.General.FinishingDate, GeneralField.FinishingDate, headers);
            CreateHeaderIfNeeded(fieldSettings.General.Rss, GeneralField.Rss, headers);

            CreateHeaderIfNeeded(fieldSettings.Registration.Description, RegistrationField.Description, headers);
            CreateHeaderIfNeeded(fieldSettings.Registration.BusinessBenefits, RegistrationField.BusinessBenefits, headers);
            CreateHeaderIfNeeded(fieldSettings.Registration.Consequence, RegistrationField.Consequence, headers);
            CreateHeaderIfNeeded(fieldSettings.Registration.Impact, RegistrationField.Impact, headers);
            CreateHeaderIfNeeded(fieldSettings.Registration.DesiredDate, RegistrationField.DesiredDate, headers);
            CreateHeaderIfNeeded(fieldSettings.Registration.Verified, RegistrationField.Verified, headers);
            CreateHeaderIfNeeded(fieldSettings.Registration.Approval, RegistrationField.Approval, headers);
            CreateHeaderIfNeeded(fieldSettings.Registration.Explanation, RegistrationField.Explanation, headers);

            CreateHeaderIfNeeded(fieldSettings.Analyze.Category, AnalyzeField.Category, headers);
            CreateHeaderIfNeeded(fieldSettings.Analyze.Priority, AnalyzeField.Priority, headers);
            CreateHeaderIfNeeded(fieldSettings.Analyze.Responsible, AnalyzeField.Responsible, headers);
            CreateHeaderIfNeeded(fieldSettings.Analyze.Solution, AnalyzeField.Solution, headers);
            CreateHeaderIfNeeded(fieldSettings.Analyze.Cost, AnalyzeField.Cost, headers);
            CreateHeaderIfNeeded(fieldSettings.Analyze.YearlyCost, AnalyzeField.YearlyCost, headers);
            CreateHeaderIfNeeded(fieldSettings.Analyze.TimeEstimatesHours, AnalyzeField.TimeEstimatesHours, headers);
            CreateHeaderIfNeeded(fieldSettings.Analyze.Risk, AnalyzeField.Risk, headers);
            CreateHeaderIfNeeded(fieldSettings.Analyze.StartDate, AnalyzeField.StartDate, headers);
            CreateHeaderIfNeeded(fieldSettings.Analyze.FinishDate, AnalyzeField.FinishDate, headers);
            CreateHeaderIfNeeded(fieldSettings.Analyze.ImplementationPlan, AnalyzeField.ImplementationPlan, headers);
            CreateHeaderIfNeeded(fieldSettings.Analyze.RecoveryPlan, AnalyzeField.RecoveryPlan, headers);
            CreateHeaderIfNeeded(fieldSettings.Analyze.Recommendation, AnalyzeField.Recommendation, headers);
            CreateHeaderIfNeeded(fieldSettings.Analyze.Approval, AnalyzeField.Approval, headers);

            CreateHeaderIfNeeded(fieldSettings.Implementation.State, ImplementationField.State, headers);
            CreateHeaderIfNeeded(fieldSettings.Implementation.RealStartDate, ImplementationField.RealStartDate, headers);
            CreateHeaderIfNeeded(fieldSettings.Implementation.BuildAndTextImplemented, ImplementationField.BuildAndTextImplemented, headers);
            CreateHeaderIfNeeded(fieldSettings.Implementation.ImplementationPlanUsed, ImplementationField.ImplementationPlanUsed, headers);
            CreateHeaderIfNeeded(fieldSettings.Implementation.Deviation, ImplementationField.Deviation, headers);
            CreateHeaderIfNeeded(fieldSettings.Implementation.RecoveryPlanUsed, ImplementationField.RecoveryPlanUsed, headers);
            CreateHeaderIfNeeded(fieldSettings.Implementation.FinishingDate, ImplementationField.FinishingDate, headers);
            CreateHeaderIfNeeded(fieldSettings.Implementation.ImplementationReady, ImplementationField.ImplementationReady, headers);

            CreateHeaderIfNeeded(fieldSettings.Evaluation.Evaluation, EvaluationField.Evaluation, headers);
            CreateHeaderIfNeeded(fieldSettings.Evaluation.EvaluationReady, EvaluationField.EvaluationReady, headers);

            var overviews = searchResult.Changes.Select(c => CreateChangeOverview(c, fieldSettings)).ToList();
            return new ChangesGridModel(searchResult.ChangesFound, headers, overviews);
        }

        private static void CreateHeaderIfNeeded(FieldOverviewSettingDto fieldSetting, string name, List<GridColumnHeaderModel> headers)
        {
            if (!fieldSetting.Show)
            {
                return;
            }

            var header = new GridColumnHeaderModel(name, fieldSetting.Caption);
            headers.Add(header);
        }

        private static ChangeOverviewModel CreateChangeOverview(ChangeDetailedOverviewDto change, FieldOverviewSettingsDto fieldSettings)
        {
            var fieldValues = new List<GridRowCellValueModel>();

            CreateFieldIfNeeded(fieldSettings.Orderer.Id, OrdererField.Id, change.Orderer.Id, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Orderer.Name, OrdererField.Name, change.Orderer.Name, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Orderer.Phone, OrdererField.Phone, change.Orderer.Phone, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Orderer.CellPhone, OrdererField.CellPhone, change.Orderer.CellPhone, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Orderer.Email, OrdererField.Email, change.Orderer.Email, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Orderer.Department, OrdererField.Department, change.Orderer.Department, fieldValues);

            CreateFieldIfNeeded(fieldSettings.General.Priority, GeneralField.Priority, change.General.Priority, fieldValues);
            CreateFieldIfNeeded(fieldSettings.General.Title, GeneralField.Title, change.General.Title, fieldValues);
            CreateFieldIfNeeded(fieldSettings.General.State, GeneralField.State, change.General.State, fieldValues);
            CreateFieldIfNeeded(fieldSettings.General.System, GeneralField.System, change.General.System, fieldValues);
            CreateFieldIfNeeded(fieldSettings.General.Object, GeneralField.Object, change.General.Object, fieldValues);
            CreateFieldIfNeeded(fieldSettings.General.Inventory, GeneralField.Inventory, change.General.Inventory, fieldValues);
            CreateFieldIfNeeded(fieldSettings.General.Owner, GeneralField.Owner, change.General.Owner, fieldValues);
            CreateFieldIfNeeded(fieldSettings.General.WorkingGroup, GeneralField.WorkingGroup, change.General.WorkingGroup, fieldValues);
            CreateFieldIfNeeded(fieldSettings.General.Administrator, GeneralField.Administrator, change.General.Administrator, fieldValues);
            CreateFieldIfNeeded(fieldSettings.General.FinishingDate, GeneralField.FinishingDate, change.General.FinishingDate, fieldValues);
            CreateFieldIfNeeded(fieldSettings.General.Rss, GeneralField.Rss, change.General.Rss, fieldValues);

            CreateFieldIfNeeded(fieldSettings.Registration.Description, RegistrationField.Description, change.Registration.Description, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Registration.BusinessBenefits, RegistrationField.BusinessBenefits, change.Registration.BusinessBenefits, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Registration.Consequence, RegistrationField.Consequence, change.Registration.Consequence, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Registration.Impact, RegistrationField.Impact, change.Registration.Impact, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Registration.DesiredDate, RegistrationField.DesiredDate, change.Registration.DesiredDate, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Registration.Verified, RegistrationField.Verified, change.Registration.Verified, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Registration.Approval, RegistrationField.Approval, change.Registration.Approval, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Registration.Explanation, RegistrationField.Explanation, change.Registration.Explanation, fieldValues);

            CreateFieldIfNeeded(fieldSettings.Analyze.Category, AnalyzeField.Category, change.Analyze.Category, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Analyze.Priority, AnalyzeField.Priority, change.Analyze.Priority, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Analyze.Responsible, AnalyzeField.Responsible, change.Analyze.Responsible, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Analyze.Solution, AnalyzeField.Solution, change.Analyze.Solution, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Analyze.Cost, AnalyzeField.Cost, change.Analyze.Cost, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Analyze.YearlyCost, AnalyzeField.YearlyCost, change.Analyze.YearlyCost, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Analyze.TimeEstimatesHours, AnalyzeField.TimeEstimatesHours, change.Analyze.TimeEstimatesHours, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Analyze.Risk, AnalyzeField.Risk, change.Analyze.Risk, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Analyze.StartDate, AnalyzeField.StartDate, change.Analyze.StartDate, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Analyze.FinishDate, AnalyzeField.FinishDate, change.Analyze.FinishDate, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Analyze.ImplementationPlan, AnalyzeField.ImplementationPlan, change.Analyze.HasImplementationPlan, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Analyze.RecoveryPlan, AnalyzeField.RecoveryPlan, change.Analyze.HasRecoveryPlan, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Analyze.Recommendation, AnalyzeField.Recommendation, change.Analyze.Recommendation, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Analyze.Approval, AnalyzeField.Approval, change.Analyze.Approval, fieldValues);

            CreateFieldIfNeeded(fieldSettings.Implementation.State, ImplementationField.State, change.Implementation.State, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Implementation.RealStartDate, ImplementationField.RealStartDate, change.Implementation.RealStartDate, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Implementation.BuildAndTextImplemented, ImplementationField.BuildAndTextImplemented, change.Implementation.BuildAndTextImplemented, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Implementation.ImplementationPlanUsed, ImplementationField.ImplementationPlanUsed, change.Implementation.ImplementationPlanUsed, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Implementation.Deviation, ImplementationField.Deviation, change.Implementation.Deviation, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Implementation.RecoveryPlanUsed, ImplementationField.RecoveryPlanUsed, change.Implementation.RecoveryPlanUsed, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Implementation.FinishingDate, ImplementationField.FinishingDate, change.Implementation.FinishingDate, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Implementation.ImplementationReady, ImplementationField.ImplementationReady, change.Implementation.ImplementationReady, fieldValues);

            CreateFieldIfNeeded(fieldSettings.Evaluation.Evaluation, EvaluationField.Evaluation, change.Evaluation.Evaluation, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Evaluation.EvaluationReady, EvaluationField.EvaluationReady, change.Evaluation.EvaluationReady, fieldValues);

            return new ChangeOverviewModel(change.Id, fieldValues);
        }

        private static void CreateFieldIfNeeded(
            FieldOverviewSettingDto fieldSetting, string name, AnalyzeResult value, List<GridRowCellValueModel> fieldValues)
        {
            CreateFieldIfNeeded(fieldSetting, name, value.ToString(), fieldValues);
        }

        private static void CreateFieldIfNeeded(
            FieldOverviewSettingDto fieldSetting, string name, bool value, List<GridRowCellValueModel> fieldValues)
        {
            CreateFieldIfNeeded(fieldSetting, name, value.ToString(), fieldValues);
        }

        private static void CreateFieldIfNeeded(
            FieldOverviewSettingDto fieldSetting, string name, DateTime? value, List<GridRowCellValueModel> fieldValues)
        {
            CreateFieldIfNeeded(fieldSetting, name, value.HasValue ? value.ToString() : null, fieldValues);
        }

        private static void CreateFieldIfNeeded(FieldOverviewSettingDto fieldSetting, string name, int? value, List<GridRowCellValueModel> fieldValues)
        {
            CreateFieldIfNeeded(fieldSetting, name, value.HasValue ? value.ToString() : null, fieldValues);
        }

        private static void CreateFieldIfNeeded(FieldOverviewSettingDto fieldSetting, string name, string value, List<GridRowCellValueModel> fieldValues)
        {
            if (!fieldSetting.Show)
            {
                return;
            }

            var fieldValue = new GridRowCellValueModel(name, value);
            fieldValues.Add(fieldValue);
        }
    }
}