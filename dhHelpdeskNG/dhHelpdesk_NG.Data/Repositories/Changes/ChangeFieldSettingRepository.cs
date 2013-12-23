//namespace dhHelpdesk_NG.Data.Repositories.Changes
//{
//    using System.Collections.Generic;
//    using System.Linq;
//
//    using dhHelpdesk_NG.DTO.DTOs.Changes.Output;
//    using dhHelpdesk_NG.Data.Infrastructure;
//    using dhHelpdesk_NG.Data.Infrastructure.Collections.Changes;
//    using dhHelpdesk_NG.Domain;
//
//    public sealed class ChangeFieldSettingRepository : RepositoryBase<ChangeFieldSettings>
//    {
////        public ChangeFieldSettingRepository(IDatabaseFactory databaseFactory)
////            : base(databaseFactory)
////        {
////        }
////
////        public FieldSettingsDto FindByCustomerIdAndLanguageId(int customerId, int languageId)
////        {
////            var fieldSettings = this.DataContext.ChangeFieldSettings.Where(s => s.Customer_id == customerId);
////            var fieldSettingCollection = new FieldSettingCollection(fieldSettings);
////
////            var orderedGroup = CreateOrderedFieldSettingGroup(fieldSettingCollection);
////            var generalGroup = CreateGeneralFieldSettingGroup(fieldSettingCollection);
////            var registrationGroup = CreateRegistrationFieldSettingGroup(fieldSettingCollection);
////            var analyzeGroup = CreateAnalyzeFieldSettingGroup(fieldSettingCollection);
////            var implementationGroup = CreateImplementationFieldSettingGroup(fieldSettingCollection);
////            var evaluationGroup = CreateEvaluationFieldSettingGroup(fieldSettingCollection);
////            var logGroup = CreateLogFieldSettingGroup(fieldSettingCollection);
////
////            return new FieldSettingsDto(
////                orderedGroup,
////                generalGroup,
////                registrationGroup,
////                analyzeGroup,
////                implementationGroup,
////                evaluationGroup,
////                logGroup);
////        }
////
////        private static OrderedFieldSettingGroupDto CreateOrderedFieldSettingGroup(FieldSettingCollection fieldSettings)
////        {
////            var name = CreateFieldSetting(fieldSettings.FindByName());
////            var phone = CreateFieldSetting(fieldSettings.FindByName());
////            var cellPhone = CreateFieldSetting(fieldSettings.FindByName());
////            var email = CreateFieldSetting(fieldSettings.FindByName());
////            var department = CreateFieldSetting(fieldSettings.FindByName());
////
////            return new OrderedFieldSettingGroupDto(name, phone, cellPhone, email, department);
////        }
////
////        private static GeneralFieldSettingGroupDto CreateGeneralFieldSettingGroup(FieldSettingCollection fieldSettings)
////        {
////            var priority = CreateFieldSetting(fieldSettings.FindByName());
////            var title = CreateFieldSetting(fieldSettings.FindByName());
////            var state = CreateFieldSetting(fieldSettings.FindByName());
////            var system = CreateFieldSetting(fieldSettings.FindByName());
////            var @object = CreateFieldSetting(fieldSettings.FindByName());
////            var inventory = CreateFieldSetting(fieldSettings.FindByName());
////            var owner = CreateFieldSetting(fieldSettings.FindByName());
////            var workingGroup = CreateFieldSetting(fieldSettings.FindByName());
////            var administrator = CreateFieldSetting(fieldSettings.FindByName());
////            var finishingDate = CreateFieldSetting(fieldSettings.FindByName());
////            var rss = CreateFieldSetting(fieldSettings.FindByName());
////
////            return new GeneralFieldSettingGroupDto(
////                priority,
////                title,
////                state,
////                system,
////                @object,
////                inventory,
////                owner,
////                workingGroup,
////                administrator,
////                finishingDate,
////                rss);
////        }
////
////        private static RegistrationFieldSettingGroupDto CreateRegistrationFieldSettingGroup(FieldSettingCollection fieldSettings)
////        {
////            var name = CreateFieldSetting();
////            var phone = CreateFieldSetting();
////            var email = CreateFieldSetting();
////            var company = CreateFieldSetting();
////            var processAffected = CreateFieldSetting();
////            var departmentAffected = CreateFieldSetting();
////            var description = CreateFieldSetting();
////            var businessBenefits = CreateFieldSetting();
////            var consequence = CreateFieldSetting();
////            var impact = CreateFieldSetting();
////            var desiredDate = CreateFieldSetting();
////            var veryfied = CreateFieldSetting();
////            var attachedFile = CreateFieldSetting();
////            var approval = CreateFieldSetting();
////            var explanation = CreateFieldSetting();
////
////            return new RegistrationFieldSettingGroupDto();
////        }
////
////        private static AnalyzeFieldSettingGroupDto CreateAnalyzeFieldSettingGroup(FieldSettingCollection fieldSettings)
////        {
////
////        }
////
////        private static ImplementationFieldSettingGroupDto CreateImplementationFieldSettingGroup(FieldSettingCollection fieldSettings)
////        {
////
////        }
////
////        private static EvaluationFieldSettingGroupDto CreateEvaluationFieldSettingGroup(FieldSettingCollection fieldSettings)
////        {
////
////        }
////
////        private static LogFieldSettingGroupDto CreateLogFieldSettingGroup(FieldSettingCollection fieldSettings)
////        {
////
////        }
////
////        private static FieldSettingDto CreateFieldSetting(ChangeFieldSettings fieldSetting)
////        {
////            return new FieldSettingDto(
////                fieldSetting.ChangeField,
////                fieldSetting.Show != 0,
////                fieldSetting.ShowInList != 0,
////                fieldSetting.ShowExternal != 0,
////                "Dummy caption",
////                fieldSetting.Required != 0,
////                fieldSetting.Bookmark);
////        }
//    }
//}
