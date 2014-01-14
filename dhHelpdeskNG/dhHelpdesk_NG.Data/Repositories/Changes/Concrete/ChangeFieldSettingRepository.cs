namespace dhHelpdesk_NG.Data.Repositories.Changes.Concrete
{
    using System;
    using System.Linq;

    using dhHelpdesk_NG.DTO.DTOs.Changes.Input;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings;
    using dhHelpdesk_NG.Data.Enums.Changes;
    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Data.Infrastructure.Collections.Changes;
    using dhHelpdesk_NG.Domain.Changes;

    public sealed class ChangeFieldSettingRepository : RepositoryBase<ChangeFieldSettings>, IChangeFieldSettingRepository
    {
        public ChangeFieldSettingRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public FieldSettingsDto FindByCustomerIdAndLanguageId(int customerId, int languageId)
        {
            var fieldSettings = this.DataContext.ChangeFieldSettings.Where(s => s.Customer_Id == customerId);
            var fieldSettingCollection = new FieldSettingCollection(fieldSettings);

            var orderedGroup = CreateOrderedFieldSettingGroup(fieldSettingCollection);
            var generalGroup = CreateGeneralFieldSettingGroup(fieldSettingCollection);
            var registrationGroup = CreateRegistrationFieldSettingGroup(fieldSettingCollection);
            var analyzeGroup = CreateAnalyzeFieldSettingGroup(fieldSettingCollection);
            var implementationGroup = CreateImplementationFieldSettingGroup(fieldSettingCollection);
            var evaluationGroup = CreateEvaluationFieldSettingGroup(fieldSettingCollection);
            var logGroup = CreateLogFieldSettingGroup(fieldSettingCollection);

            return new FieldSettingsDto(
                orderedGroup,
                generalGroup,
                registrationGroup,
                analyzeGroup,
                implementationGroup,
                evaluationGroup,
                logGroup);
        }

        public void UpdateSettings(UpdatedFieldSettingsDto updatedSettings)
        {
            var fieldSettings =
                this.DataContext.ChangeFieldSettings.Where(s => s.Customer_Id == updatedSettings.CustomerId);

            var fieldSettingCollection = new FieldSettingCollection(fieldSettings);

            UpdateOrdererFieldSettingGroup(fieldSettingCollection, updatedSettings.OrdererFieldSettingGroup);
            UpdateGeneralFieldSettingGroup(fieldSettingCollection, updatedSettings.GeneralFieldSettingGroup);
            UpdateRegistrationFieldSettingGroup(fieldSettingCollection, updatedSettings.RegistrationFieldSettingGroup);
            UpdateAnalyzeFieldSettingGroup(fieldSettingCollection, updatedSettings.AnalyzeFieldSettingGroup);

            UpdateImplementationFieldSettingGroup(
                fieldSettingCollection, updatedSettings.ImplementationFieldSettingGroup);

            UpdateEvaluationFieldSettingGroup(fieldSettingCollection, updatedSettings.EvaluationFieldSettingGroup);
            UpdateLogFieldSettingGroup(fieldSettingCollection, updatedSettings.LogFieldSettingGroup);
        }

        private static void UpdateOrdererFieldSettingGroup(
            FieldSettingCollection existingSettings, UpdatedOrdererFieldSettingGroupDto updatedSettings)
        {
            var id = existingSettings.FindByName(OrdererFieldName.Id);
            UpdateFieldSetting(id, updatedSettings.Id);

            var name = existingSettings.FindByName(OrdererFieldName.Name);
            UpdateFieldSetting(name, updatedSettings.Name);

            var phone = existingSettings.FindByName(OrdererFieldName.Phone);
            UpdateFieldSetting(phone, updatedSettings.Phone);

            var cellPhone = existingSettings.FindByName(OrdererFieldName.CellPhone);
            UpdateFieldSetting(cellPhone, updatedSettings.CellPhone);

            var email = existingSettings.FindByName(OrdererFieldName.Email);
            UpdateFieldSetting(email, updatedSettings.Email);

            var department = existingSettings.FindByName(OrdererFieldName.Department);
            UpdateFieldSetting(department, updatedSettings.Department);
        }

        private static void UpdateGeneralFieldSettingGroup(
            FieldSettingCollection existingSettings, UpdatedGeneralFieldSettingGroupDto updatedSettings)
        {
            var priority = existingSettings.FindByName(GeneralFieldName.Priority);
            UpdateFieldSetting(priority, updatedSettings.Priority);

            var title = existingSettings.FindByName(GeneralFieldName.Title);
            UpdateFieldSetting(title, updatedSettings.Title);

            var state = existingSettings.FindByName(GeneralFieldName.State);
            UpdateFieldSetting(state, updatedSettings.State);

            var system = existingSettings.FindByName(GeneralFieldName.System);
            UpdateFieldSetting(system, updatedSettings.System);

            var @object = existingSettings.FindByName(GeneralFieldName.Object);
            UpdateFieldSetting(@object, updatedSettings.Object);

            var inventory = existingSettings.FindByName(GeneralFieldName.Inventory);
            UpdateFieldSetting(inventory, updatedSettings.Inventory);

            var owner = existingSettings.FindByName(GeneralFieldName.Owner);
            UpdateFieldSetting(owner, updatedSettings.Owner);

            var workingGroup = existingSettings.FindByName(GeneralFieldName.WorkingGroup);
            UpdateFieldSetting(workingGroup, updatedSettings.WorkingGroup);

            var administrator = existingSettings.FindByName(GeneralFieldName.Administrator);
            UpdateFieldSetting(administrator, updatedSettings.Administrator);

            var finishingDate = existingSettings.FindByName(GeneralFieldName.FinishingDate);
            UpdateFieldSetting(finishingDate, updatedSettings.FinishingDate);

            var rss = existingSettings.FindByName(GeneralFieldName.Rss);
            UpdateFieldSetting(rss, updatedSettings.Rss);
        }

        private static void UpdateRegistrationFieldSettingGroup(
            FieldSettingCollection existingSettings, UpdatedRegistrationFieldSettingGroupDto updatedSettings)
        {
            var name = existingSettings.FindByName(RegistrationFieldName.Name);
            UpdateFieldSetting(name, updatedSettings.Name);

            var phone = existingSettings.FindByName(RegistrationFieldName.Phone);
            UpdateFieldSetting(phone, updatedSettings.Phone);

            var email = existingSettings.FindByName(RegistrationFieldName.Email);
            UpdateFieldSetting(email, updatedSettings.Email);

            var company = existingSettings.FindByName(RegistrationFieldName.Company);
            UpdateFieldSetting(company, updatedSettings.Company);

            var processAffected = existingSettings.FindByName(RegistrationFieldName.ProcessAffected);
            UpdateFieldSetting(processAffected, updatedSettings.ProcessAffected);

            var departmentAffected = existingSettings.FindByName(RegistrationFieldName.DepartmentAffected);
            UpdateFieldSetting(departmentAffected, updatedSettings.DepartmentAffected);

            var description = existingSettings.FindByName(RegistrationFieldName.Description);
            UpdateStringFieldSetting(description, updatedSettings.Description);

            var businessBenefits = existingSettings.FindByName(RegistrationFieldName.BusinessBenefits);
            UpdateStringFieldSetting(businessBenefits, updatedSettings.BusinessBenefits);

            var consequence = existingSettings.FindByName(RegistrationFieldName.Consequence);
            UpdateStringFieldSetting(consequence, updatedSettings.Consequence);

            var impact = existingSettings.FindByName(RegistrationFieldName.Impact);
            UpdateFieldSetting(impact, updatedSettings.Impact);

            var desiredDate = existingSettings.FindByName(RegistrationFieldName.DesiredDate);
            UpdateFieldSetting(desiredDate, updatedSettings.DesiredDate);

            var verified = existingSettings.FindByName(RegistrationFieldName.Verified);
            UpdateFieldSetting(verified, updatedSettings.Verified);

            var attachedFile = existingSettings.FindByName(RegistrationFieldName.AttachedFile);
            UpdateFieldSetting(attachedFile, updatedSettings.AttachedFile);

            var approval = existingSettings.FindByName(RegistrationFieldName.Approval);
            UpdateFieldSetting(approval, updatedSettings.Approval);

            var explanation = existingSettings.FindByName(RegistrationFieldName.Explanation);
            UpdateFieldSetting(explanation, updatedSettings.Explanation);
        }

        private static void UpdateAnalyzeFieldSettingGroup(
            FieldSettingCollection existingSettings, UpdatedAnalyzeFieldSettingGroupDto updatedSettings)
        {
            var category = existingSettings.FindByName(AnalyzeFieldName.Category);
            UpdateFieldSetting(category, updatedSettings.Category);

            var priority = existingSettings.FindByName(AnalyzeFieldName.Priority);
            UpdateFieldSetting(priority, updatedSettings.Priority);

            var responsible = existingSettings.FindByName(AnalyzeFieldName.Responsible);
            UpdateFieldSetting(responsible, updatedSettings.Responsible);

            var solution = existingSettings.FindByName(AnalyzeFieldName.Solution);
            UpdateStringFieldSetting(solution, updatedSettings.Solution);

            var cost = existingSettings.FindByName(AnalyzeFieldName.Cost);
            UpdateFieldSetting(cost, updatedSettings.Cost);

            var yearlyCost = existingSettings.FindByName(AnalyzeFieldName.YearlyCost);
            UpdateFieldSetting(yearlyCost, updatedSettings.YearlyCost);

            var timeEstimatesHours = existingSettings.FindByName(AnalyzeFieldName.TimeEstimatesHours);
            UpdateFieldSetting(timeEstimatesHours, updatedSettings.TimeEstimatesHours);

            var risk = existingSettings.FindByName(AnalyzeFieldName.Risk);
            UpdateStringFieldSetting(risk, updatedSettings.Risk);

            var startDate = existingSettings.FindByName(AnalyzeFieldName.StartDate);
            UpdateFieldSetting(startDate, updatedSettings.StartDate);

            var finishDate = existingSettings.FindByName(AnalyzeFieldName.FinishDate);
            UpdateFieldSetting(finishDate, updatedSettings.FinishDate);

            var implementationPlan = existingSettings.FindByName(AnalyzeFieldName.ImplementationPlan);
            UpdateFieldSetting(implementationPlan, updatedSettings.ImplementationPlan);

            var recoveryPlan = existingSettings.FindByName(AnalyzeFieldName.RecoveryPlan);
            UpdateFieldSetting(recoveryPlan, updatedSettings.RecoveryPlan);

            var recommendation = existingSettings.FindByName(AnalyzeFieldName.Recommendation);
            UpdateStringFieldSetting(recommendation, updatedSettings.Recommendation);

            var attachedFile = existingSettings.FindByName(AnalyzeFieldName.AttachedFile);
            UpdateFieldSetting(attachedFile, updatedSettings.AttachedFile);

            var log = existingSettings.FindByName(AnalyzeFieldName.Log);
            UpdateFieldSetting(log, updatedSettings.Log);

            var approval = existingSettings.FindByName(AnalyzeFieldName.Approval);
            UpdateFieldSetting(approval, updatedSettings.Approval);
        }

        private static void UpdateImplementationFieldSettingGroup(
            FieldSettingCollection existingSettings, UpdatedImplementationFieldSettingGroupDto updatedSettings)
        {
            var state = existingSettings.FindByName(ImplementationFieldName.State);
            UpdateFieldSetting(state, updatedSettings.State);

            var realStartDate = existingSettings.FindByName(ImplementationFieldName.RealStartDate);
            UpdateFieldSetting(realStartDate, updatedSettings.RealStartDate);

            var buildAndTextImplemented = existingSettings.FindByName(ImplementationFieldName.BuildAndTextImplemented);
            UpdateFieldSetting(buildAndTextImplemented, updatedSettings.BuildAndTextImplemented);

            var implementationPlanUsed = existingSettings.FindByName(ImplementationFieldName.ImplementationPlanUsed);
            UpdateFieldSetting(implementationPlanUsed, updatedSettings.ImplementationPlanUsed);

            var deviation = existingSettings.FindByName(ImplementationFieldName.Deviation);
            UpdateStringFieldSetting(deviation, updatedSettings.Deviation);

            var recoveryPlanUsed = existingSettings.FindByName(ImplementationFieldName.RecoveryPlanUsed);
            UpdateFieldSetting(recoveryPlanUsed, updatedSettings.RecoveryPlanUsed);

            var finishingDate = existingSettings.FindByName(ImplementationFieldName.FinishingDate);
            UpdateFieldSetting(finishingDate, updatedSettings.FinishingDate);

            var attachedFile = existingSettings.FindByName(ImplementationFieldName.AttachedFile);
            UpdateFieldSetting(attachedFile, updatedSettings.AttachedFile);

            var log = existingSettings.FindByName(ImplementationFieldName.Log);
            UpdateFieldSetting(log, updatedSettings.Log);

            var implementationReady = existingSettings.FindByName(ImplementationFieldName.ImplementationReady);
            UpdateFieldSetting(implementationReady, updatedSettings.ImplementationReady);
        }

        private static void UpdateEvaluationFieldSettingGroup(
            FieldSettingCollection existingSettings, UpdatedEvaluationFieldSettingGroupDto updatedSettings)
        {
            var evaluation = existingSettings.FindByName(EvaluationFieldName.Evaluation);
            UpdateStringFieldSetting(evaluation, updatedSettings.Evaluation);

            var attachedFile = existingSettings.FindByName(EvaluationFieldName.AttachedFile);
            UpdateFieldSetting(attachedFile, updatedSettings.AttachedFile);

            var log = existingSettings.FindByName(EvaluationFieldName.Log);
            UpdateFieldSetting(log, updatedSettings.Log);

            var evaluationReady = existingSettings.FindByName(EvaluationFieldName.EvaluationReady);
            UpdateFieldSetting(evaluationReady, updatedSettings.EvaluationReady);
        }

        private static void UpdateLogFieldSettingGroup(FieldSettingCollection existingSettings, UpdatedLogFieldSettingGroupDto updatedSettings)
        {
            var log = existingSettings.FindByName(LogFieldName.Log);
            UpdateFieldSetting(log, updatedSettings.Log);
        }

        private static void UpdateFieldSetting(ChangeFieldSettings fieldSetting, UpdatedFieldSettingDto updatedSetting)
        {
            UpdateFieldSettingCore(
                fieldSetting,
                updatedSetting.Bookmark,
                updatedSetting.ChangedDateTime,
                null,
                updatedSetting.Required,
                updatedSetting.ShowInDetails,
                updatedSetting.ShowInSelfService,
                updatedSetting.ShowInChanges);
        }

        private static void UpdateStringFieldSetting(ChangeFieldSettings fieldSetting, UpdatedStringFieldSettingDto updatedSetting)
        {
            UpdateFieldSettingCore(
               fieldSetting,
               updatedSetting.Bookmark,
               updatedSetting.ChangedDateTime,
               updatedSetting.DefaultValue,
               updatedSetting.Required,
               updatedSetting.ShowInDetails,
               updatedSetting.ShowInSelfService,
               updatedSetting.ShowInChanges);
        }

        private static void UpdateFieldSettingCore(
            ChangeFieldSettings fieldSetting, 
            string bookmark, 
            DateTime changedDateTime,
            string defaultValue,
            bool required,
            bool showInDetails,
            bool showInSelfService,
            bool showInChanges)
        {
            fieldSetting.Bookmark = bookmark;
            fieldSetting.ChangedDate = changedDateTime;
            fieldSetting.InitialValue = defaultValue;
            fieldSetting.Required = required ? 1 : 0;
            fieldSetting.Show = showInDetails ? 1 : 0;
            fieldSetting.ShowExternal = showInSelfService ? 1 : 0;
            fieldSetting.ShowInList = showInChanges ? 1 : 0;
        }

        private static OrderedFieldSettingGroupDto CreateOrderedFieldSettingGroup(FieldSettingCollection fieldSettings)
        {
            var id = CreateFieldSetting(fieldSettings.FindByName(OrdererFieldName.Id));
            var name = CreateFieldSetting(fieldSettings.FindByName(OrdererFieldName.Name));
            var phone = CreateFieldSetting(fieldSettings.FindByName(OrdererFieldName.Phone));
            var cellPhone = CreateFieldSetting(fieldSettings.FindByName(OrdererFieldName.CellPhone));
            var email = CreateFieldSetting(fieldSettings.FindByName(OrdererFieldName.Email));
            var department = CreateFieldSetting(fieldSettings.FindByName(OrdererFieldName.Department));

            return new OrderedFieldSettingGroupDto(id, name, phone, cellPhone, email, department);
        }

        private static GeneralFieldSettingGroupDto CreateGeneralFieldSettingGroup(FieldSettingCollection fieldSettings)
        {
            var priority = CreateFieldSetting(fieldSettings.FindByName(GeneralFieldName.Priority));
            var title = CreateFieldSetting(fieldSettings.FindByName(GeneralFieldName.Title));
            var state = CreateFieldSetting(fieldSettings.FindByName(GeneralFieldName.State));
            var system = CreateFieldSetting(fieldSettings.FindByName(GeneralFieldName.System));
            var @object = CreateFieldSetting(fieldSettings.FindByName(GeneralFieldName.Object));
            var inventory = CreateFieldSetting(fieldSettings.FindByName(GeneralFieldName.Inventory));
            var owner = CreateFieldSetting(fieldSettings.FindByName(GeneralFieldName.Owner));
            var workingGroup = CreateFieldSetting(fieldSettings.FindByName(GeneralFieldName.WorkingGroup));
            var administrator = CreateFieldSetting(fieldSettings.FindByName(GeneralFieldName.Administrator));
            var finishingDate = CreateFieldSetting(fieldSettings.FindByName(GeneralFieldName.FinishingDate));
            var rss = CreateFieldSetting(fieldSettings.FindByName(GeneralFieldName.Rss));

            return new GeneralFieldSettingGroupDto(
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

        private static RegistrationFieldSettingGroupDto CreateRegistrationFieldSettingGroup(FieldSettingCollection fieldSettings)
        {
            var name = CreateFieldSetting(fieldSettings.FindByName(RegistrationFieldName.Name));
            var phone = CreateFieldSetting(fieldSettings.FindByName(RegistrationFieldName.Phone));
            var email = CreateFieldSetting(fieldSettings.FindByName(RegistrationFieldName.Email));
            var company = CreateFieldSetting(fieldSettings.FindByName(RegistrationFieldName.Company));
            var processAffected = CreateFieldSetting(fieldSettings.FindByName(RegistrationFieldName.ProcessAffected));

            var departmentAffected =
                CreateFieldSetting(fieldSettings.FindByName(RegistrationFieldName.DepartmentAffected));
            
            var description = CreateStringFieldSetting(fieldSettings.FindByName(RegistrationFieldName.Description));
            
            var businessBenefits =
                CreateStringFieldSetting(fieldSettings.FindByName(RegistrationFieldName.BusinessBenefits));
            
            var consequence = CreateStringFieldSetting(fieldSettings.FindByName(RegistrationFieldName.Consequence));
            var impact = CreateFieldSetting(fieldSettings.FindByName(RegistrationFieldName.Impact));
            var desiredDate = CreateFieldSetting(fieldSettings.FindByName(RegistrationFieldName.DesiredDate));
            var verified = CreateFieldSetting(fieldSettings.FindByName(RegistrationFieldName.Verified));
            var attachedFile = CreateFieldSetting(fieldSettings.FindByName(RegistrationFieldName.AttachedFile));
            var approval = CreateFieldSetting(fieldSettings.FindByName(RegistrationFieldName.Approval));
            var explanation = CreateFieldSetting(fieldSettings.FindByName(RegistrationFieldName.Explanation));

            return new RegistrationFieldSettingGroupDto(
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

        private static AnalyzeFieldSettingGroupDto CreateAnalyzeFieldSettingGroup(FieldSettingCollection fieldSettings)
        {
            var category = CreateFieldSetting(fieldSettings.FindByName(AnalyzeFieldName.Category));
            var priority = CreateFieldSetting(fieldSettings.FindByName(AnalyzeFieldName.Priority));
            var responsible = CreateFieldSetting(fieldSettings.FindByName(AnalyzeFieldName.Responsible));
            var solution = CreateStringFieldSetting(fieldSettings.FindByName(AnalyzeFieldName.Solution));
            var cost = CreateFieldSetting(fieldSettings.FindByName(AnalyzeFieldName.Cost));
            var yearlyCost = CreateFieldSetting(fieldSettings.FindByName(AnalyzeFieldName.YearlyCost));
            var timeEstimatesHours = CreateFieldSetting(fieldSettings.FindByName(AnalyzeFieldName.TimeEstimatesHours));
            var risk = CreateStringFieldSetting(fieldSettings.FindByName(AnalyzeFieldName.Risk));
            var startDate = CreateFieldSetting(fieldSettings.FindByName(AnalyzeFieldName.StartDate));
            var finishDate = CreateFieldSetting(fieldSettings.FindByName(AnalyzeFieldName.FinishDate));
            var implementationPlan = CreateFieldSetting(fieldSettings.FindByName(AnalyzeFieldName.ImplementationPlan));
            var recoveryPlan = CreateFieldSetting(fieldSettings.FindByName(AnalyzeFieldName.RecoveryPlan));
            var recommendation = CreateStringFieldSetting(fieldSettings.FindByName(AnalyzeFieldName.Recommendation));
            var attachedFile = CreateFieldSetting(fieldSettings.FindByName(AnalyzeFieldName.AttachedFile));
            var log = CreateFieldSetting(fieldSettings.FindByName(AnalyzeFieldName.Log));
            var approval = CreateFieldSetting(fieldSettings.FindByName(AnalyzeFieldName.Approval));

            return new AnalyzeFieldSettingGroupDto(
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

        private static ImplementationFieldSettingGroupDto CreateImplementationFieldSettingGroup(FieldSettingCollection fieldSettings)
        {
            var state = CreateFieldSetting(fieldSettings.FindByName(ImplementationFieldName.State));
            var realStartDate = CreateFieldSetting(fieldSettings.FindByName(ImplementationFieldName.RealStartDate));
            var buildAndTextImplemented = CreateFieldSetting(fieldSettings.FindByName(ImplementationFieldName.BuildAndTextImplemented));
            var implementationPlanUsed = CreateFieldSetting(fieldSettings.FindByName(ImplementationFieldName.ImplementationPlanUsed));
            var deviation = CreateStringFieldSetting(fieldSettings.FindByName(ImplementationFieldName.Deviation));
            var recoveryPlanUsed = CreateFieldSetting(fieldSettings.FindByName(ImplementationFieldName.RecoveryPlanUsed));
            var finishingDate = CreateFieldSetting(fieldSettings.FindByName(ImplementationFieldName.FinishingDate));
            var attachedFile = CreateFieldSetting(fieldSettings.FindByName(ImplementationFieldName.AttachedFile));
            var log = CreateFieldSetting(fieldSettings.FindByName(ImplementationFieldName.Log));
            var implementationReady = CreateFieldSetting(fieldSettings.FindByName(ImplementationFieldName.ImplementationReady));

            return new ImplementationFieldSettingGroupDto(
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

        private static EvaluationFieldSettingGroupDto CreateEvaluationFieldSettingGroup(FieldSettingCollection fieldSettings)
        {
            var evaluation = CreateStringFieldSetting(fieldSettings.FindByName(EvaluationFieldName.Evaluation));
            var attachedFile = CreateFieldSetting(fieldSettings.FindByName(EvaluationFieldName.AttachedFile));
            var log = CreateFieldSetting(fieldSettings.FindByName(EvaluationFieldName.Log));
            var evaluationReady = CreateFieldSetting(fieldSettings.FindByName(EvaluationFieldName.EvaluationReady));

            return new EvaluationFieldSettingGroupDto(
                evaluation,
                attachedFile,
                log,
                evaluationReady);
        }

        private static LogFieldSettingGroupDto CreateLogFieldSettingGroup(FieldSettingCollection fieldSettings)
        {
            var log = CreateFieldSetting(fieldSettings.FindByName(LogFieldName.Log));
            return new LogFieldSettingGroupDto(log);
        }

        private static StringFieldSettingDto CreateStringFieldSetting(ChangeFieldSettings fieldSetting)
        {
            return new StringFieldSettingDto(
                fieldSetting.ChangeField,
                fieldSetting.Show != 0,
                fieldSetting.ShowInList != 0,
                fieldSetting.ShowExternal != 0,
                "Dummy caption",
                fieldSetting.Required != 0,
                fieldSetting.InitialValue,
                fieldSetting.Bookmark);
        }

        private static FieldSettingDto CreateFieldSetting(ChangeFieldSettings fieldSetting)
        {
            return new FieldSettingDto(
                fieldSetting.ChangeField,
                fieldSetting.Show != 0,
                fieldSetting.ShowInList != 0,
                fieldSetting.ShowExternal != 0,
                "Dummy caption",
                fieldSetting.Required != 0,
                fieldSetting.Bookmark);
        }
    }
}
