namespace DH.Helpdesk.Dal.Repositories.Notifiers.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Notifiers.Settings.NotifierOverview;
    using DH.Helpdesk.BusinessData.Models.Notifiers.Settings.NotifierProcessing;
    using DH.Helpdesk.BusinessData.Models.Notifiers.Settings.SettingsEdit;
    using DH.Helpdesk.Dal.Enums.Notifiers;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Infrastructure.ModelFactories.Notifiers;
    using DH.Helpdesk.Domain.Computers;

    public sealed class NotifierFieldSettingRepository : RepositoryBase<ComputerUserFieldSettings>, INotifierFieldSettingRepository
    {
        private readonly INotifierFieldSettingsFactory notifierFieldSettingsFactory;

        public NotifierFieldSettingRepository(
            IDatabaseFactory databaseFactory, 
            INotifierFieldSettingsFactory notifierFieldSettingsFactory)
            : base(databaseFactory)
        {
            this.notifierFieldSettingsFactory = notifierFieldSettingsFactory;
        }

        public NotifierProcessingSettings GetProcessingSettings(int customerId)
        {
            var settings = this.FindByCustomerId(customerId);

            var userId = CreateDisplayRule(settings, GeneralField.UserId);
            var domain = CreateDisplayRule(settings, GeneralField.Domain);
            var loginName = CreateDisplayRule(settings, GeneralField.LoginName);
            var firstName = CreateDisplayRule(settings, GeneralField.FirstName);
            var initials = CreateDisplayRule(settings, GeneralField.Initials);
            var lastName = CreateDisplayRule(settings, GeneralField.LastName);
            var displayName = CreateDisplayRule(settings, GeneralField.DisplayName);
            var place = CreateDisplayRule(settings, GeneralField.Place);
            var phone = CreateDisplayRule(settings, GeneralField.Phone);
            var cellPhone = CreateDisplayRule(settings, GeneralField.CellPhone);
            var email = CreateDisplayRule(settings, GeneralField.Email);
            var code = CreateDisplayRule(settings, GeneralField.Code);
            var postalAddress = CreateDisplayRule(settings, AddressField.PostalAddress);
            var postalCode = CreateDisplayRule(settings, AddressField.PostalCode);
            var city = CreateDisplayRule(settings, AddressField.City);
            var title = CreateDisplayRule(settings, OrganizationField.Title);
            var department = CreateDisplayRule(settings, OrganizationField.Department);
            var unit = CreateDisplayRule(settings, OrganizationField.Unit);
            var organizationUnit = CreateDisplayRule(settings, OrganizationField.OrganizationUnit);
            var division = CreateDisplayRule(settings, OrganizationField.Division);
            var manager = CreateDisplayRule(settings, OrganizationField.Manager);
            var group = CreateDisplayRule(settings, OrganizationField.Group);
            var other = CreateDisplayRule(settings, OrganizationField.Other);
            var ordered = CreateDisplayRule(settings, OrdererField.Ordered);
            var changedDate = CreateDisplayRule(settings, StateField.ChangedDate);

            return new NotifierProcessingSettings(
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
                other,
                ordered,
                changedDate);
        } 

        public void UpdateSettings(FieldSettings fieldSettings)
        {
            var settings = this.FindByCustomerId(fieldSettings.CustomerId);

            this.UpdateFieldSetting(settings, GeneralField.CellPhone, fieldSettings.CellPhone, fieldSettings.LanguageId, fieldSettings.ChangedDateAndTime);
            this.UpdateFieldSetting(settings, StateField.ChangedDate, fieldSettings.ChangedDate, fieldSettings.LanguageId, fieldSettings.ChangedDateAndTime);
            this.UpdateFieldSetting(settings, AddressField.City, fieldSettings.City, fieldSettings.LanguageId, fieldSettings.ChangedDateAndTime);
            this.UpdateFieldSetting(settings, GeneralField.Code, fieldSettings.Code, fieldSettings.LanguageId, fieldSettings.ChangedDateAndTime);
            this.UpdateFieldSetting(settings, StateField.CreatedDate, fieldSettings.CreatedDate, fieldSettings.LanguageId, fieldSettings.ChangedDateAndTime);
            this.UpdateFieldSetting(settings, OrganizationField.Region, fieldSettings.Region, fieldSettings.LanguageId, fieldSettings.ChangedDateAndTime);
            this.UpdateFieldSetting(settings, OrganizationField.Department, fieldSettings.Department, fieldSettings.LanguageId, fieldSettings.ChangedDateAndTime);
            this.UpdateFieldSetting(settings, GeneralField.DisplayName, fieldSettings.DisplayName, fieldSettings.LanguageId, fieldSettings.ChangedDateAndTime);
            this.UpdateFieldSetting(settings, OrganizationField.Division, fieldSettings.Division, fieldSettings.LanguageId, fieldSettings.ChangedDateAndTime);
            this.UpdateFieldSetting(settings, GeneralField.Domain, fieldSettings.Domain, fieldSettings.LanguageId, fieldSettings.ChangedDateAndTime);
            this.UpdateFieldSetting(settings, GeneralField.Email, fieldSettings.Email, fieldSettings.LanguageId, fieldSettings.ChangedDateAndTime);
            this.UpdateFieldSetting(settings, GeneralField.FirstName, fieldSettings.FirstName, fieldSettings.LanguageId, fieldSettings.ChangedDateAndTime);
            this.UpdateFieldSetting(settings, OrganizationField.Group, fieldSettings.Group, fieldSettings.LanguageId, fieldSettings.ChangedDateAndTime);
            this.UpdateFieldSetting(settings, GeneralField.Initials, fieldSettings.Initials, fieldSettings.LanguageId, fieldSettings.ChangedDateAndTime);
            this.UpdateFieldSetting(settings, GeneralField.LastName, fieldSettings.LastName, fieldSettings.LanguageId, fieldSettings.ChangedDateAndTime);
            this.UpdateFieldSetting(settings, GeneralField.LoginName, fieldSettings.LoginName, fieldSettings.LanguageId, fieldSettings.ChangedDateAndTime);
            this.UpdateFieldSetting(settings, OrganizationField.Manager, fieldSettings.Manager, fieldSettings.LanguageId, fieldSettings.ChangedDateAndTime);
            this.UpdateFieldSetting(settings, OrdererField.Ordered, fieldSettings.Ordered, fieldSettings.LanguageId, fieldSettings.ChangedDateAndTime);
            this.UpdateFieldSetting(settings, OrganizationField.OrganizationUnit, fieldSettings.OrganizationUnit, fieldSettings.LanguageId, fieldSettings.ChangedDateAndTime);
            this.UpdateFieldSetting(settings, OrganizationField.Other, fieldSettings.Other, fieldSettings.LanguageId, fieldSettings.ChangedDateAndTime);
            this.UpdateFieldSetting(settings, GeneralField.Phone, fieldSettings.Phone, fieldSettings.LanguageId, fieldSettings.ChangedDateAndTime);
            this.UpdateFieldSetting(settings, GeneralField.Place, fieldSettings.Place, fieldSettings.LanguageId, fieldSettings.ChangedDateAndTime);
            this.UpdateFieldSetting(settings, AddressField.PostalAddress, fieldSettings.PostalAddress, fieldSettings.LanguageId, fieldSettings.ChangedDateAndTime);
            this.UpdateFieldSetting(settings, AddressField.PostalCode, fieldSettings.PostalCode, fieldSettings.LanguageId, fieldSettings.ChangedDateAndTime);
            this.UpdateFieldSetting(settings, StateField.SynchronizationDate, fieldSettings.SynchronizationDate, fieldSettings.LanguageId, fieldSettings.ChangedDateAndTime);
            this.UpdateFieldSetting(settings, OrganizationField.Title, fieldSettings.Title, fieldSettings.LanguageId, fieldSettings.ChangedDateAndTime);
            this.UpdateFieldSetting(settings, OrganizationField.Unit, fieldSettings.Unit, fieldSettings.LanguageId, fieldSettings.ChangedDateAndTime);
            this.UpdateFieldSetting(settings, GeneralField.UserId, fieldSettings.UserId, fieldSettings.LanguageId, fieldSettings.ChangedDateAndTime);
        }

        public NotifierOverviewSettings FindDisplayFieldSettingsByCustomerIdAndLanguageId(int customerId, int languageId)
        {
            var settings = this.FindByCustomerId(customerId);

            var userId = this.CreateDisplayFieldSetting(settings, GeneralField.UserId, languageId);
            var domain = this.CreateDisplayFieldSetting(settings, GeneralField.Domain, languageId);
            var loginName = this.CreateDisplayFieldSetting(settings, GeneralField.LoginName, languageId);
            var firstName = this.CreateDisplayFieldSetting(settings, GeneralField.FirstName, languageId);
            var initials = this.CreateDisplayFieldSetting(settings, GeneralField.Initials, languageId);
            var lastName = this.CreateDisplayFieldSetting(settings, GeneralField.LastName, languageId);
            var displayName = this.CreateDisplayFieldSetting(settings, GeneralField.DisplayName, languageId);
            var place = this.CreateDisplayFieldSetting(settings, GeneralField.Place, languageId);
            var phone = this.CreateDisplayFieldSetting(settings, GeneralField.Phone, languageId);
            var cellPhone = this.CreateDisplayFieldSetting(settings, GeneralField.CellPhone, languageId);
            var email = this.CreateDisplayFieldSetting(settings, GeneralField.Email, languageId);
            var code = this.CreateDisplayFieldSetting(settings, GeneralField.Code, languageId);
            var postalAddress = this.CreateDisplayFieldSetting(settings, AddressField.PostalAddress, languageId);
            var postalCode = this.CreateDisplayFieldSetting(settings, AddressField.PostalCode, languageId);
            var city = this.CreateDisplayFieldSetting(settings, AddressField.City, languageId);
            var title = this.CreateDisplayFieldSetting(settings, OrganizationField.Title, languageId);
            var region = this.CreateDisplayFieldSetting(settings, OrganizationField.Region, languageId);
            var department = this.CreateDisplayFieldSetting(settings, OrganizationField.Department, languageId);
            var unit = this.CreateDisplayFieldSetting(settings, OrganizationField.Unit, languageId);
            var organizationUnit = this.CreateDisplayFieldSetting(settings, OrganizationField.OrganizationUnit, languageId);
            var division = this.CreateDisplayFieldSetting(settings, OrganizationField.Division, languageId);
            var manager = this.CreateDisplayFieldSetting(settings, OrganizationField.Manager, languageId);
            var group = this.CreateDisplayFieldSetting(settings, OrganizationField.Group, languageId);
            var other = this.CreateDisplayFieldSetting(settings, OrganizationField.Other, languageId);
            var ordered = this.CreateDisplayFieldSetting(settings, OrdererField.Ordered, languageId);
            var createdDate = this.CreateDisplayFieldSetting(settings, StateField.CreatedDate, languageId);
            var changedDate = this.CreateDisplayFieldSetting(settings, StateField.ChangedDate, languageId);
            var synchronizationDate = this.CreateDisplayFieldSetting(settings, StateField.SynchronizationDate, languageId);

            return new NotifierOverviewSettings(
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
                region,
                department,
                unit,
                organizationUnit,
                division,
                manager,
                group,
                other,
                ordered,
                createdDate,
                changedDate,
                synchronizationDate);
        }

        public FieldSettings FindByCustomerIdAndLanguageId(int customerId, int languageId)
        {
            var settings = this.FindByCustomerId(customerId);
            if (!settings.Any())
            {
                return FieldSettings.CreateEmpty();
            }

            var userId = this.CreateFieldSetting(settings, GeneralField.UserId, languageId);
            var domain = this.CreateFieldSetting(settings, GeneralField.Domain, languageId);
            var loginName = this.CreateFieldSetting(settings, GeneralField.LoginName, languageId);
            var firstName = this.CreateFieldSetting(settings, GeneralField.FirstName, languageId);
            var initials = this.CreateFieldSetting(settings, GeneralField.Initials, languageId);
            var lastName = this.CreateFieldSetting(settings, GeneralField.LastName, languageId);
            var displayName = this.CreateFieldSetting(settings, GeneralField.DisplayName, languageId);
            var place = this.CreateFieldSetting(settings, GeneralField.Place, languageId);
            var phone = this.CreateFieldSetting(settings, GeneralField.Phone, languageId);
            var cellPhone = this.CreateFieldSetting(settings, GeneralField.CellPhone, languageId);
            var email = this.CreateFieldSetting(settings, GeneralField.Email, languageId);
            var code = this.CreateFieldSetting(settings, GeneralField.Code, languageId);
            var postalAddress = this.CreateFieldSetting(settings, AddressField.PostalAddress, languageId);
            var postalCode = this.CreateFieldSetting(settings, AddressField.PostalCode, languageId);
            var city = this.CreateFieldSetting(settings, AddressField.City, languageId);
            var title = this.CreateFieldSetting(settings, OrganizationField.Title, languageId);
            var region = this.CreateFieldSetting(settings, OrganizationField.Region, languageId);
            var department = this.CreateFieldSetting(settings, OrganizationField.Department, languageId);
            var unit = this.CreateFieldSetting(settings, OrganizationField.Unit, languageId);
            var organizationUnit = this.CreateFieldSetting(settings, OrganizationField.OrganizationUnit, languageId);
            var division = this.CreateFieldSetting(settings, OrganizationField.Division, languageId);
            var manager = this.CreateFieldSetting(settings, OrganizationField.Manager, languageId);
            var group = this.CreateFieldSetting(settings, OrganizationField.Group, languageId);
            var other = this.CreateFieldSetting(settings, OrganizationField.Other, languageId);
            var ordered = this.CreateFieldSetting(settings, OrdererField.Ordered, languageId);
            var createdDate = this.CreateFieldSetting(settings, StateField.CreatedDate, languageId);
            var changedDate = this.CreateFieldSetting(settings, StateField.ChangedDate, languageId);
            var synchronizationDate = this.CreateFieldSetting(settings, StateField.SynchronizationDate, languageId);

            return FieldSettings.CreateForEdit(
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
                region,
                department,
                unit,
                organizationUnit,
                division,
                manager,
                group,
                other,
                ordered,
                createdDate,
                changedDate,
                synchronizationDate);
        }

        private static FieldProcessingSetting CreateDisplayRule(List<ComputerUserFieldSettings> settings, string fieldName)
        {
            var setting = FilterSettingByFieldName(settings, fieldName);
            if (setting == null)
            {
                return FieldProcessingSetting.CreateEmpty();
            }

            return new FieldProcessingSetting(setting.Show != 0, setting.Required != 0);
        }

        private FieldSetting CreateFieldSetting(List<ComputerUserFieldSettings> settings, string createSettingName, int languageId)
        {
            var setting = FilterSettingByFieldName(settings, createSettingName);
            if (setting == null)
            {
                return this.notifierFieldSettingsFactory.CreateEmpty();
            }

            var translation = this.GetTranslationBySettingIdAndLanguageId(setting.Id, languageId);
            if (translation == null)
            {
                return this.notifierFieldSettingsFactory.CreateEmpty();
            }

            return this.notifierFieldSettingsFactory.Create(
                                                    setting.Show != 0,
                                                    setting.ShowInList != 0,
                                                    translation.Label,
                                                    setting.Required != 0,
                                                    setting.LDAPAttribute);
        }
        
        private static ComputerUserFieldSettings FilterSettingByFieldName(List<ComputerUserFieldSettings> settings, string name)
        {
            return settings.SingleOrDefault(s => s.ComputerUserField.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }

        private ComputerUserFieldSettingsLanguage GetTranslationBySettingIdAndLanguageId(int settingId, int languageId)
        {
            return
                this.DataContext.ComputerUserFieldSettingsLanguages.SingleOrDefault(
                    t => t.ComputerUserFieldSettings_Id == settingId && t.Language_Id == languageId);
        }

        private FieldOverviewSetting CreateDisplayFieldSetting(List<ComputerUserFieldSettings> settings, string createSettingName, int languageId)
        {
            var setting = FilterSettingByFieldName(settings, createSettingName);
            if (setting == null)
            {
                return FieldOverviewSetting.CreateEmpty();
            }

            var translation = this.GetTranslationBySettingIdAndLanguageId(setting.Id, languageId);
            if (translation == null)
            {
                return FieldOverviewSetting.CreateEmpty();
            }

            return new FieldOverviewSetting(setting.Show != 0, translation.Label, setting.Required != 0);
        }
        
        private List<ComputerUserFieldSettings> FindByCustomerId(int customerId)
        {
            return this.DataContext.ComputerUserFieldSettings.Where(s => s.Customer_Id == customerId).ToList();
        } 

        private void UpdateFieldSetting(
            List<ComputerUserFieldSettings> settings,
            string updateSettingName,
            FieldSetting updatedSetting,
            int languageId,
            DateTime changedDateAndTime)
        {
            var setting = FilterSettingByFieldName(settings, updateSettingName);
            if (setting == null)
            {
                return;
            }

            setting.ChangedDate = changedDateAndTime;
            setting.LDAPAttribute = updatedSetting.LdapAttribute ?? string.Empty;

            var translation =
                this.DataContext.ComputerUserFieldSettingsLanguages.SingleOrDefault(
                    t => t.ComputerUserFieldSettings_Id == setting.Id && t.Language_Id == languageId);
            if (translation != null)
            {
                translation.Label = updatedSetting.Caption;                
            }

            setting.Required = updatedSetting.Required ? 1 : 0;
            setting.Show = updatedSetting.ShowInDetails ? 1 : 0;
            setting.ShowInList = updatedSetting.ShowInNotifiers ? 1 : 0;
        }
    }
}
