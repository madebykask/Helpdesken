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
            NamedObjectCollection<FieldEditSettingMapperData> fieldSettings)
        {
            var id = CreateFieldSetting(fieldSettings.FindByName(OrdererField.Id));
            var name = CreateFieldSetting(fieldSettings.FindByName(OrdererField.Name));
            var phone = CreateFieldSetting(fieldSettings.FindByName(OrdererField.Phone));
            var cellPhone = CreateFieldSetting(fieldSettings.FindByName(OrdererField.CellPhone));
            var email = CreateFieldSetting(fieldSettings.FindByName(OrdererField.Email));
            var department = CreateFieldSetting(fieldSettings.FindByName(OrdererField.Department));

            return new OrdererEditSettings(id, name, phone, cellPhone, email, department);
        }

        private static GeneralEditSettings CreateGeneralSettings(
            NamedObjectCollection<FieldEditSettingMapperData> fieldSettings)
        {
            var priority = CreateFieldSetting(fieldSettings.FindByName(GeneralField.Priority));
            var title = CreateFieldSetting(fieldSettings.FindByName(GeneralField.Title));
            var state = CreateFieldSetting(fieldSettings.FindByName(GeneralField.Status));
            var system = CreateFieldSetting(fieldSettings.FindByName(GeneralField.System));
            var @object = CreateFieldSetting(fieldSettings.FindByName(GeneralField.Object));
            var inventory = CreateFieldSetting(fieldSettings.FindByName(GeneralField.Inventory));
            var workingGroup = CreateFieldSetting(fieldSettings.FindByName(GeneralField.WorkingGroup));
            var administrator = CreateFieldSetting(fieldSettings.FindByName(GeneralField.Administrator));
            var finishingDate = CreateFieldSetting(fieldSettings.FindByName(GeneralField.FinishingDate));
            var rss = CreateFieldSetting(fieldSettings.FindByName(GeneralField.Rss));

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
            NamedObjectCollection<FieldEditSettingMapperData> fieldSettings)
        {
            var owner = CreateFieldSetting(fieldSettings.FindByName(RegistrationField.Owner));
            var affectedProcesses = CreateFieldSetting(fieldSettings.FindByName(RegistrationField.AffectedProcesses));

            var affectedDepartments = CreateFieldSetting(
                fieldSettings.FindByName(RegistrationField.AffectedDepartments));

            var description = CreateTextFieldSetting(fieldSettings.FindByName(RegistrationField.Description));
            var businessBenefits = CreateTextFieldSetting(fieldSettings.FindByName(RegistrationField.BusinessBenefits));
            var consequence = CreateTextFieldSetting(fieldSettings.FindByName(RegistrationField.Consequence));
            var impact = CreateFieldSetting(fieldSettings.FindByName(RegistrationField.Impact));
            var desiredDate = CreateFieldSetting(fieldSettings.FindByName(RegistrationField.DesiredDate));
            var verified = CreateFieldSetting(fieldSettings.FindByName(RegistrationField.Verified));
            var attachedFiles = CreateFieldSetting(fieldSettings.FindByName(RegistrationField.AttachedFiles));
            var approval = CreateFieldSetting(fieldSettings.FindByName(RegistrationField.Approval));
            var rejectExplanation = CreateFieldSetting(fieldSettings.FindByName(RegistrationField.RejectExplanation));

            return new RegistrationEditSettings(
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
            NamedObjectCollection<FieldEditSettingMapperData> fieldSettings)
        {
            var category = CreateFieldSetting(fieldSettings.FindByName(AnalyzeField.Category));
            var priority = CreateFieldSetting(fieldSettings.FindByName(AnalyzeField.Priority));
            var responsible = CreateFieldSetting(fieldSettings.FindByName(AnalyzeField.Responsible));
            var solution = CreateTextFieldSetting(fieldSettings.FindByName(AnalyzeField.Solution));
            var cost = CreateFieldSetting(fieldSettings.FindByName(AnalyzeField.Cost));
            var yearlyCost = CreateFieldSetting(fieldSettings.FindByName(AnalyzeField.YearlyCost));
            var estimatedTimeInHours = CreateFieldSetting(fieldSettings.FindByName(AnalyzeField.EstimatedTimeInHours));
            var risk = CreateTextFieldSetting(fieldSettings.FindByName(AnalyzeField.Risk));
            var startDate = CreateFieldSetting(fieldSettings.FindByName(AnalyzeField.StartDate));
            var finishDate = CreateFieldSetting(fieldSettings.FindByName(AnalyzeField.FinishDate));
            var hasImplementationPlan = CreateFieldSetting(fieldSettings.FindByName(AnalyzeField.HasImplementationPlan));
            var hasRecoveryPlan = CreateFieldSetting(fieldSettings.FindByName(AnalyzeField.HasRecoveryPlan));
            var attachedFiles = CreateFieldSetting(fieldSettings.FindByName(AnalyzeField.AttachedFiles));
            var logs = CreateFieldSetting(fieldSettings.FindByName(AnalyzeField.Logs));
            var approval = CreateFieldSetting(fieldSettings.FindByName(AnalyzeField.Approval));
            var rejectExplanation = CreateTextFieldSetting(fieldSettings.FindByName(AnalyzeField.RejectExplanation));

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
            NamedObjectCollection<FieldEditSettingMapperData> fieldSettings)
        {
            var status = CreateFieldSetting(fieldSettings.FindByName(ImplementationField.Status));
            var realStartDate = CreateFieldSetting(fieldSettings.FindByName(ImplementationField.RealStartDate));
            var buildImplemented = CreateFieldSetting(fieldSettings.FindByName(ImplementationField.BuildImplemented));

            var implementationPlanUsed =
                CreateFieldSetting(fieldSettings.FindByName(ImplementationField.ImplementationPlanUsed));

            var deviation = CreateTextFieldSetting(fieldSettings.FindByName(ImplementationField.Deviation));
            var recoveryPlanUsed = CreateFieldSetting(fieldSettings.FindByName(ImplementationField.RecoveryPlanUsed));
            var finishingDate = CreateFieldSetting(fieldSettings.FindByName(ImplementationField.FinishingDate));
            var attachedFiles = CreateFieldSetting(fieldSettings.FindByName(ImplementationField.AttachedFiles));
            var logs = CreateFieldSetting(fieldSettings.FindByName(ImplementationField.Logs));

            var implementationsReady =
                CreateFieldSetting(fieldSettings.FindByName(ImplementationField.ImplementationReady));

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
            NamedObjectCollection<FieldEditSettingMapperData> fieldSettings)
        {
            var changeEvaluation = CreateTextFieldSetting(fieldSettings.FindByName(EvaluationField.ChangeEvaluation));
            var attachedFiles = CreateFieldSetting(fieldSettings.FindByName(EvaluationField.ChangeEvaluation));
            var logs = CreateFieldSetting(fieldSettings.FindByName(EvaluationField.ChangeEvaluation));
            var evaluationReady = CreateFieldSetting(fieldSettings.FindByName(EvaluationField.ChangeEvaluation));

            return new EvaluationEditSettings(changeEvaluation, attachedFiles, logs, evaluationReady);
        }

        private static LogFieldEditSettings CreateLogSettings(
            NamedObjectCollection<FieldEditSettingMapperData> fieldSettings)
        {
            var logs = CreateFieldSetting(fieldSettings.FindByName(LogField.Logs));
            return new LogFieldEditSettings(logs);
        }

        private static FieldEditSetting CreateFieldSetting(FieldEditSettingMapperData fieldSetting)
        {
            return new FieldEditSetting(
                fieldSetting.Show.ToBool(),
                fieldSetting.Caption,
                fieldSetting.Required.ToBool(),
                fieldSetting.Bookmark);
        }

        private static TextFieldEditSetting CreateTextFieldSetting(FieldEditSettingMapperData fieldSetting)
        {
            return new TextFieldEditSetting(
                fieldSetting.Show.ToBool(),
                fieldSetting.Caption,
                fieldSetting.Required.ToBool(),
                fieldSetting.InitialValue,
                fieldSetting.Bookmark);
        }
    }
}
