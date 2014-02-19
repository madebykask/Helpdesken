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
    using DH.Helpdesk.Web.Enums.Changes;
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
            CreateFieldIfNeeded(settings.Category, AnalyzeField.Category, fields.Category, values);
            CreateFieldIfNeeded(settings.Priority, AnalyzeField.Priority, fields.Priority, values);
            CreateFieldIfNeeded(settings.Responsible, AnalyzeField.Responsible, fields.Responsible, values);
            CreateFieldIfNeeded(settings.Solution, AnalyzeField.Solution, fields.Solution, values);
            CreateFieldIfNeeded(settings.Cost, AnalyzeField.Cost, fields.Cost, values);
            CreateFieldIfNeeded(settings.YearlyCost, AnalyzeField.YearlyCost, fields.YearlyCost, values);

            CreateFieldIfNeeded(
                settings.EstimatedTimeInHours, AnalyzeField.EstimatedTimeInHours, fields.EstimatedTimeInHours, values);

            CreateFieldIfNeeded(settings.Risk, AnalyzeField.Risk, fields.Risk, values);
            CreateFieldIfNeeded(settings.StartDate, AnalyzeField.StartDate, fields.StartDate, values);
            CreateFieldIfNeeded(settings.FinishDate, AnalyzeField.FinishDate, fields.FinishDate, values);

            CreateFieldIfNeeded(
                settings.HasImplementationPlan, AnalyzeField.HasImplementationPlan, fields.HasHasImplementationPlan, values);

            CreateFieldIfNeeded(settings.HasRecoveryPlan, AnalyzeField.HasRecoveryPlan, fields.HasHasRecoveryPlan, values);

            CreateFieldIfNeeded(
                settings.RejectRecommendation, AnalyzeField.RejectExplanation, fields.RejectExplanation, values);

            CreateFieldIfNeeded(settings.Approval, AnalyzeField.Approval, fields.Approval, values);
        }

        private static void CreateAnalyzeHeaders(AnalyzeOverviewSettings settings, List<GridColumnHeaderModel> headers)
        {
            CreateHeaderIfNeeded(settings.Category, AnalyzeField.Category, headers);
            CreateHeaderIfNeeded(settings.Priority, AnalyzeField.Priority, headers);
            CreateHeaderIfNeeded(settings.Responsible, AnalyzeField.Responsible, headers);
            CreateHeaderIfNeeded(settings.Solution, AnalyzeField.Solution, headers);
            CreateHeaderIfNeeded(settings.Cost, AnalyzeField.Cost, headers);
            CreateHeaderIfNeeded(settings.YearlyCost, AnalyzeField.YearlyCost, headers);
            CreateHeaderIfNeeded(settings.EstimatedTimeInHours, AnalyzeField.EstimatedTimeInHours, headers);
            CreateHeaderIfNeeded(settings.Risk, AnalyzeField.Risk, headers);
            CreateHeaderIfNeeded(settings.StartDate, AnalyzeField.StartDate, headers);
            CreateHeaderIfNeeded(settings.FinishDate, AnalyzeField.FinishDate, headers);
            CreateHeaderIfNeeded(settings.HasImplementationPlan, AnalyzeField.HasImplementationPlan, headers);
            CreateHeaderIfNeeded(settings.HasRecoveryPlan, AnalyzeField.HasRecoveryPlan, headers);
            CreateHeaderIfNeeded(settings.Approval, AnalyzeField.Approval, headers);
            CreateHeaderIfNeeded(settings.RejectRecommendation, AnalyzeField.RejectExplanation, headers);
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
                settings.ChangeEvaluation, EvaluationField.ChangeEvaluation, fields.ChangeEvaluation, values);

            CreateFieldIfNeeded(
                settings.EvaluationReady, EvaluationField.EvaluationReady, fields.EvaluationReady, values);
        }

        private static void CreateEvaluationHeaders(
            EvaluationOverviewSettings settings, List<GridColumnHeaderModel> headers)
        {
            CreateHeaderIfNeeded(settings.ChangeEvaluation, EvaluationField.ChangeEvaluation, headers);
            CreateHeaderIfNeeded(settings.EvaluationReady, EvaluationField.EvaluationReady, headers);
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
            CreateFieldIfNeeded(settings.Priority, GeneralField.Priority, fields.Priority, values);
            CreateFieldIfNeeded(settings.Title, GeneralField.Title, fields.Title, values);
            CreateFieldIfNeeded(settings.Status, GeneralField.Status, fields.State, values);
            CreateFieldIfNeeded(settings.System, GeneralField.System, fields.System, values);
            CreateFieldIfNeeded(settings.Object, GeneralField.Object, fields.Object, values);
            CreateFieldIfNeeded(settings.Inventory, GeneralField.Inventory, fields.Inventory, values);
            CreateFieldIfNeeded(settings.WorkingGroup, GeneralField.WorkingGroup, fields.WorkingGroup, values);
            CreateFieldIfNeeded(settings.Administrator, GeneralField.Administrator, fields.Administrator, values);
            CreateFieldIfNeeded(settings.FinishingDate, GeneralField.FinishingDate, fields.FinishingDate, values);
            CreateFieldIfNeeded(settings.Rss, GeneralField.Rss, fields.Rss, values);
        }

        private static void CreateGeneralHeaders(GeneralOverviewSettings settings, List<GridColumnHeaderModel> headers)
        {
            CreateHeaderIfNeeded(settings.Priority, GeneralField.Priority, headers);
            CreateHeaderIfNeeded(settings.Title, GeneralField.Title, headers);
            CreateHeaderIfNeeded(settings.Status, GeneralField.Status, headers);
            CreateHeaderIfNeeded(settings.System, GeneralField.System, headers);
            CreateHeaderIfNeeded(settings.Object, GeneralField.Object, headers);
            CreateHeaderIfNeeded(settings.Inventory, GeneralField.Inventory, headers);
            CreateHeaderIfNeeded(settings.WorkingGroup, GeneralField.WorkingGroup, headers);
            CreateHeaderIfNeeded(settings.Administrator, GeneralField.Administrator, headers);
            CreateHeaderIfNeeded(settings.FinishingDate, GeneralField.FinishingDate, headers);
            CreateHeaderIfNeeded(settings.Rss, GeneralField.Rss, headers);
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
            CreateFieldIfNeeded(settings.Status, ImplementationField.Status, fields.Status, values);
            CreateFieldIfNeeded(settings.RealStartDate, ImplementationField.RealStartDate, fields.RealStartDate, values);

            CreateFieldIfNeeded(
                settings.BuildImplemented, ImplementationField.BuildImplemented, fields.BuildImplemented, values);

            CreateFieldIfNeeded(
                settings.ImplementationPlanUsed,
                ImplementationField.ImplementationPlanUsed,
                fields.ImplementationPlanUsed,
                values);

            CreateFieldIfNeeded(settings.Deviation, ImplementationField.Deviation, fields.Deviation, values);

            CreateFieldIfNeeded(
                settings.RecoveryPlanUsed, ImplementationField.RecoveryPlanUsed, fields.RecoveryPlanUsed, values);

            CreateFieldIfNeeded(settings.FinishingDate, ImplementationField.FinishingDate, fields.FinishingDate, values);

            CreateFieldIfNeeded(
                settings.ImplementationReady,
                ImplementationField.ImplementationReady,
                fields.ImplementationReady,
                values);
        }

        private static void CreateImplementationHeaders(
            ImplementationOverviewSettings settings, List<GridColumnHeaderModel> headers)
        {
            CreateHeaderIfNeeded(settings.Status, ImplementationField.Status, headers);
            CreateHeaderIfNeeded(settings.RealStartDate, ImplementationField.RealStartDate, headers);
            CreateHeaderIfNeeded(settings.BuildImplemented, ImplementationField.BuildImplemented, headers);
            CreateHeaderIfNeeded(settings.ImplementationPlanUsed, ImplementationField.ImplementationPlanUsed, headers);
            CreateHeaderIfNeeded(settings.Deviation, ImplementationField.Deviation, headers);
            CreateHeaderIfNeeded(settings.RecoveryPlanUsed, ImplementationField.RecoveryPlanUsed, headers);
            CreateHeaderIfNeeded(settings.FinishingDate, ImplementationField.FinishingDate, headers);
            CreateHeaderIfNeeded(settings.ImplementationReady, ImplementationField.ImplementationReady, headers);
        }

        private static void CreateOrdererFields(
            OrdererOverviewSettings settings, OrdererFields fields, List<GridRowCellValueModel> values)
        {
            CreateFieldIfNeeded(settings.Id, OrdererField.Id, fields.Id, values);
            CreateFieldIfNeeded(settings.Name, OrdererField.Name, fields.Name, values);
            CreateFieldIfNeeded(settings.Phone, OrdererField.Phone, fields.Phone, values);
            CreateFieldIfNeeded(settings.CellPhone, OrdererField.CellPhone, fields.CellPhone, values);
            CreateFieldIfNeeded(settings.Email, OrdererField.Email, fields.Email, values);
            CreateFieldIfNeeded(settings.Department, OrdererField.Department, fields.Department, values);
        }

        private static void CreateOrdererHeaders(OrdererOverviewSettings settings, List<GridColumnHeaderModel> headers)
        {
            CreateHeaderIfNeeded(settings.Id, OrdererField.Id, headers);
            CreateHeaderIfNeeded(settings.Name, OrdererField.Name, headers);
            CreateHeaderIfNeeded(settings.Phone, OrdererField.Phone, headers);
            CreateHeaderIfNeeded(settings.CellPhone, OrdererField.CellPhone, headers);
            CreateHeaderIfNeeded(settings.Email, OrdererField.Email, headers);
            CreateHeaderIfNeeded(settings.Department, OrdererField.Department, headers);
        }

        private static void CreateRegistrationFields(
            RegistrationOverviewSettings settings, List<GridColumnHeaderModel> headers)
        {
            CreateHeaderIfNeeded(settings.Owner, RegistrationField.Owner, headers);
            CreateHeaderIfNeeded(settings.Description, RegistrationField.Description, headers);
            CreateHeaderIfNeeded(settings.BusinessBenefits, RegistrationField.BusinessBenefits, headers);
            CreateHeaderIfNeeded(settings.Consequence, RegistrationField.Consequence, headers);
            CreateHeaderIfNeeded(settings.Impact, RegistrationField.Impact, headers);
            CreateHeaderIfNeeded(settings.DesiredDate, RegistrationField.DesiredDate, headers);
            CreateHeaderIfNeeded(settings.Verified, RegistrationField.Verified, headers);
            CreateHeaderIfNeeded(settings.Approval, RegistrationField.Approval, headers);
            CreateHeaderIfNeeded(settings.RejectExplanation, RegistrationField.RejectExplanation, headers);
        }

        private static void CreateRegistrationFields(
            RegistrationOverviewSettings settings, RegistrationFields fields, List<GridRowCellValueModel> values)
        {
            CreateFieldIfNeeded(settings.Owner, RegistrationField.Owner, fields.Owner, values);
            CreateFieldIfNeeded(settings.Description, RegistrationField.Description, fields.Description, values);

            CreateFieldIfNeeded(
                settings.BusinessBenefits, RegistrationField.BusinessBenefits, fields.BusinessBenefits, values);

            CreateFieldIfNeeded(settings.Consequence, RegistrationField.Consequence, fields.Consequence, values);
            CreateFieldIfNeeded(settings.Impact, RegistrationField.Impact, fields.Impact, values);
            CreateFieldIfNeeded(settings.DesiredDate, RegistrationField.DesiredDate, fields.DesiredDate, values);
            CreateFieldIfNeeded(settings.Verified, RegistrationField.Verified, fields.Verified, values);
            CreateFieldIfNeeded(settings.Approval, RegistrationField.Approval, fields.Approval, values);

            CreateFieldIfNeeded(
                settings.RejectExplanation, RegistrationField.RejectExplanation, fields.RejectExplanation, values);
        }

        #endregion
    }
}