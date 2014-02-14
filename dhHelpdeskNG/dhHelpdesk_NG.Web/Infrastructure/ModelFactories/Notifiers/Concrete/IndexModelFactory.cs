namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Notifiers.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.BusinessData.Models.Notifiers.Output;
    using DH.Helpdesk.Web.Infrastructure.Extensions.HtmlHelperExtensions.Content;
    using DH.Helpdesk.Web.Infrastructure.Filters.Notifiers;
    using DH.Helpdesk.Web.Models.Notifiers.Output;

    public sealed class IndexModelFactory : IIndexModelFactory
    {
        private readonly INotifiersModelFactory notifiersModelFactory;

        public IndexModelFactory(INotifiersModelFactory notifiersModelFactory)
        {
            this.notifiersModelFactory = notifiersModelFactory;
        }

        public IndexModel Create(
            FieldSettingsDto displaySettings,
            List<ItemOverview> searchDomains,
            List<ItemOverview> searchRegions,
            List<ItemOverview> searchDepartments,
            List<ItemOverview> searchDivisions,
            NotifiersFilter predefinedFilters,
            Enums.Show showDefaultValue,
            int recordsOnPageDefaultValue,
            SearchResultDto searchResult,
            List<ItemOverview> languages,
            int selectedLanguageId)
        {
            var notifiersModel = this.notifiersModelFactory.Create(
                displaySettings,
                searchDomains,
                searchRegions,
                searchDepartments,
                searchDivisions,
                predefinedFilters,
                showDefaultValue,
                recordsOnPageDefaultValue,
                searchResult);

            var languageItems =
                languages.Select(
                    l => new DropDownItem(Translation.Get(l.Name, Enums.TranslationSource.TextTranslation), l.Value))
                         .ToList();

            var languageDropDownContent = new DropDownContent(
                languageItems, selectedLanguageId.ToString(CultureInfo.InvariantCulture));

            var userId = FieldSettingToModel(displaySettings.UserId);
            var domain = FieldSettingToModel(displaySettings.Domain);
            var loginName = FieldSettingToModel(displaySettings.LoginName);
            var firstName = FieldSettingToModel(displaySettings.FirstName);
            var initials = FieldSettingToModel(displaySettings.Initials);
            var lastName = FieldSettingToModel(displaySettings.LastName);
            var displayName = FieldSettingToModel(displaySettings.DisplayName);
            var place = FieldSettingToModel(displaySettings.Place);
            var phone = FieldSettingToModel(displaySettings.Phone);
            var cellPhone = FieldSettingToModel(displaySettings.CellPhone);
            var email = FieldSettingToModel(displaySettings.Email);
            var code = FieldSettingToModel(displaySettings.Code);
            var postalAddress = FieldSettingToModel(displaySettings.PostalAddress);
            var postalCode = FieldSettingToModel(displaySettings.PostalCode);
            var city = FieldSettingToModel(displaySettings.City);
            var title = FieldSettingToModel(displaySettings.Title);
            var department = FieldSettingToModel(displaySettings.Department);
            var unit = FieldSettingToModel(displaySettings.Unit);
            var organizationUnit = FieldSettingToModel(displaySettings.OrganizationUnit);
            var division = FieldSettingToModel(displaySettings.Division);
            var manager = FieldSettingToModel(displaySettings.Manager);
            var group = FieldSettingToModel(displaySettings.Group);
            var password = FieldSettingToModel(displaySettings.Password);
            var other = FieldSettingToModel(displaySettings.Other);
            var ordered = FieldSettingToModel(displaySettings.Ordered);
            var createdDate = FieldSettingToModel(displaySettings.CreatedDate);
            var changedDate = FieldSettingToModel(displaySettings.ChangedDate);
            var synchronizationDate = FieldSettingToModel(displaySettings.SynchronizationDate);

            var settingsModel = new SettingsModel(
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

            return new IndexModel(notifiersModel, settingsModel);
        }

        private static FieldSettingModel FieldSettingToModel(FieldSettingDto fieldSetting)
        {
            return new FieldSettingModel(
                fieldSetting.Name,
                fieldSetting.ShowInDetails,
                fieldSetting.ShowInNotifiers,
                fieldSetting.Caption,
                fieldSetting.Required,
                fieldSetting.LdapAttribute);
        }
    }
}