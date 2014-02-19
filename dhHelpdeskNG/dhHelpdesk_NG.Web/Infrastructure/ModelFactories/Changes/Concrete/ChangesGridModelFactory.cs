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

        public ChangesGridModel Create(SearchResult result, ChangeOverviewSettings settings)
        {
            var headers = new List<GridColumnHeaderModel>();

            CreateOrdererHeaders(settings.Orderer, headers);
            CreateGeneralHeaders(settings.General, headers);
            CreateRegistrationFields(settings.Registration, headers);
            CreateAnalyzeHeaders(settings.Analyze, headers);
            CreateImplementationHeaders(settings.Implementation, headers);
            CreateEvaluationHeaders(settings.Evaluation, headers);

            var overviews = result.Changes.Select(c => CreateChangeOverview(c, settings)).ToList();
            return new ChangesGridModel(result.ChangesFound, headers, overviews);
        }

        #endregion

        #region Methods

        private static void CreateAnalyzeFields(
            AnalyzeOverviewSettings settings, AnalyzeFields fields, List<GridRowCellValueModel> values)
        {
            CreateFieldIfNeeded(settings.Category, Enums.AnalyzeField.Category, fields.Category, values);
            CreateFieldIfNeeded(settings.Priority, Enums.AnalyzeField.Priority, fields.Priority, values);
            CreateFieldIfNeeded(settings.Responsible, Enums.AnalyzeField.Responsible, fields.Responsible, values);
            CreateFieldIfNeeded(settings.Solution, Enums.AnalyzeField.Solution, fields.Solution, values);
            CreateFieldIfNeeded(settings.Cost, Enums.AnalyzeField.Cost, fields.Cost, values);
            CreateFieldIfNeeded(settings.YearlyCost, Enums.AnalyzeField.YearlyCost, fields.YearlyCost, values);

            CreateFieldIfNeeded(
                settings.EstimatedTimeInHours,
                Enums.AnalyzeField.TimeEstimatesHours,
                fields.EstimatedTimeInHours,
                values);

            CreateFieldIfNeeded(settings.Risk, Enums.AnalyzeField.Risk, fields.Risk, values);
            CreateFieldIfNeeded(settings.StartDate, Enums.AnalyzeField.StartDate, fields.StartDate, values);
            CreateFieldIfNeeded(settings.FinishDate, Enums.AnalyzeField.FinishDate, fields.FinishDate, values);

            CreateFieldIfNeeded(
                settings.HasImplementationPlan,
                Enums.AnalyzeField.ImplementationPlan,
                fields.HasHasImplementationPlan,
                values);

            CreateFieldIfNeeded(
                settings.HasRecoveryPlan, Enums.AnalyzeField.RecoveryPlan, fields.HasHasRecoveryPlan, values);

            CreateFieldIfNeeded(
                settings.RejectRecommendation, Enums.AnalyzeField.Recommendation, fields.RejectExplanation, values);

            CreateFieldIfNeeded(settings.Approval, Enums.AnalyzeField.Approval, fields.Approval, values);
        }

        private static void CreateAnalyzeHeaders(AnalyzeOverviewSettings settings, List<GridColumnHeaderModel> headers)
        {
            CreateHeaderIfNeeded(settings.Category, Enums.AnalyzeField.Category, headers);
            CreateHeaderIfNeeded(settings.Priority, Enums.AnalyzeField.Priority, headers);
            CreateHeaderIfNeeded(settings.Responsible, Enums.AnalyzeField.Responsible, headers);
            CreateHeaderIfNeeded(settings.Solution, Enums.AnalyzeField.Solution, headers);
            CreateHeaderIfNeeded(settings.Cost, Enums.AnalyzeField.Cost, headers);
            CreateHeaderIfNeeded(settings.YearlyCost, Enums.AnalyzeField.YearlyCost, headers);
            CreateHeaderIfNeeded(settings.EstimatedTimeInHours, Enums.AnalyzeField.TimeEstimatesHours, headers);
            CreateHeaderIfNeeded(settings.Risk, Enums.AnalyzeField.Risk, headers);
            CreateHeaderIfNeeded(settings.StartDate, Enums.AnalyzeField.StartDate, headers);
            CreateHeaderIfNeeded(settings.FinishDate, Enums.AnalyzeField.FinishDate, headers);
            CreateHeaderIfNeeded(settings.HasImplementationPlan, Enums.AnalyzeField.ImplementationPlan, headers);
            CreateHeaderIfNeeded(settings.HasRecoveryPlan, Enums.AnalyzeField.RecoveryPlan, headers);
            CreateHeaderIfNeeded(settings.Approval, Enums.AnalyzeField.Approval, headers);
            CreateHeaderIfNeeded(settings.RejectRecommendation, Enums.AnalyzeField.Recommendation, headers);
        }

        private static ChangeOverviewModel CreateChangeOverview(
            ChangeDetailedOverview change, ChangeOverviewSettings settings)
        {
            var values = new List<GridRowCellValueModel>();

            CreateOrdererFields(settings.Orderer, change.Orderer, values);
            CreateGeneralFields(settings.General, change.General, values);
            CreateRegistrationFields(settings.Registration, change.Registration, values);
            CreateAnalyzeFields(settings.Analyze, change.Analyze, values);
            CreateImplementationFields(settings.Implementation, change.Implementation, values);
            CreateEvaluationFields(settings.Evaluation, change.Evaluation, values);

            return new ChangeOverviewModel(change.Id, values);
        }

        private static void CreateEvaluationFields(
            EvaluationOverviewSettings settings, EvaluationFields fields, List<GridRowCellValueModel> values)
        {
            CreateFieldIfNeeded(
                settings.ChangeEvaluation, Enums.EvaluationField.Evaluation, fields.ChangeEvaluation, values);

            CreateFieldIfNeeded(
                settings.EvaluationReady, Enums.EvaluationField.EvaluationReady, fields.EvaluationReady, values);
        }

        private static void CreateEvaluationHeaders(
            EvaluationOverviewSettings settings, List<GridColumnHeaderModel> headers)
        {
            CreateHeaderIfNeeded(settings.ChangeEvaluation, Enums.EvaluationField.Evaluation, headers);
            CreateHeaderIfNeeded(settings.EvaluationReady, Enums.EvaluationField.EvaluationReady, headers);
        }

        private static void CreateFieldIfNeeded(
            FieldOverviewSetting setting,
            string fieldName,
            RegistrationApprovalResult value,
            List<GridRowCellValueModel> values)
        {
            CreateFieldIfNeeded(setting, fieldName, value.ToString(), values);
        }

        private static void CreateFieldIfNeeded(
            FieldOverviewSetting setting,
            string fieldName,
            AnalyzeApprovalResult value,
            List<GridRowCellValueModel> values)
        {
            CreateFieldIfNeeded(setting, fieldName, value.ToString(), values);
        }

        private static void CreateFieldIfNeeded(
            FieldOverviewSetting setting, string fieldName, bool value, List<GridRowCellValueModel> values)
        {
            CreateFieldIfNeeded(setting, fieldName, value.ToString(), values);
        }

        private static void CreateFieldIfNeeded(
            FieldOverviewSetting setting, string fieldName, DateTime? value, List<GridRowCellValueModel> values)
        {
            CreateFieldIfNeeded(setting, fieldName, value.HasValue ? value.ToString() : null, values);
        }

        private static void CreateFieldIfNeeded(
            FieldOverviewSetting setting, string fieldName, int? value, List<GridRowCellValueModel> values)
        {
            CreateFieldIfNeeded(setting, fieldName, value.HasValue ? value.ToString() : null, values);
        }

        private static void CreateFieldIfNeeded(
            FieldOverviewSetting setting, string fieldName, UserName value, List<GridRowCellValueModel> values)
        {
            CreateFieldIfNeeded(setting, fieldName, value.FirstName + " " + value.LastName, values);
        }

        private static void CreateFieldIfNeeded(
            FieldOverviewSetting setting, string fieldName, string value, List<GridRowCellValueModel> values)
        {
            if (!setting.Show)
            {
                return;
            }

            var fieldValue = new GridRowCellValueModel(fieldName, value);
            values.Add(fieldValue);
        }

        private static void CreateGeneralFields(
            GeneralOverviewSettings settings, GeneralFields fields, List<GridRowCellValueModel> values)
        {
            CreateFieldIfNeeded(settings.Priority, Enums.GeneralField.Priority, fields.Priority, values);
            CreateFieldIfNeeded(settings.Title, Enums.GeneralField.Title, fields.Title, values);
            CreateFieldIfNeeded(settings.Status, Enums.GeneralField.State, fields.State, values);
            CreateFieldIfNeeded(settings.System, Enums.GeneralField.System, fields.System, values);
            CreateFieldIfNeeded(settings.Object, Enums.GeneralField.Object, fields.Object, values);
            CreateFieldIfNeeded(settings.Inventory, Enums.GeneralField.Inventory, fields.Inventory, values);
            CreateFieldIfNeeded(settings.WorkingGroup, Enums.GeneralField.WorkingGroup, fields.WorkingGroup, values);
            CreateFieldIfNeeded(settings.Administrator, Enums.GeneralField.Administrator, fields.Administrator, values);
            CreateFieldIfNeeded(settings.FinishingDate, Enums.GeneralField.FinishingDate, fields.FinishingDate, values);
            CreateFieldIfNeeded(settings.Rss, Enums.GeneralField.Rss, fields.Rss, values);
        }

        private static void CreateGeneralHeaders(GeneralOverviewSettings settings, List<GridColumnHeaderModel> headers)
        {
            CreateHeaderIfNeeded(settings.Priority, Enums.GeneralField.Priority, headers);
            CreateHeaderIfNeeded(settings.Title, Enums.GeneralField.Title, headers);
            CreateHeaderIfNeeded(settings.Status, Enums.GeneralField.State, headers);
            CreateHeaderIfNeeded(settings.System, Enums.GeneralField.System, headers);
            CreateHeaderIfNeeded(settings.Object, Enums.GeneralField.Object, headers);
            CreateHeaderIfNeeded(settings.Inventory, Enums.GeneralField.Inventory, headers);
            CreateHeaderIfNeeded(settings.WorkingGroup, Enums.GeneralField.WorkingGroup, headers);
            CreateHeaderIfNeeded(settings.Administrator, Enums.GeneralField.Administrator, headers);
            CreateHeaderIfNeeded(settings.FinishingDate, Enums.GeneralField.FinishingDate, headers);
            CreateHeaderIfNeeded(settings.Rss, Enums.GeneralField.Rss, headers);
        }

        private static void CreateHeaderIfNeeded(
            FieldOverviewSetting overviewSettings, string fieldName, List<GridColumnHeaderModel> headers)
        {
            if (!overviewSettings.Show)
            {
                return;
            }

            var header = new GridColumnHeaderModel(fieldName, overviewSettings.Caption);
            headers.Add(header);
        }

        private static void CreateImplementationFields(
            ImplementationOverviewSettings settings, ImplementationFields fields, List<GridRowCellValueModel> values)
        {
            CreateFieldIfNeeded(settings.Status, Enums.ImplementationField.State, fields.Status, values);

            CreateFieldIfNeeded(
                settings.RealStartDate, Enums.ImplementationField.RealStartDate, fields.RealStartDate, values);

            CreateFieldIfNeeded(
                settings.BuildImplemented, Enums.ImplementationField.BuildImplemented, fields.BuildImplemented, values);

            CreateFieldIfNeeded(
                settings.ImplementationPlanUsed,
                Enums.ImplementationField.ImplementationPlanUsed,
                fields.ImplementationPlanUsed,
                values);

            CreateFieldIfNeeded(settings.Deviation, Enums.ImplementationField.Deviation, fields.Deviation, values);

            CreateFieldIfNeeded(
                settings.RecoveryPlanUsed, Enums.ImplementationField.RecoveryPlanUsed, fields.RecoveryPlanUsed, values);

            CreateFieldIfNeeded(
                settings.FinishingDate, Enums.ImplementationField.FinishingDate, fields.FinishingDate, values);

            CreateFieldIfNeeded(
                settings.ImplementationReady,
                Enums.ImplementationField.ImplementationReady,
                fields.ImplementationReady,
                values);
        }

        private static void CreateImplementationHeaders(
            ImplementationOverviewSettings settings, List<GridColumnHeaderModel> headers)
        {
            CreateHeaderIfNeeded(settings.Status, Enums.ImplementationField.State, headers);
            CreateHeaderIfNeeded(settings.RealStartDate, Enums.ImplementationField.RealStartDate, headers);
            CreateHeaderIfNeeded(settings.BuildImplemented, Enums.ImplementationField.BuildImplemented, headers);

            CreateHeaderIfNeeded(
                settings.ImplementationPlanUsed, Enums.ImplementationField.ImplementationPlanUsed, headers);

            CreateHeaderIfNeeded(settings.Deviation, Enums.ImplementationField.Deviation, headers);
            CreateHeaderIfNeeded(settings.RecoveryPlanUsed, Enums.ImplementationField.RecoveryPlanUsed, headers);
            CreateHeaderIfNeeded(settings.FinishingDate, Enums.ImplementationField.FinishingDate, headers);
            CreateHeaderIfNeeded(settings.ImplementationReady, Enums.ImplementationField.ImplementationReady, headers);
        }

        private static void CreateOrdererFields(
            OrdererOverviewSettings settings, OrdererFields fields, List<GridRowCellValueModel> values)
        {
            CreateFieldIfNeeded(settings.Id, Enums.OrdererField.Id, fields.Id, values);
            CreateFieldIfNeeded(settings.Name, Enums.OrdererField.Name, fields.Name, values);
            CreateFieldIfNeeded(settings.Phone, Enums.OrdererField.Phone, fields.Phone, values);
            CreateFieldIfNeeded(settings.CellPhone, Enums.OrdererField.CellPhone, fields.CellPhone, values);
            CreateFieldIfNeeded(settings.Email, Enums.OrdererField.Email, fields.Email, values);
            CreateFieldIfNeeded(settings.Department, Enums.OrdererField.Department, fields.Department, values);
        }

        private static void CreateOrdererHeaders(OrdererOverviewSettings settings, List<GridColumnHeaderModel> headers)
        {
            CreateHeaderIfNeeded(settings.Id, Enums.OrdererField.Id, headers);
            CreateHeaderIfNeeded(settings.Name, Enums.OrdererField.Name, headers);
            CreateHeaderIfNeeded(settings.Phone, Enums.OrdererField.Phone, headers);
            CreateHeaderIfNeeded(settings.CellPhone, Enums.OrdererField.CellPhone, headers);
            CreateHeaderIfNeeded(settings.Email, Enums.OrdererField.Email, headers);
            CreateHeaderIfNeeded(settings.Department, Enums.OrdererField.Department, headers);
        }

        private static void CreateRegistrationFields(
            RegistrationOverviewSettings settings, List<GridColumnHeaderModel> headers)
        {
            CreateHeaderIfNeeded(settings.Owner, Enums.RegistrationField.Owner, headers);
            CreateHeaderIfNeeded(settings.Description, Enums.RegistrationField.Description, headers);
            CreateHeaderIfNeeded(settings.BusinessBenefits, Enums.RegistrationField.BusinessBenefits, headers);
            CreateHeaderIfNeeded(settings.Consequence, Enums.RegistrationField.Consequence, headers);
            CreateHeaderIfNeeded(settings.Impact, Enums.RegistrationField.Impact, headers);
            CreateHeaderIfNeeded(settings.DesiredDate, Enums.RegistrationField.DesiredDate, headers);
            CreateHeaderIfNeeded(settings.Verified, Enums.RegistrationField.Verified, headers);
            CreateHeaderIfNeeded(settings.Approval, Enums.RegistrationField.Approval, headers);
            CreateHeaderIfNeeded(settings.RejectExplanation, Enums.RegistrationField.RejectExplanation, headers);
        }

        private static void CreateRegistrationFields(
            RegistrationOverviewSettings settings, RegistrationFields fields, List<GridRowCellValueModel> values)
        {
            CreateFieldIfNeeded(settings.Owner, Enums.RegistrationField.Owner, fields.Owner, values);
            CreateFieldIfNeeded(settings.Description, Enums.RegistrationField.Description, fields.Description, values);

            CreateFieldIfNeeded(
                settings.BusinessBenefits, Enums.RegistrationField.BusinessBenefits, fields.BusinessBenefits, values);

            CreateFieldIfNeeded(settings.Consequence, Enums.RegistrationField.Consequence, fields.Consequence, values);
            CreateFieldIfNeeded(settings.Impact, Enums.RegistrationField.Impact, fields.Impact, values);
            CreateFieldIfNeeded(settings.DesiredDate, Enums.RegistrationField.DesiredDate, fields.DesiredDate, values);
            CreateFieldIfNeeded(settings.Verified, Enums.RegistrationField.Verified, fields.Verified, values);
            CreateFieldIfNeeded(settings.Approval, Enums.RegistrationField.Approval, fields.Approval, values);

            CreateFieldIfNeeded(
                settings.RejectExplanation, Enums.RegistrationField.RejectExplanation, fields.RejectExplanation, values);
        }

        #endregion
    }
}