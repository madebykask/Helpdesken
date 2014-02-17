namespace DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Changes.Concrete
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Changes.Input.Settings;
    using DH.Helpdesk.Web.Models.Changes.Settings;

    public sealed class UpdatedSettingsFactory : IUpdatedSettingsFactory
    {
        public UpdatedSettings Create(
            SettingsModel settings, int customerId, int languageId, DateTime changedDateAndTime)
        {
            var orderer = CreateOrdererSettings(settings.Orderer, changedDateAndTime);
            var general = CreateGeneralSettings(settings.General, changedDateAndTime);
            var registration = CreateRegistrationSettings(settings.Registration, changedDateAndTime);
            var analyze = CreateAnalyzeSettings(settings.Analyze, changedDateAndTime);
            var implementation = CreateImplementationSettings(settings.Implementation, changedDateAndTime);
            var evaluation = CreateEvaluationSettings(settings.Evaluation, changedDateAndTime);
            var log = CreateLogSettings(settings.Log, changedDateAndTime);

            return new UpdatedSettings(
                customerId, languageId, orderer, general, registration, analyze, implementation, evaluation, log);
        }

        private static UpdatedOrdererSettings CreateOrdererSettings(
            OrdererFieldSettingsModel settings, DateTime changedDateAndTime)
        {
            var id = CreateFieldSetting(settings.Id, changedDateAndTime);
            var name = CreateFieldSetting(settings.Name, changedDateAndTime);
            var phone = CreateFieldSetting(settings.Phone, changedDateAndTime);
            var cellPhone = CreateFieldSetting(settings.CellPhone, changedDateAndTime);
            var email = CreateFieldSetting(settings.Email, changedDateAndTime);
            var department = CreateFieldSetting(settings.Department, changedDateAndTime);

            return new UpdatedOrdererSettings(id, name, phone, cellPhone, email, department);
        }

        private static UpdatedGeneralSettings CreateGeneralSettings(
            GeneralFieldSettingsModel settings, DateTime changedDateAndTime)
        {
            var priority = CreateFieldSetting(settings.Priority, changedDateAndTime);
            var title = CreateFieldSetting(settings.Title, changedDateAndTime);
            var status = CreateFieldSetting(settings.Status, changedDateAndTime);
            var system = CreateFieldSetting(settings.System, changedDateAndTime);
            var @object = CreateFieldSetting(settings.Object, changedDateAndTime);
            var inventory = CreateFieldSetting(settings.Inventory, changedDateAndTime);
            var workingGroup = CreateFieldSetting(settings.WorkingGroup, changedDateAndTime);
            var administrator = CreateFieldSetting(settings.Administrator, changedDateAndTime);
            var finishingDate = CreateFieldSetting(settings.FinishingDate, changedDateAndTime);
            var rss = CreateFieldSetting(settings.Rss, changedDateAndTime);

            return new UpdatedGeneralSettings(
                priority, title, status, system, @object, inventory, workingGroup, administrator, finishingDate, rss);
        }

        private static UpdatedRegistrationSettings CreateRegistrationSettings(
            RegistrationFieldSettingsModel settings, DateTime changedDateAndTime)
        {
            var owner = CreateFieldSetting(settings.Owner, changedDateAndTime);
            var affectedProcesses = CreateFieldSetting(settings.AffectedProcesses, changedDateAndTime);
            var affectedDepartments = CreateFieldSetting(settings.AffectedDepartments, changedDateAndTime);
            var description = CreateFieldSetting(settings.Description, changedDateAndTime);
            var businessBenefits = CreateFieldSetting(settings.BusinessBenefits, changedDateAndTime);
            var consequence = CreateFieldSetting(settings.Consequence, changedDateAndTime);
            var impact = CreateFieldSetting(settings.Impact, changedDateAndTime);
            var desiredDate = CreateFieldSetting(settings.DesiredDate, changedDateAndTime);
            var verified = CreateFieldSetting(settings.Verified, changedDateAndTime);
            var attachedFiles = CreateFieldSetting(settings.AttachedFiles, changedDateAndTime);
            var approval = CreateFieldSetting(settings.Approval, changedDateAndTime);
            var rejectExplanation = CreateFieldSetting(settings.RejectExplanation, changedDateAndTime);

            return new UpdatedRegistrationSettings(
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

        private static UpdatedAnalyzeSettings CreateAnalyzeSettings(
            AnalyzeFieldSettingsModel settings, DateTime changedDateAndTime)
        {
            var category = CreateFieldSetting(settings.Category, changedDateAndTime);
            var priority = CreateFieldSetting(settings.Priority, changedDateAndTime);
            var responsible = CreateFieldSetting(settings.Responsible, changedDateAndTime);
            var solution = CreateFieldSetting(settings.Solution, changedDateAndTime);
            var cost = CreateFieldSetting(settings.Cost, changedDateAndTime);
            var yearlyCost = CreateFieldSetting(settings.YearlyCost, changedDateAndTime);
            var estimatedTimeInHours = CreateFieldSetting(settings.EstimatedTimeInHours, changedDateAndTime);
            var risk = CreateFieldSetting(settings.Risk, changedDateAndTime);
            var startDate = CreateFieldSetting(settings.StartDate, changedDateAndTime);
            var finishDate = CreateFieldSetting(settings.FinishDate, changedDateAndTime);
            var hasImplementationPlan = CreateFieldSetting(settings.HasImplementationPlan, changedDateAndTime);
            var hasRecoveryPlan = CreateFieldSetting(settings.HasRecoveryPlan, changedDateAndTime);
            var attachedFiles = CreateFieldSetting(settings.AttachedFiles, changedDateAndTime);
            var logs = CreateFieldSetting(settings.Logs, changedDateAndTime);
            var approval = CreateFieldSetting(settings.Approval, changedDateAndTime);
            var rejectExplanation = CreateFieldSetting(settings.RejectExplanation, changedDateAndTime);

            return new UpdatedAnalyzeSettings(
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

        private static UpdatedImplementationSettings CreateImplementationSettings(
            ImplementationFieldSettingsModel settings, DateTime changedDateAndTime)
        {
            var status = CreateFieldSetting(settings.Status, changedDateAndTime);
            var realStartDate = CreateFieldSetting(settings.RealStartDate, changedDateAndTime);
            var buildImplemented = CreateFieldSetting(settings.BuildImplemented, changedDateAndTime);
            var implementationPlanUsed = CreateFieldSetting(settings.ImplementationPlanUsed, changedDateAndTime);
            var deviation = CreateFieldSetting(settings.Deviation, changedDateAndTime);
            var recoveryPlanUsed = CreateFieldSetting(settings.RecoveryPlanUsed, changedDateAndTime);
            var finisingDate = CreateFieldSetting(settings.FinishingDate, changedDateAndTime);
            var attachedFiles = CreateFieldSetting(settings.AttachedFiles, changedDateAndTime);
            var logs = CreateFieldSetting(settings.Logs, changedDateAndTime);
            var implementationReady = CreateFieldSetting(settings.ImplementationReady, changedDateAndTime);

            return new UpdatedImplementationSettings(
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

        private static UpdatedEvaluationSettings CreateEvaluationSettings(
            EvaluationFieldSettingsModel settings, DateTime changedDateAndTime)
        {
            var evaluation = CreateFieldSetting(settings.ChangeEvaluation, changedDateAndTime);
            var attachedFiles = CreateFieldSetting(settings.AttachedFiles, changedDateAndTime);
            var logs = CreateFieldSetting(settings.Logs, changedDateAndTime);
            var evaluationReady = CreateFieldSetting(settings.EvaluationReady, changedDateAndTime);

            return new UpdatedEvaluationSettings(evaluation, attachedFiles, logs, evaluationReady);
        }

        private static UpdatedLogSettings CreateLogSettings(LogFieldSettingsModel settings, DateTime changedDateAndTime)
        {
            var logs = CreateFieldSetting(settings.Logs, changedDateAndTime);
            return new UpdatedLogSettings(logs);
        }

        private static UpdatedFieldSetting CreateFieldSetting(FieldSettingModel setting, DateTime changedDateAndTime)
        {
            return new UpdatedFieldSetting(
                setting.ShowInDetails,
                setting.ShowInChanges,
                setting.ShowInSelfService,
                setting.Caption,
                setting.Required,
                setting.Bookmark,
                changedDateAndTime);
        }

        private static UpdatedTextFieldSetting CreateFieldSetting(
            StringFieldSettingModel setting, DateTime changedDateAndTime)
        {
            return new UpdatedTextFieldSetting(
                setting.ShowInDetails,
                setting.ShowInChanges,
                setting.ShowInSelfService,
                setting.Caption,
                setting.Required,
                setting.DefaultValue,
                setting.Bookmark,
                changedDateAndTime);
        }
    }
}