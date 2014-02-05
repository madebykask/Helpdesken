namespace dhHelpdesk_NG.Data.Repositories.Changes.Concrete
{
    using System;
    using System.Linq;

    using dhHelpdesk_NG.Common.Enums;
    using dhHelpdesk_NG.Data.Collections.Changes;
    using dhHelpdesk_NG.Data.Enums.Changes;
    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain.Changes;
    using dhHelpdesk_NG.DTO.DTOs;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Input.Settings;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings.ChangeEdit;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings.ChangesOverview;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings.SettingsEdit;

    public sealed class ChangeFieldSettingRepository : RepositoryBase<ChangeFieldSettingsEntity>,
        IChangeFieldSettingRepository
    {
        #region Constructors and Destructors

        public ChangeFieldSettingRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        #endregion

        #region Public Methods and Operators

        public ChangeEditSettings FindChangeEditSettings(int customerId, int languageId)
        {
            var fieldSettings = this.DataContext.ChangeFieldSettings.Where(s => s.Customer_Id == customerId).ToList();
            var fieldSettingCollection = new FieldSettingCollection(fieldSettings);

            var ordererSettings = CreateOrdererFieldEditSettings(fieldSettingCollection);
            var generalSettings = CreateGeneralFieldEditSettings(fieldSettingCollection);
            var registrationSettings = CreateRegistrationFieldEditSettings(fieldSettingCollection);
            var analyzeSettings = CreateAnalyzeFieldEditSettings(fieldSettingCollection);
            var implementationSettings = CreateImplementationFieldEditSettings(fieldSettingCollection);
            var evaluationSettings = CreateEvaluationFieldEditSettings(fieldSettingCollection);
            var logSettings = CreateLogFieldEditSettings(fieldSettingCollection);

            return new ChangeEditSettings(
                ordererSettings,
                generalSettings,
                registrationSettings,
                analyzeSettings,
                implementationSettings,
                evaluationSettings,
                logSettings);
        }

        public FieldSettings FindByCustomerIdAndLanguageId(int customerId, int languageId)
        {
            var fieldSettings = this.DataContext.ChangeFieldSettings.Where(s => s.Customer_Id == customerId).ToList();
            var fieldSettingCollection = new FieldSettingCollection(fieldSettings);

            var orderedGroup = CreateOrderedFieldSettingGroup(fieldSettingCollection);
            var generalGroup = CreateGeneralFieldSettingGroup(fieldSettingCollection);
            var registrationGroup = CreateRegistrationFieldSettingGroup(fieldSettingCollection);
            var analyzeGroup = CreateAnalyzeFieldSettingGroup(fieldSettingCollection);
            var implementationGroup = CreateImplementationFieldSettingGroup(fieldSettingCollection);
            var evaluationGroup = CreateEvaluationFieldSettingGroup(fieldSettingCollection);
            var logGroup = CreateLogFieldSettingGroup(fieldSettingCollection);

            return new FieldSettings(
                orderedGroup,
                generalGroup,
                registrationGroup,
                analyzeGroup,
                implementationGroup,
                evaluationGroup,
                logGroup);
        }

        public FieldOverviewSettings FindEnglishByCustomerId(int customerId)
        {
            return this.FindForSpecifiedLanguageByCustomerId(customerId, LanguageTextId.English);
        }

        public FieldOverviewSettings FindSwedishByCustomerId(int customerId)
        {
            return this.FindForSpecifiedLanguageByCustomerId(customerId, LanguageTextId.Swedish);
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
                fieldSettingCollection,
                updatedSettings.ImplementationFieldSettingGroup);

            UpdateEvaluationFieldSettingGroup(fieldSettingCollection, updatedSettings.EvaluationFieldSettingGroup);
            UpdateLogFieldSettingGroup(fieldSettingCollection, updatedSettings.LogFieldSettingGroup);
        }

        #endregion

        #region Methods

        private static AnalyzeFieldOverviewSettings CreateAnalyzeFieldOverviewSettingGroup(
            FieldSettingCollection fieldSettings,
            string languageTextId)
        {
            var category = CreateFieldOverviewSetting(
                fieldSettings.FindByName(AnalyzeFieldName.Category),
                languageTextId);
           
            var priority = CreateFieldOverviewSetting(
                fieldSettings.FindByName(AnalyzeFieldName.Priority),
                languageTextId);
          
            var responsible = CreateFieldOverviewSetting(
                fieldSettings.FindByName(AnalyzeFieldName.Responsible),
                languageTextId);
           
            var solution = CreateFieldOverviewSetting(
                fieldSettings.FindByName(AnalyzeFieldName.Solution),
                languageTextId);
           
            var cost = CreateFieldOverviewSetting(fieldSettings.FindByName(AnalyzeFieldName.Cost), languageTextId);
          
            var yearlyCost = CreateFieldOverviewSetting(
                fieldSettings.FindByName(AnalyzeFieldName.YearlyCost),
                languageTextId);
          
            var timeEstimatesHours =
                CreateFieldOverviewSetting(
                    fieldSettings.FindByName(AnalyzeFieldName.TimeEstimatesHours),
                    languageTextId);
           
            var risk = CreateFieldOverviewSetting(fieldSettings.FindByName(AnalyzeFieldName.Risk), languageTextId);
          
            var startDate = CreateFieldOverviewSetting(
                fieldSettings.FindByName(AnalyzeFieldName.StartDate),
                languageTextId);
           
            var finishDate = CreateFieldOverviewSetting(
                fieldSettings.FindByName(AnalyzeFieldName.FinishDate),
                languageTextId);
            
            var implementationPlan =
                CreateFieldOverviewSetting(
                    fieldSettings.FindByName(AnalyzeFieldName.ImplementationPlan),
                    languageTextId);
           
            var recoveryPlan = CreateFieldOverviewSetting(
                fieldSettings.FindByName(AnalyzeFieldName.RecoveryPlan),
                languageTextId);
           
            var recommendation = CreateFieldOverviewSetting(
                fieldSettings.FindByName(AnalyzeFieldName.Recommendation),
                languageTextId);
         
            var approval = CreateFieldOverviewSetting(
                fieldSettings.FindByName(AnalyzeFieldName.Approval),
                languageTextId);

            return new AnalyzeFieldOverviewSettings(
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
                approval);
        }

        private static AnalyzeFieldEditSettings CreateAnalyzeFieldEditSettings(FieldSettingCollection fieldSettings)
        {
            var category = CreateFieldEditSetting(fieldSettings.FindByName(AnalyzeFieldName.Category));
            var priority = CreateFieldEditSetting(fieldSettings.FindByName(AnalyzeFieldName.Priority));
            var responsible = CreateFieldEditSetting(fieldSettings.FindByName(AnalyzeFieldName.Responsible));
            var solution = CreateTextFieldEditSetting(fieldSettings.FindByName(AnalyzeFieldName.Solution));
            var cost = CreateFieldEditSetting(fieldSettings.FindByName(AnalyzeFieldName.Cost));
            var yearlyCost = CreateFieldEditSetting(fieldSettings.FindByName(AnalyzeFieldName.YearlyCost));
          
            var timeEstimatesHours =
                CreateFieldEditSetting(fieldSettings.FindByName(AnalyzeFieldName.TimeEstimatesHours));
          
            var risk = CreateTextFieldEditSetting(fieldSettings.FindByName(AnalyzeFieldName.Risk));
            var startDate = CreateFieldEditSetting(fieldSettings.FindByName(AnalyzeFieldName.StartDate));
            var finishDate = CreateFieldEditSetting(fieldSettings.FindByName(AnalyzeFieldName.FinishDate));
           
            var hasImplementationPlan =
                CreateFieldEditSetting(fieldSettings.FindByName(AnalyzeFieldName.ImplementationPlan));
           
            var hasRecoveryPlan = CreateFieldEditSetting(fieldSettings.FindByName(AnalyzeFieldName.RecoveryPlan));
            var recommendation = CreateTextFieldEditSetting(fieldSettings.FindByName(AnalyzeFieldName.Recommendation));
            var attachedFile = CreateFieldEditSetting(fieldSettings.FindByName(AnalyzeFieldName.AttachedFile));
            var logs = CreateFieldEditSetting(fieldSettings.FindByName(AnalyzeFieldName.Log));
            var approval = CreateFieldEditSetting(fieldSettings.FindByName(AnalyzeFieldName.Approval));

            return new AnalyzeFieldEditSettings(
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
                hasImplementationPlan,
                hasRecoveryPlan,
                recommendation,
                attachedFile,
                logs,
                approval);
        }

        private static AnalyzeFieldSettings CreateAnalyzeFieldSettingGroup(FieldSettingCollection fieldSettings)
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

            return new AnalyzeFieldSettings(
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

        private static EvaluationFieldOverviewSettings CreateEvaluationFieldOverviewSettingGroup(
            FieldSettingCollection fieldSettings,
            string languageTextId)
        {
            var evaluation = CreateFieldOverviewSetting(
                fieldSettings.FindByName(EvaluationFieldName.Evaluation),
                languageTextId);
         
            var evaluationReady =
                CreateFieldOverviewSetting(
                    fieldSettings.FindByName(EvaluationFieldName.EvaluationReady),
                    languageTextId);

            return new EvaluationFieldOverviewSettings(evaluation, evaluationReady);
        }

        private static EvaluationFieldEditSettings CreateEvaluationFieldEditSettings(
            FieldSettingCollection fieldSettings)
        {
            var evaluation = CreateTextFieldEditSetting(fieldSettings.FindByName(EvaluationFieldName.Evaluation));
            var attachedFiles = CreateFieldEditSetting(fieldSettings.FindByName(EvaluationFieldName.Evaluation));
            var logs = CreateFieldEditSetting(fieldSettings.FindByName(EvaluationFieldName.Evaluation));
            var evaluationReady = CreateFieldEditSetting(fieldSettings.FindByName(EvaluationFieldName.Evaluation));

            return new EvaluationFieldEditSettings(evaluation, attachedFiles, logs, evaluationReady);
        }

        private static EvaluationFieldSettings CreateEvaluationFieldSettingGroup(FieldSettingCollection fieldSettings)
        {
            var evaluation = CreateStringFieldSetting(fieldSettings.FindByName(EvaluationFieldName.Evaluation));
            var attachedFile = CreateFieldSetting(fieldSettings.FindByName(EvaluationFieldName.AttachedFile));
            var log = CreateFieldSetting(fieldSettings.FindByName(EvaluationFieldName.Log));
            var evaluationReady = CreateFieldSetting(fieldSettings.FindByName(EvaluationFieldName.EvaluationReady));

            return new EvaluationFieldSettings(evaluation, attachedFile, log, evaluationReady);
        }

        private static FieldOverviewSetting CreateFieldOverviewSetting(
            ChangeFieldSettingsEntity fieldSetting,
            string languageTextId)
        {
            switch (languageTextId)
            {
                case LanguageTextId.English:
                    return new FieldOverviewSetting(fieldSetting.ShowInList != 0, fieldSetting.Label_ENG);
                case LanguageTextId.Swedish:
                    return new FieldOverviewSetting(fieldSetting.ShowInList != 0, fieldSetting.Label);
                default:
                    throw new ArgumentOutOfRangeException("languageTextId", languageTextId);
            }
        }

        private static FieldSetting CreateFieldSetting(ChangeFieldSettingsEntity fieldSetting)
        {
            return new FieldSetting(
                fieldSetting.ChangeField,
                fieldSetting.Show != 0,
                fieldSetting.ShowInList != 0,
                fieldSetting.ShowExternal != 0,
                "Dummy caption",
                fieldSetting.Required != 0,
                fieldSetting.Bookmark);
        }

        private static FieldEditSetting CreateFieldEditSetting(ChangeFieldSettingsEntity fieldSetting)
        {
            return new FieldEditSetting(
                fieldSetting.Show != 0,
                "Dummy caption",
                fieldSetting.Required != 0,
                fieldSetting.Bookmark);
        }

        private static TextFieldEditSetting CreateTextFieldEditSetting(ChangeFieldSettingsEntity fieldSetting)
        {
            return new TextFieldEditSetting(
                fieldSetting.Show != 0,
                "Dummy caption",
                fieldSetting.Required != 0,
                fieldSetting.InitialValue,
                fieldSetting.Bookmark);
        }

        private static GeneralFieldOverviewSettings CreateGeneralFieldOverviewSettingGroup(
            FieldSettingCollection fieldSettings,
            string languageTextId)
        {
            var priority = CreateFieldOverviewSetting(
                fieldSettings.FindByName(GeneralFieldName.Priority),
                languageTextId);
          
            var title = CreateFieldOverviewSetting(fieldSettings.FindByName(GeneralFieldName.Title), languageTextId);
            var state = CreateFieldOverviewSetting(fieldSettings.FindByName(GeneralFieldName.State), languageTextId);
            var system = CreateFieldOverviewSetting(fieldSettings.FindByName(GeneralFieldName.System), languageTextId);
            var @object = CreateFieldOverviewSetting(fieldSettings.FindByName(GeneralFieldName.Object), languageTextId);
           
            var inventory = CreateFieldOverviewSetting(
                fieldSettings.FindByName(GeneralFieldName.Inventory),
                languageTextId);
           
            var owner = CreateFieldOverviewSetting(fieldSettings.FindByName(GeneralFieldName.Owner), languageTextId);
           
            var workingGroup = CreateFieldOverviewSetting(
                fieldSettings.FindByName(GeneralFieldName.WorkingGroup),
                languageTextId);
         
            var administrator = CreateFieldOverviewSetting(
                fieldSettings.FindByName(GeneralFieldName.Administrator),
                languageTextId);
           
            var finishingDate = CreateFieldOverviewSetting(
                fieldSettings.FindByName(GeneralFieldName.FinishingDate),
                languageTextId);
          
            var rss = CreateFieldOverviewSetting(fieldSettings.FindByName(GeneralFieldName.Rss), languageTextId);

            return new GeneralFieldOverviewSettings(
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

        private static GeneralFieldEditSettings CreateGeneralFieldEditSettings(FieldSettingCollection fieldSettings)
        {
            var priority = CreateFieldEditSetting(fieldSettings.FindByName(GeneralFieldName.Priority));
            var title = CreateFieldEditSetting(fieldSettings.FindByName(GeneralFieldName.Title));
            var state = CreateFieldEditSetting(fieldSettings.FindByName(GeneralFieldName.State));
            var system = CreateFieldEditSetting(fieldSettings.FindByName(GeneralFieldName.System));
            var @object = CreateFieldEditSetting(fieldSettings.FindByName(GeneralFieldName.Object));
            var inventory = CreateFieldEditSetting(fieldSettings.FindByName(GeneralFieldName.Inventory));
            var owner = CreateFieldEditSetting(fieldSettings.FindByName(GeneralFieldName.Owner));
            var workingGroup = CreateFieldEditSetting(fieldSettings.FindByName(GeneralFieldName.WorkingGroup));
            var administrator = CreateFieldEditSetting(fieldSettings.FindByName(GeneralFieldName.Administrator));
            var finishingDate = CreateFieldEditSetting(fieldSettings.FindByName(GeneralFieldName.FinishingDate));
            var rss = CreateFieldEditSetting(fieldSettings.FindByName(GeneralFieldName.Rss));

            return new GeneralFieldEditSettings(
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

        private static GeneralFieldSettings CreateGeneralFieldSettingGroup(FieldSettingCollection fieldSettings)
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

            return new GeneralFieldSettings(
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

        private static ImplementationFieldOverviewSettings CreateImplementationFieldOverviewSettingGroup(
            FieldSettingCollection fieldSettings,
            string languageTextId)
        {
            var state = CreateFieldOverviewSetting(
                fieldSettings.FindByName(ImplementationFieldName.State),
                languageTextId);
           
            var realStartDate =
                CreateFieldOverviewSetting(
                    fieldSettings.FindByName(ImplementationFieldName.RealStartDate),
                    languageTextId);
          
            var buildAndTextImplemented =
                CreateFieldOverviewSetting(
                    fieldSettings.FindByName(ImplementationFieldName.BuildAndTextImplemented),
                    languageTextId);
          
            var implementationPlanUsed =
                CreateFieldOverviewSetting(
                    fieldSettings.FindByName(ImplementationFieldName.ImplementationPlanUsed),
                    languageTextId);
          
            var deviation = CreateFieldOverviewSetting(
                fieldSettings.FindByName(ImplementationFieldName.Deviation),
                languageTextId);
           
            var recoveryPlanUsed =
                CreateFieldOverviewSetting(
                    fieldSettings.FindByName(ImplementationFieldName.RecoveryPlanUsed),
                    languageTextId);
          
            var finishingDate =
                CreateFieldOverviewSetting(
                    fieldSettings.FindByName(ImplementationFieldName.FinishingDate),
                    languageTextId);
            
            var implementationReady =
                CreateFieldOverviewSetting(
                    fieldSettings.FindByName(ImplementationFieldName.ImplementationReady),
                    languageTextId);

            return new ImplementationFieldOverviewSettings(
                state,
                realStartDate,
                buildAndTextImplemented,
                implementationPlanUsed,
                deviation,
                recoveryPlanUsed,
                finishingDate,
                implementationReady);
        }

        private static ImplementationFieldEditSettings CreateImplementationFieldEditSettings(
            FieldSettingCollection fieldSettings)
        {
            var state = CreateFieldEditSetting(fieldSettings.FindByName(ImplementationFieldName.State));
            var realStartDate = CreateFieldEditSetting(fieldSettings.FindByName(ImplementationFieldName.RealStartDate));

            var buildAndTextImplemented =
                CreateFieldEditSetting(fieldSettings.FindByName(ImplementationFieldName.BuildAndTextImplemented));

            var implementationPlanUsed =
                CreateFieldEditSetting(fieldSettings.FindByName(ImplementationFieldName.ImplementationPlanUsed));

            var deviation = CreateTextFieldEditSetting(fieldSettings.FindByName(ImplementationFieldName.Deviation));

            var recoveryPlanUsed =
                CreateFieldEditSetting(fieldSettings.FindByName(ImplementationFieldName.RecoveryPlanUsed));

            var finishingDate = CreateFieldEditSetting(fieldSettings.FindByName(ImplementationFieldName.FinishingDate));
            var attachedFiles = CreateFieldEditSetting(fieldSettings.FindByName(ImplementationFieldName.AttachedFile));
            var logs = CreateFieldEditSetting(fieldSettings.FindByName(ImplementationFieldName.Log));

            var implementationsReady =
                CreateFieldEditSetting(fieldSettings.FindByName(ImplementationFieldName.ImplementationReady));

            return new ImplementationFieldEditSettings(
                state,
                realStartDate,
                buildAndTextImplemented,
                implementationPlanUsed,
                deviation,
                recoveryPlanUsed,
                finishingDate,
                attachedFiles,
                logs,
                implementationsReady);
        }

        private static ImplementationFieldSettings CreateImplementationFieldSettingGroup(
            FieldSettingCollection fieldSettings)
        {
            var state = CreateFieldSetting(fieldSettings.FindByName(ImplementationFieldName.State));
            var realStartDate = CreateFieldSetting(fieldSettings.FindByName(ImplementationFieldName.RealStartDate));
           
            var buildAndTextImplemented =
                CreateFieldSetting(fieldSettings.FindByName(ImplementationFieldName.BuildAndTextImplemented));
           
            var implementationPlanUsed =
                CreateFieldSetting(fieldSettings.FindByName(ImplementationFieldName.ImplementationPlanUsed));
           
            var deviation = CreateStringFieldSetting(fieldSettings.FindByName(ImplementationFieldName.Deviation));
           
            var recoveryPlanUsed = CreateFieldSetting(
                fieldSettings.FindByName(ImplementationFieldName.RecoveryPlanUsed));
           
            var finishingDate = CreateFieldSetting(fieldSettings.FindByName(ImplementationFieldName.FinishingDate));
            var attachedFile = CreateFieldSetting(fieldSettings.FindByName(ImplementationFieldName.AttachedFile));
            var log = CreateFieldSetting(fieldSettings.FindByName(ImplementationFieldName.Log));
           
            var implementationReady =
                CreateFieldSetting(fieldSettings.FindByName(ImplementationFieldName.ImplementationReady));

            return new ImplementationFieldSettings(
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

        private static LogFieldEditSettings CreateLogFieldEditSettings(FieldSettingCollection fieldSettings)
        {
            var logs = CreateFieldEditSetting(fieldSettings.FindByName(LogFieldName.Log));
            return new LogFieldEditSettings(logs);
        }

        private static LogFieldSettings CreateLogFieldSettingGroup(FieldSettingCollection fieldSettings)
        {
            var log = CreateFieldSetting(fieldSettings.FindByName(LogFieldName.Log));
            return new LogFieldSettings(log);
        }

        private static OrdererFieldEditSettings CreateOrdererFieldEditSettings(FieldSettingCollection fieldSettings)
        {
            var id = CreateFieldEditSetting(fieldSettings.FindByName(OrdererFieldName.Id));
            var name = CreateFieldEditSetting(fieldSettings.FindByName(OrdererFieldName.Name));
            var phone = CreateFieldEditSetting(fieldSettings.FindByName(OrdererFieldName.Phone));
            var cellPhone = CreateFieldEditSetting(fieldSettings.FindByName(OrdererFieldName.CellPhone));
            var email = CreateFieldEditSetting(fieldSettings.FindByName(OrdererFieldName.Email));
            var department = CreateFieldEditSetting(fieldSettings.FindByName(OrdererFieldName.Department));

            return new OrdererFieldEditSettings(id, name, phone, cellPhone, email, department);
        }

        private static OrdererFieldSettings CreateOrderedFieldSettingGroup(FieldSettingCollection fieldSettings)
        {
            var id = CreateFieldSetting(fieldSettings.FindByName(OrdererFieldName.Id));
            var name = CreateFieldSetting(fieldSettings.FindByName(OrdererFieldName.Name));
            var phone = CreateFieldSetting(fieldSettings.FindByName(OrdererFieldName.Phone));
            var cellPhone = CreateFieldSetting(fieldSettings.FindByName(OrdererFieldName.CellPhone));
            var email = CreateFieldSetting(fieldSettings.FindByName(OrdererFieldName.Email));
            var department = CreateFieldSetting(fieldSettings.FindByName(OrdererFieldName.Department));

            return new OrdererFieldSettings(id, name, phone, cellPhone, email, department);
        }

        private static OrdererFieldOverviewSettings CreateOrdererFieldOverviewSettingGroup(
            FieldSettingCollection fieldSettings,
            string languageTextId)
        {
            var id = CreateFieldOverviewSetting(fieldSettings.FindByName(OrdererFieldName.Id), languageTextId);
            var name = CreateFieldOverviewSetting(fieldSettings.FindByName(OrdererFieldName.Name), languageTextId);
            var phone = CreateFieldOverviewSetting(fieldSettings.FindByName(OrdererFieldName.Phone), languageTextId);
           
            var cellPhone = CreateFieldOverviewSetting(
                fieldSettings.FindByName(OrdererFieldName.CellPhone),
                languageTextId);
           
            var email = CreateFieldOverviewSetting(fieldSettings.FindByName(OrdererFieldName.Email), languageTextId);
           
            var department = CreateFieldOverviewSetting(
                fieldSettings.FindByName(OrdererFieldName.Department),
                languageTextId);

            return new OrdererFieldOverviewSettings(id, name, phone, cellPhone, email, department);
        }

        private static RegistrationFieldOverviewSettings CreateRegistrationFieldOverviewSettingGroup(
            FieldSettingCollection fieldSettings,
            string languageTextId)
        {
            var description = CreateFieldOverviewSetting(
                fieldSettings.FindByName(RegistrationFieldName.Description),
                languageTextId);
           
            var businessBenefits =
                CreateFieldOverviewSetting(
                    fieldSettings.FindByName(RegistrationFieldName.BusinessBenefits),
                    languageTextId);
           
            var consequence = CreateFieldOverviewSetting(
                fieldSettings.FindByName(RegistrationFieldName.Consequence),
                languageTextId);
            
            var impact = CreateFieldOverviewSetting(
                fieldSettings.FindByName(RegistrationFieldName.Impact),
                languageTextId);
           
            var desiredDate = CreateFieldOverviewSetting(
                fieldSettings.FindByName(RegistrationFieldName.DesiredDate),
                languageTextId);
           
            var verified = CreateFieldOverviewSetting(
                fieldSettings.FindByName(RegistrationFieldName.Verified),
                languageTextId);
           
            var approval = CreateFieldOverviewSetting(
                fieldSettings.FindByName(RegistrationFieldName.Approval),
                languageTextId);
           
            var explanation = CreateFieldOverviewSetting(
                fieldSettings.FindByName(RegistrationFieldName.Explanation),
                languageTextId);

            return new RegistrationFieldOverviewSettings(
                description,
                businessBenefits,
                consequence,
                impact,
                desiredDate,
                verified,
                approval,
                explanation);
        }

        private static RegistrationFieldEditSettings CreateRegistrationFieldEditSettings(
            FieldSettingCollection fieldSettings)
        {
            var name = CreateFieldEditSetting(fieldSettings.FindByName(RegistrationFieldName.Name));
            var phone = CreateFieldEditSetting(fieldSettings.FindByName(RegistrationFieldName.Phone));
            var email = CreateFieldEditSetting(fieldSettings.FindByName(RegistrationFieldName.Email));
            var company = CreateFieldEditSetting(fieldSettings.FindByName(RegistrationFieldName.Company));
          
            var processesAffected =
                CreateFieldEditSetting(fieldSettings.FindByName(RegistrationFieldName.ProcessAffected));
           
            var departmentsAffected =
                CreateFieldEditSetting(fieldSettings.FindByName(RegistrationFieldName.DepartmentAffected));
           
            var description = CreateTextFieldEditSetting(fieldSettings.FindByName(RegistrationFieldName.Description));
         
            var businessBenefits =
                CreateTextFieldEditSetting(fieldSettings.FindByName(RegistrationFieldName.BusinessBenefits));
         
            var consequence = CreateTextFieldEditSetting(fieldSettings.FindByName(RegistrationFieldName.Consequence));
            var impact = CreateFieldEditSetting(fieldSettings.FindByName(RegistrationFieldName.Impact));
            var desiredDate = CreateFieldEditSetting(fieldSettings.FindByName(RegistrationFieldName.DesiredDate));
            var verified = CreateFieldEditSetting(fieldSettings.FindByName(RegistrationFieldName.Verified));
            var attachedFile = CreateFieldEditSetting(fieldSettings.FindByName(RegistrationFieldName.AttachedFile));
            var approval = CreateFieldEditSetting(fieldSettings.FindByName(RegistrationFieldName.Approval));
            var explanation = CreateFieldEditSetting(fieldSettings.FindByName(RegistrationFieldName.Explanation));

            return new RegistrationFieldEditSettings(
                name,
                phone,
                email,
                company,
                processesAffected,
                departmentsAffected,
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

        private static RegistrationFieldSettings CreateRegistrationFieldSettingGroup(
            FieldSettingCollection fieldSettings)
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

            return new RegistrationFieldSettings(
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

        private static StringFieldSetting CreateStringFieldSetting(ChangeFieldSettingsEntity fieldSetting)
        {
            return new StringFieldSetting(
                fieldSetting.ChangeField,
                fieldSetting.Show != 0,
                fieldSetting.ShowInList != 0,
                fieldSetting.ShowExternal != 0,
                "Dummy caption",
                fieldSetting.Required != 0,
                fieldSetting.InitialValue,
                fieldSetting.Bookmark);
        }

        private static void UpdateAnalyzeFieldSettingGroup(
            FieldSettingCollection existingSettings,
            UpdatedAnalyzeFieldSettingGroupDto updatedSettings)
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

        private static void UpdateEvaluationFieldSettingGroup(
            FieldSettingCollection existingSettings,
            UpdatedEvaluationFieldSettingGroupDto updatedSettings)
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

        private static void UpdateFieldSetting(
            ChangeFieldSettingsEntity fieldSetting,
            UpdatedFieldSettingDto updatedSetting)
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

        private static void UpdateFieldSettingCore(
            ChangeFieldSettingsEntity fieldSetting,
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

        private static void UpdateGeneralFieldSettingGroup(
            FieldSettingCollection existingSettings,
            UpdatedGeneralFieldSettingGroupDto updatedSettings)
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

        private static void UpdateImplementationFieldSettingGroup(
            FieldSettingCollection existingSettings,
            UpdatedImplementationFieldSettingGroupDto updatedSettings)
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

        private static void UpdateLogFieldSettingGroup(
            FieldSettingCollection existingSettings,
            UpdatedLogFieldSettingGroupDto updatedSettings)
        {
            var log = existingSettings.FindByName(LogFieldName.Log);
            UpdateFieldSetting(log, updatedSettings.Log);
        }

        private static void UpdateOrdererFieldSettingGroup(
            FieldSettingCollection existingSettings,
            UpdatedOrdererFieldSettingGroupDto updatedSettings)
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

        private static void UpdateRegistrationFieldSettingGroup(
            FieldSettingCollection existingSettings,
            UpdatedRegistrationFieldSettingGroupDto updatedSettings)
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

        private static void UpdateStringFieldSetting(
            ChangeFieldSettingsEntity fieldSetting,
            UpdatedStringFieldSettingDto updatedSetting)
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

        private IQueryable<ChangeFieldSettingsEntity> FindByCustomerId(int customerId)
        {
            return this.DataContext.ChangeFieldSettings.Where(s => s.Customer_Id == customerId);
        }

        private FieldOverviewSettings FindForSpecifiedLanguageByCustomerId(int customerId, string languageTextId)
        {
            var fieldSettings = this.FindByCustomerId(customerId);
            var fieldSettingCollection = new FieldSettingCollection(fieldSettings);

            var orderer = CreateOrdererFieldOverviewSettingGroup(fieldSettingCollection, languageTextId);
            var general = CreateGeneralFieldOverviewSettingGroup(fieldSettingCollection, languageTextId);
            var registration = CreateRegistrationFieldOverviewSettingGroup(fieldSettingCollection, languageTextId);
            var analyze = CreateAnalyzeFieldOverviewSettingGroup(fieldSettingCollection, languageTextId);
            var implementation = CreateImplementationFieldOverviewSettingGroup(fieldSettingCollection, languageTextId);
            var evaluation = CreateEvaluationFieldOverviewSettingGroup(fieldSettingCollection, languageTextId);

            return new FieldOverviewSettings(orderer, general, registration, analyze, implementation, evaluation);
        }

        #endregion
    }
}