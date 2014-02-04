namespace dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Changes.Concrete
{
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings;
    using dhHelpdesk_NG.Web.Models.Changes;

    public sealed class SettingsModelFactory : ISettingsModelFactory
    {
        public SettingsModel Create(FieldSettingsDto fieldSettings)
        {
            var ordererFieldSettingGroup = CreateOrdererFieldSettingGroup(fieldSettings.Ordered);
            var generalFieldSettingGroup = CreateGeneralFieldSettingGroup(fieldSettings.General);
            var registrationFieldSettingGroup = CreateRegistrationFieldSettingGroup(fieldSettings.Registration);
            var analyzeFieldSettingGroup = CreateAnalyzeFieldSettingGroup(fieldSettings.Analyze);
            var implementationFieldSettingGroup = CreateImplementationFieldSettingGroup(fieldSettings.Implementation);
            var evaluationFieldSettingGroup = CreateEvaluationFieldSettingGroup(fieldSettings.Evaluation);
            var logFieldSettingGroup = CreateLogFieldSettingGroup(fieldSettings.Log);

            return new SettingsModel(
                ordererFieldSettingGroup,
                generalFieldSettingGroup,
                registrationFieldSettingGroup,
                analyzeFieldSettingGroup,
                implementationFieldSettingGroup,
                evaluationFieldSettingGroup,
                logFieldSettingGroup);
        }

        private static OrderedFieldSettingGroupModel CreateOrdererFieldSettingGroup(OrderedFieldSettingGroupDto fieldSettings)
        {
            var id = CreateFieldSettingModel(fieldSettings.Id);
            var name = CreateFieldSettingModel(fieldSettings.Name);
            var phone = CreateFieldSettingModel(fieldSettings.Phone);
            var cellPhone = CreateFieldSettingModel(fieldSettings.CellPhone);
            var email = CreateFieldSettingModel(fieldSettings.Email);
            var department = CreateFieldSettingModel(fieldSettings.Department);

            return new OrderedFieldSettingGroupModel(id, name, phone, cellPhone, email, department);
        }

        private static GeneralFieldSettingGroupModel CreateGeneralFieldSettingGroup(GeneralFieldSettingGroupDto fieldSettings)
        {
            var priority = CreateFieldSettingModel(fieldSettings.Priority);
            var title = CreateFieldSettingModel(fieldSettings.Title);
            var state = CreateFieldSettingModel(fieldSettings.State);
            var system = CreateFieldSettingModel(fieldSettings.System);
            var @object = CreateFieldSettingModel(fieldSettings.Object);
            var inventory = CreateFieldSettingModel(fieldSettings.Inventory);
            var owner = CreateFieldSettingModel(fieldSettings.Owner);
            var workingGroup = CreateFieldSettingModel(fieldSettings.WorkingGroup);
            var administrator = CreateFieldSettingModel(fieldSettings.Administrator);
            var finishingDate = CreateFieldSettingModel(fieldSettings.FinishingDate);
            var rss = CreateFieldSettingModel(fieldSettings.Rss);

            return new GeneralFieldSettingGroupModel(
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

        private static RegistrationFieldSettingGroupModel CreateRegistrationFieldSettingGroup(RegistrationFieldSettingGroupDto fieldSettings)
        {
            var name = CreateFieldSettingModel(fieldSettings.Name);
            var phone = CreateFieldSettingModel(fieldSettings.Phone);
            var email = CreateFieldSettingModel(fieldSettings.Email);
            var company = CreateFieldSettingModel(fieldSettings.Company);
            var processAffected = CreateFieldSettingModel(fieldSettings.ProcessAffected);
            var departmentAffected = CreateFieldSettingModel(fieldSettings.DepartmentAffected);
            var description = CreateStringFieldSettingModel(fieldSettings.Description);
            var businessBenefits = CreateStringFieldSettingModel(fieldSettings.BusinessBenefits);
            var consequence = CreateStringFieldSettingModel(fieldSettings.Consequence);
            var impact = CreateFieldSettingModel(fieldSettings.Impact);
            var desiredDate = CreateFieldSettingModel(fieldSettings.DesiredDate);
            var verified = CreateFieldSettingModel(fieldSettings.Verified);
            var attachedFile = CreateFieldSettingModel(fieldSettings.AttachedFile);
            var approval = CreateFieldSettingModel(fieldSettings.Approval);
            var explanation = CreateFieldSettingModel(fieldSettings.Explanation);

            return new RegistrationFieldSettingGroupModel(
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

        private static AnalyzeFieldSettingGroupModel CreateAnalyzeFieldSettingGroup(AnalyzeFieldSettingGroupDto fieldSettings)
        {
            var category = CreateFieldSettingModel(fieldSettings.Category);
            var priority = CreateFieldSettingModel(fieldSettings.Priority);
            var responsible = CreateFieldSettingModel(fieldSettings.Responsible);
            var solution = CreateStringFieldSettingModel(fieldSettings.Solution);
            var cost = CreateFieldSettingModel(fieldSettings.Cost);
            var yearlyCost = CreateFieldSettingModel(fieldSettings.YearlyCost);
            var timeEstimatesHours = CreateFieldSettingModel(fieldSettings.TimeEstimatesHours);
            var risk = CreateStringFieldSettingModel(fieldSettings.Risk);
            var startDate = CreateFieldSettingModel(fieldSettings.StartDate);
            var finishDate = CreateFieldSettingModel(fieldSettings.FinishDate);
            var implementationPlan = CreateFieldSettingModel(fieldSettings.ImplementationPlan);
            var recoveryPlan = CreateFieldSettingModel(fieldSettings.RecoveryPlan);
            var recommendation = CreateStringFieldSettingModel(fieldSettings.Recommendation);
            var attachedFile = CreateFieldSettingModel(fieldSettings.AttachedFile);
            var log = CreateFieldSettingModel(fieldSettings.Log);
            var approval = CreateFieldSettingModel(fieldSettings.Approval);

            return new AnalyzeFieldSettingGroupModel(
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

        private static ImplementationFieldSettingGroupModel CreateImplementationFieldSettingGroup(ImplementationFieldSettingGroupDto fieldSettings)
        {
            var state = CreateFieldSettingModel(fieldSettings.State);
            var realStartDate = CreateFieldSettingModel(fieldSettings.RealStartDate);
            var buildAndTextImplemented = CreateFieldSettingModel(fieldSettings.BuildAndTextImplemented);
            var implementationPlanUsed = CreateFieldSettingModel(fieldSettings.ImplementationPlanUsed);
            var deviation = CreateStringFieldSettingModel(fieldSettings.Deviation);
            var recoveryPlanUsed = CreateFieldSettingModel(fieldSettings.RecoveryPlanUsed);
            var finishingDate = CreateFieldSettingModel(fieldSettings.FinishingDate);
            var attachedFile = CreateFieldSettingModel(fieldSettings.AttachedFile);
            var log = CreateFieldSettingModel(fieldSettings.Log);
            var implementationReady = CreateFieldSettingModel(fieldSettings.ImplementationReady);

            return new ImplementationFieldSettingGroupModel(
                state,
                realStartDate,
                buildAndTextImplemented,
                implementationPlanUsed,
                deviation,
                recoveryPlanUsed,
                finishingDate,
                attachedFile,
                log,
                implementationReady);
        }

        private static EvaluationFieldSettingGroupModel CreateEvaluationFieldSettingGroup(
            EvaluationFieldSettingGroupDto fieldSettings)
        {
            var evaluation = CreateStringFieldSettingModel(fieldSettings.Evaluation);
            var attachedFile = CreateFieldSettingModel(fieldSettings.AttachedFile);
            var log = CreateFieldSettingModel(fieldSettings.Log);
            var evaluationReady = CreateFieldSettingModel(fieldSettings.EvaluationReady);

            return new EvaluationFieldSettingGroupModel(evaluation, attachedFile, log, evaluationReady);
        }

        private static LogFieldSettingGroupModel CreateLogFieldSettingGroup(LogFieldSettingGroupDto fieldSettings)
        {
            var log = CreateFieldSettingModel(fieldSettings.Log);
            return new LogFieldSettingGroupModel(log);
        }

        private static FieldSettingModel CreateFieldSettingModel(FieldSettingDto fieldSetting)
        {
            return new FieldSettingModel(
                fieldSetting.ShowInDetails,
                fieldSetting.ShowInChanges,
                fieldSetting.ShowInSelfService,
                fieldSetting.Caption,
                fieldSetting.Required,
                fieldSetting.Bookmark);
        }

        private static StringFieldSettingModel CreateStringFieldSettingModel(StringFieldSettingDto fieldSetting)
        {
            return new StringFieldSettingModel(
                fieldSetting.ShowInDetails,
                fieldSetting.ShowInChanges,
                fieldSetting.ShowInSelfService,
                fieldSetting.Caption,
                fieldSetting.Required,
                fieldSetting.DefaultValue,
                fieldSetting.Bookmark);
        }
    }
}