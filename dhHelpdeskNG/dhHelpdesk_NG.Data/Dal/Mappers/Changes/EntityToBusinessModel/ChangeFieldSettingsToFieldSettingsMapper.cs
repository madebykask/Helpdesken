namespace DH.Helpdesk.Dal.Dal.Mappers.Changes.EntityToBusinessModel
{
    using DH.Helpdesk.BusinessData.Models.Changes.Settings.SettingsEdit;
    using DH.Helpdesk.Common.Collections;
    using DH.Helpdesk.Common.Extensions.Integer;
    using DH.Helpdesk.Dal.Enums.Changes;
    using DH.Helpdesk.Dal.MapperData.Changes;

    public sealed class ChangeFieldSettingsToFieldSettingsMapper :
        IEntityToBusinessModelMapper<NamedObjectCollection<FieldSettingMapperData>, ChangeFieldSettings>
    {
        public ChangeFieldSettings Map(NamedObjectCollection<FieldSettingMapperData> entity)
        {
            var orderer = CreateOrderedSettings(entity);
            var general = CreateGeneralSettings(entity);
            var registration = CreateRegistrationSettings(entity);
            var analyze = CreateAnalyzeSettings(entity);
            var implementation = CreateImplementationSettings(entity);
            var evaluation = CreateEvaluationSettings(entity);
            var log = CreateLogSettings(entity);

            return ChangeFieldSettings.CreateForEdit(
                orderer, general, registration, analyze, implementation, evaluation, log);
        }

        private static OrdererFieldSettings CreateOrderedSettings(
            NamedObjectCollection<FieldSettingMapperData> fieldSettings)
        {
            var id = CreateFieldSetting(fieldSettings.FindByName(OrdererField.Id));
            var name = CreateFieldSetting(fieldSettings.FindByName(OrdererField.Name));
            var phone = CreateFieldSetting(fieldSettings.FindByName(OrdererField.Phone));
            var cellPhone = CreateFieldSetting(fieldSettings.FindByName(OrdererField.CellPhone));
            var email = CreateFieldSetting(fieldSettings.FindByName(OrdererField.Email));
            var department = CreateFieldSetting(fieldSettings.FindByName(OrdererField.Department));

            return new OrdererFieldSettings(id, name, phone, cellPhone, email, department);
        }

        private static GeneralFieldSettings CreateGeneralSettings(
            NamedObjectCollection<FieldSettingMapperData> fieldSettings)
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

            return new GeneralFieldSettings(
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

        private static RegistrationFieldSettings CreateRegistrationSettings(
            NamedObjectCollection<FieldSettingMapperData> fieldSettings)
        {
            var owner = CreateFieldSetting(fieldSettings.FindByName(RegistrationField.Owner));
            var affectedProcesses = CreateFieldSetting(fieldSettings.FindByName(RegistrationField.AffectedProcesses));

            var affectedDepartments = CreateFieldSetting(
                fieldSettings.FindByName(RegistrationField.AffectedDepartments));

            var description = CreateStringFieldSetting(fieldSettings.FindByName(RegistrationField.Description));

            var businessBenefits = CreateStringFieldSetting(
                fieldSettings.FindByName(RegistrationField.BusinessBenefits));

            var consequence = CreateStringFieldSetting(fieldSettings.FindByName(RegistrationField.Consequence));
            var impact = CreateFieldSetting(fieldSettings.FindByName(RegistrationField.Impact));
            var desiredDate = CreateFieldSetting(fieldSettings.FindByName(RegistrationField.DesiredDate));
            var verified = CreateFieldSetting(fieldSettings.FindByName(RegistrationField.Verified));
            var attachedFiles = CreateFieldSetting(fieldSettings.FindByName(RegistrationField.AttachedFiles));
            var approval = CreateFieldSetting(fieldSettings.FindByName(RegistrationField.Approval));
            var rejectExplanation = CreateFieldSetting(fieldSettings.FindByName(RegistrationField.RejectExplanation));

            return new RegistrationFieldSettings(
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

        private static AnalyzeFieldSettings CreateAnalyzeSettings(
            NamedObjectCollection<FieldSettingMapperData> fieldSettings)
        {
            var category = CreateFieldSetting(fieldSettings.FindByName(AnalyzeField.Category));
            var priority = CreateFieldSetting(fieldSettings.FindByName(AnalyzeField.Priority));
            var responsible = CreateFieldSetting(fieldSettings.FindByName(AnalyzeField.Responsible));
            var solution = CreateStringFieldSetting(fieldSettings.FindByName(AnalyzeField.Solution));
            var cost = CreateFieldSetting(fieldSettings.FindByName(AnalyzeField.Cost));
            var yearlyCost = CreateFieldSetting(fieldSettings.FindByName(AnalyzeField.YearlyCost));
            var estimatedTimeInHours = CreateFieldSetting(fieldSettings.FindByName(AnalyzeField.EstimatedTimeInHours));
            var risk = CreateStringFieldSetting(fieldSettings.FindByName(AnalyzeField.Risk));
            var startDate = CreateFieldSetting(fieldSettings.FindByName(AnalyzeField.StartDate));
            var finishDate = CreateFieldSetting(fieldSettings.FindByName(AnalyzeField.FinishDate));
            var hasImplementationPlan = CreateFieldSetting(fieldSettings.FindByName(AnalyzeField.HasImplementationPlan));
            var hasRecoveryPlan = CreateFieldSetting(fieldSettings.FindByName(AnalyzeField.HasRecoveryPlan));
            var attachedFiles = CreateFieldSetting(fieldSettings.FindByName(AnalyzeField.AttachedFiles));
            var logs = CreateFieldSetting(fieldSettings.FindByName(AnalyzeField.Logs));
            var approval = CreateFieldSetting(fieldSettings.FindByName(AnalyzeField.Approval));
            var rejectExplanation = CreateStringFieldSetting(fieldSettings.FindByName(AnalyzeField.RejectExplanation));

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

        private static ImplementationFieldSettings CreateImplementationSettings(
            NamedObjectCollection<FieldSettingMapperData> fieldSettings)
        {
            var status = CreateFieldSetting(fieldSettings.FindByName(ImplementationField.Status));
            var realStartDate = CreateFieldSetting(fieldSettings.FindByName(ImplementationField.RealStartDate));
            var buildImplemented = CreateFieldSetting(fieldSettings.FindByName(ImplementationField.BuildImplemented));

            var implementationPlanUsed =
                CreateFieldSetting(fieldSettings.FindByName(ImplementationField.ImplementationPlanUsed));

            var deviation = CreateStringFieldSetting(fieldSettings.FindByName(ImplementationField.Deviation));
            var recoveryPlanUsed = CreateFieldSetting(fieldSettings.FindByName(ImplementationField.RecoveryPlanUsed));
            var finishingDate = CreateFieldSetting(fieldSettings.FindByName(ImplementationField.FinishingDate));
            var attachedFiles = CreateFieldSetting(fieldSettings.FindByName(ImplementationField.AttachedFiles));
            var logs = CreateFieldSetting(fieldSettings.FindByName(ImplementationField.Logs));

            var implementationReady =
                CreateFieldSetting(fieldSettings.FindByName(ImplementationField.ImplementationReady));

            return new ImplementationFieldSettings(
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

        private static EvaluationFieldSettings CreateEvaluationSettings(
            NamedObjectCollection<FieldSettingMapperData> fieldSettings)
        {
            var evaluation = CreateStringFieldSetting(fieldSettings.FindByName(EvaluationField.ChangeEvaluation));
            var attachedFiles = CreateFieldSetting(fieldSettings.FindByName(EvaluationField.AttachedFiles));
            var logs = CreateFieldSetting(fieldSettings.FindByName(EvaluationField.Logs));
            var evaluationReady = CreateFieldSetting(fieldSettings.FindByName(EvaluationField.EvaluationReady));

            return new EvaluationFieldSettings(evaluation, attachedFiles, logs, evaluationReady);
        }

        private static LogFieldSettings CreateLogSettings(NamedObjectCollection<FieldSettingMapperData> fieldSettings)
        {
            var logs = CreateFieldSetting(fieldSettings.FindByName(LogField.Logs));
            return new LogFieldSettings(logs);
        }

        private static TextFieldSetting CreateStringFieldSetting(FieldSettingMapperData data)
        {
            return TextFieldSetting.CreateForEdit(
                data.Show.ToBool(),
                data.ShowInList.ToBool(),
                data.ShowExternal.ToBool(),
                data.Caption,
                data.Required.ToBool(),
                data.InitialValue,
                data.Bookmark);
        }

        private static FieldSetting CreateFieldSetting(FieldSettingMapperData data)
        {
            return FieldSetting.CreateForEdit(
                data.Show.ToBool(),
                data.ShowInList.ToBool(),
                data.ShowExternal.ToBool(),
                data.Caption,
                data.Required.ToBool(),
                data.Bookmark);
        }
    }
}
