namespace DH.Helpdesk.Dal.Repositories.Notifiers.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Notifiers.Input;
    using DH.Helpdesk.BusinessData.Models.Notifiers.Output;
    using DH.Helpdesk.Dal.Enums.Notifiers;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Computers;

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
            var loginName = CreateDisplayRule(settings, NotifierField.LoginName);
            var firstName = CreateDisplayRule(settings, NotifierField.FirstName);
            var initials = CreateDisplayRule(settings, NotifierField.Initials);
            var lastName = CreateDisplayRule(settings, NotifierField.LastName);
            var displayName = CreateDisplayRule(settings, NotifierField.DisplayName);
            var place = CreateDisplayRule(settings, NotifierField.Place);
            var phone = CreateDisplayRule(settings, NotifierField.Phone);
            var cellPhone = CreateDisplayRule(settings, NotifierField.CellPhone);
            var email = CreateDisplayRule(settings, NotifierField.Email);
            var code = CreateDisplayRule(settings, NotifierField.Code);
            var postalAddress = CreateDisplayRule(settings, NotifierField.PostalAddress);
            var postalCode = CreateDisplayRule(settings, NotifierField.PostalCode);
            var city = CreateDisplayRule(settings, NotifierField.City);
            var title = CreateDisplayRule(settings, NotifierField.Title);
            var department = CreateDisplayRule(settings, NotifierField.Department);
            var unit = CreateDisplayRule(settings, NotifierField.Unit);
            var organizationUnit = CreateDisplayRule(settings, NotifierField.OrganizationUnit);
            var division = CreateDisplayRule(settings, NotifierField.Division);
            var manager = CreateDisplayRule(settings, NotifierField.Manager);
            var group = CreateDisplayRule(settings, NotifierField.Group);
            var password = CreateDisplayRule(settings, NotifierField.Password);
            var other = CreateDisplayRule(settings, NotifierField.Other);
            var ordered = CreateDisplayRule(settings, NotifierField.Ordered);
            var changedDate = CreateDisplayRule(settings, NotifierField.ChangedDate);

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
                ordered,
                changedDate);
        } 

        public void UpdateSettings(UpdatedFieldSettingsDto fieldSettings)
        {
            var settings = this.FindByCustomerId(fieldSettings.CustomerId);

            this.UpdateFieldSetting(settings, NotifierField.CellPhone, fieldSettings.CellPhone, fieldSettings.LanguageId);
            this.UpdateFieldSetting(settings, NotifierField.ChangedDate, fieldSettings.ChangedDate, fieldSettings.LanguageId);
            this.UpdateFieldSetting(settings, NotifierField.City, fieldSettings.City, fieldSettings.LanguageId);
            this.UpdateFieldSetting(settings, NotifierField.Code, fieldSettings.Code, fieldSettings.LanguageId);
            this.UpdateFieldSetting(settings, NotifierField.CreatedDate, fieldSettings.CreatedDate, fieldSettings.LanguageId);
            this.UpdateFieldSetting(settings, NotifierField.Department, fieldSettings.Department, fieldSettings.LanguageId);
            
            this.UpdateFieldSetting(
                settings, NotifierField.DisplayName, fieldSettings.DisplayName, fieldSettings.LanguageId);
            
            this.UpdateFieldSetting(settings, NotifierField.Division, fieldSettings.Division, fieldSettings.LanguageId);
            this.UpdateFieldSetting(settings, NotifierField.Domain, fieldSettings.Domain, fieldSettings.LanguageId);
            this.UpdateFieldSetting(settings, NotifierField.Email, fieldSettings.Email, fieldSettings.LanguageId);
            this.UpdateFieldSetting(settings, NotifierField.FirstName, fieldSettings.FirstName, fieldSettings.LanguageId);
            this.UpdateFieldSetting(settings, NotifierField.Group, fieldSettings.Group, fieldSettings.LanguageId);
            this.UpdateFieldSetting(settings, NotifierField.Initials, fieldSettings.Initials, fieldSettings.LanguageId);
            this.UpdateFieldSetting(settings, NotifierField.LastName, fieldSettings.LastName, fieldSettings.LanguageId);
            this.UpdateFieldSetting(settings, NotifierField.LoginName, fieldSettings.LoginName, fieldSettings.LanguageId);
            this.UpdateFieldSetting(settings, NotifierField.Manager, fieldSettings.Manager, fieldSettings.LanguageId);
            this.UpdateFieldSetting(settings, NotifierField.Ordered, fieldSettings.Ordered, fieldSettings.LanguageId);

            this.UpdateFieldSetting(
                settings, NotifierField.OrganizationUnit, fieldSettings.OrganizationUnit, fieldSettings.LanguageId);
            
            this.UpdateFieldSetting(settings, NotifierField.Other, fieldSettings.Other, fieldSettings.LanguageId);
            this.UpdateFieldSetting(settings, NotifierField.Password, fieldSettings.Password, fieldSettings.LanguageId);
            this.UpdateFieldSetting(settings, NotifierField.Phone, fieldSettings.Phone, fieldSettings.LanguageId);
            this.UpdateFieldSetting(settings, NotifierField.Place, fieldSettings.Place, fieldSettings.LanguageId);

            this.UpdateFieldSetting(
                settings, NotifierField.PostalAddress, fieldSettings.PostalAddress, fieldSettings.LanguageId);

            this.UpdateFieldSetting(
                settings, NotifierField.PostalCode, fieldSettings.PostalCode, fieldSettings.LanguageId);

            this.UpdateFieldSetting(
                settings, NotifierField.SynchronizationDate, fieldSettings.SynchronizationDate, fieldSettings.LanguageId);
            
            this.UpdateFieldSetting(settings, NotifierField.Title, fieldSettings.Title, fieldSettings.LanguageId);
            this.UpdateFieldSetting(settings, NotifierField.Unit, fieldSettings.Unit, fieldSettings.LanguageId);
            this.UpdateFieldSetting(settings, NotifierField.UserId, fieldSettings.UserId, fieldSettings.LanguageId);
        }

        public DisplayFieldSettingsDto FindDisplayFieldSettingsByCustomerIdAndLanguageId(int customerId, int languageId)
        {
            var settings = this.FindByCustomerId(customerId);

            var userId = this.CreateDisplayFieldSetting(settings, NotifierField.UserId, languageId);
            var domain = this.CreateDisplayFieldSetting(settings, NotifierField.Domain, languageId);
            var loginName = this.CreateDisplayFieldSetting(settings, NotifierField.LoginName, languageId);
            var firstName = this.CreateDisplayFieldSetting(settings, NotifierField.FirstName, languageId);
            var initials = this.CreateDisplayFieldSetting(settings, NotifierField.Initials, languageId);
            var lastName = this.CreateDisplayFieldSetting(settings, NotifierField.LastName, languageId);
            var displayName = this.CreateDisplayFieldSetting(settings, NotifierField.DisplayName, languageId);
            var place = this.CreateDisplayFieldSetting(settings, NotifierField.Place, languageId);
            var phone = this.CreateDisplayFieldSetting(settings, NotifierField.Phone, languageId);
            var cellPhone = this.CreateDisplayFieldSetting(settings, NotifierField.CellPhone, languageId);
            var email = this.CreateDisplayFieldSetting(settings, NotifierField.Email, languageId);
            var code = this.CreateDisplayFieldSetting(settings, NotifierField.Code, languageId);
            var postalAddress = this.CreateDisplayFieldSetting(settings, NotifierField.PostalAddress, languageId);
            var postalCode = this.CreateDisplayFieldSetting(settings, NotifierField.PostalCode, languageId);
            var city = this.CreateDisplayFieldSetting(settings, NotifierField.City, languageId);
            var title = this.CreateDisplayFieldSetting(settings, NotifierField.Title, languageId);
            var department = this.CreateDisplayFieldSetting(settings, NotifierField.Department, languageId);
            var unit = this.CreateDisplayFieldSetting(settings, NotifierField.Unit, languageId);
            var organizationUnit = this.CreateDisplayFieldSetting(settings, NotifierField.OrganizationUnit, languageId);
            var division = this.CreateDisplayFieldSetting(settings, NotifierField.Division, languageId);
            var manager = this.CreateDisplayFieldSetting(settings, NotifierField.Manager, languageId);
            var group = this.CreateDisplayFieldSetting(settings, NotifierField.Group, languageId);
            var password = this.CreateDisplayFieldSetting(settings, NotifierField.Password, languageId);
            var other = this.CreateDisplayFieldSetting(settings, NotifierField.Other, languageId);
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

            var userId = this.CreateFieldSetting(settings, NotifierField.UserId, languageId);
            var domain = this.CreateFieldSetting(settings, NotifierField.Domain, languageId);
            var loginName = this.CreateFieldSetting(settings, NotifierField.LoginName, languageId);
            var firstName = this.CreateFieldSetting(settings, NotifierField.FirstName, languageId);
            var initials = this.CreateFieldSetting(settings, NotifierField.Initials, languageId);
            var lastName = this.CreateFieldSetting(settings, NotifierField.LastName, languageId);
            var displayName = this.CreateFieldSetting(settings, NotifierField.DisplayName, languageId);
            var place = this.CreateFieldSetting(settings, NotifierField.Place, languageId);
            var phone = this.CreateFieldSetting(settings, NotifierField.Phone, languageId);
            var cellPhone = this.CreateFieldSetting(settings, NotifierField.CellPhone, languageId);
            var email = this.CreateFieldSetting(settings, NotifierField.Email, languageId);
            var code = this.CreateFieldSetting(settings, NotifierField.Code, languageId);
            var postalAddress = this.CreateFieldSetting(settings, NotifierField.PostalAddress, languageId);
            var postalCode = this.CreateFieldSetting(settings, NotifierField.PostalCode, languageId);
            var city = this.CreateFieldSetting(settings, NotifierField.City, languageId);
            var title = this.CreateFieldSetting(settings, NotifierField.Title, languageId);
            var department = this.CreateFieldSetting(settings, NotifierField.Department, languageId);
            var unit = this.CreateFieldSetting(settings, NotifierField.Unit, languageId);
            var organizationUnit = this.CreateFieldSetting(settings, NotifierField.OrganizationUnit, languageId);
            var division = this.CreateFieldSetting(settings, NotifierField.Division, languageId);
            var manager = this.CreateFieldSetting(settings, NotifierField.Manager, languageId);
            var group = this.CreateFieldSetting(settings, NotifierField.Group, languageId);
            var password = this.CreateFieldSetting(settings, NotifierField.Password, languageId);
            var other = this.CreateFieldSetting(settings, NotifierField.Other, languageId);
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

        private static FieldDisplayRuleDto CreateDisplayRule(List<ComputerUserFieldSettings> settings, string fieldName)
        {
            var setting = FilterSettingByFieldName(settings, fieldName);
            return new FieldDisplayRuleDto(setting.Show != 0, setting.Required != 0);
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

        private void UpdateFieldSetting(
            List<ComputerUserFieldSettings> settings,
            string updateSettingName,
            UpdatedFieldSettingDto updatedSetting,
            int languageId)
        {
            var setting = FilterSettingByFieldName(settings, updateSettingName);

            setting.ChangedDate = updatedSetting.ChangedDateTime;
            setting.LDAPAttribute = updatedSetting.LdapAttribute ?? string.Empty;

            var translation =
                this.DataContext.ComputerUserFieldSettingsLanguages.Single(
                    t => t.ComputerUserFieldSettings_Id == setting.Id && t.Language_Id == languageId);

            translation.Label = updatedSetting.Caption;
            setting.Required = updatedSetting.Required ? 1 : 0;
            setting.Show = updatedSetting.ShowInDetails ? 1 : 0;
            setting.ShowInList = updatedSetting.ShowInNotifiers ? 1 : 0;
        }
    }
}
