namespace dhHelpdesk_NG.Web.Infrastructure.Converters.Changes
{
    using System;

    using dhHelpdesk_NG.DTO.DTOs.Changes.Input;
    using dhHelpdesk_NG.Web.Models.Changes;

    public sealed class SettingsModelToSettingsDtoConverter
    {
        public UpdatedFieldSettingsDto Convert(SettingsModel model, int customerId, int languageId, DateTime changedDateTime)
        {
            var orderedFieldSettingGroup = CreateOrderedFieldSettingGroup(model.Ordered, changedDateTime);
            var generalFieldSettingGroup = CreateUpdatedGeneralFieldSettingGroup(model.General, changedDateTime);
            var registrationFieldSettingGroup = CreateUpdatedRegistrationFieldSettingGroup(model.Registration, changedDateTime);
            var analyzeFieldSettingGroup = CreateUpdatedAnalyzeFieldSettingGroup(model.Analyze, changedDateTime);
            var implementationFieldSettingGroup = CreateUpdatedImplementationFieldSettingGroup(model.Implementation, changedDateTime);
            var evaluationFieldSettingGroup = CreateUpdatedEvaluationFieldSettingGroup(model.Evaluation, changedDateTime);
            var logFieldSettingGroup = CreateUpdatedLogFieldSettingGroup(model.Log, changedDateTime);

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

        private static UpdatedOrdererFieldSettingGroupDto CreateOrderedFieldSettingGroup(OrderedFieldSettingGroupModel model, DateTime changedDateTime)
        {
            var id = CreateFieldSetting(model.Id, changedDateTime);
            var name = CreateFieldSetting(model.Name, changedDateTime);
            var phone = CreateFieldSetting(model.Phone, changedDateTime);
            var cellPhone = CreateFieldSetting(model.CellPhone, changedDateTime);
            var email = CreateFieldSetting(model.Email, changedDateTime);
            var department = CreateFieldSetting(model.Department, changedDateTime);

            return new UpdatedOrdererFieldSettingGroupDto(id, name, phone, cellPhone, email, department);
        }

        private static UpdatedGeneralFieldSettingGroupDto CreateUpdatedGeneralFieldSettingGroup(GeneralFieldSettingGroupModel model, DateTime changedDateTime)
        {
            var priority = CreateFieldSetting(model.Priority, changedDateTime);
            var title = CreateFieldSetting(model.Title, changedDateTime);
            var state = CreateFieldSetting(model.State, changedDateTime);
            var system = CreateFieldSetting(model.System, changedDateTime);
            var @object = CreateFieldSetting(model.Object, changedDateTime);
            var inventory = CreateFieldSetting(model.Inventory, changedDateTime);
            var owner = CreateFieldSetting(model.Owner, changedDateTime);
            var workingGroup = CreateFieldSetting(model.WorkingGroup, changedDateTime);
            var administrator = CreateFieldSetting(model.Administrator, changedDateTime);
            var finishingDate = CreateFieldSetting(model.FinishingDate, changedDateTime);
            var rss = CreateFieldSetting(model.Rss, changedDateTime);

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

        private static UpdatedRegistrationFieldSettingGroupDto CreateUpdatedRegistrationFieldSettingGroup(
            RegistrationFieldSettingGroupModel model, DateTime changedDateTime)
        {
            var name = CreateFieldSetting(model.Name, changedDateTime);
            var phone = CreateFieldSetting(model.Phone, changedDateTime);
            var email = CreateFieldSetting(model.Email, changedDateTime);
            var company = CreateFieldSetting(model.Company, changedDateTime);
            var processAffected = CreateFieldSetting(model.ProcessAffected, changedDateTime);
            var departmentAffected = CreateFieldSetting(model.DepartmentAffected, changedDateTime);
            var description = CreateFieldSetting(model.Description, changedDateTime);
            var businessBenefits = CreateFieldSetting(model.BusinessBenefits, changedDateTime);
            var consequence = CreateFieldSetting(model.Consequence, changedDateTime);
            var impact = CreateFieldSetting(model.Impact, changedDateTime);
            var desiredDate = CreateFieldSetting(model.DesiredDate, changedDateTime);
            var verified = CreateFieldSetting(model.Verified, changedDateTime);
            var attachedFile = CreateFieldSetting(model.AttachedFile, changedDateTime);
            var approval = CreateFieldSetting(model.Approval, changedDateTime);
            var explanation = CreateFieldSetting(model.Explanation, changedDateTime);

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

        private static UpdatedAnalyzeFieldSettingGroupDto CreateUpdatedAnalyzeFieldSettingGroup(AnalyzeFieldSettingGroupModel model, DateTime changedDateTime)
        {
            var category = CreateFieldSetting(model.Category, changedDateTime);
            var priority = CreateFieldSetting(model.Priority, changedDateTime);
            var responsible = CreateFieldSetting(model.Responsible, changedDateTime);
            var solution = CreateFieldSetting(model.Solution, changedDateTime);
            var cost = CreateFieldSetting(model.Cost, changedDateTime);
            var yearlyCost = CreateFieldSetting(model.YearlyCost, changedDateTime);
            var timeEstimatesHours = CreateFieldSetting(model.TimeEstimatesHours, changedDateTime);
            var risk = CreateFieldSetting(model.Risk, changedDateTime);
            var startDate = CreateFieldSetting(model.StartDate, changedDateTime);
            var finishDate = CreateFieldSetting(model.FinishDate, changedDateTime);
            var implementationPlan = CreateFieldSetting(model.ImplementationPlan, changedDateTime);
            var recoveryPlan = CreateFieldSetting(model.RecoveryPlan, changedDateTime);
            var recommendation = CreateFieldSetting(model.Recommendation, changedDateTime);
            var attachedFile = CreateFieldSetting(model.AttachedFile, changedDateTime);
            var log = CreateFieldSetting(model.Log, changedDateTime);
            var approval = CreateFieldSetting(model.Approval, changedDateTime);

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

        private static UpdatedImplementationFieldSettingGroupDto CreateUpdatedImplementationFieldSettingGroup(
            ImplementationFieldSettingGroupModel model, DateTime changedDateTime)
        {
            var state = CreateFieldSetting(model.State, changedDateTime);
            var realStartDate = CreateFieldSetting(model.RealStartDate, changedDateTime);
            var buildAndTextImplemented = CreateFieldSetting(model.BuildAndTextImplemented, changedDateTime);
            var implementationPlanUsed = CreateFieldSetting(model.ImplementationPlanUsed, changedDateTime);
            var deviation = CreateFieldSetting(model.Deviation, changedDateTime);
            var recoveryPlanUsed = CreateFieldSetting(model.RecoveryPlanUsed, changedDateTime);
            var finisingDate = CreateFieldSetting(model.FinishingDate, changedDateTime);
            var attachedFile = CreateFieldSetting(model.AttachedFile, changedDateTime);
            var log = CreateFieldSetting(model.Log, changedDateTime);
            var implementationReady = CreateFieldSetting(model.ImplementationReady, changedDateTime);

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

        private static UpdatedEvaluationFieldSettingGroupDto CreateUpdatedEvaluationFieldSettingGroup(
            EvaluationFieldSettingGroupModel model, DateTime changedDateTime)
        {
            var evaluation = CreateFieldSetting(model.Evaluation, changedDateTime);
            var attachedFile = CreateFieldSetting(model.AttachedFile, changedDateTime);
            var log = CreateFieldSetting(model.Log, changedDateTime);
            var evaluationReady = CreateFieldSetting(model.EvaluationReady, changedDateTime);

            return new UpdatedEvaluationFieldSettingGroupDto(evaluation, attachedFile, log, evaluationReady);
        }

        private static UpdatedLogFieldSettingGroupDto CreateUpdatedLogFieldSettingGroup(LogFieldSettingGroupModel model, DateTime changedDateTime)
        {
            var log = CreateFieldSetting(model.Log, changedDateTime);
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