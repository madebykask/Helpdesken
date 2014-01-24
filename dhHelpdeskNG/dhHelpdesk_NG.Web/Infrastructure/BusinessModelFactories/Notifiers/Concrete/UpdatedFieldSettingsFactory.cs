namespace dhHelpdesk_NG.Web.Infrastructure.BusinessModelFactories.Notifiers.Concrete
{
    using System;

    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Input;
    using dhHelpdesk_NG.Web.Models.Notifiers.Input;

    public sealed class UpdatedFieldSettingsFactory : IUpdatedFieldSettingsFactory
    {
        public UpdatedFieldSettingsDto Convert(SettingsInputModel model, DateTime changedDateTime, int customerId)
        {
            var userId = FieldSettingModelToDto(model.UserId, changedDateTime);
            var domain = FieldSettingModelToDto(model.Domain, changedDateTime);
            var loginName = FieldSettingModelToDto(model.LoginName, changedDateTime);
            var firstName = FieldSettingModelToDto(model.FirstName, changedDateTime);
            var initials = FieldSettingModelToDto(model.Initials, changedDateTime);
            var lastName = FieldSettingModelToDto(model.LastName, changedDateTime);
            var displayName = FieldSettingModelToDto(model.DisplayName, changedDateTime);
            var place = FieldSettingModelToDto(model.Place, changedDateTime);
            var phone = FieldSettingModelToDto(model.Phone, changedDateTime);
            var cellPhone = FieldSettingModelToDto(model.CellPhone, changedDateTime);
            var email = FieldSettingModelToDto(model.Email, changedDateTime);
            var code = FieldSettingModelToDto(model.Code, changedDateTime);
            var postalAddress = FieldSettingModelToDto(model.PostalAddress, changedDateTime);
            var postalCode = FieldSettingModelToDto(model.PostalCode, changedDateTime);
            var city = FieldSettingModelToDto(model.City, changedDateTime);
            var title = FieldSettingModelToDto(model.Title, changedDateTime);
            var department = FieldSettingModelToDto(model.Department, changedDateTime);
            var unit = FieldSettingModelToDto(model.Unit, changedDateTime);
            var organizationUnit = FieldSettingModelToDto(model.OrganizationUnit, changedDateTime);
            var division = FieldSettingModelToDto(model.Division, changedDateTime);
            var manager = FieldSettingModelToDto(model.Manager, changedDateTime);
            var group = FieldSettingModelToDto(model.Group, changedDateTime);
            var password = FieldSettingModelToDto(model.Password, changedDateTime);
            var other = FieldSettingModelToDto(model.Other, changedDateTime);
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