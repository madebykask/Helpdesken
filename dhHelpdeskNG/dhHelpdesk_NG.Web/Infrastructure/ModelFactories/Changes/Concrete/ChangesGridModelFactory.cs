namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Enums.Changes;
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

            CreateHeaderIfNeeded(displaySettings.Orderer.Id, OrdererField.Id, headers);
            CreateHeaderIfNeeded(displaySettings.Orderer.Name, OrdererField.Name, headers);
            CreateHeaderIfNeeded(displaySettings.Orderer.Phone, OrdererField.Phone, headers);
            CreateHeaderIfNeeded(displaySettings.Orderer.CellPhone, OrdererField.CellPhone, headers);
            CreateHeaderIfNeeded(displaySettings.Orderer.Email, OrdererField.Email, headers);
            CreateHeaderIfNeeded(displaySettings.Orderer.Department, OrdererField.Department, headers);

            CreateHeaderIfNeeded(displaySettings.General.Priority, GeneralField.Priority, headers);
            CreateHeaderIfNeeded(displaySettings.General.Title, GeneralField.Title, headers);
            CreateHeaderIfNeeded(displaySettings.General.State, GeneralField.State, headers);
            CreateHeaderIfNeeded(displaySettings.General.System, GeneralField.System, headers);
            CreateHeaderIfNeeded(displaySettings.General.Object, GeneralField.Object, headers);
            CreateHeaderIfNeeded(displaySettings.General.Inventory, GeneralField.Inventory, headers);
            CreateHeaderIfNeeded(displaySettings.General.Owner, GeneralField.Owner, headers);
            CreateHeaderIfNeeded(displaySettings.General.WorkingGroup, GeneralField.WorkingGroup, headers);
            CreateHeaderIfNeeded(displaySettings.General.Administrator, GeneralField.Administrator, headers);
            CreateHeaderIfNeeded(displaySettings.General.FinishingDate, GeneralField.FinishingDate, headers);
            CreateHeaderIfNeeded(displaySettings.General.Rss, GeneralField.Rss, headers);

            CreateHeaderIfNeeded(displaySettings.Registration.Description, RegistrationField.Description, headers);
            CreateHeaderIfNeeded(displaySettings.Registration.BusinessBenefits, RegistrationField.BusinessBenefits, headers);
            CreateHeaderIfNeeded(displaySettings.Registration.Consequence, RegistrationField.Consequence, headers);
            CreateHeaderIfNeeded(displaySettings.Registration.Impact, RegistrationField.Impact, headers);
            CreateHeaderIfNeeded(displaySettings.Registration.DesiredDate, RegistrationField.DesiredDate, headers);
            CreateHeaderIfNeeded(displaySettings.Registration.Verified, RegistrationField.Verified, headers);
            CreateHeaderIfNeeded(displaySettings.Registration.Approval, RegistrationField.Approval, headers);
            CreateHeaderIfNeeded(displaySettings.Registration.Explanation, RegistrationField.Explanation, headers);

            CreateHeaderIfNeeded(displaySettings.Analyze.Category, AnalyzeField.Category, headers);
            CreateHeaderIfNeeded(displaySettings.Analyze.Priority, AnalyzeField.Priority, headers);
            CreateHeaderIfNeeded(displaySettings.Analyze.Responsible, AnalyzeField.Responsible, headers);
            CreateHeaderIfNeeded(displaySettings.Analyze.Solution, AnalyzeField.Solution, headers);
            CreateHeaderIfNeeded(displaySettings.Analyze.Cost, AnalyzeField.Cost, headers);
            CreateHeaderIfNeeded(displaySettings.Analyze.YearlyCost, AnalyzeField.YearlyCost, headers);
            CreateHeaderIfNeeded(displaySettings.Analyze.TimeEstimatesHours, AnalyzeField.TimeEstimatesHours, headers);
            CreateHeaderIfNeeded(displaySettings.Analyze.Risk, AnalyzeField.Risk, headers);
            CreateHeaderIfNeeded(displaySettings.Analyze.StartDate, AnalyzeField.StartDate, headers);
            CreateHeaderIfNeeded(displaySettings.Analyze.FinishDate, AnalyzeField.FinishDate, headers);
            CreateHeaderIfNeeded(displaySettings.Analyze.ImplementationPlan, AnalyzeField.ImplementationPlan, headers);
            CreateHeaderIfNeeded(displaySettings.Analyze.RecoveryPlan, AnalyzeField.RecoveryPlan, headers);
            CreateHeaderIfNeeded(displaySettings.Analyze.Recommendation, AnalyzeField.Recommendation, headers);
            CreateHeaderIfNeeded(displaySettings.Analyze.Approval, AnalyzeField.Approval, headers);

            CreateHeaderIfNeeded(displaySettings.Implementation.State, ImplementationField.State, headers);
            CreateHeaderIfNeeded(displaySettings.Implementation.RealStartDate, ImplementationField.RealStartDate, headers);
            CreateHeaderIfNeeded(displaySettings.Implementation.BuildAndTextImplemented, ImplementationField.BuildAndTextImplemented, headers);
            CreateHeaderIfNeeded(displaySettings.Implementation.ImplementationPlanUsed, ImplementationField.ImplementationPlanUsed, headers);
            CreateHeaderIfNeeded(displaySettings.Implementation.Deviation, ImplementationField.Deviation, headers);
            CreateHeaderIfNeeded(displaySettings.Implementation.RecoveryPlanUsed, ImplementationField.RecoveryPlanUsed, headers);
            CreateHeaderIfNeeded(displaySettings.Implementation.FinishingDate, ImplementationField.FinishingDate, headers);
            CreateHeaderIfNeeded(displaySettings.Implementation.ImplementationReady, ImplementationField.ImplementationReady, headers);

            CreateHeaderIfNeeded(displaySettings.Evaluation.Evaluation, EvaluationField.Evaluation, headers);
            CreateHeaderIfNeeded(displaySettings.Evaluation.EvaluationReady, EvaluationField.EvaluationReady, headers);

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
            FieldOverviewSetting fieldSetting, string name, AnalyzeResult value, List<GridRowCellValueModel> fieldValues)
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