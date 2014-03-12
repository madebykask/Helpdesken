namespace DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Notifiers.Concrete
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Notifiers.Input;
    using DH.Helpdesk.Web.Models.Notifiers.Input;

    public sealed class UpdatedFieldSettingsFactory : IUpdatedFieldSettingsFactory
    {
        public UpdatedFieldSettings Convert(SettingsInputModel model, DateTime changedDateAndTime, int customerId)
        {
            var userId = CreateFieldSetting(model.UserId, changedDateAndTime);
            var domain = CreateFieldSetting(model.Domain, changedDateAndTime);
            var loginName = CreateFieldSetting(model.LoginName, changedDateAndTime);
            var firstName = CreateFieldSetting(model.FirstName, changedDateAndTime);
            var initials = CreateFieldSetting(model.Initials, changedDateAndTime);
            var lastName = CreateFieldSetting(model.LastName, changedDateAndTime);
            var displayName = CreateFieldSetting(model.DisplayName, changedDateAndTime);
            var place = CreateFieldSetting(model.Place, changedDateAndTime);
            var phone = CreateFieldSetting(model.Phone, changedDateAndTime);
            var cellPhone = CreateFieldSetting(model.CellPhone, changedDateAndTime);
            var email = CreateFieldSetting(model.Email, changedDateAndTime);
            var code = CreateFieldSetting(model.Code, changedDateAndTime);
            var postalAddress = CreateFieldSetting(model.PostalAddress, changedDateAndTime);
            var postalCode = CreateFieldSetting(model.PostalCode, changedDateAndTime);
            var city = CreateFieldSetting(model.City, changedDateAndTime);
            var title = CreateFieldSetting(model.Title, changedDateAndTime);
            var region = CreateFieldSetting(model.Region, changedDateAndTime);
            var department = CreateFieldSetting(model.Department, changedDateAndTime);
            var unit = CreateFieldSetting(model.Unit, changedDateAndTime);
            var organizationUnit = CreateFieldSetting(model.OrganizationUnit, changedDateAndTime);
            var division = CreateFieldSetting(model.Division, changedDateAndTime);
            var manager = CreateFieldSetting(model.Manager, changedDateAndTime);
            var group = CreateFieldSetting(model.Group, changedDateAndTime);
            var other = CreateFieldSetting(model.Other, changedDateAndTime);
            var ordered = CreateFieldSetting(model.Ordered, changedDateAndTime);
            var createdDate = CreateFieldSetting(model.CreatedDate, changedDateAndTime);
            var changedDate = CreateFieldSetting(model.ChangedDate, changedDateAndTime);
            var synchronizationDate = CreateFieldSetting(model.SynchronizationDate, changedDateAndTime);

            return new UpdatedFieldSettings(
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

        private static UpdatedFieldSetting CreateFieldSetting(FieldSettingInputModel model, DateTime changedDateTime)
        {
            return new UpdatedFieldSetting(
                model.ShowInDetails,
                model.ShowInNotifiers,
                model.Caption,
                model.Required,
                model.LdapAttribute,
                changedDateTime);
        }
    }
}