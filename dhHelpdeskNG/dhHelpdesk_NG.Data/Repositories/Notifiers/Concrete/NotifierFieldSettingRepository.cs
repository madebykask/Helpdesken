namespace dhHelpdesk_NG.Data.Repositories.Notifiers.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Input;
    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Output;
    using dhHelpdesk_NG.Data.Enums.Notifiers;
    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain.Notifiers;

    public sealed class NotifierFieldSettingRepository : RepositoryBase<ComputerUserFieldSettings>, INotifierFieldSettingRepository
    {
        public NotifierFieldSettingRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public FieldDisplayRulesDto FindFieldDisplayRulesByCustomerId(int customerId)
        {
            var settings = this.FindByCustomerId(customerId);
            
            var domain = CreateDisplayRule(settings, NotifierField.Domain);
            var loginName = CreateStringDisplayRule(settings, NotifierField.LoginName);
            var firstName = CreateStringDisplayRule(settings, NotifierField.FirstName);
            var initials = CreateStringDisplayRule(settings, NotifierField.Initials);
            var lastName = CreateStringDisplayRule(settings, NotifierField.LastName);
            var displayName = CreateStringDisplayRule(settings, NotifierField.DisplayName);
            var place = CreateStringDisplayRule(settings, NotifierField.Place);
            var phone = CreateStringDisplayRule(settings, NotifierField.Phone);
            var cellPhone = CreateStringDisplayRule(settings, NotifierField.CellPhone);
            var email = CreateStringDisplayRule(settings, NotifierField.Email);
            var code = CreateStringDisplayRule(settings, NotifierField.Code);
            var postalAddress = CreateStringDisplayRule(settings, NotifierField.PostalAddress);
            var postalCode = CreateStringDisplayRule(settings, NotifierField.PostalCode);
            var city = CreateStringDisplayRule(settings, NotifierField.City);
            var title = CreateStringDisplayRule(settings, NotifierField.Title);
            var department = CreateDisplayRule(settings, NotifierField.Department);
            var unit = CreateStringDisplayRule(settings, NotifierField.Unit);
            var organizationUnit = CreateDisplayRule(settings, NotifierField.OrganizationUnit);
            var division = CreateDisplayRule(settings, NotifierField.Division);
            var manager = CreateDisplayRule(settings, NotifierField.Manager);
            var group = CreateDisplayRule(settings, NotifierField.Group);
            var password = CreateStringDisplayRule(settings, NotifierField.Password);
            var other = CreateStringDisplayRule(settings, NotifierField.Other);
            var ordered = CreateDisplayRule(settings, NotifierField.Ordered);

            return new FieldDisplayRulesDto(
                domain,
                loginName,
                firstName,
                initials,
                lastName,
                displayName,
                place,
                phone,
                cellPhone,
                email,
                code,
                postalAddress,
                postalCode,
                city,
                title,
                department,
                unit,
                organizationUnit,
                division,
                manager,
                group,
                password,
                other,
                ordered);
        } 

        public void UpdateSettings(UpdatedFieldSettingsDto fieldSettings)
        {
            var settings = this.FindByCustomerId(fieldSettings.CustomerId);

            this.UpdateStringFieldSetting(settings, NotifierField.CellPhone, fieldSettings.CellPhone, fieldSettings.LanguageId);
            this.UpdateFieldSetting(settings, NotifierField.ChangedDate, fieldSettings.ChangedDate, fieldSettings.LanguageId);
            this.UpdateStringFieldSetting(settings, NotifierField.City, fieldSettings.City, fieldSettings.LanguageId);
            this.UpdateStringFieldSetting(settings, NotifierField.Code, fieldSettings.Code, fieldSettings.LanguageId);
            this.UpdateFieldSetting(settings, NotifierField.CreatedDate, fieldSettings.CreatedDate, fieldSettings.LanguageId);
            this.UpdateFieldSetting(settings, NotifierField.Department, fieldSettings.Department, fieldSettings.LanguageId);
            
            this.UpdateStringFieldSetting(
                settings, NotifierField.DisplayName, fieldSettings.DisplayName, fieldSettings.LanguageId);
            
            this.UpdateFieldSetting(settings, NotifierField.Division, fieldSettings.Division, fieldSettings.LanguageId);
            this.UpdateFieldSetting(settings, NotifierField.Domain, fieldSettings.Domain, fieldSettings.LanguageId);
            this.UpdateStringFieldSetting(settings, NotifierField.Email, fieldSettings.Email, fieldSettings.LanguageId);
            this.UpdateStringFieldSetting(settings, NotifierField.FirstName, fieldSettings.FirstName, fieldSettings.LanguageId);
            this.UpdateFieldSetting(settings, NotifierField.Group, fieldSettings.Group, fieldSettings.LanguageId);
            this.UpdateStringFieldSetting(settings, NotifierField.Initials, fieldSettings.Initials, fieldSettings.LanguageId);
            this.UpdateStringFieldSetting(settings, NotifierField.LastName, fieldSettings.LastName, fieldSettings.LanguageId);
            this.UpdateStringFieldSetting(settings, NotifierField.LoginName, fieldSettings.LoginName, fieldSettings.LanguageId);
            this.UpdateFieldSetting(settings, NotifierField.Manager, fieldSettings.Manager, fieldSettings.LanguageId);
            this.UpdateFieldSetting(settings, NotifierField.Ordered, fieldSettings.Ordered, fieldSettings.LanguageId);

            this.UpdateFieldSetting(
                settings, NotifierField.OrganizationUnit, fieldSettings.OrganizationUnit, fieldSettings.LanguageId);
            
            this.UpdateStringFieldSetting(settings, NotifierField.Other, fieldSettings.Other, fieldSettings.LanguageId);
            this.UpdateStringFieldSetting(settings, NotifierField.Password, fieldSettings.Password, fieldSettings.LanguageId);
            this.UpdateStringFieldSetting(settings, NotifierField.Phone, fieldSettings.Phone, fieldSettings.LanguageId);
            this.UpdateStringFieldSetting(settings, NotifierField.Place, fieldSettings.Place, fieldSettings.LanguageId);

            this.UpdateStringFieldSetting(
                settings, NotifierField.PostalAddress, fieldSettings.PostalAddress, fieldSettings.LanguageId);

            this.UpdateStringFieldSetting(
                settings, NotifierField.PostalCode, fieldSettings.PostalCode, fieldSettings.LanguageId);

            this.UpdateFieldSetting(
                settings, NotifierField.SynchronizationDate, fieldSettings.SynchronizationDate, fieldSettings.LanguageId);
            
            this.UpdateStringFieldSetting(settings, NotifierField.Title, fieldSettings.Title, fieldSettings.LanguageId);
            this.UpdateStringFieldSetting(settings, NotifierField.Unit, fieldSettings.Unit, fieldSettings.LanguageId);
            this.UpdateStringFieldSetting(settings, NotifierField.UserId, fieldSettings.UserId, fieldSettings.LanguageId);
        }

        public DisplayFieldSettingsDto FindDisplayFieldSettingsByCustomerIdAndLanguageId(int customerId, int languageId)
        {
            var settings = this.FindByCustomerId(customerId);

            var userId = this.CreateDisplayStringFieldSetting(settings, NotifierField.UserId, languageId);
            var domain = this.CreateDisplayFieldSetting(settings, NotifierField.Domain, languageId);
            var loginName = this.CreateDisplayStringFieldSetting(settings, NotifierField.LoginName, languageId);
            var firstName = this.CreateDisplayStringFieldSetting(settings, NotifierField.FirstName, languageId);
            var initials = this.CreateDisplayStringFieldSetting(settings, NotifierField.Initials, languageId);
            var lastName = this.CreateDisplayStringFieldSetting(settings, NotifierField.LastName, languageId);
            var displayName = this.CreateDisplayStringFieldSetting(settings, NotifierField.DisplayName, languageId);
            var place = this.CreateDisplayStringFieldSetting(settings, NotifierField.Place, languageId);
            var phone = this.CreateDisplayStringFieldSetting(settings, NotifierField.Phone, languageId);
            var cellPhone = this.CreateDisplayStringFieldSetting(settings, NotifierField.CellPhone, languageId);
            var email = this.CreateDisplayStringFieldSetting(settings, NotifierField.Email, languageId);
            var code = this.CreateDisplayStringFieldSetting(settings, NotifierField.Code, languageId);
            var postalAddress = this.CreateDisplayStringFieldSetting(settings, NotifierField.PostalAddress, languageId);
            var postalCode = this.CreateDisplayStringFieldSetting(settings, NotifierField.PostalCode, languageId);
            var city = this.CreateDisplayStringFieldSetting(settings, NotifierField.City, languageId);
            var title = this.CreateDisplayStringFieldSetting(settings, NotifierField.Title, languageId);
            var department = this.CreateDisplayFieldSetting(settings, NotifierField.Department, languageId);
            var unit = this.CreateDisplayStringFieldSetting(settings, NotifierField.Unit, languageId);
            var organizationUnit = this.CreateDisplayFieldSetting(settings, NotifierField.OrganizationUnit, languageId);
            var division = this.CreateDisplayFieldSetting(settings, NotifierField.Division, languageId);
            var manager = this.CreateDisplayFieldSetting(settings, NotifierField.Manager, languageId);
            var group = this.CreateDisplayFieldSetting(settings, NotifierField.Group, languageId);
            var password = this.CreateDisplayStringFieldSetting(settings, NotifierField.Password, languageId);
            var other = this.CreateDisplayStringFieldSetting(settings, NotifierField.Other, languageId);
            var ordered = this.CreateDisplayFieldSetting(settings, NotifierField.Ordered, languageId);
            var createdDate = this.CreateDisplayFieldSetting(settings, NotifierField.CreatedDate, languageId);
            var changedDate = this.CreateDisplayFieldSetting(settings, NotifierField.ChangedDate, languageId);
            
            var synchronizationDate = this.CreateDisplayFieldSetting(
                settings, NotifierField.SynchronizationDate, languageId);

            return new DisplayFieldSettingsDto(
                userId,
                domain,
                loginName,
                firstName,
                initials,
                lastName,
                displayName,
                place,
                phone,
                cellPhone,
                email,
                code,
                postalAddress,
                postalCode,
                city,
                title,
                department,
                unit,
                organizationUnit,
                division,
                manager,
                group,
                password,
                other,
                ordered,
                createdDate,
                changedDate,
                synchronizationDate);
        }

        public FieldSettingsDto FindByCustomerIdAndLanguageId(int customerId, int languageId)
        {
            var settings = this.FindByCustomerId(customerId);

            var userId = this.CreateStringFieldSetting(settings, NotifierField.UserId, languageId);
            var domain = this.CreateFieldSetting(settings, NotifierField.Domain, languageId);
            var loginName = this.CreateStringFieldSetting(settings, NotifierField.LoginName, languageId);
            var firstName = this.CreateStringFieldSetting(settings, NotifierField.FirstName, languageId);
            var initials = this.CreateStringFieldSetting(settings, NotifierField.Initials, languageId);
            var lastName = this.CreateStringFieldSetting(settings, NotifierField.LastName, languageId);
            var displayName = this.CreateStringFieldSetting(settings, NotifierField.DisplayName, languageId);
            var place = this.CreateStringFieldSetting(settings, NotifierField.Place, languageId);
            var phone = this.CreateStringFieldSetting(settings, NotifierField.Phone, languageId);
            var cellPhone = this.CreateStringFieldSetting(settings, NotifierField.CellPhone, languageId);
            var email = this.CreateStringFieldSetting(settings, NotifierField.Email, languageId);
            var code = this.CreateStringFieldSetting(settings, NotifierField.Code, languageId);
            var postalAddress = this.CreateStringFieldSetting(settings, NotifierField.PostalAddress, languageId);
            var postalCode = this.CreateStringFieldSetting(settings, NotifierField.PostalCode, languageId);
            var city = this.CreateStringFieldSetting(settings, NotifierField.City, languageId);
            var title = this.CreateStringFieldSetting(settings, NotifierField.Title, languageId);
            var department = this.CreateFieldSetting(settings, NotifierField.Department, languageId);
            var unit = this.CreateStringFieldSetting(settings, NotifierField.Unit, languageId);
            var organizationUnit = this.CreateFieldSetting(settings, NotifierField.OrganizationUnit, languageId);
            var division = this.CreateFieldSetting(settings, NotifierField.Division, languageId);
            var manager = this.CreateFieldSetting(settings, NotifierField.Manager, languageId);
            var group = this.CreateFieldSetting(settings, NotifierField.Group, languageId);
            var password = this.CreateStringFieldSetting(settings, NotifierField.Password, languageId);
            var other = this.CreateStringFieldSetting(settings, NotifierField.Other, languageId);
            var ordered = this.CreateFieldSetting(settings, NotifierField.Ordered, languageId);
            var createdDate = this.CreateFieldSetting(settings, NotifierField.CreatedDate, languageId);
            var changedDate = this.CreateFieldSetting(settings, NotifierField.ChangedDate, languageId);
            var synchronizationDate = this.CreateFieldSetting(settings, NotifierField.SynchronizationDate, languageId);

            return new FieldSettingsDto(
                userId,
                domain,
                loginName,
                firstName,
                initials,
                lastName,
                displayName,
                place,
                phone,
                cellPhone,
                email,
                code,
                postalAddress,
                postalCode,
                city,
                title,
                department,
                unit,
                organizationUnit,
                division,
                manager,
                group,
                password,
                other,
                ordered,
                createdDate,
                changedDate,
                synchronizationDate);
        }

        private static StringFieldDisplayRuleDto CreateStringDisplayRule(List<ComputerUserFieldSettings> settings, string fieldName)
        {
            var setting = FilterSettingByFieldName(settings, fieldName);
            return new StringFieldDisplayRuleDto(setting.Show != 0, setting.Required != 0, setting.MinLength);
        }

        private static FieldDisplayRuleDto CreateDisplayRule(List<ComputerUserFieldSettings> settings, string fieldName)
        {
            var setting = FilterSettingByFieldName(settings, fieldName);
            return new FieldDisplayRuleDto(setting.Show != 0, setting.Required != 0);
        }

        private StringFieldSettingDto CreateStringFieldSetting(
            List<ComputerUserFieldSettings> settings, string createSettingName, int languageId)
        {
            var setting = FilterSettingByFieldName(settings, createSettingName);
            var translation = this.GetTranslationBySettingIdAndLanguageId(setting.Id, languageId);

            return new StringFieldSettingDto(
                setting.ComputerUserField,
                setting.Show != 0,
                setting.ShowInList != 0,
                translation.Label,
                setting.Required != 0,
                setting.MinLength,
                setting.LDAPAttribute);
        }

        private FieldSettingDto CreateFieldSetting(List<ComputerUserFieldSettings> settings, string createSettingName, int languageId)
        {
            var setting = FilterSettingByFieldName(settings, createSettingName);
            var translation = this.GetTranslationBySettingIdAndLanguageId(setting.Id, languageId);

            return new FieldSettingDto(
                setting.ComputerUserField,
                setting.Show != 0,
                setting.ShowInList != 0,
                translation.Label,
                setting.Required != 0,
                setting.LDAPAttribute);
        }
        
        private static ComputerUserFieldSettings FilterSettingByFieldName(List<ComputerUserFieldSettings> settings, string name)
        {
            return settings.Single(s => s.ComputerUserField.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }

        private ComputerUserFieldSettingsLanguage GetTranslationBySettingIdAndLanguageId(int settingId, int languageId)
        {
            return
                this.DataContext.ComputerUserFieldSettingsLanguages.Single(
                    t => t.ComputerUserFieldSettings_Id == settingId && t.Language_Id == languageId);
        }

        private DisplayStringFieldSettingDto CreateDisplayStringFieldSetting(List<ComputerUserFieldSettings> settings, string createSettingName, int languageId)
        {
            var setting = FilterSettingByFieldName(settings, createSettingName);
            var translation = this.GetTranslationBySettingIdAndLanguageId(setting.Id, languageId);

            return new DisplayStringFieldSettingDto(
                setting.Show != 0, translation.Label, setting.Required != 0, setting.MinLength);
        }

        private DisplayFieldSettingDto CreateDisplayFieldSetting(List<ComputerUserFieldSettings> settings, string createSettingName, int languageId)
        {
            var setting = FilterSettingByFieldName(settings, createSettingName);
            var translation = this.GetTranslationBySettingIdAndLanguageId(setting.Id, languageId);
            return new DisplayFieldSettingDto(setting.Show != 0, translation.Label, setting.Required != 0);
        }
        
        private List<ComputerUserFieldSettings> FindByCustomerId(int customerId)
        {
            return this.DataContext.ComputerUserFieldSettings.Where(s => s.Customer_Id == customerId).ToList();
        } 

        private void UpdateFieldSettingCore(
            List<ComputerUserFieldSettings> settings,
            string updateSettingName,
            int languageId,
            DateTime changedDateTime, 
            string ldapAttribute,
            int? minLength,
            string caption,
            bool required,
            bool showInDetails,
            bool showInNotifiers)
        {
            var setting = FilterSettingByFieldName(settings, updateSettingName);

            setting.ChangedDate = changedDateTime;
            setting.LDAPAttribute = ldapAttribute ?? string.Empty;
            setting.MinLength = minLength ?? 0;

            var translation =
                this.DataContext.ComputerUserFieldSettingsLanguages.Single(
                    t => t.ComputerUserFieldSettings_Id == setting.Id && t.Language_Id == languageId);

            translation.Label = caption;
            setting.Required = required ? 1 : 0;
            setting.Show = showInDetails ? 1 : 0;
            setting.ShowInList = showInNotifiers ? 1 : 0;
        }

        private void UpdateStringFieldSetting(
            List<ComputerUserFieldSettings> settings,
            string updateSettingName,
            UpdatedStringFieldSettingDto updatedSetting,
            int languageId)
        {
            this.UpdateFieldSettingCore(
                settings,
                updateSettingName,
                languageId,
                updatedSetting.ChangedDateTime,
                updatedSetting.LdapAttribute,
                updatedSetting.MinLength,
                updatedSetting.Caption,
                updatedSetting.Required,
                updatedSetting.ShowInDetails,
                updatedSetting.ShowInNotifiers);
        }

        private void UpdateFieldSetting(
            List<ComputerUserFieldSettings> settings,
            string updateSettingName,
            UpdatedFieldSettingDto updatedSetting,
            int languageId)
        {
            this.UpdateFieldSettingCore(
                settings,
                updateSettingName,
                languageId,
                updatedSetting.ChangedDateTime,
                updatedSetting.LdapAttribute,
                null,
                updatedSetting.Caption,
                updatedSetting.Required,
                updatedSetting.ShowInDetails,
                updatedSetting.ShowInNotifiers);
        }
    }
}
