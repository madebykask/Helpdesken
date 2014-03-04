namespace DH.Helpdesk.Dal.Dal.Mappers.Changes.EntityToBusinessModel
{
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeProcessing;
    using DH.Helpdesk.Common.Collections;
    using DH.Helpdesk.Common.Extensions.Integer;
    using DH.Helpdesk.Dal.Enums.Changes;
    using DH.Helpdesk.Dal.MapperData.Changes;

    public sealed class ChangeFieldSettingsToChangeProcessingSettingsMapper :
        IEntityToBusinessModelMapper<NamedObjectCollection<FieldProcessingSettingMapperData>, ChangeProcessingSettings>
    {
        public ChangeProcessingSettings Map(NamedObjectCollection<FieldProcessingSettingMapperData> entity)
        {
            var orderer = CreateOrdererSettings(entity);
            var general = CreateGeneralSettings(entity);
            var registration = CreateRegistrationSettings(entity);
            var analyze = CreateAnalyzeSettings(entity);
            var implementation = CreateImplementationSettings(entity);
            var evaluation = CreateEvaluationSettings(entity);

            return new ChangeProcessingSettings(orderer, general, registration, analyze, implementation, evaluation);
        }

        private static OrdererProcessingSettings CreateOrdererSettings(
            NamedObjectCollection<FieldProcessingSettingMapperData> entity)
        {
            var id = CreateFieldSetting(entity.FindByName(OrdererField.Id));
            var name = CreateFieldSetting(entity.FindByName(OrdererField.Name));
            var phone = CreateFieldSetting(entity.FindByName(OrdererField.Phone));
            var cellPhone = CreateFieldSetting(entity.FindByName(OrdererField.CellPhone));
            var email = CreateFieldSetting(entity.FindByName(OrdererField.Email));
            var department = CreateFieldSetting(entity.FindByName(OrdererField.Department));

            return new OrdererProcessingSettings(id, name, phone, cellPhone, email, department);
        }

        private static GeneralProcessingSettings CreateGeneralSettings(
            NamedObjectCollection<FieldProcessingSettingMapperData> entity)
        {
            var priority = CreateFieldSetting(entity.FindByName(GeneralField.Priority));
            var title = CreateFieldSetting(entity.FindByName(GeneralField.Title));
            var status = CreateFieldSetting(entity.FindByName(GeneralField.Status));
            var system = CreateFieldSetting(entity.FindByName(GeneralField.System));
            var @object = CreateFieldSetting(entity.FindByName(GeneralField.Object));
            var workingGroup = CreateFieldSetting(entity.FindByName(GeneralField.WorkingGroup));
            var administrator = CreateFieldSetting(entity.FindByName(GeneralField.Administrator));
            var finishingDate = CreateFieldSetting(entity.FindByName(GeneralField.FinishingDate));
            var rss = CreateFieldSetting(entity.FindByName(GeneralField.Rss));

            return new GeneralProcessingSettings(
                priority, title, status, system, @object, workingGroup, administrator, finishingDate, rss);
        }

        private static RegistrationProcessingSettings CreateRegistrationSettings(
            NamedObjectCollection<FieldProcessingSettingMapperData> entity)
        {
            var owner = CreateFieldSetting(entity.FindByName(RegistrationField.Owner));
            var affectedProcesses = CreateFieldSetting(entity.FindByName(RegistrationField.AffectedProcesses));
            var affectedDepartments = CreateFieldSetting(entity.FindByName(RegistrationField.AffectedDepartments));
            var description = CreateFieldSetting(entity.FindByName(RegistrationField.Description));
            var businessBenefits = CreateFieldSetting(entity.FindByName(RegistrationField.BusinessBenefits));
            var consequence = CreateFieldSetting(entity.FindByName(RegistrationField.Consequence));
            var impact = CreateFieldSetting(entity.FindByName(RegistrationField.Impact));
            var desiredDate = CreateFieldSetting(entity.FindByName(RegistrationField.DesiredDate));
            var verified = CreateFieldSetting(entity.FindByName(RegistrationField.Verified));
            var attachedFiles = CreateFieldSetting(entity.FindByName(RegistrationField.AttachedFiles));
            var approval = CreateFieldSetting(entity.FindByName(RegistrationField.Approval));
            var rejectExplanation = CreateFieldSetting(entity.FindByName(RegistrationField.RejectExplanation));

            return new RegistrationProcessingSettings(
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

        private static AnalyzeProcessingSettings CreateAnalyzeSettings(
            NamedObjectCollection<FieldProcessingSettingMapperData> entity)
        {
            var category = CreateFieldSetting(entity.FindByName(AnalyzeField.Category));
            var priority = CreateFieldSetting(entity.FindByName(AnalyzeField.Priority));
            var responsible = CreateFieldSetting(entity.FindByName(AnalyzeField.Responsible));
            var solution = CreateFieldSetting(entity.FindByName(AnalyzeField.Solution));
            var cost = CreateFieldSetting(entity.FindByName(AnalyzeField.Cost));
            var estimatedTimeInHours = CreateFieldSetting(entity.FindByName(AnalyzeField.EstimatedTimeInHours));
            var risk = CreateFieldSetting(entity.FindByName(AnalyzeField.Risk));
            var startDate = CreateFieldSetting(entity.FindByName(AnalyzeField.StartDate));
            var finishDate = CreateFieldSetting(entity.FindByName(AnalyzeField.FinishDate));
            var hasImplementationPlan = CreateFieldSetting(entity.FindByName(AnalyzeField.HasImplementationPlan));
            var hasRecoveryPlan = CreateFieldSetting(entity.FindByName(AnalyzeField.HasRecoveryPlan));
            var attachedFiles = CreateFieldSetting(entity.FindByName(AnalyzeField.AttachedFiles));
            var logs = CreateFieldSetting(entity.FindByName(AnalyzeField.Logs));
            var approval = CreateFieldSetting(entity.FindByName(AnalyzeField.Approval));
            var rejectExplanation = CreateFieldSetting(entity.FindByName(AnalyzeField.RejectExplanation));

            return new AnalyzeProcessingSettings(
                category,
                priority,
                responsible,
                solution,
                cost,
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

        private static ImplementationProcessingSettings CreateImplementationSettings(
            NamedObjectCollection<FieldProcessingSettingMapperData> entity)
        {
            var status = CreateFieldSetting(entity.FindByName(ImplementationField.Status));
            var realStartDate = CreateFieldSetting(entity.FindByName(ImplementationField.RealStartDate));
            var finishingDate = CreateFieldSetting(entity.FindByName(ImplementationField.FinishingDate));
            var buildImplemented = CreateFieldSetting(entity.FindByName(ImplementationField.BuildImplemented));

            var implementationPlanUsed =
                CreateFieldSetting(entity.FindByName(ImplementationField.ImplementationPlanUsed));

            var deviation = CreateFieldSetting(entity.FindByName(ImplementationField.Deviation));
            var recoveryPlanUsed = CreateFieldSetting(entity.FindByName(ImplementationField.RecoveryPlanUsed));
            var attachedFiles = CreateFieldSetting(entity.FindByName(ImplementationField.AttachedFiles));
            var logs = CreateFieldSetting(entity.FindByName(ImplementationField.Logs));
            var implementationReady = CreateFieldSetting(entity.FindByName(ImplementationField.ImplementationReady));

            return new ImplementationProcessingSettings(
                status,
                realStartDate,
                finishingDate,
                buildImplemented,
                implementationPlanUsed,
                deviation,
                recoveryPlanUsed,
                attachedFiles,
                logs,
                implementationReady);
        }

        private static EvaluationProcessingSettings CreateEvaluationSettings(
            NamedObjectCollection<FieldProcessingSettingMapperData> entity)
        {
            var changeEvaluation = CreateFieldSetting(entity.FindByName(EvaluationField.ChangeEvaluation));
            var attachedFiles = CreateFieldSetting(entity.FindByName(EvaluationField.AttachedFiles));
            var logs = CreateFieldSetting(entity.FindByName(EvaluationField.Logs));
            var evaluationReady = CreateFieldSetting(entity.FindByName(EvaluationField.EvaluationReady));

            return new EvaluationProcessingSettings(changeEvaluation, attachedFiles, logs, evaluationReady);
        }

        private static FieldProcessingSetting CreateFieldSetting(FieldProcessingSettingMapperData data)
        {
            return new FieldProcessingSetting(data.Show.ToBool(), data.Required.ToBool());
        }
    }
}
