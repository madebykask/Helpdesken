namespace DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Changes.Concrete
{
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Changes.Settings.SettingsEdit;
    using DH.Helpdesk.Web.Models.Changes.SettingsEdit;

    public sealed class UpdatedSettingsFactory : IUpdatedSettingsFactory
    {
        #region Public Methods and Operators

        public ChangeFieldSettings Create(SettingsModel settings, OperationContext context)
        {
            var orderer = CreateOrdererSettings(settings.Orderer);
            var general = CreateGeneralSettings(settings.General);
            var registration = CreateRegistrationSettings(settings.Registration);
            var analyze = CreateAnalyzeSettings(settings.Analyze);
            var implementation = CreateImplementationSettings(settings.Implementation);
            var evaluation = CreateEvaluationSettings(settings.Evaluation);
            var log = CreateLogSettings(settings.Log);
            
            return ChangeFieldSettings.CreateUpdated(
                context.CustomerId,
                settings.LanguageId,
                orderer,
                general,
                registration,
                analyze,
                implementation,
                evaluation,
                log,
                context.DateAndTime);
        }

        #endregion

        #region Methods

        private static AnalyzeFieldSettings CreateAnalyzeSettings(AnalyzeSettingsModel settings)
        {
            var category = CreateFieldSetting(settings.Category);
            var priority = CreateFieldSetting(settings.Priority);
            var responsible = CreateFieldSetting(settings.Responsible);
            var solution = CreateFieldSetting(settings.Solution);
            var cost = CreateFieldSetting(settings.Cost);
            var yearlyCost = CreateFieldSetting(settings.YearlyCost);
            var estimatedTimeInHours = CreateFieldSetting(settings.EstimatedTimeInHours);
            var risk = CreateFieldSetting(settings.Risk);
            var startDate = CreateFieldSetting(settings.StartDate);
            var finishDate = CreateFieldSetting(settings.FinishDate);
            var hasImplementationPlan = CreateFieldSetting(settings.HasImplementationPlan);
            var hasRecoveryPlan = CreateFieldSetting(settings.HasRecoveryPlan);
            var attachedFiles = CreateFieldSetting(settings.AttachedFiles);
            var logs = CreateFieldSetting(settings.Logs);
            var approval = CreateFieldSetting(settings.Approval);
            var rejectExplanation = CreateFieldSetting(settings.RejectExplanation);

            return new AnalyzeFieldSettings(
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
                rejectExplanation,
                attachedFiles,
                logs,
                approval);
        }

        private static EvaluationFieldSettings CreateEvaluationSettings(EvaluationSettingsModel settings)
        {
            var evaluation = CreateFieldSetting(settings.ChangeEvaluation);
            var attachedFiles = CreateFieldSetting(settings.AttachedFiles);
            var logs = CreateFieldSetting(settings.Logs);
            var evaluationReady = CreateFieldSetting(settings.EvaluationReady);

            return new EvaluationFieldSettings(evaluation, attachedFiles, logs, evaluationReady);
        }

        private static FieldSetting CreateFieldSetting(FieldSettingModel setting)
        {
            return FieldSetting.CreateUpdated(
                setting.ShowInDetails,
                setting.ShowInChanges,
                setting.ShowInSelfService,
                setting.Caption,
                setting.Required,
                setting.Bookmark);
        }

        private static TextFieldSetting CreateFieldSetting(TextFieldSettingModel setting)
        {
            return TextFieldSetting.CreateUpdated(
                setting.ShowInDetails,
                setting.ShowInChanges,
                setting.ShowInSelfService,
                setting.Caption,
                setting.Required,
                setting.DefaultValue,
                setting.Bookmark);
        }

        private static GeneralFieldSettings CreateGeneralSettings(GeneralSettingsModel settings)
        {
            var priority = CreateFieldSetting(settings.Prioritisation);
            var title = CreateFieldSetting(settings.Title);
            var status = CreateFieldSetting(settings.Status);
            var system = CreateFieldSetting(settings.System);
            var @object = CreateFieldSetting(settings.Object);
            var inventory = CreateFieldSetting(settings.Inventory);
            var workingGroup = CreateFieldSetting(settings.WorkingGroup);
            var administrator = CreateFieldSetting(settings.Administrator);
            var finishingDate = CreateFieldSetting(settings.FinishingDate);
            var rss = CreateFieldSetting(settings.Rss);

            return new GeneralFieldSettings(
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

        private static ImplementationFieldSettings CreateImplementationSettings(
            ImplementationSettingsModel settings)
        {
            var status = CreateFieldSetting(settings.ImplementationStatus);
            var realStartDate = CreateFieldSetting(settings.RealStartDate);
            var buildImplemented = CreateFieldSetting(settings.BuildImplemented);
            var implementationPlanUsed = CreateFieldSetting(settings.ImplementationPlanUsed);
            var deviation = CreateFieldSetting(settings.Deviation);
            var recoveryPlanUsed = CreateFieldSetting(settings.RecoveryPlanUsed);
            var finisingDate = CreateFieldSetting(settings.FinishingDate);
            var attachedFiles = CreateFieldSetting(settings.AttachedFiles);
            var logs = CreateFieldSetting(settings.Logs);
            var implementationReady = CreateFieldSetting(settings.ImplementationReady);

            return new ImplementationFieldSettings(
                status,
                realStartDate,
                buildImplemented,
                implementationPlanUsed,
                deviation,
                recoveryPlanUsed,
                finisingDate,
                attachedFiles,
                logs,
                implementationReady);
        }

        private static LogFieldSettings CreateLogSettings(LogSettingsModel settings)
        {
            var logs = CreateFieldSetting(settings.Logs);
            return new LogFieldSettings(logs);
        }

        private static OrdererFieldSettings CreateOrdererSettings(OrdererSettingsModel settings)
        {
            var id = CreateFieldSetting(settings.Id);
            var name = CreateFieldSetting(settings.Name);
            var phone = CreateFieldSetting(settings.Phone);
            var cellPhone = CreateFieldSetting(settings.CellPhone);
            var email = CreateFieldSetting(settings.Email);
            var department = CreateFieldSetting(settings.Department);

            return new OrdererFieldSettings(id, name, phone, cellPhone, email, department);
        }

        private static RegistrationFieldSettings CreateRegistrationSettings(RegistrationSettingsModel settings)
        {
            var name = CreateFieldSetting(settings.Name);
            var phone = CreateFieldSetting(settings.Phone);
            var email = CreateFieldSetting(settings.Email);
            var company = CreateFieldSetting(settings.Company);
            var owner = CreateFieldSetting(settings.Owner);
            var affectedProcesses = CreateFieldSetting(settings.AffectedProcesses);
            var affectedDepartments = CreateFieldSetting(settings.AffectedDepartments);
            var description = CreateFieldSetting(settings.Description);
            var businessBenefits = CreateFieldSetting(settings.BusinessBenefits);
            var consequence = CreateFieldSetting(settings.Consequence);
            var impact = CreateFieldSetting(settings.Impact);
            var desiredDate = CreateFieldSetting(settings.DesiredDate);
            var verified = CreateFieldSetting(settings.Verified);
            var attachedFiles = CreateFieldSetting(settings.AttachedFiles);
            var approval = CreateFieldSetting(settings.Approval);
            var rejectExplanation = CreateFieldSetting(settings.RejectExplanation);

            return new RegistrationFieldSettings(
                name,
                phone,
                email,
                company,
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

        #endregion
    }
}