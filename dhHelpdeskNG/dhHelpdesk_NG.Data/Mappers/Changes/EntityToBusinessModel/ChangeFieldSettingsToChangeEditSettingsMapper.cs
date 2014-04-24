namespace DH.Helpdesk.Dal.Mappers.Changes.EntityToBusinessModel
{
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.Common.Collections;
    using DH.Helpdesk.Common.Extensions.Integer;
    using DH.Helpdesk.Dal.Enums.Changes;
    using DH.Helpdesk.Dal.MapperData.Changes;

    public sealed class ChangeFieldSettingsToChangeEditSettingsMapper :
        IEntityToBusinessModelMapper<NamedObjectCollection<FieldEditSettingMapperData>, ChangeEditSettings>
    {
        public ChangeEditSettings Map(NamedObjectCollection<FieldEditSettingMapperData> entity)
        {
            var orderer = CreateOrdererSettings(entity);
            var general = CreateGeneralSettings(entity);
            var registration = CreateRegistrationSettings(entity);
            var analyze = CreateAnalyzeSettings(entity);
            var implementation = CreateImplementationSettings(entity);
            var evaluation = CreateEvaluationSettings(entity);
            var log = CreateLogSettings(entity);

            return new ChangeEditSettings(orderer, general, registration, analyze, implementation, evaluation, log);
        }

        private static OrdererEditSettings CreateOrdererSettings(
            NamedObjectCollection<FieldEditSettingMapperData> settings)
        {
            var id = CreateFieldSetting(settings.FindByName(OrdererField.Id));
            var name = CreateFieldSetting(settings.FindByName(OrdererField.Name));
            var phone = CreateFieldSetting(settings.FindByName(OrdererField.Phone));
            var cellPhone = CreateFieldSetting(settings.FindByName(OrdererField.CellPhone));
            var email = CreateFieldSetting(settings.FindByName(OrdererField.Email));
            var department = CreateFieldSetting(settings.FindByName(OrdererField.Department));

            return new OrdererEditSettings(id, name, phone, cellPhone, email, department);
        }

        private static GeneralEditSettings CreateGeneralSettings(
            NamedObjectCollection<FieldEditSettingMapperData> settings)
        {
            var priority = CreateFieldSetting(settings.FindByName(GeneralField.Priority));
            var title = CreateFieldSetting(settings.FindByName(GeneralField.Title));
            var state = CreateFieldSetting(settings.FindByName(GeneralField.Status));
            var system = CreateFieldSetting(settings.FindByName(GeneralField.System));
            var @object = CreateFieldSetting(settings.FindByName(GeneralField.Object));
            var inventory = CreateFieldSetting(settings.FindByName(GeneralField.Inventory));
            var workingGroup = CreateFieldSetting(settings.FindByName(GeneralField.WorkingGroup));
            var administrator = CreateFieldSetting(settings.FindByName(GeneralField.Administrator));
            var finishingDate = CreateFieldSetting(settings.FindByName(GeneralField.FinishingDate));
            var rss = CreateFieldSetting(settings.FindByName(GeneralField.Rss));

            return new GeneralEditSettings(
                priority,
                title,
                state,
                system,
                @object,
                inventory,
                workingGroup,
                administrator,
                finishingDate,
                rss);
        }

        private static RegistrationEditSettings CreateRegistrationSettings(
            NamedObjectCollection<FieldEditSettingMapperData> settings)
        {
            var name = CreateFieldSetting(settings.FindByName(RegistrationField.Name));
            var phone = CreateFieldSetting(settings.FindByName(RegistrationField.Phone));
            var email = CreateFieldSetting(settings.FindByName(RegistrationField.Email));
            var company = CreateFieldSetting(settings.FindByName(RegistrationField.Company));
            var owner = CreateFieldSetting(settings.FindByName(RegistrationField.Owner));
            var affectedProcesses = CreateFieldSetting(settings.FindByName(RegistrationField.AffectedProcesses));
            var affectedDepartments = CreateFieldSetting(settings.FindByName(RegistrationField.AffectedDepartments));
            var description = CreateTextFieldSetting(settings.FindByName(RegistrationField.Description));
            var businessBenefits = CreateTextFieldSetting(settings.FindByName(RegistrationField.BusinessBenefits));
            var consequence = CreateTextFieldSetting(settings.FindByName(RegistrationField.Consequence));
            var impact = CreateFieldSetting(settings.FindByName(RegistrationField.Impact));
            var desiredDate = CreateFieldSetting(settings.FindByName(RegistrationField.DesiredDate));
            var verified = CreateFieldSetting(settings.FindByName(RegistrationField.Verified));
            var attachedFiles = CreateFieldSetting(settings.FindByName(RegistrationField.AttachedFiles));
            var approval = CreateFieldSetting(settings.FindByName(RegistrationField.Approval));
            var rejectExplanation = CreateFieldSetting(settings.FindByName(RegistrationField.RejectExplanation));

            return new RegistrationEditSettings(
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

        private static AnalyzeEditSettings CreateAnalyzeSettings(
            NamedObjectCollection<FieldEditSettingMapperData> settings)
        {
            var category = CreateFieldSetting(settings.FindByName(AnalyzeField.Category));
            var priority = CreateFieldSetting(settings.FindByName(AnalyzeField.Priority));
            var responsible = CreateFieldSetting(settings.FindByName(AnalyzeField.Responsible));
            var solution = CreateTextFieldSetting(settings.FindByName(AnalyzeField.Solution));
            var cost = CreateFieldSetting(settings.FindByName(AnalyzeField.Cost));
            var yearlyCost = CreateFieldSetting(settings.FindByName(AnalyzeField.YearlyCost));
            var estimatedTimeInHours = CreateFieldSetting(settings.FindByName(AnalyzeField.EstimatedTimeInHours));
            var risk = CreateTextFieldSetting(settings.FindByName(AnalyzeField.Risk));
            var startDate = CreateFieldSetting(settings.FindByName(AnalyzeField.StartDate));
            var finishDate = CreateFieldSetting(settings.FindByName(AnalyzeField.FinishDate));
            var hasImplementationPlan = CreateFieldSetting(settings.FindByName(AnalyzeField.HasImplementationPlan));
            var hasRecoveryPlan = CreateFieldSetting(settings.FindByName(AnalyzeField.HasRecoveryPlan));
            var attachedFiles = CreateFieldSetting(settings.FindByName(AnalyzeField.AttachedFiles));
            var logs = CreateFieldSetting(settings.FindByName(AnalyzeField.Logs));
            var approval = CreateFieldSetting(settings.FindByName(AnalyzeField.Approval));
            var rejectExplanation = CreateTextFieldSetting(settings.FindByName(AnalyzeField.RejectExplanation));

            return new AnalyzeEditSettings(
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

        private static ImplementationEditSettings CreateImplementationSettings(
            NamedObjectCollection<FieldEditSettingMapperData> settings)
        {
            var status = CreateFieldSetting(settings.FindByName(ImplementationField.ImplementationStatus));
            var realStartDate = CreateFieldSetting(settings.FindByName(ImplementationField.RealStartDate));
            var buildImplemented = CreateFieldSetting(settings.FindByName(ImplementationField.BuildImplemented));

            var implementationPlanUsed =
                CreateFieldSetting(settings.FindByName(ImplementationField.ImplementationPlanUsed));

            var deviation = CreateTextFieldSetting(settings.FindByName(ImplementationField.Deviation));
            var recoveryPlanUsed = CreateFieldSetting(settings.FindByName(ImplementationField.RecoveryPlanUsed));
            var finishingDate = CreateFieldSetting(settings.FindByName(ImplementationField.FinishingDate));
            var attachedFiles = CreateFieldSetting(settings.FindByName(ImplementationField.AttachedFiles));
            var logs = CreateFieldSetting(settings.FindByName(ImplementationField.Logs));

            var implementationsReady = CreateFieldSetting(settings.FindByName(ImplementationField.ImplementationReady));

            return new ImplementationEditSettings(
                status,
                realStartDate,
                buildImplemented,
                implementationPlanUsed,
                deviation,
                recoveryPlanUsed,
                finishingDate,
                attachedFiles,
                logs,
                implementationsReady);
        }

        private static EvaluationEditSettings CreateEvaluationSettings(
            NamedObjectCollection<FieldEditSettingMapperData> settings)
        {
            var changeEvaluation = CreateTextFieldSetting(settings.FindByName(EvaluationField.ChangeEvaluation));
            var attachedFiles = CreateFieldSetting(settings.FindByName(EvaluationField.ChangeEvaluation));
            var logs = CreateFieldSetting(settings.FindByName(EvaluationField.ChangeEvaluation));
            var evaluationReady = CreateFieldSetting(settings.FindByName(EvaluationField.ChangeEvaluation));

            return new EvaluationEditSettings(changeEvaluation, attachedFiles, logs, evaluationReady);
        }

        private static LogEditSettings CreateLogSettings(NamedObjectCollection<FieldEditSettingMapperData> settings)
        {
            var logs = CreateFieldSetting(settings.FindByName(LogField.Logs));
            return new LogEditSettings(logs);
        }

        private static FieldEditSetting CreateFieldSetting(FieldEditSettingMapperData setting)
        {
            return new FieldEditSetting(
                setting.Show.ToBool(),
                setting.Caption,
                setting.Required.ToBool(),
                setting.Bookmark);
        }

        private static TextFieldEditSetting CreateTextFieldSetting(FieldEditSettingMapperData setting)
        {
            return new TextFieldEditSetting(
                setting.Show.ToBool(),
                setting.Caption,
                setting.Required.ToBool(),
                setting.InitialValue,
                setting.Bookmark);
        }
    }
}