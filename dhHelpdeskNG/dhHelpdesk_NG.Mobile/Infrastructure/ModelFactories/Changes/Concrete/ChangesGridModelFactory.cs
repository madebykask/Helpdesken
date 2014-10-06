namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.BusinessData.Enums.Changes.Fields;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.ChangeDetailedOverview;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeOverview;
    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Common.Types;
    using DH.Helpdesk.Services.DisplayValues;
    using DH.Helpdesk.Services.DisplayValues.Changes;
    using DH.Helpdesk.Services.Response.Changes;
    using DH.Helpdesk.Web.Models.Changes.ChangesGrid;
    using DH.Helpdesk.Web.Models.Shared;

    public sealed class ChangesGridModelFactory : IChangesGridModelFactory
    {
        #region Public Methods and Operators

        public ChangesGridModel Create(SearchResponse response, SortField sortField)
        {
            var headers = new List<GridColumnHeaderModel>();

            CreateOrdererHeaders(response.OverviewSettings.Orderer, headers);
            CreateGeneralHeaders(response.OverviewSettings.General, headers);
            CreateRegistrationHeaders(response.OverviewSettings.Registration, headers);
            CreateAnalyzeHeaders(response.OverviewSettings.Analyze, headers);
            CreateImplementationHeaders(response.OverviewSettings.Implementation, headers);
            CreateEvaluationHeaders(response.OverviewSettings.Evaluation, headers);

            var changeOverviews =
                response.SearchResult.Changes.Select(c => CreateChangeOverview(c, response.OverviewSettings)).ToList();

            return new ChangesGridModel(response.SearchResult.ChangesFound, headers, changeOverviews, sortField);
        }

        #endregion

        #region Methods

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

        private static void CreateAnalyzeValues(
            AnalyzeOverviewSettings settings,
            AnalyzeFields fields,
            List<NewGridRowCellValueModel> values)
        {
            CreateValueIfNeeded(settings.Category, AnalyzeField.Category, fields.Category, values);
            CreateValueIfNeeded(settings.Priority, AnalyzeField.Priority, fields.Priority, values);
            CreateValueIfNeeded(settings.Responsible, AnalyzeField.Responsible, fields.Responsible, values);
            CreateValueIfNeeded(settings.Solution, AnalyzeField.Solution, fields.Solution, values);
            CreateValueIfNeeded(settings.Cost, AnalyzeField.Cost, fields.Cost, values);
            CreateValueIfNeeded(settings.YearlyCost, AnalyzeField.YearlyCost, fields.YearlyCost, values);

            CreateValueIfNeeded(
                settings.EstimatedTimeInHours,
                AnalyzeField.EstimatedTimeInHours,
                fields.EstimatedTimeInHours,
                values);

            CreateValueIfNeeded(settings.Risk, AnalyzeField.Risk, fields.Risk, values);
            CreateValueIfNeeded(settings.StartDate, AnalyzeField.StartDate, fields.StartDate, values);
            CreateValueIfNeeded(settings.FinishDate, AnalyzeField.FinishDate, fields.FinishDate, values);

            CreateValueIfNeeded(
                settings.HasImplementationPlan,
                AnalyzeField.HasImplementationPlan,
                fields.HasImplementationPlan,
                values);

            CreateValueIfNeeded(
                settings.HasRecoveryPlan,
                AnalyzeField.HasRecoveryPlan,
                fields.HasHasRecoveryPlan,
                values);

            CreateValueIfNeeded(
                settings.RejectRecommendation,
                AnalyzeField.RejectExplanation,
                fields.RejectExplanation,
                values);

            CreateValueIfNeeded(settings.Approval, AnalyzeField.Approval, fields.Approval, values);
        }

        private static ChangeOverviewModel CreateChangeOverview(
            ChangeDetailedOverview change,
            ChangeOverviewSettings settings)
        {
            var values = new List<NewGridRowCellValueModel>();

            CreateOrdererValues(settings.Orderer, change.Orderer, values);
            CreateGeneralValues(settings.General, change.General, values);
            CreateRegistrationValues(settings.Registration, change.Registration, values);
            CreateAnalyzeValues(settings.Analyze, change.Analyze, values);
            CreateImplementationValues(settings.Implementation, change.Implementation, values);
            CreateEvaluationValues(settings.Evaluation, change.Evaluation, values);

            return new ChangeOverviewModel(
                change.Id,
                change.Registration.Approval,
                change.Analyze.Approval,
                change.Implementation.ImplementationReady,
                change.Evaluation.EvaluationReady,
                values);
        }

        private static void CreateEvaluationHeaders(
            EvaluationOverviewSettings settings,
            List<GridColumnHeaderModel> headers)
        {
            CreateHeaderIfNeeded(settings.ChangeEvaluation, EvaluationField.ChangeEvaluation, headers);
            CreateHeaderIfNeeded(settings.EvaluationReady, EvaluationField.EvaluationReady, headers);
        }

        private static void CreateEvaluationValues(
            EvaluationOverviewSettings settings,
            EvaluationFields fields,
            List<NewGridRowCellValueModel> values)
        {
            CreateValueIfNeeded(
                settings.ChangeEvaluation,
                EvaluationField.ChangeEvaluation,
                fields.ChangeEvaluation,
                values);

            CreateValueIfNeeded(
                settings.EvaluationReady,
                EvaluationField.EvaluationReady,
                fields.EvaluationReady,
                values);
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

        private static void CreateGeneralValues(
            GeneralOverviewSettings settings,
            GeneralFields fields,
            List<NewGridRowCellValueModel> values)
        {
            CreateValueIfNeeded(settings.Priority, GeneralField.Priority, fields.Priority, values);
            CreateValueIfNeeded(settings.Title, GeneralField.Title, fields.Title, values);
            CreateValueIfNeeded(settings.Status, GeneralField.Status, fields.State, values);
            CreateValueIfNeeded(settings.System, GeneralField.System, fields.System, values);
            CreateValueIfNeeded(settings.Object, GeneralField.Object, fields.Object, values);
            CreateValueIfNeeded(settings.Inventory, GeneralField.Inventory, fields.Inventory, values);
            CreateValueIfNeeded(settings.WorkingGroup, GeneralField.WorkingGroup, fields.WorkingGroup, values);
            CreateValueIfNeeded(settings.Administrator, GeneralField.Administrator, fields.Administrator, values);
            CreateValueIfNeeded(settings.FinishingDate, GeneralField.FinishingDate, fields.FinishingDate, values);
            CreateValueIfNeeded(settings.Rss, GeneralField.Rss, fields.Rss, values);
        }

        private static void CreateHeaderIfNeeded(
            FieldOverviewSetting setting,
            string fieldName,
            List<GridColumnHeaderModel> headers)
        {
            if (!setting.Show)
            {
                return;
            }

            var header = new GridColumnHeaderModel(fieldName, setting.Caption);
            headers.Add(header);
        }

        private static void CreateImplementationHeaders(
            ImplementationOverviewSettings settings,
            List<GridColumnHeaderModel> headers)
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

        private static void CreateImplementationValues(
            ImplementationOverviewSettings settings,
            ImplementationFields fields,
            List<NewGridRowCellValueModel> values)
        {
            CreateValueIfNeeded(settings.Status, ImplementationField.Status, fields.Status, values);
            CreateValueIfNeeded(settings.RealStartDate, ImplementationField.RealStartDate, fields.RealStartDate, values);

            CreateValueIfNeeded(
                settings.BuildImplemented,
                ImplementationField.BuildImplemented,
                fields.BuildImplemented,
                values);

            CreateValueIfNeeded(
                settings.ImplementationPlanUsed,
                ImplementationField.ImplementationPlanUsed,
                fields.ImplementationPlanUsed,
                values);

            CreateValueIfNeeded(settings.Deviation, ImplementationField.Deviation, fields.Deviation, values);

            CreateValueIfNeeded(
                settings.RecoveryPlanUsed,
                ImplementationField.RecoveryPlanUsed,
                fields.RecoveryPlanUsed,
                values);

            CreateValueIfNeeded(settings.FinishingDate, ImplementationField.FinishingDate, fields.FinishingDate, values);

            CreateValueIfNeeded(
                settings.ImplementationReady,
                ImplementationField.ImplementationReady,
                fields.ImplementationReady,
                values);
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

        private static void CreateOrdererValues(
            OrdererOverviewSettings settings,
            OrdererFields fields,
            List<NewGridRowCellValueModel> values)
        {
            CreateValueIfNeeded(settings.Id, OrdererField.Id, fields.Id, values);
            CreateValueIfNeeded(settings.Name, OrdererField.Name, fields.Name, values);
            CreateValueIfNeeded(settings.Phone, OrdererField.Phone, fields.Phone, values);
            CreateValueIfNeeded(settings.CellPhone, OrdererField.CellPhone, fields.CellPhone, values);
            CreateValueIfNeeded(settings.Email, OrdererField.Email, fields.Email, values);
            CreateValueIfNeeded(settings.Department, OrdererField.Department, fields.Department, values);
        }

        private static void CreateRegistrationHeaders(
            RegistrationOverviewSettings settings,
            List<GridColumnHeaderModel> headers)
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

        private static void CreateRegistrationValues(
            RegistrationOverviewSettings settings,
            RegistrationFields fields,
            List<NewGridRowCellValueModel> values)
        {
            CreateValueIfNeeded(settings.Owner, RegistrationField.Owner, fields.Owner, values);
            CreateValueIfNeeded(settings.Description, RegistrationField.Description, fields.Description, values);

            CreateValueIfNeeded(
                settings.BusinessBenefits,
                RegistrationField.BusinessBenefits,
                fields.BusinessBenefits,
                values);

            CreateValueIfNeeded(settings.Consequence, RegistrationField.Consequence, fields.Consequence, values);
            CreateValueIfNeeded(settings.Impact, RegistrationField.Impact, fields.Impact, values);
            CreateValueIfNeeded(settings.DesiredDate, RegistrationField.DesiredDate, fields.DesiredDate, values);
            CreateValueIfNeeded(settings.Verified, RegistrationField.Verified, fields.Verified, values);
            CreateValueIfNeeded(settings.Approval, RegistrationField.Approval, fields.Approval, values);

            CreateValueIfNeeded(
                settings.RejectExplanation,
                RegistrationField.RejectExplanation,
                fields.RejectExplanation,
                values);
        }

        private static void CreateValueIfNeeded(
            FieldOverviewSetting setting,
            string fieldName,
            StepStatus value,
            List<NewGridRowCellValueModel> values)
        {
            var displayValue = new StepStatusDisplayValue(value);
            CreateValueIfNeeded(setting, fieldName, displayValue, values);
        }

        private static void CreateValueIfNeeded(
            FieldOverviewSetting setting,
            string fieldName,
            bool value,
            List<NewGridRowCellValueModel> values)
        {
            var displayValue = new BooleanDisplayValue(value);
            CreateValueIfNeeded(setting, fieldName, displayValue, values);
        }

        private static void CreateValueIfNeeded(
            FieldOverviewSetting setting,
            string fieldName,
            DateTime? value,
            List<NewGridRowCellValueModel> values)
        {
            var displayValue = new DateTimeDisplayValue(value);
            CreateValueIfNeeded(setting, fieldName, displayValue, values);
        }

        private static void CreateValueIfNeeded(
            FieldOverviewSetting setting,
            string fieldName,
            string value,
            List<NewGridRowCellValueModel> values)
        {
            var displayValue = new StringDisplayValue(value);
            CreateValueIfNeeded(setting, fieldName, displayValue, values);
        }

        private static void CreateValueIfNeeded(
            FieldOverviewSetting setting,
            string fieldName,
            int? value,
            List<NewGridRowCellValueModel> values)
        {
            var displayValue = new IntegerDisplayValue(value);
            CreateValueIfNeeded(setting, fieldName, displayValue, values);
        }

        private static void CreateValueIfNeeded(
            FieldOverviewSetting setting,
            string fieldName,
            UserName value,
            List<NewGridRowCellValueModel> values)
        {
            var displayValue = new UserNameDisplayValue(value);
            CreateValueIfNeeded(setting, fieldName, displayValue, values);
        }

        private static void CreateValueIfNeeded(
            FieldOverviewSetting setting,
            string fieldName,
            DisplayValue value,
            List<NewGridRowCellValueModel> values)
        {
            if (!setting.Show)
            {
                return;
            }

            var fieldValue = new NewGridRowCellValueModel(fieldName, value);
            values.Add(fieldValue);
        }

        #endregion
    }
}