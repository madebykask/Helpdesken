namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Enums.Changes.ApprovalResult;
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.ChangeDetailedOverview;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeOverview;
    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.Web.Models.Changes;
    using DH.Helpdesk.Web.Models.Common;

    public sealed class ChangesGridModelFactory : IChangesGridModelFactory
    {
        #region Public Methods and Operators

        public ChangesGridModel Create(SearchResult searchResult, ChangeOverviewSettings overviewSettings)
        {
            var headers = new List<GridColumnHeaderModel>();

            CreateHeaderIfNeeded(overviewSettings.Orderer.Id, Enums.OrdererField.Id, headers);
            CreateHeaderIfNeeded(overviewSettings.Orderer.Name, Enums.OrdererField.Name, headers);
            CreateHeaderIfNeeded(overviewSettings.Orderer.Phone, Enums.OrdererField.Phone, headers);
            CreateHeaderIfNeeded(overviewSettings.Orderer.CellPhone, Enums.OrdererField.CellPhone, headers);
            CreateHeaderIfNeeded(overviewSettings.Orderer.Email, Enums.OrdererField.Email, headers);
            CreateHeaderIfNeeded(overviewSettings.Orderer.Department, Enums.OrdererField.Department, headers);

            CreateHeaderIfNeeded(overviewSettings.General.Priority, Enums.GeneralField.Priority, headers);
            CreateHeaderIfNeeded(overviewSettings.General.Title, Enums.GeneralField.Title, headers);
            CreateHeaderIfNeeded(overviewSettings.General.Status, Enums.GeneralField.State, headers);
            CreateHeaderIfNeeded(overviewSettings.General.System, Enums.GeneralField.System, headers);
            CreateHeaderIfNeeded(overviewSettings.General.Object, Enums.GeneralField.Object, headers);
            CreateHeaderIfNeeded(overviewSettings.General.Inventory, Enums.GeneralField.Inventory, headers);
            CreateHeaderIfNeeded(overviewSettings.General.WorkingGroup, Enums.GeneralField.WorkingGroup, headers);
            CreateHeaderIfNeeded(overviewSettings.General.Administrator, Enums.GeneralField.Administrator, headers);
            CreateHeaderIfNeeded(overviewSettings.General.FinishingDate, Enums.GeneralField.FinishingDate, headers);
            CreateHeaderIfNeeded(overviewSettings.General.Rss, Enums.GeneralField.Rss, headers);

            CreateHeaderIfNeeded(overviewSettings.Registration.Owner, Enums.RegistrationField.Owner, headers);
            CreateHeaderIfNeeded(
                overviewSettings.Registration.Description,
                Enums.RegistrationField.Description,
                headers);
            CreateHeaderIfNeeded(
                overviewSettings.Registration.BusinessBenefits,
                Enums.RegistrationField.BusinessBenefits,
                headers);
            CreateHeaderIfNeeded(
                overviewSettings.Registration.Consequence,
                Enums.RegistrationField.Consequence,
                headers);
            CreateHeaderIfNeeded(overviewSettings.Registration.Impact, Enums.RegistrationField.Impact, headers);
            CreateHeaderIfNeeded(
                overviewSettings.Registration.DesiredDate,
                Enums.RegistrationField.DesiredDate,
                headers);
            CreateHeaderIfNeeded(overviewSettings.Registration.Verified, Enums.RegistrationField.Verified, headers);
            CreateHeaderIfNeeded(overviewSettings.Registration.Approval, Enums.RegistrationField.Approval, headers);
            CreateHeaderIfNeeded(
                overviewSettings.Registration.RejectExplanation,
                Enums.RegistrationField.RejectExplanation,
                headers);

            CreateHeaderIfNeeded(overviewSettings.Analyze.Category, Enums.AnalyzeField.Category, headers);
            CreateHeaderIfNeeded(overviewSettings.Analyze.Priority, Enums.AnalyzeField.Priority, headers);
            CreateHeaderIfNeeded(overviewSettings.Analyze.Responsible, Enums.AnalyzeField.Responsible, headers);
            CreateHeaderIfNeeded(overviewSettings.Analyze.Solution, Enums.AnalyzeField.Solution, headers);
            CreateHeaderIfNeeded(overviewSettings.Analyze.Cost, Enums.AnalyzeField.Cost, headers);
            CreateHeaderIfNeeded(overviewSettings.Analyze.YearlyCost, Enums.AnalyzeField.YearlyCost, headers);
            CreateHeaderIfNeeded(
                overviewSettings.Analyze.EstimatedTimeInHours,
                Enums.AnalyzeField.TimeEstimatesHours,
                headers);
            CreateHeaderIfNeeded(overviewSettings.Analyze.Risk, Enums.AnalyzeField.Risk, headers);
            CreateHeaderIfNeeded(overviewSettings.Analyze.StartDate, Enums.AnalyzeField.StartDate, headers);
            CreateHeaderIfNeeded(overviewSettings.Analyze.FinishDate, Enums.AnalyzeField.FinishDate, headers);
            CreateHeaderIfNeeded(
                overviewSettings.Analyze.HasImplementationPlan,
                Enums.AnalyzeField.ImplementationPlan,
                headers);
            CreateHeaderIfNeeded(overviewSettings.Analyze.HasRecoveryPlan, Enums.AnalyzeField.RecoveryPlan, headers);
            CreateHeaderIfNeeded(overviewSettings.Analyze.Approval, Enums.AnalyzeField.Approval, headers);
            CreateHeaderIfNeeded(
                overviewSettings.Analyze.RejectRecommendation,
                Enums.AnalyzeField.Recommendation,
                headers);

            CreateHeaderIfNeeded(overviewSettings.Implementation.Status, Enums.ImplementationField.State, headers);
            CreateHeaderIfNeeded(
                overviewSettings.Implementation.RealStartDate,
                Enums.ImplementationField.RealStartDate,
                headers);
            CreateHeaderIfNeeded(
                overviewSettings.Implementation.BuildImplemented,
                Enums.ImplementationField.BuildAndTextImplemented,
                headers);
            CreateHeaderIfNeeded(
                overviewSettings.Implementation.ImplementationPlanUsed,
                Enums.ImplementationField.ImplementationPlanUsed,
                headers);
            CreateHeaderIfNeeded(
                overviewSettings.Implementation.Deviation,
                Enums.ImplementationField.Deviation,
                headers);
            CreateHeaderIfNeeded(
                overviewSettings.Implementation.RecoveryPlanUsed,
                Enums.ImplementationField.RecoveryPlanUsed,
                headers);
            CreateHeaderIfNeeded(
                overviewSettings.Implementation.FinishingDate,
                Enums.ImplementationField.FinishingDate,
                headers);
            CreateHeaderIfNeeded(
                overviewSettings.Implementation.ImplementationReady,
                Enums.ImplementationField.ImplementationReady,
                headers);

            CreateHeaderIfNeeded(
                overviewSettings.Evaluation.ChangeEvaluation,
                Enums.EvaluationField.Evaluation,
                headers);
            CreateHeaderIfNeeded(
                overviewSettings.Evaluation.EvaluationReady,
                Enums.EvaluationField.EvaluationReady,
                headers);

            var overviews = searchResult.Changes.Select(c => CreateChangeOverview(c, overviewSettings)).ToList();
            return new ChangesGridModel(searchResult.ChangesFound, headers, overviews);
        }

        #endregion

        #region Methods

        private static ChangeOverviewModel CreateChangeOverview(
            ChangeDetailedOverview change,
            ChangeOverviewSettings overviewSettings)
        {
            var fieldValues = new List<GridRowCellValueModel>();

            CreateFieldIfNeeded(overviewSettings.Orderer.Id, Enums.OrdererField.Id, change.Orderer.Id, fieldValues);
            CreateFieldIfNeeded(
                overviewSettings.Orderer.Name,
                Enums.OrdererField.Name,
                change.Orderer.Name,
                fieldValues);
            CreateFieldIfNeeded(
                overviewSettings.Orderer.Phone,
                Enums.OrdererField.Phone,
                change.Orderer.Phone,
                fieldValues);
            CreateFieldIfNeeded(
                overviewSettings.Orderer.CellPhone,
                Enums.OrdererField.CellPhone,
                change.Orderer.CellPhone,
                fieldValues);
            CreateFieldIfNeeded(
                overviewSettings.Orderer.Email,
                Enums.OrdererField.Email,
                change.Orderer.Email,
                fieldValues);
            CreateFieldIfNeeded(
                overviewSettings.Orderer.Department,
                Enums.OrdererField.Department,
                change.Orderer.Department,
                fieldValues);

            CreateFieldIfNeeded(
                overviewSettings.General.Priority,
                Enums.GeneralField.Priority,
                change.General.Priority,
                fieldValues);
            CreateFieldIfNeeded(
                overviewSettings.General.Title,
                Enums.GeneralField.Title,
                change.General.Title,
                fieldValues);
            CreateFieldIfNeeded(
                overviewSettings.General.Status,
                Enums.GeneralField.State,
                change.General.State,
                fieldValues);
            CreateFieldIfNeeded(
                overviewSettings.General.System,
                Enums.GeneralField.System,
                change.General.System,
                fieldValues);
            CreateFieldIfNeeded(
                overviewSettings.General.Object,
                Enums.GeneralField.Object,
                change.General.Object,
                fieldValues);
            CreateFieldIfNeeded(
                overviewSettings.General.Inventory,
                Enums.GeneralField.Inventory,
                change.General.Inventory,
                fieldValues);
            CreateFieldIfNeeded(
                overviewSettings.General.WorkingGroup,
                Enums.GeneralField.WorkingGroup,
                change.General.WorkingGroup,
                fieldValues);
            CreateFieldIfNeeded(
                overviewSettings.General.Administrator,
                Enums.GeneralField.Administrator,
                change.General.Administrator,
                fieldValues);
            CreateFieldIfNeeded(
                overviewSettings.General.FinishingDate,
                Enums.GeneralField.FinishingDate,
                change.General.FinishingDate,
                fieldValues);
            CreateFieldIfNeeded(overviewSettings.General.Rss, Enums.GeneralField.Rss, change.General.Rss, fieldValues);

            CreateFieldIfNeeded(
                overviewSettings.Registration.Owner,
                Enums.RegistrationField.Owner,
                change.Registration.Owner,
                fieldValues);
            CreateFieldIfNeeded(
                overviewSettings.Registration.Description,
                Enums.RegistrationField.Description,
                change.Registration.Description,
                fieldValues);
            CreateFieldIfNeeded(
                overviewSettings.Registration.BusinessBenefits,
                Enums.RegistrationField.BusinessBenefits,
                change.Registration.BusinessBenefits,
                fieldValues);
            CreateFieldIfNeeded(
                overviewSettings.Registration.Consequence,
                Enums.RegistrationField.Consequence,
                change.Registration.Consequence,
                fieldValues);
            CreateFieldIfNeeded(
                overviewSettings.Registration.Impact,
                Enums.RegistrationField.Impact,
                change.Registration.Impact,
                fieldValues);
            CreateFieldIfNeeded(
                overviewSettings.Registration.DesiredDate,
                Enums.RegistrationField.DesiredDate,
                change.Registration.DesiredDate,
                fieldValues);
            CreateFieldIfNeeded(
                overviewSettings.Registration.Verified,
                Enums.RegistrationField.Verified,
                change.Registration.Verified,
                fieldValues);
            CreateFieldIfNeeded(
                overviewSettings.Registration.Approval,
                Enums.RegistrationField.Approval,
                change.Registration.Approval,
                fieldValues);
            CreateFieldIfNeeded(
                overviewSettings.Registration.RejectExplanation,
                Enums.RegistrationField.RejectExplanation,
                change.Registration.RejectExplanation,
                fieldValues);

            CreateFieldIfNeeded(
                overviewSettings.Analyze.Category,
                Enums.AnalyzeField.Category,
                change.Analyze.Category,
                fieldValues);
            CreateFieldIfNeeded(
                overviewSettings.Analyze.Priority,
                Enums.AnalyzeField.Priority,
                change.Analyze.Priority,
                fieldValues);
            CreateFieldIfNeeded(
                overviewSettings.Analyze.Responsible,
                Enums.AnalyzeField.Responsible,
                change.Analyze.Responsible,
                fieldValues);
            CreateFieldIfNeeded(
                overviewSettings.Analyze.Solution,
                Enums.AnalyzeField.Solution,
                change.Analyze.Solution,
                fieldValues);
            CreateFieldIfNeeded(
                overviewSettings.Analyze.Cost,
                Enums.AnalyzeField.Cost,
                change.Analyze.Cost,
                fieldValues);
            CreateFieldIfNeeded(
                overviewSettings.Analyze.YearlyCost,
                Enums.AnalyzeField.YearlyCost,
                change.Analyze.YearlyCost,
                fieldValues);
            CreateFieldIfNeeded(
                overviewSettings.Analyze.EstimatedTimeInHours,
                Enums.AnalyzeField.TimeEstimatesHours,
                change.Analyze.EstimatedTimeInHours,
                fieldValues);
            CreateFieldIfNeeded(
                overviewSettings.Analyze.Risk,
                Enums.AnalyzeField.Risk,
                change.Analyze.Risk,
                fieldValues);
            CreateFieldIfNeeded(
                overviewSettings.Analyze.StartDate,
                Enums.AnalyzeField.StartDate,
                change.Analyze.StartDate,
                fieldValues);
            CreateFieldIfNeeded(
                overviewSettings.Analyze.FinishDate,
                Enums.AnalyzeField.FinishDate,
                change.Analyze.FinishDate,
                fieldValues);
            CreateFieldIfNeeded(
                overviewSettings.Analyze.HasImplementationPlan,
                Enums.AnalyzeField.ImplementationPlan,
                change.Analyze.HasHasImplementationPlan,
                fieldValues);
            CreateFieldIfNeeded(
                overviewSettings.Analyze.HasRecoveryPlan,
                Enums.AnalyzeField.RecoveryPlan,
                change.Analyze.HasHasRecoveryPlan,
                fieldValues);
            CreateFieldIfNeeded(
                overviewSettings.Analyze.RejectRecommendation,
                Enums.AnalyzeField.Recommendation,
                change.Analyze.RejectExplanation,
                fieldValues);
            CreateFieldIfNeeded(
                overviewSettings.Analyze.Approval,
                Enums.AnalyzeField.Approval,
                change.Analyze.Approval,
                fieldValues);

            CreateFieldIfNeeded(
                overviewSettings.Implementation.Status,
                Enums.ImplementationField.State,
                change.Implementation.Status,
                fieldValues);
            CreateFieldIfNeeded(
                overviewSettings.Implementation.RealStartDate,
                Enums.ImplementationField.RealStartDate,
                change.Implementation.RealStartDate,
                fieldValues);
            CreateFieldIfNeeded(
                overviewSettings.Implementation.BuildImplemented,
                Enums.ImplementationField.BuildAndTextImplemented,
                change.Implementation.BuildImplemented,
                fieldValues);
            CreateFieldIfNeeded(
                overviewSettings.Implementation.ImplementationPlanUsed,
                Enums.ImplementationField.ImplementationPlanUsed,
                change.Implementation.ImplementationPlanUsed,
                fieldValues);
            CreateFieldIfNeeded(
                overviewSettings.Implementation.Deviation,
                Enums.ImplementationField.Deviation,
                change.Implementation.Deviation,
                fieldValues);
            CreateFieldIfNeeded(
                overviewSettings.Implementation.RecoveryPlanUsed,
                Enums.ImplementationField.RecoveryPlanUsed,
                change.Implementation.RecoveryPlanUsed,
                fieldValues);
            CreateFieldIfNeeded(
                overviewSettings.Implementation.FinishingDate,
                Enums.ImplementationField.FinishingDate,
                change.Implementation.FinishingDate,
                fieldValues);
            CreateFieldIfNeeded(
                overviewSettings.Implementation.ImplementationReady,
                Enums.ImplementationField.ImplementationReady,
                change.Implementation.ImplementationReady,
                fieldValues);

            CreateFieldIfNeeded(
                overviewSettings.Evaluation.ChangeEvaluation,
                Enums.EvaluationField.Evaluation,
                change.Evaluation.ChangeEvaluation,
                fieldValues);
            CreateFieldIfNeeded(
                overviewSettings.Evaluation.EvaluationReady,
                Enums.EvaluationField.EvaluationReady,
                change.Evaluation.EvaluationReady,
                fieldValues);

            return new ChangeOverviewModel(change.Id, fieldValues);
        }

        private static void CreateFieldIfNeeded(
            FieldOverviewSetting overviewSetting,
            string fieldName,
            RegistrationApprovalResult value,
            List<GridRowCellValueModel> fieldValues)
        {
            CreateFieldIfNeeded(overviewSetting, fieldName, value.ToString(), fieldValues);
        }

        private static void CreateFieldIfNeeded(
            FieldOverviewSetting overviewSetting,
            string fieldName,
            AnalyzeApprovalResult value,
            List<GridRowCellValueModel> fieldValues)
        {
            CreateFieldIfNeeded(overviewSetting, fieldName, value.ToString(), fieldValues);
        }

        private static void CreateFieldIfNeeded(
            FieldOverviewSetting overviewSetting,
            string fieldName,
            bool value,
            List<GridRowCellValueModel> fieldValues)
        {
            CreateFieldIfNeeded(overviewSetting, fieldName, value.ToString(), fieldValues);
        }

        private static void CreateFieldIfNeeded(
            FieldOverviewSetting overviewSetting,
            string fieldName,
            DateTime? value,
            List<GridRowCellValueModel> fieldValues)
        {
            CreateFieldIfNeeded(overviewSetting, fieldName, value.HasValue ? value.ToString() : null, fieldValues);
        }

        private static void CreateFieldIfNeeded(
            FieldOverviewSetting overviewSetting,
            string fieldName,
            int? value,
            List<GridRowCellValueModel> fieldValues)
        {
            CreateFieldIfNeeded(overviewSetting, fieldName, value.HasValue ? value.ToString() : null, fieldValues);
        }

        private static void CreateFieldIfNeeded(
            FieldOverviewSetting overviewSetting,
            string fieldName,
            UserName value,
            List<GridRowCellValueModel> fieldValues)
        {
            CreateFieldIfNeeded(overviewSetting, fieldName, value.FirstName + " " + value.LastName, fieldValues);
        }

        private static void CreateFieldIfNeeded(
            FieldOverviewSetting overviewSetting,
            string fieldName,
            string value,
            List<GridRowCellValueModel> fieldValues)
        {
            if (!overviewSetting.Show)
            {
                return;
            }

            var fieldValue = new GridRowCellValueModel(fieldName, value);
            fieldValues.Add(fieldValue);
        }

        private static void CreateHeaderIfNeeded(
            FieldOverviewSetting overviewSettings,
            string fieldName,
            List<GridColumnHeaderModel> headers)
        {
            if (!overviewSettings.Show)
            {
                return;
            }

            var header = new GridColumnHeaderModel(fieldName, overviewSettings.Caption);
            headers.Add(header);
        }

        #endregion
    }
}