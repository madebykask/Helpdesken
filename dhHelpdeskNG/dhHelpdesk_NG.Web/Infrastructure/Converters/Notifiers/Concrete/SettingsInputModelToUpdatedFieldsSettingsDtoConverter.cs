namespace dhHelpdesk_NG.Web.Infrastructure.Converters.Notifiers.Concrete
{
    using System;

    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Input;
    using dhHelpdesk_NG.Web.Models.Notifiers.Input;

    public sealed class SettingsInputModelToUpdatedFieldsSettingsDtoConverter : ISettingsInputModelToUpdatedFieldsSettingsDtoConverter
    {
        public UpdatedFieldsSettingsDto Convert(SettingsInputModel model, DateTime changedDate, int customerId)
        {
            var userIdSetting = FieldSettingModelToDto(model.UserId, changedDate);
            var domainSetting = FieldSettingModelToDto(model.Domain, changedDate);
            var loginNameSetting = FieldSettingModelToDto(model.LoginName, changedDate);
            var firstNameSetting = FieldSettingModelToDto(model.FirstName, changedDate);
            var initialsSetting = FieldSettingModelToDto(model.Initials, changedDate);
            var lastNameSetting = FieldSettingModelToDto(model.LastName, changedDate);
            var displayNameSetting = FieldSettingModelToDto(model.DisplayName, changedDate);
            var placeSetting = FieldSettingModelToDto(model.Place, changedDate);
            var phoneSetting = FieldSettingModelToDto(model.Phone, changedDate);
            var cellPhoneSetting = FieldSettingModelToDto(model.CellPhone, changedDate);
            var emailSetting = FieldSettingModelToDto(model.Email, changedDate);
            var codeSetting = FieldSettingModelToDto(model.Code, changedDate);
            var postalAddressSetting = FieldSettingModelToDto(model.PostalAddress, changedDate);
            var postalCodeSetting = FieldSettingModelToDto(model.PostalCode, changedDate);
            var citySetting = FieldSettingModelToDto(model.City, changedDate);
            var titleSetting = FieldSettingModelToDto(model.Title, changedDate);
            var departmentSetting = FieldSettingModelToDto(model.Department, changedDate);
            var unitSetting = FieldSettingModelToDto(model.Unit, changedDate);
            var organizationUnitSetting = FieldSettingModelToDto(model.OrganizationUnit, changedDate);
            var divisionSetting = FieldSettingModelToDto(model.Division, changedDate);
            var managerSetting = FieldSettingModelToDto(model.Manager, changedDate);
            var groupSetting = FieldSettingModelToDto(model.Group, changedDate);
            var passwordSetting = FieldSettingModelToDto(model.Password, changedDate);
            var otherSetting = FieldSettingModelToDto(model.Other, changedDate);
            var orderedSetting = FieldSettingModelToDto(model.Ordered, changedDate);
            var createdDateSetting = FieldSettingModelToDto(model.CreatedDate, changedDate);
            var changedDateSetting = FieldSettingModelToDto(model.ChangedDate, changedDate);
            var synchronizationDateSetting = FieldSettingModelToDto(model.SynchronizationDate, changedDate);
            
            return new UpdatedFieldsSettingsDto(
                customerId,
                model.LanguageId,
                userIdSetting,
                domainSetting,
                loginNameSetting,
                firstNameSetting,
                initialsSetting,
                lastNameSetting,
                displayNameSetting,
                placeSetting,
                phoneSetting,
                cellPhoneSetting,
                emailSetting,
                codeSetting,
                postalAddressSetting,
                postalCodeSetting,
                citySetting,
                titleSetting,
                departmentSetting,
                unitSetting,
                organizationUnitSetting,
                divisionSetting,
                managerSetting,
                groupSetting,
                passwordSetting,
                otherSetting,
                orderedSetting,
                createdDateSetting,
                changedDateSetting,
                synchronizationDateSetting);
        }

        private static UpdatedFieldSettingDto FieldSettingModelToDto(SettingInputModel model, DateTime changedDate)
        {
            return new UpdatedFieldSettingDto(
                model.ShowInDetails,
                model.ShowInNotifiers,
                model.Caption,
                model.Required,
                model.LdapAttribute,
                changedDate);
        }
    }
}