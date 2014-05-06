namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.Concrete
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Changes.Settings.SettingsEdit;
    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.Web.Models.Changes.SettingsEdit;

    using iTextSharp.text;

    public sealed class SettingsModelFactory : ISettingsModelFactory
    {
        #region Public Methods and Operators

        public SettingsModel Create(ChangeFieldSettings settings, List<ItemOverview> languages)
        {
            var languageList = new SelectList(languages, "Value", "Name");

            var orderer = CreateOrdererSettings(settings.Orderer);
            var general = CreateGeneralSettings(settings.General);
            var registration = CreateRegistrationSettings(settings.Registration);
            var analyze = CreateAnalyzeSettings(settings.Analyze);
            var implementation = CreateImplementationSettings(settings.Implementation);
            var evaluation = CreateEvaluationSettings(settings.Evaluation);
            var log = CreateLogSettings(settings.Log);

            return new SettingsModel(
                settings.LanguageId.Value,
                languageList,
                orderer,
                general,
                registration,
                analyze,
                implementation,
                evaluation,
                log);
        }

        #endregion

        #region Methods

        private static AnalyzeSettingsModel CreateAnalyzeSettings(AnalyzeFieldSettings settings)
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

            return new AnalyzeSettingsModel(
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

        private static EvaluationSettingsModel CreateEvaluationSettings(EvaluationFieldSettings settings)
        {
            var changeEvaluation = CreateTextFieldSettingModel(settings.Evaluation);
            var attachedFiles = CreateFieldSettingModel(settings.AttachedFiles);
            var logs = CreateFieldSettingModel(settings.Logs);
            var evaluationReady = CreateFieldSettingModel(settings.EvaluationReady);

            return new EvaluationSettingsModel(changeEvaluation, attachedFiles, logs, evaluationReady);
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

        private static GeneralSettingsModel CreateGeneralSettings(GeneralFieldSettings settings)
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

            return new GeneralSettingsModel(
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

        private static ImplementationSettingsModel CreateImplementationSettings(ImplementationFieldSettings settings)
        {
            var implementationStatus = CreateFieldSettingModel(settings.Status);
            var realStartDate = CreateFieldSettingModel(settings.RealStartDate);
            var buildImplemented = CreateFieldSettingModel(settings.BuildImplemented);
            var implementationPlanUsed = CreateFieldSettingModel(settings.ImplementationPlanUsed);
            var deviation = CreateTextFieldSettingModel(settings.Deviation);
            var recoveryPlanUsed = CreateFieldSettingModel(settings.RecoveryPlanUsed);
            var finishingDate = CreateFieldSettingModel(settings.FinishingDate);
            var attachedFiles = CreateFieldSettingModel(settings.AttachedFiles);
            var logs = CreateFieldSettingModel(settings.Logs);
            var implementationReady = CreateFieldSettingModel(settings.ImplementationReady);

            return new ImplementationSettingsModel(
                implementationStatus,
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

        private static LogSettingsModel CreateLogSettings(LogFieldSettings settings)
        {
            var logs = CreateFieldSettingModel(settings.Logs);
            return new LogSettingsModel(logs);
        }

        private static OrdererSettingsModel CreateOrdererSettings(OrdererFieldSettings settings)
        {
            var id = CreateFieldSettingModel(settings.Id);
            var name = CreateFieldSettingModel(settings.Name);
            var phone = CreateFieldSettingModel(settings.Phone);
            var cellPhone = CreateFieldSettingModel(settings.CellPhone);
            var email = CreateFieldSettingModel(settings.Email);
            var department = CreateFieldSettingModel(settings.Department);

            return new OrdererSettingsModel(id, name, phone, cellPhone, email, department);
        }

        private static RegistrationSettingsModel CreateRegistrationSettings(RegistrationFieldSettings settings)
        {
            var name = CreateFieldSettingModel(settings.Name);
            var phone = CreateFieldSettingModel(settings.Phone);
            var email = CreateFieldSettingModel(settings.Email);
            var company = CreateFieldSettingModel(settings.Company);
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

            return new RegistrationSettingsModel(
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

        private static TextFieldSettingModel CreateTextFieldSettingModel(TextFieldSetting setting)
        {
            return new TextFieldSettingModel(
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