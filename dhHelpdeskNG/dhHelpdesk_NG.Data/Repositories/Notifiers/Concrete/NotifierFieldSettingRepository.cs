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
            var costCentre = CreateDisplayRule(settings, OrganizationField.CostCentre);
            var division = CreateDisplayRule(settings, OrganizationField.Division);
            var manager = CreateDisplayRule(settings, OrganizationField.Manager);
            var group = CreateDisplayRule(settings, OrganizationField.Group);
            var other = CreateDisplayRule(settings, OrganizationField.Other);
            var ordered = CreateDisplayRule(settings, OrdererField.Ordered);
            var changedDate = CreateDisplayRule(settings, StateField.ChangedDate);

            FieldProcessingSetting language = new FieldProcessingSetting(true, false);

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
                costCentre,
                division,
                manager,
                group,
                other,
                ordered,
                changedDate,
                language);
        }

        public void UpdateSettings(FieldSettings fieldSettings)
        {
            var settings = this.FindByCustomerId(fieldSettings.CustomerId);

            var supportedSettings = new Dictionary<string, FieldSetting>()
                                        {
                                            { GeneralField.CellPhone, fieldSettings.CellPhone },
                                            { StateField.ChangedDate, fieldSettings.ChangedDate },
                                            { AddressField.City, fieldSettings.City },
                                            { GeneralField.Code, fieldSettings.Code },
                                            { StateField.CreatedDate, fieldSettings.CreatedDate },
                                            { OrganizationField.Region, fieldSettings.Region },
                                            { OrganizationField.Department, fieldSettings.Department },
                                            { GeneralField.DisplayName, fieldSettings.DisplayName },
                                            { OrganizationField.Division, fieldSettings.Division },
                                            { GeneralField.Domain, fieldSettings.Domain },
                                            { GeneralField.Email, fieldSettings.Email },
                                            { GeneralField.FirstName, fieldSettings.FirstName },
                                            { OrganizationField.Group, fieldSettings.Group },
                                            { GeneralField.Initials, fieldSettings.Initials },
                                            { GeneralField.LastName, fieldSettings.LastName },
                                            { GeneralField.LoginName, fieldSettings.LoginName },
                                            { OrganizationField.Manager, fieldSettings.Manager },
                                            { OrdererField.Ordered, fieldSettings.Ordered },
                                            { OrganizationField.OrganizationUnit, fieldSettings.OrganizationUnit },
                                            { OrganizationField.CostCentre, fieldSettings.CostCentre },
                                            { OrganizationField.Other, fieldSettings.Other },
                                            { GeneralField.Phone, fieldSettings.Phone },
                                            { GeneralField.Place, fieldSettings.Place },
                                            { AddressField.PostalAddress, fieldSettings.PostalAddress },
                                            { AddressField.PostalCode, fieldSettings.PostalCode },
                                            { StateField.SynchronizationDate, fieldSettings.SynchronizationDate },
                                            { OrganizationField.Title, fieldSettings.Title },
                                            { OrganizationField.Unit, fieldSettings.Unit },
                                            { GeneralField.UserId, fieldSettings.UserId },
                                            {GeneralField.Language, fieldSettings.Language }
                                        };
            var settingsToAdd = new Dictionary<ComputerUserFieldSettings, ComputerUserFieldSettingsLanguage>();
            foreach (var supportedSetting in supportedSettings)
            {
                var settingName = supportedSetting.Key;
                var settingData = supportedSetting.Value;
                var setting = FilterSettingByFieldName(settings, settingName);
                if (setting == null)
                {
                    var newCustomerComputerUserFS = new ComputerUserFieldSettings() { };
                    newCustomerComputerUserFS.Customer_Id = fieldSettings.CustomerId;
                    newCustomerComputerUserFS.ComputerUserField = settingName;
                    newCustomerComputerUserFS.Show = settingData.ShowInDetails ? 1 : 0;
                    newCustomerComputerUserFS.Required = settingData.Required ? 1 : 0;
                    newCustomerComputerUserFS.MinLength = 0;
                    newCustomerComputerUserFS.ShowInList = settingData.ShowInNotifiers ? 1 : 0;
                    newCustomerComputerUserFS.LDAPAttribute = settingData.LdapAttribute ?? string.Empty;
                    this.DataContext.ComputerUserFieldSettings.Add(newCustomerComputerUserFS);
                    settingsToAdd.Add(newCustomerComputerUserFS, new ComputerUserFieldSettingsLanguage
                    {
                        Language_Id =
                                                                          fieldSettings
                                                                          .LanguageId,
                        Label =
                                                                          settingData
                                                                          .Caption
                    });
                }
                else
                {
                    this.UpdateFieldSetting(settings, supportedSetting.Key, supportedSetting.Value, fieldSettings.LanguageId, fieldSettings.ChangedDateAndTime);
                }
            }

            if (settingsToAdd.Count > 0)
            {
                this.DataContext.SaveChanges();
                foreach (var settingKV in settingsToAdd)
                {
                    settingKV.Value.ComputerUserFieldSettings_Id = settingKV.Key.Id;
                    this.DataContext.ComputerUserFieldSettingsLanguages.Add(settingKV.Value);
                }
                this.DataContext.SaveChanges();
            }
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
            var costCentre = this.CreateDisplayFieldSetting(settings, OrganizationField.CostCentre, languageId);
            var division = this.CreateDisplayFieldSetting(settings, OrganizationField.Division, languageId);
            var manager = this.CreateDisplayFieldSetting(settings, OrganizationField.Manager, languageId);
            var group = this.CreateDisplayFieldSetting(settings, OrganizationField.Group, languageId);
            var other = this.CreateDisplayFieldSetting(settings, OrganizationField.Other, languageId);
            var ordered = this.CreateDisplayFieldSetting(settings, OrdererField.Ordered, languageId);
            var createdDate = this.CreateDisplayFieldSetting(settings, StateField.CreatedDate, languageId);
            var changedDate = this.CreateDisplayFieldSetting(settings, StateField.ChangedDate, languageId);
            var synchronizationDate = this.CreateDisplayFieldSetting(settings, StateField.SynchronizationDate, languageId);
            var lang = this.CreateDisplayFieldSetting(settings, GeneralField.Language, languageId);

            //FieldOverviewSetting language = new FieldOverviewSetting(true,"LanguageId", false);            


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
                costCentre,
                division,
                manager,
                group,
                other,
                ordered,
                createdDate,
                changedDate,
                synchronizationDate,
                lang);
        }

        public FieldSettings FindByCustomerIdAndLanguageId(int customerId, int languageId)
        {
            var settings = this.FindByCustomerId(customerId);
            var userId = this.CreateFieldSetting(settings, GeneralField.UserId, languageId, GeneralFieldLabel.UserId);
            var domain = this.CreateFieldSetting(settings, GeneralField.Domain, languageId, GeneralFieldLabel.Domain);
            var loginName = this.CreateFieldSetting(settings, GeneralField.LoginName, languageId, GeneralFieldLabel.LoginName);
            var firstName = this.CreateFieldSetting(settings, GeneralField.FirstName, languageId, GeneralFieldLabel.FirstName);
            var initials = this.CreateFieldSetting(settings, GeneralField.Initials, languageId, GeneralFieldLabel.Initials);
            var lastName = this.CreateFieldSetting(settings, GeneralField.LastName, languageId, GeneralFieldLabel.LastName);
            var displayName = this.CreateFieldSetting(settings, GeneralField.DisplayName, languageId, GeneralFieldLabel.DisplayName);
            var place = this.CreateFieldSetting(settings, GeneralField.Place, languageId, GeneralFieldLabel.Place);
            var phone = this.CreateFieldSetting(settings, GeneralField.Phone, languageId, GeneralFieldLabel.Phone);
            var cellPhone = this.CreateFieldSetting(settings, GeneralField.CellPhone, languageId, GeneralFieldLabel.CellPhone);
            var email = this.CreateFieldSetting(settings, GeneralField.Email, languageId, GeneralFieldLabel.Email);
            var code = this.CreateFieldSetting(settings, GeneralField.Code, languageId, GeneralFieldLabel.Code);

            var postalAddress = this.CreateFieldSetting(settings, AddressField.PostalAddress, languageId, AddressFieldLable.PostalAddress);
            var postalCode = this.CreateFieldSetting(settings, AddressField.PostalCode, languageId, AddressFieldLable.PostalCode);
            var city = this.CreateFieldSetting(settings, AddressField.City, languageId, AddressFieldLable.City);

            var title = this.CreateFieldSetting(settings, OrganizationField.Title, languageId, OrganizationFieldLabel.Title);
            var region = this.CreateFieldSetting(settings, OrganizationField.Region, languageId, OrganizationFieldLabel.Region);
            var department = this.CreateFieldSetting(settings, OrganizationField.Department, languageId, OrganizationFieldLabel.Department);
            var unit = this.CreateFieldSetting(settings, OrganizationField.Unit, languageId, OrganizationFieldLabel.Unit);
            var organizationUnit = this.CreateFieldSetting(settings, OrganizationField.OrganizationUnit, languageId, OrganizationFieldLabel.OrganizationUnit);
            var costCentre = this.CreateFieldSetting(settings, OrganizationField.CostCentre, languageId, OrganizationFieldLabel.CostCentre);
            var division = this.CreateFieldSetting(settings, OrganizationField.Division, languageId, OrganizationFieldLabel.Division);
            var manager = this.CreateFieldSetting(settings, OrganizationField.Manager, languageId, OrganizationFieldLabel.Manager);
            var group = this.CreateFieldSetting(settings, OrganizationField.Group, languageId, OrganizationFieldLabel.Group);
            var other = this.CreateFieldSetting(settings, OrganizationField.Other, languageId, OrganizationFieldLabel.Other);

            var ordered = this.CreateFieldSetting(settings, OrdererField.Ordered, languageId, OrdererFieldLable.Ordered);

            var createdDate = this.CreateFieldSetting(settings, StateField.CreatedDate, languageId, StateFieldLable.CreatedDate);
            var changedDate = this.CreateFieldSetting(settings, StateField.ChangedDate, languageId, StateFieldLable.ChangedDate);
            var synchronizationDate = this.CreateFieldSetting(settings, StateField.SynchronizationDate, languageId, StateFieldLable.SynchronizationDate);


            var language = this.CreateFieldSetting(settings, GeneralField.LanguageId, languageId, GeneralFieldLabel.LanguageId);


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
                costCentre,
                division,
                manager,
                group,
                other,
                ordered,
                createdDate,
                changedDate,
                synchronizationDate,
                language);
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

        private FieldSetting CreateFieldSetting(List<ComputerUserFieldSettings> settings, string createSettingName, int languageId, string lableText)
        {
            var setting = FilterSettingByFieldName(settings, createSettingName);
            if (setting == null)
            {
                return this.notifierFieldSettingsFactory.CreateEmpty(lableText);
            }

            var translation = this.GetTranslationBySettingIdAndLanguageId(setting.Id, languageId);

            if (translation == null)
            {
                return this.notifierFieldSettingsFactory.CreateEmpty(lableText);
            }

            return this.notifierFieldSettingsFactory.Create(
                                                    setting.Show != 0,
                                                    setting.ShowInList != 0,
                                                    translation.Label,
                                                    lableText,
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
                DataContext.ComputerUserFieldSettingsLanguages.SingleOrDefault(
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
            else
            {
                ComputerUserFieldSettingsLanguage NewComputerUserFieldSettingLanguage = new ComputerUserFieldSettingsLanguage();
                NewComputerUserFieldSettingLanguage.ComputerUserFieldSettings_Id = setting.Id;
                NewComputerUserFieldSettingLanguage.Language_Id = languageId;
                NewComputerUserFieldSettingLanguage.Label = updatedSetting.Caption;
                this.DataContext.ComputerUserFieldSettingsLanguages.Add(NewComputerUserFieldSettingLanguage);
            }

            setting.Required = updatedSetting.Required ? 1 : 0;
            setting.Show = updatedSetting.ShowInDetails ? 1 : 0;
            setting.ShowInList = updatedSetting.ShowInNotifiers ? 1 : 0;
        }
    }
}
