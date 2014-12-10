namespace DH.Helpdesk.Mobile.Infrastructure.ModelFactories.Notifiers.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Notifiers.Settings;
    using DH.Helpdesk.BusinessData.Models.Notifiers.Settings.SettingsEdit;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Mobile.Infrastructure.Extensions.HtmlHelperExtensions.Content;
    using DH.Helpdesk.Mobile.Models.Notifiers;

    public sealed class SettingsModelFactory : ISettingsModelFactory
    {
        #region Public Methods and Operators

        public SettingsModel Create(FieldSettings settings, List<ItemOverview> languages, int selectedLanguageId)
        {
            var languageItems =
                languages.Select(
                    l => new DropDownItem(Translation.Get(l.Name, Enums.TranslationSource.TextTranslation), l.Value))
                    .ToList();

            var languageDropDownContent = new DropDownContent(
                languageItems,
                selectedLanguageId.ToString(CultureInfo.InvariantCulture));

            var userId = CreateSettingModel(settings.UserId);
            var domain = CreateSettingModel(settings.Domain);
            var loginName = CreateSettingModel(settings.LoginName);
            var firstName = CreateSettingModel(settings.FirstName);
            var initials = CreateSettingModel(settings.Initials);
            var lastName = CreateSettingModel(settings.LastName);
            var displayName = CreateSettingModel(settings.DisplayName);
            var place = CreateSettingModel(settings.Place);
            var phone = CreateSettingModel(settings.Phone);
            var cellPhone = CreateSettingModel(settings.CellPhone);
            var email = CreateSettingModel(settings.Email);
            var code = CreateSettingModel(settings.Code);
            var postalAddress = CreateSettingModel(settings.PostalAddress);
            var postalCode = CreateSettingModel(settings.PostalCode);
            var city = CreateSettingModel(settings.City);
            var title = CreateSettingModel(settings.Title);
            var region = CreateSettingModel(settings.Region);
            var department = CreateSettingModel(settings.Department);
            var unit = CreateSettingModel(settings.Unit);
            var organizationUnit = CreateSettingModel(settings.OrganizationUnit);
            var division = CreateSettingModel(settings.Division);
            var manager = CreateSettingModel(settings.Manager);
            var group = CreateSettingModel(settings.Group);
            var other = CreateSettingModel(settings.Other);
            var ordered = CreateSettingModel(settings.Ordered);
            var createdDate = CreateSettingModel(settings.CreatedDate);
            var changedDate = CreateSettingModel(settings.ChangedDate);
            var synchronizationDate = CreateSettingModel(settings.SynchronizationDate);

            return new SettingsModel(
                languageDropDownContent,
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

        #endregion

        #region Methods

        private static SettingModel CreateSettingModel(FieldSetting setting)
        {
            return new SettingModel(
                setting.ShowInDetails,
                setting.ShowInNotifiers,
                setting.Caption,
                setting.LableText,
                setting.Required,
                setting.LdapAttribute);
        }

        #endregion
    }
}