namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.BusinessData.Enums.Changes.ApprovalResult;
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.ChangeDetailedOverview;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangesOverview;
    using DH.Helpdesk.Web.Models;
    using DH.Helpdesk.Web.Models.Changes;

    public sealed class ChangesGridModelFactory : IChangesGridModelFactory
    {
        public ChangesGridModel Create(SearchResultDto searchResult, FieldOverviewSettings displaySettings)
        {
            var headers = new List<GridColumnHeaderModel>();

            CreateHeaderIfNeeded(displaySettings.Orderer.Id, Enums.OrdererField.Id, headers);
            CreateHeaderIfNeeded(displaySettings.Orderer.Name, Enums.OrdererField.Name, headers);
            CreateHeaderIfNeeded(displaySettings.Orderer.Phone, Enums.OrdererField.Phone, headers);
            CreateHeaderIfNeeded(displaySettings.Orderer.CellPhone, Enums.OrdererField.CellPhone, headers);
            CreateHeaderIfNeeded(displaySettings.Orderer.Email, Enums.OrdererField.Email, headers);
            CreateHeaderIfNeeded(displaySettings.Orderer.Department, Enums.OrdererField.Department, headers);

            CreateHeaderIfNeeded(displaySettings.General.Priority, Enums.GeneralField.Priority, headers);
            CreateHeaderIfNeeded(displaySettings.General.Title, Enums.GeneralField.Title, headers);
            CreateHeaderIfNeeded(displaySettings.General.State, Enums.GeneralField.State, headers);
            CreateHeaderIfNeeded(displaySettings.General.System, Enums.GeneralField.System, headers);
            CreateHeaderIfNeeded(displaySettings.General.Object, Enums.GeneralField.Object, headers);
            CreateHeaderIfNeeded(displaySettings.General.Inventory, Enums.GeneralField.Inventory, headers);
            CreateHeaderIfNeeded(displaySettings.General.Owner, Enums.GeneralField.Owner, headers);
            CreateHeaderIfNeeded(displaySettings.General.WorkingGroup, Enums.GeneralField.WorkingGroup, headers);
            CreateHeaderIfNeeded(displaySettings.General.Administrator, Enums.GeneralField.Administrator, headers);
            CreateHeaderIfNeeded(displaySettings.General.FinishingDate, Enums.GeneralField.FinishingDate, headers);
            CreateHeaderIfNeeded(displaySettings.General.Rss, Enums.GeneralField.Rss, headers);

            CreateHeaderIfNeeded(displaySettings.Registration.Description, Enums.RegistrationField.Description, headers);
            CreateHeaderIfNeeded(displaySettings.Registration.BusinessBenefits, Enums.RegistrationField.BusinessBenefits, headers);
            CreateHeaderIfNeeded(displaySettings.Registration.Consequence, Enums.RegistrationField.Consequence, headers);
            CreateHeaderIfNeeded(displaySettings.Registration.Impact, Enums.RegistrationField.Impact, headers);
            CreateHeaderIfNeeded(displaySettings.Registration.DesiredDate, Enums.RegistrationField.DesiredDate, headers);
            CreateHeaderIfNeeded(displaySettings.Registration.Verified, Enums.RegistrationField.Verified, headers);
            CreateHeaderIfNeeded(displaySettings.Registration.Approval, Enums.RegistrationField.Approval, headers);
            CreateHeaderIfNeeded(displaySettings.Registration.Explanation, Enums.RegistrationField.Explanation, headers);

            CreateHeaderIfNeeded(displaySettings.Analyze.Category, Enums.AnalyzeField.Category, headers);
            CreateHeaderIfNeeded(displaySettings.Analyze.Priority, Enums.AnalyzeField.Priority, headers);
            CreateHeaderIfNeeded(displaySettings.Analyze.Responsible, Enums.AnalyzeField.Responsible, headers);
            CreateHeaderIfNeeded(displaySettings.Analyze.Solution, Enums.AnalyzeField.Solution, headers);
            CreateHeaderIfNeeded(displaySettings.Analyze.Cost, Enums.AnalyzeField.Cost, headers);
            CreateHeaderIfNeeded(displaySettings.Analyze.YearlyCost, Enums.AnalyzeField.YearlyCost, headers);
            CreateHeaderIfNeeded(displaySettings.Analyze.TimeEstimatesHours, Enums.AnalyzeField.TimeEstimatesHours, headers);
            CreateHeaderIfNeeded(displaySettings.Analyze.Risk, Enums.AnalyzeField.Risk, headers);
            CreateHeaderIfNeeded(displaySettings.Analyze.StartDate, name: Enums.AnalyzeField.StartDate, headers: headers);
            CreateHeaderIfNeeded(displaySettings.Analyze.FinishDate, Enums.AnalyzeField.FinishDate, headers);
            CreateHeaderIfNeeded(displaySettings.Analyze.ImplementationPlan, Enums.AnalyzeField.ImplementationPlan, headers);
            CreateHeaderIfNeeded(displaySettings.Analyze.RecoveryPlan, Enums.AnalyzeField.RecoveryPlan, headers);
            CreateHeaderIfNeeded(displaySettings.Analyze.Recommendation, Enums.AnalyzeField.Recommendation, headers);
            CreateHeaderIfNeeded(displaySettings.Analyze.Approval, Enums.AnalyzeField.Approval, headers);

            CreateHeaderIfNeeded(displaySettings.Implementation.State, Enums.ImplementationField.State, headers);
            CreateHeaderIfNeeded(displaySettings.Implementation.RealStartDate, Enums.ImplementationField.RealStartDate, headers);
            CreateHeaderIfNeeded(displaySettings.Implementation.BuildAndTextImplemented, Enums.ImplementationField.BuildAndTextImplemented, headers);
            CreateHeaderIfNeeded(displaySettings.Implementation.ImplementationPlanUsed, Enums.ImplementationField.ImplementationPlanUsed, headers);
            CreateHeaderIfNeeded(displaySettings.Implementation.Deviation, Enums.ImplementationField.Deviation, headers);
            CreateHeaderIfNeeded(displaySettings.Implementation.RecoveryPlanUsed, Enums.ImplementationField.RecoveryPlanUsed, headers);
            CreateHeaderIfNeeded(displaySettings.Implementation.FinishingDate, Enums.ImplementationField.FinishingDate, headers);
            CreateHeaderIfNeeded(displaySettings.Implementation.ImplementationReady, Enums.ImplementationField.ImplementationReady, headers);

            CreateHeaderIfNeeded(displaySettings.Evaluation.Evaluation, Enums.EvaluationField.Evaluation, headers);
            CreateHeaderIfNeeded(displaySettings.Evaluation.EvaluationReady, Enums.EvaluationField.EvaluationReady, headers);

            var overviews = searchResult.Changes.Select(c => CreateChangeOverview(c, displaySettings)).ToList();
            return new ChangesGridModel(searchResult.ChangesFound, headers, overviews);
        }

        private static void CreateHeaderIfNeeded(FieldOverviewSetting fieldSetting, string name, List<GridColumnHeaderModel> headers)
        {
            if (!fieldSetting.Show)
            {
                return;
            }

            var header = new GridColumnHeaderModel(name, fieldSetting.Caption);
            headers.Add(header);
        }

        private static ChangeOverviewModel CreateChangeOverview(ChangeDetailedOverview change, FieldOverviewSettings fieldSettings)
        {
            var fieldValues = new List<GridRowCellValueModel>();

            CreateFieldIfNeeded(fieldSettings.Orderer.Id, Enums.OrdererField.Id, change.Orderer.Id, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Orderer.Name, Enums.OrdererField.Name, change.Orderer.Name, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Orderer.Phone, Enums.OrdererField.Phone, change.Orderer.Phone, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Orderer.CellPhone, Enums.OrdererField.CellPhone, change.Orderer.CellPhone, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Orderer.Email, Enums.OrdererField.Email, change.Orderer.Email, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Orderer.Department, Enums.OrdererField.Department, change.Orderer.Department, fieldValues);

            CreateFieldIfNeeded(fieldSettings.General.Priority, Enums.GeneralField.Priority, change.General.Priority, fieldValues);
            CreateFieldIfNeeded(fieldSettings.General.Title, Enums.GeneralField.Title, change.General.Title, fieldValues);
            CreateFieldIfNeeded(fieldSettings.General.State, Enums.GeneralField.State, change.General.State, fieldValues);
            CreateFieldIfNeeded(fieldSettings.General.System, Enums.GeneralField.System, change.General.System, fieldValues);
            CreateFieldIfNeeded(fieldSettings.General.Object, Enums.GeneralField.Object, change.General.Object, fieldValues);
            CreateFieldIfNeeded(fieldSettings.General.Inventory, Enums.GeneralField.Inventory, change.General.Inventory, fieldValues);
            CreateFieldIfNeeded(fieldSettings.General.Owner, Enums.GeneralField.Owner, change.General.Owner, fieldValues);
            CreateFieldIfNeeded(fieldSettings.General.WorkingGroup, Enums.GeneralField.WorkingGroup, change.General.WorkingGroup, fieldValues);
            CreateFieldIfNeeded(fieldSettings.General.Administrator, Enums.GeneralField.Administrator, change.General.Administrator, fieldValues);
            CreateFieldIfNeeded(fieldSettings.General.FinishingDate, Enums.GeneralField.FinishingDate, change.General.FinishingDate, fieldValues);
            CreateFieldIfNeeded(fieldSettings.General.Rss, Enums.GeneralField.Rss, change.General.Rss, fieldValues);

            CreateFieldIfNeeded(fieldSettings.Registration.Description, Enums.RegistrationField.Description, change.Registration.Description, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Registration.BusinessBenefits, Enums.RegistrationField.BusinessBenefits, change.Registration.BusinessBenefits, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Registration.Consequence, Enums.RegistrationField.Consequence, change.Registration.Consequence, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Registration.Impact, Enums.RegistrationField.Impact, change.Registration.Impact, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Registration.DesiredDate, Enums.RegistrationField.DesiredDate, change.Registration.DesiredDate, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Registration.Verified, Enums.RegistrationField.Verified, change.Registration.Verified, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Registration.Approval, Enums.RegistrationField.Approval, change.Registration.Approval, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Registration.Explanation, Enums.RegistrationField.Explanation, change.Registration.Explanation, fieldValues);

            CreateFieldIfNeeded(fieldSettings.Analyze.Category, Enums.AnalyzeField.Category, change.Analyze.Category, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Analyze.Priority, Enums.AnalyzeField.Priority, change.Analyze.Priority, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Analyze.Responsible, Enums.AnalyzeField.Responsible, change.Analyze.Responsible, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Analyze.Solution, Enums.AnalyzeField.Solution, change.Analyze.Solution, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Analyze.Cost, Enums.AnalyzeField.Cost, change.Analyze.Cost, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Analyze.YearlyCost, Enums.AnalyzeField.YearlyCost, change.Analyze.YearlyCost, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Analyze.TimeEstimatesHours, Enums.AnalyzeField.TimeEstimatesHours, change.Analyze.TimeEstimatesHours, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Analyze.Risk, Enums.AnalyzeField.Risk, change.Analyze.Risk, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Analyze.StartDate, Enums.AnalyzeField.StartDate, change.Analyze.StartDate, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Analyze.FinishDate, Enums.AnalyzeField.FinishDate, change.Analyze.FinishDate, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Analyze.ImplementationPlan, Enums.AnalyzeField.ImplementationPlan, change.Analyze.HasImplementationPlan, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Analyze.RecoveryPlan, Enums.AnalyzeField.RecoveryPlan, change.Analyze.HasRecoveryPlan, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Analyze.Recommendation, Enums.AnalyzeField.Recommendation, change.Analyze.Recommendation, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Analyze.Approval, Enums.AnalyzeField.Approval, change.Analyze.Approval, fieldValues);

            CreateFieldIfNeeded(fieldSettings.Implementation.State, Enums.ImplementationField.State, change.Implementation.State, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Implementation.RealStartDate, Enums.ImplementationField.RealStartDate, change.Implementation.RealStartDate, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Implementation.BuildAndTextImplemented, Enums.ImplementationField.BuildAndTextImplemented, change.Implementation.BuildAndTextImplemented, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Implementation.ImplementationPlanUsed, Enums.ImplementationField.ImplementationPlanUsed, change.Implementation.ImplementationPlanUsed, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Implementation.Deviation, Enums.ImplementationField.Deviation, change.Implementation.Deviation, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Implementation.RecoveryPlanUsed, Enums.ImplementationField.RecoveryPlanUsed, change.Implementation.RecoveryPlanUsed, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Implementation.FinishingDate, Enums.ImplementationField.FinishingDate, change.Implementation.FinishingDate, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Implementation.ImplementationReady, Enums.ImplementationField.ImplementationReady, change.Implementation.ImplementationReady, fieldValues);

            CreateFieldIfNeeded(fieldSettings.Evaluation.Evaluation, Enums.EvaluationField.Evaluation, change.Evaluation.Evaluation, fieldValues);
            CreateFieldIfNeeded(fieldSettings.Evaluation.EvaluationReady, Enums.EvaluationField.EvaluationReady, change.Evaluation.EvaluationReady, fieldValues);

            return new ChangeOverviewModel(change.Id, fieldValues);
        }

        private static void CreateFieldIfNeeded(
            FieldOverviewSetting fieldSetting, string name, AnalyzeApprovalResult value, List<GridRowCellValueModel> fieldValues)
        {
            CreateFieldIfNeeded(fieldSetting, name, value.ToString(), fieldValues);
        }

        private static void CreateFieldIfNeeded(
            FieldOverviewSetting fieldSetting, string name, bool value, List<GridRowCellValueModel> fieldValues)
        {
            CreateFieldIfNeeded(fieldSetting, name, value.ToString(), fieldValues);
        }

        private static void CreateFieldIfNeeded(
            FieldOverviewSetting fieldSetting, string name, DateTime? value, List<GridRowCellValueModel> fieldValues)
        {
            CreateFieldIfNeeded(fieldSetting, name, value.HasValue ? value.ToString() : null, fieldValues);
        }

        private static void CreateFieldIfNeeded(FieldOverviewSetting fieldSetting, string name, int? value, List<GridRowCellValueModel> fieldValues)
        {
            CreateFieldIfNeeded(fieldSetting, name, value.HasValue ? value.ToString() : null, fieldValues);
        }

        private static void CreateFieldIfNeeded(FieldOverviewSetting fieldSetting, string name, string value, List<GridRowCellValueModel> fieldValues)
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