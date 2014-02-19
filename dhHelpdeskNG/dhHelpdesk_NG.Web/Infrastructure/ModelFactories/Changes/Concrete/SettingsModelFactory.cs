namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.SettingsEdit;
    using DH.Helpdesk.Web.Models.Changes.Settings;

    public sealed class SettingsModelFactory : ISettingsModelFactory
    {
        #region Public Methods and Operators

        public SettingsModel Create(ChangeFieldSettings settings)
        {
            var orderer = CreateOrdererSettings(settings.Orderer);
            var general = CreateGeneralSettings(settings.General);
            var registration = CreateRegistrationSettings(settings.Registration);
            var analyze = CreateAnalyzeSettings(settings.Analyze);
            var implementation = CreateImplementationSettings(settings.Implementation);
            var evaluation = CreateEvaluationSettings(settings.Evaluation);
            var log = CreateLogSettings(settings.Log);

            return new SettingsModel(orderer, general, registration, analyze, implementation, evaluation, log);
        }

        #endregion

        #region Methods

        private static AnalyzeFieldSettingsModel CreateAnalyzeSettings(AnalyzeFieldSettings settings)
        {
            var category = CreateFieldSettingModel(settings.Category);
            var priority = CreateFieldSettingModel(settings.Priority);
            var responsible = CreateFieldSettingModel(settings.Responsible);
            var solution = CreateTextFieldSettingModel(settings.Solution);
            var cost = CreateFieldSettingModel(settings.Cost);
            var yearlyCost = CreateFieldSettingModel(settings.YearlyCost);
            var estimatedTimeInHours = CreateFieldSettingModel(settings.EstimatedTimeInHours);
            var risk = CreateTextFieldSettingModel(settings.Risk);
            var startDate = CreateFieldSettingModel(settings.StartDate);
            var finishDate = CreateFieldSettingModel(settings.FinishDate);
            var hasImplementationPlan = CreateFieldSettingModel(settings.HasImplementationPlan);
            var hasRecoveryPlan = CreateFieldSettingModel(settings.HasRecoveryPlan);
            var attachedFiles = CreateFieldSettingModel(settings.AttachedFiles);
            var logs = CreateFieldSettingModel(settings.Logs);
            var approval = CreateFieldSettingModel(settings.Approval);
            var rejectExplanation = CreateTextFieldSettingModel(settings.RejectExplanation);

            return new AnalyzeFieldSettingsModel(
                category,
                priority,
                responsible,
                solution,
                cost,
                yearlyCost,
                estimatedTimeInHours,
                risk,
                startDate,
                finishDate,
                hasImplementationPlan,
                hasRecoveryPlan,
                attachedFiles,
                logs,
                approval,
                rejectExplanation);
        }

        private static EvaluationFieldSettingsModel CreateEvaluationSettings(EvaluationFieldSettings settings)
        {
            var changeEvaluation = CreateTextFieldSettingModel(settings.Evaluation);
            var attachedFiles = CreateFieldSettingModel(settings.AttachedFiles);
            var logs = CreateFieldSettingModel(settings.Logs);
            var evaluationReady = CreateFieldSettingModel(settings.EvaluationReady);

            return new EvaluationFieldSettingsModel(changeEvaluation, attachedFiles, logs, evaluationReady);
        }

        private static FieldSettingModel CreateFieldSettingModel(FieldSetting settings)
        {
            return new FieldSettingModel(
                settings.ShowInDetails,
                settings.ShowInChanges,
                settings.ShowInSelfService,
                settings.Caption,
                settings.Required,
                settings.Bookmark);
        }

        private static GeneralFieldSettingsModel CreateGeneralSettings(GeneralFieldSettings settings)
        {
            var priority = CreateFieldSettingModel(settings.Priority);
            var title = CreateFieldSettingModel(settings.Title);
            var status = CreateFieldSettingModel(settings.Status);
            var system = CreateFieldSettingModel(settings.System);
            var @object = CreateFieldSettingModel(settings.Object);
            var inventory = CreateFieldSettingModel(settings.Inventory);
            var workingGroup = CreateFieldSettingModel(settings.WorkingGroup);
            var administrator = CreateFieldSettingModel(settings.Administrator);
            var finishingDate = CreateFieldSettingModel(settings.FinishingDate);
            var rss = CreateFieldSettingModel(settings.Rss);

            return new GeneralFieldSettingsModel(
                priority, title, status, system, @object, inventory, workingGroup, administrator, finishingDate, rss);
        }

        private static ImplementationFieldSettingsModel CreateImplementationSettings(
            ImplementationFieldSettings settings)
        {
            var status = CreateFieldSettingModel(settings.Status);
            var realStartDate = CreateFieldSettingModel(settings.RealStartDate);
            var buildImplemented = CreateFieldSettingModel(settings.BuildImplemented);
            var implementationPlanUsed = CreateFieldSettingModel(settings.ImplementationPlanUsed);
            var deviation = CreateTextFieldSettingModel(settings.Deviation);
            var recoveryPlanUsed = CreateFieldSettingModel(settings.RecoveryPlanUsed);
            var finishingDate = CreateFieldSettingModel(settings.FinishingDate);
            var attachedFiles = CreateFieldSettingModel(settings.AttachedFiles);
            var logs = CreateFieldSettingModel(settings.Logs);
            var implementationReady = CreateFieldSettingModel(settings.ImplementationReady);

            return new ImplementationFieldSettingsModel(
                status,
                realStartDate,
                buildImplemented,
                implementationPlanUsed,
                deviation,
                recoveryPlanUsed,
                finishingDate,
                attachedFiles,
                logs,
                implementationReady);
        }

        private static LogFieldSettingsModel CreateLogSettings(LogFieldSettings settings)
        {
            var logs = CreateFieldSettingModel(settings.Logs);
            return new LogFieldSettingsModel(logs);
        }

        private static OrdererFieldSettingsModel CreateOrdererSettings(OrdererFieldSettings settings)
        {
            var id = CreateFieldSettingModel(settings.Id);
            var name = CreateFieldSettingModel(settings.Name);
            var phone = CreateFieldSettingModel(settings.Phone);
            var cellPhone = CreateFieldSettingModel(settings.CellPhone);
            var email = CreateFieldSettingModel(settings.Email);
            var department = CreateFieldSettingModel(settings.Department);

            return new OrdererFieldSettingsModel(id, name, phone, cellPhone, email, department);
        }

        private static RegistrationFieldSettingsModel CreateRegistrationSettings(RegistrationFieldSettings settings)
        {
            var owner = CreateFieldSettingModel(settings.Owner);
            var affectedProcesses = CreateFieldSettingModel(settings.AffectedProcesses);
            var affectedDepartments = CreateFieldSettingModel(settings.AffectedDepartments);
            var description = CreateTextFieldSettingModel(settings.Description);
            var businessBenefits = CreateTextFieldSettingModel(settings.BusinessBenefits);
            var consequence = CreateTextFieldSettingModel(settings.Consequence);
            var impact = CreateFieldSettingModel(settings.Impact);
            var desiredDate = CreateFieldSettingModel(settings.DesiredDate);
            var verified = CreateFieldSettingModel(settings.Verified);
            var attachedFiles = CreateFieldSettingModel(settings.AttachedFiles);
            var approval = CreateFieldSettingModel(settings.Approval);
            var rejectExplanation = CreateFieldSettingModel(settings.RejectExplanation);

            return new RegistrationFieldSettingsModel(
                owner,
                affectedProcesses,
                affectedDepartments,
                description,
                businessBenefits,
                consequence,
                impact,
                desiredDate,
                verified,
                attachedFiles,
                approval,
                rejectExplanation);
        }

        private static StringFieldSettingModel CreateTextFieldSettingModel(TextFieldSetting setting)
        {
            return new StringFieldSettingModel(
                setting.ShowInDetails,
                setting.ShowInChanges,
                setting.ShowInSelfService,
                setting.Caption,
                setting.Required,
                setting.DefaultValue,
                setting.Bookmark);
        }

        #endregion
    }
}