namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.SettingsEdit;
    using DH.Helpdesk.Web.Models.Changes.Settings;

    public sealed class SettingsModelFactory : ISettingsModelFactory
    {
        #region Public Methods and Operators

        public SettingsModel Create(ChangeFieldSettings settings)
        {
            var ordererSettings = CreateOrdererSettings(settings.Orderer);
            var generalSettings = CreateGeneralSettings(settings.General);
            var registrationSettings = CreateRegistrationSettings(settings.Registration);
            var analyzeSettings = CreateAnalyzeSettings(settings.Analyze);
            var implementationSettings = CreateImplementationSettings(settings.Implementation);
            var evaluationSettings = CreateEvaluationSettings(settings.Evaluation);
            var logSettings = CreateLogSettings(settings.Log);

            return new SettingsModel(
                ordererSettings,
                generalSettings,
                registrationSettings,
                analyzeSettings,
                implementationSettings,
                evaluationSettings,
                logSettings);
        }

        #endregion

        #region Methods

        private static AnalyzeFieldSettingsModel CreateAnalyzeSettings(AnalyzeFieldSettings fieldSettings)
        {
            var category = CreateFieldSettingModel(fieldSettings.Category);
            var priority = CreateFieldSettingModel(fieldSettings.Priority);
            var responsible = CreateFieldSettingModel(fieldSettings.Responsible);
            var solution = CreateStringFieldSettingModel(fieldSettings.Solution);
            var cost = CreateFieldSettingModel(fieldSettings.Cost);
            var yearlyCost = CreateFieldSettingModel(fieldSettings.YearlyCost);
            var estimatedTimeInHours = CreateFieldSettingModel(fieldSettings.EstimatedTimeInHours);
            var risk = CreateStringFieldSettingModel(fieldSettings.Risk);
            var startDate = CreateFieldSettingModel(fieldSettings.StartDate);
            var finishDate = CreateFieldSettingModel(fieldSettings.FinishDate);
            var hasImplementationPlan = CreateFieldSettingModel(fieldSettings.HasImplementationPlan);
            var hasRecoveryPlan = CreateFieldSettingModel(fieldSettings.HasRecoveryPlan);
            var attachedFiles = CreateFieldSettingModel(fieldSettings.AttachedFiles);
            var logs = CreateFieldSettingModel(fieldSettings.Logs);
            var approval = CreateFieldSettingModel(fieldSettings.Approval);
            var rejectExplanation = CreateStringFieldSettingModel(fieldSettings.RejectExplanation);

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

        private static EvaluationFieldSettingsModel CreateEvaluationSettings(EvaluationFieldSettings fieldSettings)
        {
            var changeEvaluation = CreateStringFieldSettingModel(fieldSettings.Evaluation);
            var attachedFiles = CreateFieldSettingModel(fieldSettings.AttachedFiles);
            var logs = CreateFieldSettingModel(fieldSettings.Logs);
            var evaluationReady = CreateFieldSettingModel(fieldSettings.EvaluationReady);

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

        private static GeneralFieldSettingsModel CreateGeneralSettings(GeneralFieldSettings fieldSettings)
        {
            var priority = CreateFieldSettingModel(fieldSettings.Priority);
            var title = CreateFieldSettingModel(fieldSettings.Title);
            var status = CreateFieldSettingModel(fieldSettings.Status);
            var system = CreateFieldSettingModel(fieldSettings.System);
            var @object = CreateFieldSettingModel(fieldSettings.Object);
            var inventory = CreateFieldSettingModel(fieldSettings.Inventory);
            var workingGroup = CreateFieldSettingModel(fieldSettings.WorkingGroup);
            var administrator = CreateFieldSettingModel(fieldSettings.Administrator);
            var finishingDate = CreateFieldSettingModel(fieldSettings.FinishingDate);
            var rss = CreateFieldSettingModel(fieldSettings.Rss);

            return new GeneralFieldSettingsModel(
                priority,
                title,
                status,
                system,
                @object,
                inventory,
                workingGroup,
                administrator,
                finishingDate,
                rss);
        }

        private static ImplementationFieldSettingsModel CreateImplementationSettings(
            ImplementationFieldSettings fieldSettings)
        {
            var status = CreateFieldSettingModel(fieldSettings.Status);
            var realStartDate = CreateFieldSettingModel(fieldSettings.RealStartDate);
            var buildImplemented = CreateFieldSettingModel(fieldSettings.BuildImplemented);
            var implementationPlanUsed = CreateFieldSettingModel(fieldSettings.ImplementationPlanUsed);
            var deviation = CreateStringFieldSettingModel(fieldSettings.Deviation);
            var recoveryPlanUsed = CreateFieldSettingModel(fieldSettings.RecoveryPlanUsed);
            var finishingDate = CreateFieldSettingModel(fieldSettings.FinishingDate);
            var attachedFiles = CreateFieldSettingModel(fieldSettings.AttachedFiles);
            var logs = CreateFieldSettingModel(fieldSettings.Logs);
            var implementationReady = CreateFieldSettingModel(fieldSettings.ImplementationReady);

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

        private static LogFieldSettingsModel CreateLogSettings(LogFieldSettings fieldSettings)
        {
            var logs = CreateFieldSettingModel(fieldSettings.Logs);
            return new LogFieldSettingsModel(logs);
        }

        private static OrdererFieldSettingsModel CreateOrdererSettings(OrdererFieldSettings fieldSettings)
        {
            var id = CreateFieldSettingModel(fieldSettings.Id);
            var name = CreateFieldSettingModel(fieldSettings.Name);
            var phone = CreateFieldSettingModel(fieldSettings.Phone);
            var cellPhone = CreateFieldSettingModel(fieldSettings.CellPhone);
            var email = CreateFieldSettingModel(fieldSettings.Email);
            var department = CreateFieldSettingModel(fieldSettings.Department);

            return new OrdererFieldSettingsModel(id, name, phone, cellPhone, email, department);
        }

        private static RegistrationFieldSettingsModel CreateRegistrationSettings(
            RegistrationFieldSettings fieldSettings)
        {
            var owner = CreateFieldSettingModel(fieldSettings.Owner);
            var affectedProcesses = CreateFieldSettingModel(fieldSettings.AffectedProcesses);
            var affectedDepartments = CreateFieldSettingModel(fieldSettings.AffectedDepartments);
            var description = CreateStringFieldSettingModel(fieldSettings.Description);
            var businessBenefits = CreateStringFieldSettingModel(fieldSettings.BusinessBenefits);
            var consequence = CreateStringFieldSettingModel(fieldSettings.Consequence);
            var impact = CreateFieldSettingModel(fieldSettings.Impact);
            var desiredDate = CreateFieldSettingModel(fieldSettings.DesiredDate);
            var verified = CreateFieldSettingModel(fieldSettings.Verified);
            var attachedFiles = CreateFieldSettingModel(fieldSettings.AttachedFiles);
            var approval = CreateFieldSettingModel(fieldSettings.Approval);
            var rejectExplanation = CreateFieldSettingModel(fieldSettings.RejectExplanation);

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

        private static StringFieldSettingModel CreateStringFieldSettingModel(TextFieldSetting settings)
        {
            return new StringFieldSettingModel(
                settings.ShowInDetails,
                settings.ShowInChanges,
                settings.ShowInSelfService,
                settings.Caption,
                settings.Required,
                settings.DefaultValue,
                settings.Bookmark);
        }

        #endregion
    }
}