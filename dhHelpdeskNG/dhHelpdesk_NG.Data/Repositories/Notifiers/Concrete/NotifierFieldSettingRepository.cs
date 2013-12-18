namespace dhHelpdesk_NG.Data.Repositories.Notifiers.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Input;
    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Output;
    using dhHelpdesk_NG.Data.Enums.Notifiers;
    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain;
    using dhHelpdesk_NG.Domain.Notifiers;

    public sealed class NotifierFieldSettingRepository : RepositoryBase<ComputerUserFieldSettings>, INotifierFieldSettingRepository
    {
        public NotifierFieldSettingRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        private static FieldValidSetting CreateValidationSetting(List<ComputerUserFieldSettings> settings, string fieldName)
        {
            var setting =
                settings.Single(s => s.ComputerUserField.Equals(fieldName, StringComparison.InvariantCultureIgnoreCase));

            return new FieldValidSetting
                       {
                           MinLength = setting.MinLength,
                           Required = setting.Required != 0,
                           Show = setting.Show != 0
                       };
        }

        public FieldDisplayRulesDto FindFieldDisplayRulesByCustomerId(int customerId)
        {
            var settings = this.FindByCustomerId(customerId).ToList();

            var userId = CreateValidationSetting(settings, NotifierField.UserId);
            var domain = CreateValidationSetting(settings, NotifierField.Domain);
            var loginName = CreateValidationSetting(settings, NotifierField.LoginName);
            var firstName = CreateValidationSetting(settings, NotifierField.FirstName);
            var initials = CreateValidationSetting(settings, NotifierField.Initials);
            var lastName = CreateValidationSetting(settings, NotifierField.LastName);
            var displayName = CreateValidationSetting(settings, NotifierField.DisplayName);
            var place = CreateValidationSetting(settings, NotifierField.Place);
            var phone = CreateValidationSetting(settings, NotifierField.Phone);
            var cellPhone = CreateValidationSetting(settings, NotifierField.CellPhone);
            var email = CreateValidationSetting(settings, NotifierField.Email);
            var code = CreateValidationSetting(settings, NotifierField.Code);
            var postalAddress = CreateValidationSetting(settings, NotifierField.PostalAddress);
            var postalCode = CreateValidationSetting(settings, NotifierField.PostalCode);
            var city = CreateValidationSetting(settings, NotifierField.City);
            var title = CreateValidationSetting(settings, NotifierField.Title);
            var department = CreateValidationSetting(settings, NotifierField.Department);
            var unit = CreateValidationSetting(settings, NotifierField.Unit);
            var organizationUnit = CreateValidationSetting(settings, NotifierField.OrganizationUnit);
            var division = CreateValidationSetting(settings, NotifierField.Division);
            var manager = CreateValidationSetting(settings, NotifierField.Manager);
            var group = CreateValidationSetting(settings, NotifierField.Group);
            var password = CreateValidationSetting(settings, NotifierField.Password);
            var other = CreateValidationSetting(settings, NotifierField.Other);
            var ordered = CreateValidationSetting(settings, NotifierField.Ordered);

            return new FieldDisplayRulesDto
                       {
                           CellPhone = cellPhone,
                           City = city,
                           Code = code,
                           Department = department,
                           DisplayName = displayName,
                           Division = division,
                           Domain = domain,
                           Email = email,
                           FirstName = firstName,
                           Group = group,
                           Initials = initials,
                           LastName = lastName,
                           LoginName = loginName,
                           Manager = manager,
                           Ordered = ordered,
                           OrganizationUnit = organizationUnit,
                           Other = other,
                           Password = password,
                           Phone = phone,
                           Place = place,
                           PostalAddress = postalAddress,
                           PostalCode = postalCode,
                           Title = title,
                           Unit = unit,
                           UserId = userId
                       };
        }

        private void UpdateSetting(List<ComputerUserFieldSettings> settings, string updateSettingName, UpdatedFieldSettingDto updatedSetting, int languageId)
        {
            var setting =
                settings.Single(
                    s => s.ComputerUserField.Equals(updateSettingName, StringComparison.InvariantCultureIgnoreCase));
            
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

        private IQueryable<ComputerUserFieldSettings> FindByCustomerId(int customerId)
        {
            return this.DataContext.ComputerUserFieldSettings.Where(s => s.Customer_Id == customerId);
        } 

        public void UpdateSetting(UpdatedFieldsSettingsDto setting)
        {
            var settings = this.FindByCustomerId(setting.CustomerId).ToList();

            this.UpdateSetting(settings, NotifierField.PostalAddress, setting.PostalAddress, setting.LanguageId);
            this.UpdateSetting(settings, NotifierField.CellPhone, setting.CellPhone, setting.LanguageId);
            this.UpdateSetting(settings, NotifierField.ChangedDate, setting.ChangedDate, setting.LanguageId);
            this.UpdateSetting(settings, NotifierField.City, setting.City, setting.LanguageId);
            this.UpdateSetting(settings, NotifierField.Code, setting.Code, setting.LanguageId);
            this.UpdateSetting(settings, NotifierField.CreatedDate, setting.CreatedDate, setting.LanguageId);
            this.UpdateSetting(settings, NotifierField.Department, setting.Department, setting.LanguageId);
            this.UpdateSetting(settings, NotifierField.DisplayName, setting.DisplayName, setting.LanguageId);
            this.UpdateSetting(settings, NotifierField.Division, setting.Division, setting.LanguageId);
            this.UpdateSetting(settings, NotifierField.Domain, setting.Domain, setting.LanguageId);
            this.UpdateSetting(settings, NotifierField.Email, setting.Email, setting.LanguageId);
            this.UpdateSetting(settings, NotifierField.FirstName, setting.FirstName, setting.LanguageId);
            this.UpdateSetting(settings, NotifierField.Group, setting.Group, setting.LanguageId);
            this.UpdateSetting(settings, NotifierField.Initials, setting.Initials, setting.LanguageId);
            this.UpdateSetting(settings, NotifierField.LastName, setting.LastName, setting.LanguageId);
            this.UpdateSetting(settings, NotifierField.LoginName, setting.LoginName, setting.LanguageId);
            this.UpdateSetting(settings, NotifierField.Manager, setting.Manager, setting.LanguageId);
            this.UpdateSetting(settings, NotifierField.Ordered, setting.Ordered, setting.LanguageId);
            this.UpdateSetting(settings, NotifierField.OrganizationUnit, setting.OrganizationUnit, setting.LanguageId);
            this.UpdateSetting(settings, NotifierField.Other, setting.Other, setting.LanguageId);
            this.UpdateSetting(settings, NotifierField.Password, setting.Password, setting.LanguageId);
            this.UpdateSetting(settings, NotifierField.Phone, setting.Phone, setting.LanguageId);
            this.UpdateSetting(settings, NotifierField.Place, setting.Place, setting.LanguageId);
            this.UpdateSetting(settings, NotifierField.PostalCode, setting.PostalCode, setting.LanguageId);
            this.UpdateSetting(settings, NotifierField.SynchronizationDate, setting.SynchronizationDate, setting.LanguageId);
            this.UpdateSetting(settings, NotifierField.Title, setting.Title, setting.LanguageId);
            this.UpdateSetting(settings, NotifierField.Unit, setting.Unit, setting.LanguageId);
            this.UpdateSetting(settings, NotifierField.UserId, setting.UserId, setting.LanguageId);
        }

        private DisplayFieldSettingDto CreateDisplayFieldSetting(List<ComputerUserFieldSettings> settings, string createSettingName, int languageId)
        {
            var setting =
                settings.Single(
                    s => s.ComputerUserField.Equals(createSettingName, StringComparison.InvariantCultureIgnoreCase));

              var translation =
                this.DataContext.ComputerUserFieldSettingsLanguages.Single(
                    t => t.ComputerUserFieldSettings_Id == setting.Id && t.Language_Id == languageId);

            return new DisplayFieldSettingDto
                       {
                           Caption = translation.Label,
                           Required = setting.Required != 0,
                           Show = setting.Show != 0
                       };
        }

        public DisplayFieldSettingsDto FindDisplayFieldSettingsByCustomerIdAndLanguageId(int customerId, int languageId)
        {
            var settings = this.FindByCustomerId(customerId).ToList();

            var address = this.CreateDisplayFieldSetting(settings, NotifierField.PostalAddress, languageId);
            var cellPhone = this.CreateDisplayFieldSetting(settings, NotifierField.CellPhone, languageId);
            var changedDate = this.CreateDisplayFieldSetting(settings, NotifierField.ChangedDate, languageId);
            var city = this.CreateDisplayFieldSetting(settings, NotifierField.City, languageId);
            var code = this.CreateDisplayFieldSetting(settings, NotifierField.Code, languageId);
            var createdDate = this.CreateDisplayFieldSetting(settings, NotifierField.CreatedDate, languageId);
            var department = this.CreateDisplayFieldSetting(settings, NotifierField.Department, languageId);
            var displayName = this.CreateDisplayFieldSetting(settings, NotifierField.DisplayName, languageId);
            var division = this.CreateDisplayFieldSetting(settings, NotifierField.Division, languageId);
            var domain = this.CreateDisplayFieldSetting(settings, NotifierField.Domain, languageId);
            var email = this.CreateDisplayFieldSetting(settings, NotifierField.Email, languageId);
            var firstName = this.CreateDisplayFieldSetting(settings, NotifierField.FirstName, languageId);
            var group = this.CreateDisplayFieldSetting(settings, NotifierField. Group, languageId);
            var initials = this.CreateDisplayFieldSetting(settings, NotifierField.Initials, languageId);
            var lastName = this.CreateDisplayFieldSetting(settings, NotifierField.LastName, languageId);
            var loginName = this.CreateDisplayFieldSetting(settings, NotifierField.LoginName, languageId);
            var manager = this.CreateDisplayFieldSetting(settings, NotifierField.Manager, languageId);
            var ordered = this.CreateDisplayFieldSetting(settings, NotifierField.Ordered, languageId);
            var organizationUnit = this.CreateDisplayFieldSetting(settings, NotifierField.OrganizationUnit, languageId);
            var other = this.CreateDisplayFieldSetting(settings, NotifierField.Other, languageId);
            var password = this.CreateDisplayFieldSetting(settings, NotifierField.Password, languageId);
            var phone = this.CreateDisplayFieldSetting(settings, NotifierField.Phone, languageId);
            var place = this.CreateDisplayFieldSetting(settings, NotifierField.Place, languageId);
            var postalCode = this.CreateDisplayFieldSetting(settings, NotifierField.PostalCode, languageId);

            var synchronizationDate = this.CreateDisplayFieldSetting(
                settings, NotifierField.SynchronizationDate, languageId);

            var title = this.CreateDisplayFieldSetting(settings, NotifierField.Title, languageId);
            var unit = this.CreateDisplayFieldSetting(settings, NotifierField.Unit, languageId);
            var userId = this.CreateDisplayFieldSetting(settings, NotifierField.UserId, languageId);

            return new DisplayFieldSettingsDto
                       {
                           PostalAddress = address,
                           CellPhone = cellPhone,
                           ChangedDate = changedDate,
                           City = city,
                           Code = code,
                           CreatedDate = createdDate,
                           Department = department,
                           Division = division,
                           Domain = domain,
                           Email = email,
                           FirstName = firstName,
                           Group = group,
                           Initials = initials,
                           LastName = lastName,
                           LoginName = loginName,
                           Manager = manager,
                           DisplayName = displayName,
                           Ordered = ordered,
                           OrganizationUnit = organizationUnit,
                           Other = other,
                           Password = password,
                           Phone = phone,
                           Place = place,
                           PostalCode = postalCode,
                           SynchronizationDate = synchronizationDate,
                           Title = title,
                           Unit = unit,
                           UserId = userId
                       };
        }

        private FieldSettingDto CreateNotifierFieldSetting(List<ComputerUserFieldSettings> settings, string createSettingName, int languageId)
        {
            var setting =
                settings.Single(
                    s => s.ComputerUserField.Equals(createSettingName, StringComparison.InvariantCultureIgnoreCase));

              var translation =
                this.DataContext.ComputerUserFieldSettingsLanguages.Single(
                    t => t.ComputerUserFieldSettings_Id == setting.Id && t.Language_Id == languageId);

            return new FieldSettingDto
                       {
                           Caption = translation.Label,
                           LdapAttribute = setting.LDAPAttribute,
                           Name = setting.ComputerUserField,
                           Required = setting.Required != 0,
                           ShowInDetails = setting.Show != 0,
                           ShowInNotifiers = setting.ShowInList != 0
                       };
        }

        public FieldSettingsDto FindByCustomerIdAndLanguageId(int customerId, int languageId)
        {
            var settings = this.FindByCustomerId(customerId).ToList();

            var address = this.CreateNotifierFieldSetting(settings, NotifierField.PostalAddress, languageId);
            var cellPhone = this.CreateNotifierFieldSetting(settings, NotifierField.CellPhone, languageId);
            var changedDate = this.CreateNotifierFieldSetting(settings, NotifierField.ChangedDate, languageId);
            var city = this.CreateNotifierFieldSetting(settings, NotifierField.City, languageId);
            var code = this.CreateNotifierFieldSetting(settings, NotifierField.Code, languageId);
            var createdDate = this.CreateNotifierFieldSetting(settings, NotifierField.CreatedDate, languageId);
            var department = this.CreateNotifierFieldSetting(settings, NotifierField.Department, languageId);
            var displayName = this.CreateNotifierFieldSetting(settings, NotifierField.DisplayName, languageId);
            var division = this.CreateNotifierFieldSetting(settings, NotifierField.Division, languageId);
            var domain = this.CreateNotifierFieldSetting(settings, NotifierField.Domain, languageId);
            var email = this.CreateNotifierFieldSetting(settings, NotifierField.Email, languageId);
            var firstName = this.CreateNotifierFieldSetting(settings, NotifierField.FirstName, languageId);
            var group = this.CreateNotifierFieldSetting(settings, NotifierField.Group, languageId);
            var initials = this.CreateNotifierFieldSetting(settings, NotifierField.Initials, languageId);
            var lastName = this.CreateNotifierFieldSetting(settings, NotifierField.LastName, languageId);
            var loginName = this.CreateNotifierFieldSetting(settings, NotifierField.LoginName, languageId);
            var manager = this.CreateNotifierFieldSetting(settings, NotifierField.Manager, languageId);
            var ordered = this.CreateNotifierFieldSetting(settings, NotifierField.Ordered, languageId);
            var organizationUnit = this.CreateNotifierFieldSetting(settings, NotifierField.OrganizationUnit, languageId);
            var other = this.CreateNotifierFieldSetting(settings, NotifierField.Other, languageId);
            var password = this.CreateNotifierFieldSetting(settings, NotifierField.Password, languageId);
            var phone = this.CreateNotifierFieldSetting(settings, NotifierField.Phone, languageId);
            var place = this.CreateNotifierFieldSetting(settings, NotifierField.Place, languageId);
            var postalCode = this.CreateNotifierFieldSetting(settings, NotifierField.PostalCode, languageId);
            var synchronizationDate = this.CreateNotifierFieldSetting(settings, NotifierField.SynchronizationDate, languageId);
            var title = this.CreateNotifierFieldSetting(settings, NotifierField.Title, languageId);
            var unit = this.CreateNotifierFieldSetting(settings, NotifierField.Unit, languageId);
            var userId = this.CreateNotifierFieldSetting(settings, NotifierField.UserId, languageId);

            return new FieldSettingsDto
                       {
                           PostalAddress = address,
                           CellPhone = cellPhone,
                           ChangedDate = changedDate,
                           City = city,
                           Code = code,
                           CreatedDate = createdDate,
                           Department = department,
                           Division = division,
                           Domain = domain,
                           Email = email,
                           FirstName = firstName,
                           Group = group,
                           Initials = initials,
                           LastName = lastName,
                           LoginName = loginName,
                           Manager = manager,
                           DisplayName = displayName,
                           Ordered = ordered,
                           OrganizationUnit = organizationUnit,
                           Other = other,
                           Password = password,
                           Phone = phone,
                           Place = place,
                           PostalCode = postalCode,
                           SynchronizationDate = synchronizationDate,
                           Title = title,
                           Unit = unit,
                           UserId = userId
                       };
        }
    }

}
