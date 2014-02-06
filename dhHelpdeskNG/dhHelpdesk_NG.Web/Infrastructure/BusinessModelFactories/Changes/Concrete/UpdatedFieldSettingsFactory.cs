namespace dhHelpdesk_NG.Web.Infrastructure.BusinessModelFactories.Changes.Concrete
{
    using System;

    using dhHelpdesk_NG.DTO.DTOs.Changes.Input.Settings;
    using dhHelpdesk_NG.Web.Models.Changes;
    using dhHelpdesk_NG.Web.Models.Changes.Settings;

    public sealed class UpdatedFieldSettingsFactory : IUpdatedFieldSettingsFactory
    {
        public UpdatedFieldSettingsDto Create(SettingsModel settings, int customerId, int languageId, DateTime changedDateTime)
        {
            var orderedFieldSettingGroup = CreateOrderedFieldSettingGroup(settings.Ordered, changedDateTime);
            var generalFieldSettingGroup = CreateGeneralFieldSettingGroup(settings.General, changedDateTime);
            var registrationFieldSettingGroup = CreateRegistrationFieldSettingGroup(settings.Registration, changedDateTime);
            var analyzeFieldSettingGroup = CreateAnalyzeFieldSettingGroup(settings.Analyze, changedDateTime);
            var implementationFieldSettingGroup = CreateImplementationFieldSettingGroup(settings.Implementation, changedDateTime);
            var evaluationFieldSettingGroup = CreateEvaluationFieldSettingGroup(settings.Evaluation, changedDateTime);
            var logFieldSettingGroup = CreateLogFieldSettingGroup(settings.Log, changedDateTime);

            return new UpdatedFieldSettingsDto(
                customerId, 
                languageId,
                orderedFieldSettingGroup, 
                generalFieldSettingGroup, 
                registrationFieldSettingGroup, 
                analyzeFieldSettingGroup, 
                implementationFieldSettingGroup, 
                evaluationFieldSettingGroup, 
                logFieldSettingGroup);
        }

        private static UpdatedOrdererFieldSettingGroupDto CreateOrderedFieldSettingGroup(OrderedFieldSettingsModel settingGroup, DateTime changedDateTime)
        {
            var id = CreateFieldSetting(settingGroup.Id, changedDateTime);
            var name = CreateFieldSetting(settingGroup.Name, changedDateTime);
            var phone = CreateFieldSetting(settingGroup.Phone, changedDateTime);
            var cellPhone = CreateFieldSetting(settingGroup.CellPhone, changedDateTime);
            var email = CreateFieldSetting(settingGroup.Email, changedDateTime);
            var department = CreateFieldSetting(settingGroup.Department, changedDateTime);

            return new UpdatedOrdererFieldSettingGroupDto(id, name, phone, cellPhone, email, department);
        }

        private static UpdatedGeneralFieldSettingGroupDto CreateGeneralFieldSettingGroup(GeneralFieldSettingsModel settingGroup, DateTime changedDateTime)
        {
            var priority = CreateFieldSetting(settingGroup.Priority, changedDateTime);
            var title = CreateFieldSetting(settingGroup.Title, changedDateTime);
            var state = CreateFieldSetting(settingGroup.State, changedDateTime);
            var system = CreateFieldSetting(settingGroup.System, changedDateTime);
            var @object = CreateFieldSetting(settingGroup.Object, changedDateTime);
            var inventory = CreateFieldSetting(settingGroup.Inventory, changedDateTime);
            var owner = CreateFieldSetting(settingGroup.Owner, changedDateTime);
            var workingGroup = CreateFieldSetting(settingGroup.WorkingGroup, changedDateTime);
            var administrator = CreateFieldSetting(settingGroup.Administrator, changedDateTime);
            var finishingDate = CreateFieldSetting(settingGroup.FinishingDate, changedDateTime);
            var rss = CreateFieldSetting(settingGroup.Rss, changedDateTime);

            return new UpdatedGeneralFieldSettingGroupDto(
                priority,
                title,
                state,
                system,
                @object,
                inventory,
                owner,
                workingGroup,
                administrator,
                finishingDate,
                rss);
        }

        private static UpdatedRegistrationFieldSettingGroupDto CreateRegistrationFieldSettingGroup(
            RegistrationFieldSettingsModel settingGroup, DateTime changedDateTime)
        {
            var name = CreateFieldSetting(settingGroup.Name, changedDateTime);
            var phone = CreateFieldSetting(settingGroup.Phone, changedDateTime);
            var email = CreateFieldSetting(settingGroup.Email, changedDateTime);
            var company = CreateFieldSetting(settingGroup.Company, changedDateTime);
            var processAffected = CreateFieldSetting(settingGroup.ProcessAffected, changedDateTime);
            var departmentAffected = CreateFieldSetting(settingGroup.DepartmentAffected, changedDateTime);
            var description = CreateFieldSetting(settingGroup.Description, changedDateTime);
            var businessBenefits = CreateFieldSetting(settingGroup.BusinessBenefits, changedDateTime);
            var consequence = CreateFieldSetting(settingGroup.Consequence, changedDateTime);
            var impact = CreateFieldSetting(settingGroup.Impact, changedDateTime);
            var desiredDate = CreateFieldSetting(settingGroup.DesiredDate, changedDateTime);
            var verified = CreateFieldSetting(settingGroup.Verified, changedDateTime);
            var attachedFile = CreateFieldSetting(settingGroup.AttachedFile, changedDateTime);
            var approval = CreateFieldSetting(settingGroup.Approval, changedDateTime);
            var explanation = CreateFieldSetting(settingGroup.Explanation, changedDateTime);

            return new UpdatedRegistrationFieldSettingGroupDto(
                name,
                phone,
                email,
                company,
                processAffected,
                departmentAffected,
                description,
                businessBenefits,
                consequence,
                impact,
                desiredDate,
                verified,
                attachedFile,
                approval,
                explanation);
        }

        private static UpdatedAnalyzeFieldSettingGroupDto CreateAnalyzeFieldSettingGroup(AnalyzeFieldSettingsModel settingGroup, DateTime changedDateTime)
        {
            var category = CreateFieldSetting(settingGroup.Category, changedDateTime);
            var priority = CreateFieldSetting(settingGroup.Priority, changedDateTime);
            var responsible = CreateFieldSetting(settingGroup.Responsible, changedDateTime);
            var solution = CreateFieldSetting(settingGroup.Solution, changedDateTime);
            var cost = CreateFieldSetting(settingGroup.Cost, changedDateTime);
            var yearlyCost = CreateFieldSetting(settingGroup.YearlyCost, changedDateTime);
            var timeEstimatesHours = CreateFieldSetting(settingGroup.TimeEstimatesHours, changedDateTime);
            var risk = CreateFieldSetting(settingGroup.Risk, changedDateTime);
            var startDate = CreateFieldSetting(settingGroup.StartDate, changedDateTime);
            var finishDate = CreateFieldSetting(settingGroup.FinishDate, changedDateTime);
            var implementationPlan = CreateFieldSetting(settingGroup.ImplementationPlan, changedDateTime);
            var recoveryPlan = CreateFieldSetting(settingGroup.RecoveryPlan, changedDateTime);
            var recommendation = CreateFieldSetting(settingGroup.Recommendation, changedDateTime);
            var attachedFile = CreateFieldSetting(settingGroup.AttachedFile, changedDateTime);
            var log = CreateFieldSetting(settingGroup.Log, changedDateTime);
            var approval = CreateFieldSetting(settingGroup.Approval, changedDateTime);

            return new UpdatedAnalyzeFieldSettingGroupDto(
                category,
                priority,
                responsible,
                solution,
                cost,
                yearlyCost,
                timeEstimatesHours,
                risk,
                startDate,
                finishDate,
                implementationPlan,
                recoveryPlan,
                recommendation,
                attachedFile,
                log,
                approval);
        }

        private static UpdatedImplementationFieldSettingGroupDto CreateImplementationFieldSettingGroup(
            ImplementationFieldSettingsModel settingGroup, DateTime changedDateTime)
        {
            var state = CreateFieldSetting(settingGroup.State, changedDateTime);
            var realStartDate = CreateFieldSetting(settingGroup.RealStartDate, changedDateTime);
            var buildAndTextImplemented = CreateFieldSetting(settingGroup.BuildAndTextImplemented, changedDateTime);
            var implementationPlanUsed = CreateFieldSetting(settingGroup.ImplementationPlanUsed, changedDateTime);
            var deviation = CreateFieldSetting(settingGroup.Deviation, changedDateTime);
            var recoveryPlanUsed = CreateFieldSetting(settingGroup.RecoveryPlanUsed, changedDateTime);
            var finisingDate = CreateFieldSetting(settingGroup.FinishingDate, changedDateTime);
            var attachedFile = CreateFieldSetting(settingGroup.AttachedFile, changedDateTime);
            var log = CreateFieldSetting(settingGroup.Log, changedDateTime);
            var implementationReady = CreateFieldSetting(settingGroup.ImplementationReady, changedDateTime);

            return new UpdatedImplementationFieldSettingGroupDto(
                state,
                realStartDate,
                buildAndTextImplemented,
                implementationPlanUsed,
                deviation,
                recoveryPlanUsed,
                finisingDate,
                attachedFile,
                log,
                implementationReady);
        }

        private static UpdatedEvaluationFieldSettingGroupDto CreateEvaluationFieldSettingGroup(
            EvaluationFieldSettingsModel settingGroup, DateTime changedDateTime)
        {
            var evaluation = CreateFieldSetting(settingGroup.Evaluation, changedDateTime);
            var attachedFile = CreateFieldSetting(settingGroup.AttachedFile, changedDateTime);
            var log = CreateFieldSetting(settingGroup.Log, changedDateTime);
            var evaluationReady = CreateFieldSetting(settingGroup.EvaluationReady, changedDateTime);

            return new UpdatedEvaluationFieldSettingGroupDto(evaluation, attachedFile, log, evaluationReady);
        }

        private static UpdatedLogFieldSettingGroupDto CreateLogFieldSettingGroup(LogFieldSettingsModel settingGroup, DateTime changedDateTime)
        {
            var log = CreateFieldSetting(settingGroup.Log, changedDateTime);
            return new UpdatedLogFieldSettingGroupDto(log);
        }

        private static UpdatedFieldSettingDto CreateFieldSetting(FieldSettingModel fieldSetting, DateTime changedDateTime)
        {
            return new UpdatedFieldSettingDto(
                fieldSetting.ShowInDetails,
                fieldSetting.ShowInChanges,
                fieldSetting.ShowInSelfService,
                fieldSetting.Caption,
                fieldSetting.Required,
                fieldSetting.Bookmark,
                changedDateTime);
        }

        private static UpdatedStringFieldSettingDto CreateFieldSetting(StringFieldSettingModel fieldSetting, DateTime changedDateTime)
        {
            return new UpdatedStringFieldSettingDto(
                fieldSetting.ShowInDetails,
                fieldSetting.ShowInChanges,
                fieldSetting.ShowInSelfService,
                fieldSetting.Caption,
                fieldSetting.Required,
                fieldSetting.DefaultValue,
                fieldSetting.Bookmark,
                changedDateTime);
        }
    }
}