namespace DH.Helpdesk.SelfService.Infrastructure.BusinessModelFactories.Notifiers.Concrete
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Notifiers.Settings.SettingsEdit;
    using DH.Helpdesk.SelfService.Models.Notifiers;

    public sealed class UpdatedSettingsFactory : IUpdatedSettingsFactory
    {
        public FieldSettings Create(SettingsModel model, int customerId, DateTime changedDateAndTime)
        {
            var userId = CreateFieldSetting(model.UserId);
            var domain = CreateFieldSetting(model.Domain);
            var loginName = CreateFieldSetting(model.LoginName);
            var firstName = CreateFieldSetting(model.FirstName);
            var initials = CreateFieldSetting(model.Initials);
            var lastName = CreateFieldSetting(model.LastName);
            var displayName = CreateFieldSetting(model.DisplayName);
            var place = CreateFieldSetting(model.Place);
            var phone = CreateFieldSetting(model.Phone);
            var cellPhone = CreateFieldSetting(model.CellPhone);
            var email = CreateFieldSetting(model.Email);
            var code = CreateFieldSetting(model.Code);
            var postalAddress = CreateFieldSetting(model.PostalAddress);
            var postalCode = CreateFieldSetting(model.PostalCode);
            var city = CreateFieldSetting(model.City);
            var title = CreateFieldSetting(model.Title);
            var region = CreateFieldSetting(model.Region);
            var department = CreateFieldSetting(model.Department);
            var unit = CreateFieldSetting(model.Unit);
            var organizationUnit = CreateFieldSetting(model.OrganizationUnit);
            var costCentre = CreateFieldSetting(model.CostCentre);
            var division = CreateFieldSetting(model.Division);
            var manager = CreateFieldSetting(model.Manager);
            var group = CreateFieldSetting(model.Group);
            var other = CreateFieldSetting(model.Other);
            var ordered = CreateFieldSetting(model.Ordered);
            var createdDate = CreateFieldSetting(model.CreatedDate);
            var changedDate = CreateFieldSetting(model.ChangedDate);
            var synchronizationDate = CreateFieldSetting(model.SynchronizationDate);
            var lang = CreateFieldSetting(model.Lang);

            return FieldSettings.CreateUpdated(
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
                costCentre,
                division,
                manager,
                group,
                other,
                ordered,
                createdDate,
                changedDate,
                synchronizationDate,
                changedDateAndTime,
                lang);
        }

        private static FieldSetting CreateFieldSetting(SettingModel model)
        {
            return new FieldSetting(
                model.ShowInDetails,
                model.ShowInNotifiers,
                model.Caption,
                model.LableText,
                model.Required,
                model.LdapAttribute);
        }
    }
}