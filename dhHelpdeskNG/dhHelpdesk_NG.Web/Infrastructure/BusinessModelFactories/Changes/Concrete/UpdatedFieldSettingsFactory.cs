namespace DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Changes.Concrete
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Changes.Input.Settings;
    using DH.Helpdesk.Web.Models.Changes.Settings;

    public sealed class UpdatedFieldSettingsFactory : IUpdatedFieldSettingsFactory
    {
//        public UpdatedSettings Create(SettingsModel settings, int customerId, int languageId, DateTime changedDateTime)
//        {
//            var orderedFieldSettingGroup = CreateOrderedFieldSettingGroup(settings.Orderer, changedDateTime);
//            var generalFieldSettingGroup = CreateGeneralFieldSettingGroup(settings.General, changedDateTime);
//            var registrationFieldSettingGroup = CreateRegistrationFieldSettingGroup(settings.Registration, changedDateTime);
//            var analyzeFieldSettingGroup = CreateAnalyzeFieldSettingGroup(settings.Analyze, changedDateTime);
//            var implementationFieldSettingGroup = CreateImplementationFieldSettingGroup(settings.Implementation, changedDateTime);
//            var evaluationFieldSettingGroup = CreateEvaluationFieldSettingGroup(settings.Evaluation, changedDateTime);
//            var logFieldSettingGroup = CreateLogFieldSettingGroup(settings.Log, changedDateTime);
//
//            return new UpdatedSettings(
//                customerId, 
//                languageId,
//                orderedFieldSettingGroup, 
//                generalFieldSettingGroup, 
//                registrationFieldSettingGroup, 
//                analyzeFieldSettingGroup, 
//                implementationFieldSettingGroup, 
//                evaluationFieldSettingGroup, 
//                logFieldSettingGroup);
//        }
//
//        private static UpdatedOrdererFieldSettings CreateOrderedFieldSettingGroup(OrdererFieldSettingsModel settingGroup, DateTime changedDateTime)
//        {
//            var id = CreateFieldSetting(settingGroup.Id, changedDateTime);
//            var name = CreateFieldSetting(settingGroup.Name, changedDateTime);
//            var phone = CreateFieldSetting(settingGroup.Phone, changedDateTime);
//            var cellPhone = CreateFieldSetting(settingGroup.CellPhone, changedDateTime);
//            var email = CreateFieldSetting(settingGroup.Email, changedDateTime);
//            var department = CreateFieldSetting(settingGroup.Department, changedDateTime);
//
//            return new UpdatedOrdererFieldSettings(id, name, phone, cellPhone, email, department);
//        }
//
//        private static UpdatedGeneralFieldSettings CreateGeneralFieldSettingGroup(GeneralFieldSettingsModel settingGroup, DateTime changedDateTime)
//        {
//            var priority = CreateFieldSetting(settingGroup.Priority, changedDateTime);
//            var title = CreateFieldSetting(settingGroup.Title, changedDateTime);
//            var state = CreateFieldSetting(settingGroup.Status, changedDateTime);
//            var system = CreateFieldSetting(settingGroup.System, changedDateTime);
//            var @object = CreateFieldSetting(settingGroup.Object, changedDateTime);
//            var inventory = CreateFieldSetting(settingGroup.Inventory, changedDateTime);
//            var owner = CreateFieldSetting(settingGroup.Owner, changedDateTime);
//            var workingGroup = CreateFieldSetting(settingGroup.WorkingGroup, changedDateTime);
//            var administrator = CreateFieldSetting(settingGroup.Administrator, changedDateTime);
//            var finishingDate = CreateFieldSetting(settingGroup.FinishingDate, changedDateTime);
//            var rss = CreateFieldSetting(settingGroup.Rss, changedDateTime);
//
//            return new UpdatedGeneralFieldSettings(
//                priority,
//                title,
//                state,
//                system,
//                @object,
//                inventory,
//                owner,
//                workingGroup,
//                administrator,
//                finishingDate,
//                rss);
//        }
//
//        private static UpdatedRegistrationFieldSettings CreateRegistrationFieldSettingGroup(
//            RegistrationFieldSettingsModel settingGroup, DateTime changedDateTime)
//        {
//            var name = CreateFieldSetting(settingGroup.Name, changedDateTime);
//            var phone = CreateFieldSetting(settingGroup.Phone, changedDateTime);
//            var email = CreateFieldSetting(settingGroup.Email, changedDateTime);
//            var company = CreateFieldSetting(settingGroup.Company, changedDateTime);
//            var processAffected = CreateFieldSetting(settingGroup.AffectedProcesses, changedDateTime);
//            var departmentAffected = CreateFieldSetting(settingGroup.AffectedDepartments, changedDateTime);
//            var description = CreateFieldSetting(settingGroup.Description, changedDateTime);
//            var businessBenefits = CreateFieldSetting(settingGroup.BusinessBenefits, changedDateTime);
//            var consequence = CreateFieldSetting(settingGroup.Consequence, changedDateTime);
//            var impact = CreateFieldSetting(settingGroup.Impact, changedDateTime);
//            var desiredDate = CreateFieldSetting(settingGroup.DesiredDate, changedDateTime);
//            var verified = CreateFieldSetting(settingGroup.Verified, changedDateTime);
//            var attachedFile = CreateFieldSetting(settingGroup.AttachedFiles, changedDateTime);
//            var approval = CreateFieldSetting(settingGroup.Approval, changedDateTime);
//            var explanation = CreateFieldSetting(settingGroup.RejectExplanation, changedDateTime);
//
//            return new UpdatedRegistrationFieldSettings(
//                name,
//                phone,
//                email,
//                company,
//                processAffected,
//                departmentAffected,
//                description,
//                businessBenefits,
//                consequence,
//                impact,
//                desiredDate,
//                verified,
//                attachedFile,
//                approval,
//                explanation);
//        }
//
//        private static UpdatedAnalyzeFieldSettings CreateAnalyzeFieldSettingGroup(AnalyzeFieldSettingsModel settingGroup, DateTime changedDateTime)
//        {
//            var category = CreateFieldSetting(settingGroup.Category, changedDateTime);
//            var priority = CreateFieldSetting(settingGroup.Priority, changedDateTime);
//            var responsible = CreateFieldSetting(settingGroup.Responsible, changedDateTime);
//            var solution = CreateFieldSetting(settingGroup.Solution, changedDateTime);
//            var cost = CreateFieldSetting(settingGroup.Cost, changedDateTime);
//            var yearlyCost = CreateFieldSetting(settingGroup.YearlyCost, changedDateTime);
//            var timeEstimatesHours = CreateFieldSetting(settingGroup.EstimatedTimeInHours, changedDateTime);
//            var risk = CreateFieldSetting(settingGroup.Risk, changedDateTime);
//            var startDate = CreateFieldSetting(settingGroup.StartDate, changedDateTime);
//            var finishDate = CreateFieldSetting(settingGroup.FinishDate, changedDateTime);
//            var implementationPlan = CreateFieldSetting(settingGroup.HasImplementationPlan, changedDateTime);
//            var recoveryPlan = CreateFieldSetting(settingGroup.HasRecoveryPlan, changedDateTime);
//            var recommendation = CreateFieldSetting(settingGroup.RejectExplanation, changedDateTime);
//            var attachedFile = CreateFieldSetting(settingGroup.AttachedFiles, changedDateTime);
//            var log = CreateFieldSetting(settingGroup.Log, changedDateTime);
//            var approval = CreateFieldSetting(settingGroup.Approval, changedDateTime);
//
//            return new UpdatedAnalyzeFieldSettings(
//                category,
//                priority,
//                responsible,
//                solution,
//                cost,
//                yearlyCost,
//                timeEstimatesHours,
//                risk,
//                startDate,
//                finishDate,
//                implementationPlan,
//                recoveryPlan,
//                recommendation,
//                attachedFile,
//                log,
//                approval);
//        }
//
//        private static UpdatedImplementationFieldSettings CreateImplementationFieldSettingGroup(
//            ImplementationFieldSettingsModel settingGroup, DateTime changedDateTime)
//        {
//            var state = CreateFieldSetting(settingGroup.Status, changedDateTime);
//            var realStartDate = CreateFieldSetting(settingGroup.RealStartDate, changedDateTime);
//            var buildAndTextImplemented = CreateFieldSetting(settingGroup.BuildImplemented, changedDateTime);
//            var implementationPlanUsed = CreateFieldSetting(settingGroup.ImplementationPlanUsed, changedDateTime);
//            var deviation = CreateFieldSetting(settingGroup.Deviation, changedDateTime);
//            var recoveryPlanUsed = CreateFieldSetting(settingGroup.RecoveryPlanUsed, changedDateTime);
//            var finisingDate = CreateFieldSetting(settingGroup.FinishingDate, changedDateTime);
//            var attachedFile = CreateFieldSetting(settingGroup.AttachedFiles, changedDateTime);
//            var log = CreateFieldSetting(settingGroup.Logs, changedDateTime);
//            var implementationReady = CreateFieldSetting(settingGroup.ImplementationReady, changedDateTime);
//
//            return new UpdatedImplementationFieldSettings(
//                state,
//                realStartDate,
//                buildAndTextImplemented,
//                implementationPlanUsed,
//                deviation,
//                recoveryPlanUsed,
//                finisingDate,
//                attachedFile,
//                log,
//                implementationReady);
//        }
//
//        private static UpdatedEvaluationFieldSettings CreateEvaluationFieldSettingGroup(
//            EvaluationFieldSettingsModel settingGroup, DateTime changedDateTime)
//        {
//            var evaluation = CreateFieldSetting(settingGroup.ChangeEvaluation, changedDateTime);
//            var attachedFile = CreateFieldSetting(settingGroup.AttachedFiles, changedDateTime);
//            var log = CreateFieldSetting(settingGroup.Log, changedDateTime);
//            var evaluationReady = CreateFieldSetting(settingGroup.EvaluationReady, changedDateTime);
//
//            return new UpdatedEvaluationFieldSettings(evaluation, attachedFile, log, evaluationReady);
//        }
//
//        private static UpdatedLogFieldSettings CreateLogFieldSettingGroup(LogFieldSettingsModel settingGroup, DateTime changedDateTime)
//        {
//            var log = CreateFieldSetting(settingGroup.Logs, changedDateTime);
//            return new UpdatedLogFieldSettings(log);
//        }
//
//        private static UpdatedFieldSetting CreateFieldSetting(FieldSettingModel fieldSetting, DateTime changedDateTime)
//        {
//            return new UpdatedFieldSetting(
//                fieldSetting.ShowInDetails,
//                fieldSetting.ShowInChanges,
//                fieldSetting.ShowInSelfService,
//                fieldSetting.Caption,
//                fieldSetting.Required,
//                fieldSetting.Bookmark,
//                changedDateTime);
//        }
//
//        private static UpdatedTextFieldSetting CreateFieldSetting(StringFieldSettingModel fieldSetting, DateTime changedDateTime)
//        {
//            return new UpdatedTextFieldSetting(
//                fieldSetting.ShowInDetails,
//                fieldSetting.ShowInChanges,
//                fieldSetting.ShowInSelfService,
//                fieldSetting.Caption,
//                fieldSetting.Required,
//                fieldSetting.DefaultValue,
//                fieldSetting.Bookmark,
//                changedDateTime);
//        }
        public UpdatedSettings Create(SettingsModel settings, int customerId, int languageId, DateTime changedDateTime)
        {
            throw new NotImplementedException();
        }
    }
}