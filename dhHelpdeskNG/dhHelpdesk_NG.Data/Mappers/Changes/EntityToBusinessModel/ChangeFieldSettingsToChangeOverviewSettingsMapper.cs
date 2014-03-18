namespace DH.Helpdesk.Dal.Mappers.Changes.EntityToBusinessModel
{
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeOverview;
    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.Common.Collections;
    using DH.Helpdesk.Common.Extensions.Integer;
    using DH.Helpdesk.Dal.Enums.Changes;
    using DH.Helpdesk.Dal.MapperData.Changes;

    public sealed class ChangeFieldSettingsToChangeOverviewSettingsMapper :
        IEntityToBusinessModelMapper<NamedObjectCollection<FieldOverviewSettingMapperData>, ChangeOverviewSettings>
    {
        public ChangeOverviewSettings Map(NamedObjectCollection<FieldOverviewSettingMapperData> entity)
        {
            var orderer = CreateOrdererSettings(entity);
            var general = CreateGeneralSettings(entity);
            var registration = CreateRegistrationSettings(entity);
            var analyze = CreateAnalyzeSettings(entity);
            var implementation = CreateImplementationSettings(entity);
            var evaluation = CreateEvaluationSettings(entity);

            return new ChangeOverviewSettings(orderer, general, registration, analyze, implementation, evaluation);
        }

        private static OrdererOverviewSettings CreateOrdererSettings(
            NamedObjectCollection<FieldOverviewSettingMapperData> fieldSettings)
        {
            var id = CreateFieldSetting(fieldSettings.FindByName(OrdererField.Id));
            var name = CreateFieldSetting(fieldSettings.FindByName(OrdererField.Name));
            var phone = CreateFieldSetting(fieldSettings.FindByName(OrdererField.Phone));
            var cellPhone = CreateFieldSetting(fieldSettings.FindByName(OrdererField.CellPhone));
            var email = CreateFieldSetting(fieldSettings.FindByName(OrdererField.Email));
            var department = CreateFieldSetting(fieldSettings.FindByName(OrdererField.Department));

            return new OrdererOverviewSettings(id, name, phone, cellPhone, email, department);
        }

        private static GeneralOverviewSettings CreateGeneralSettings(
            NamedObjectCollection<FieldOverviewSettingMapperData> fieldSettings)
        {
            var priority = CreateFieldSetting(fieldSettings.FindByName(GeneralField.Priority));
            var title = CreateFieldSetting(fieldSettings.FindByName(GeneralField.Title));
            var status = CreateFieldSetting(fieldSettings.FindByName(GeneralField.Status));
            var system = CreateFieldSetting(fieldSettings.FindByName(GeneralField.System));
            var @object = CreateFieldSetting(fieldSettings.FindByName(GeneralField.Object));
            var inventory = CreateFieldSetting(fieldSettings.FindByName(GeneralField.Inventory));
            var workingGroup = CreateFieldSetting(fieldSettings.FindByName(GeneralField.WorkingGroup));
            var administrator = CreateFieldSetting(fieldSettings.FindByName(GeneralField.Administrator));
            var finishingDate = CreateFieldSetting(fieldSettings.FindByName(GeneralField.FinishingDate));
            var rss = CreateFieldSetting(fieldSettings.FindByName(GeneralField.Rss));

            return new GeneralOverviewSettings(
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

        private static RegistrationOverviewSettings CreateRegistrationSettings(
            NamedObjectCollection<FieldOverviewSettingMapperData> fieldSettings)
        {
            var owner = CreateFieldSetting(fieldSettings.FindByName(RegistrationField.Owner));
            var description = CreateFieldSetting(fieldSettings.FindByName(RegistrationField.Description));
            var businessBenefits = CreateFieldSetting(fieldSettings.FindByName(RegistrationField.BusinessBenefits));
            var consequence = CreateFieldSetting(fieldSettings.FindByName(RegistrationField.Consequence));
            var impact = CreateFieldSetting(fieldSettings.FindByName(RegistrationField.Impact));
            var desiredDate = CreateFieldSetting(fieldSettings.FindByName(RegistrationField.DesiredDate));
            var verified = CreateFieldSetting(fieldSettings.FindByName(RegistrationField.Verified));
            var approval = CreateFieldSetting(fieldSettings.FindByName(RegistrationField.Approval));
            var rejectExplanation = CreateFieldSetting(fieldSettings.FindByName(RegistrationField.RejectExplanation));

            return new RegistrationOverviewSettings(
                owner,
                description,
                businessBenefits,
                consequence,
                impact,
                desiredDate,
                verified,
                approval,
                rejectExplanation);
        }

        private static AnalyzeOverviewSettings CreateAnalyzeSettings(
            NamedObjectCollection<FieldOverviewSettingMapperData> fieldSettings)
        {
            var category = CreateFieldSetting(fieldSettings.FindByName(AnalyzeField.Category));
            var priority = CreateFieldSetting(fieldSettings.FindByName(AnalyzeField.Priority));
            var responsible = CreateFieldSetting(fieldSettings.FindByName(AnalyzeField.Responsible));
            var solution = CreateFieldSetting(fieldSettings.FindByName(AnalyzeField.Solution));
            var cost = CreateFieldSetting(fieldSettings.FindByName(AnalyzeField.Cost));
            var yearlyCost = CreateFieldSetting(fieldSettings.FindByName(AnalyzeField.YearlyCost));
            var estimatedTimeInHours = CreateFieldSetting(fieldSettings.FindByName(AnalyzeField.EstimatedTimeInHours));
            var risk = CreateFieldSetting(fieldSettings.FindByName(AnalyzeField.Risk));
            var startDate = CreateFieldSetting(fieldSettings.FindByName(AnalyzeField.StartDate));
            var finishDate = CreateFieldSetting(fieldSettings.FindByName(AnalyzeField.FinishDate));
            var hasImplementationPlan = CreateFieldSetting(fieldSettings.FindByName(AnalyzeField.HasImplementationPlan));
            var hasRecoveryPlan = CreateFieldSetting(fieldSettings.FindByName(AnalyzeField.HasRecoveryPlan));
            var approval = CreateFieldSetting(fieldSettings.FindByName(AnalyzeField.Approval));
            var rejectExplanation = CreateFieldSetting(fieldSettings.FindByName(AnalyzeField.RejectExplanation));

            return new AnalyzeOverviewSettings(
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
                approval,
                rejectExplanation);
        }

        private static ImplementationOverviewSettings CreateImplementationSettings(
            NamedObjectCollection<FieldOverviewSettingMapperData> fieldSettings)
        {
            var status = CreateFieldSetting(fieldSettings.FindByName(ImplementationField.Status));
            var realStartDate = CreateFieldSetting(fieldSettings.FindByName(ImplementationField.RealStartDate));
            var buildImplemented = CreateFieldSetting(fieldSettings.FindByName(ImplementationField.BuildImplemented));

            var implementationPlanUsed =
                CreateFieldSetting(fieldSettings.FindByName(ImplementationField.ImplementationPlanUsed));

            var deviation = CreateFieldSetting(fieldSettings.FindByName(ImplementationField.Deviation));
            var recoveryPlanUsed = CreateFieldSetting(fieldSettings.FindByName(ImplementationField.RecoveryPlanUsed));
            var finishingDate = CreateFieldSetting(fieldSettings.FindByName(ImplementationField.FinishingDate));

            var implementationReady =
                CreateFieldSetting(fieldSettings.FindByName(ImplementationField.ImplementationReady));

            return new ImplementationOverviewSettings(
                status,
                realStartDate,
                buildImplemented,
                implementationPlanUsed,
                deviation,
                recoveryPlanUsed,
                finishingDate,
                implementationReady);
        }

        private static EvaluationOverviewSettings CreateEvaluationSettings(
            NamedObjectCollection<FieldOverviewSettingMapperData> fieldSettings)
        {
            var changeEvaluation = CreateFieldSetting(fieldSettings.FindByName(EvaluationField.ChangeEvaluation));
            var evaluationReady = CreateFieldSetting(fieldSettings.FindByName(EvaluationField.EvaluationReady));

            return new EvaluationOverviewSettings(changeEvaluation, evaluationReady);
        }

        private static FieldOverviewSetting CreateFieldSetting(FieldOverviewSettingMapperData fieldSetting)
        {
            return new FieldOverviewSetting(fieldSetting.Show.ToBool(), fieldSetting.Caption);
        }
    }
}