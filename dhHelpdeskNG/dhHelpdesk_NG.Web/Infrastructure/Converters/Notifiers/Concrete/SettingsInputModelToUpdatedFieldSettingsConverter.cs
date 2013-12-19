namespace dhHelpdesk_NG.Web.Infrastructure.Converters.Notifiers.Concrete
{
    using System;

    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Input;
    using dhHelpdesk_NG.Web.Models.Notifiers.Input;

    public sealed class SettingsInputModelToUpdatedFieldSettingsConverter : ISettingsInputModelToUpdatedFieldSettingsConverter
    {
        public UpdatedFieldSettingsDto Convert(SettingsInputModel model, DateTime changedDateTime, int customerId)
        {
            var userId = StringFieldSettingModelToDto(model.UserId, changedDateTime);
            var domain = FieldSettingModelToDto(model.Domain, changedDateTime);
            var loginName = StringFieldSettingModelToDto(model.LoginName, changedDateTime);
            var firstName = StringFieldSettingModelToDto(model.FirstName, changedDateTime);
            var initials = StringFieldSettingModelToDto(model.Initials, changedDateTime);
            var lastName = StringFieldSettingModelToDto(model.LastName, changedDateTime);
            var displayName = StringFieldSettingModelToDto(model.DisplayName, changedDateTime);
            var place = StringFieldSettingModelToDto(model.Place, changedDateTime);
            var phone = StringFieldSettingModelToDto(model.Phone, changedDateTime);
            var cellPhone = StringFieldSettingModelToDto(model.CellPhone, changedDateTime);
            var email = StringFieldSettingModelToDto(model.Email, changedDateTime);
            var code = StringFieldSettingModelToDto(model.Code, changedDateTime);
            var postalAddress = StringFieldSettingModelToDto(model.PostalAddress, changedDateTime);
            var postalCode = StringFieldSettingModelToDto(model.PostalCode, changedDateTime);
            var city = StringFieldSettingModelToDto(model.City, changedDateTime);
            var title = StringFieldSettingModelToDto(model.Title, changedDateTime);
            var department = FieldSettingModelToDto(model.Department, changedDateTime);
            var unit = StringFieldSettingModelToDto(model.Unit, changedDateTime);
            var organizationUnit = FieldSettingModelToDto(model.OrganizationUnit, changedDateTime);
            var division = FieldSettingModelToDto(model.Division, changedDateTime);
            var manager = FieldSettingModelToDto(model.Manager, changedDateTime);
            var group = FieldSettingModelToDto(model.Group, changedDateTime);
            var password = StringFieldSettingModelToDto(model.Password, changedDateTime);
            var other = StringFieldSettingModelToDto(model.Other, changedDateTime);
            var ordered = FieldSettingModelToDto(model.Ordered, changedDateTime);
            var createdDate = FieldSettingModelToDto(model.CreatedDate, changedDateTime);
            var changedDate = FieldSettingModelToDto(model.ChangedDate, changedDateTime);
            var synchronizationDate = FieldSettingModelToDto(model.SynchronizationDate, changedDateTime);
            
            return new UpdatedFieldSettingsDto(
                customerId,
                model.LanguageId,
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

        private static UpdatedStringFieldSettingDto StringFieldSettingModelToDto(StringFieldSettingInputModel model, DateTime changedDateTime)
        {
            return new UpdatedStringFieldSettingDto(
                model.ShowInDetails,
                model.ShowInNotifiers,
                model.Caption,
                model.Required,
                model.MinLength,
                model.LdapAttribute,
                changedDateTime);
        }

        private static UpdatedFieldSettingDto FieldSettingModelToDto(FieldSettingInputModel model, DateTime changedDateTime)
        {
            return new UpdatedFieldSettingDto(
                model.ShowInDetails,
                model.ShowInNotifiers,
                model.Caption,
                model.Required,
                model.LdapAttribute,
                changedDateTime);
        }
    }
}