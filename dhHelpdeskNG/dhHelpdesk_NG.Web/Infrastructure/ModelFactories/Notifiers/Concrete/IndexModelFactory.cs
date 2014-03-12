namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Notifiers.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Enums.Notifiers;
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
            FieldSettings settings,
            List<ItemOverview> searchDomains,
            List<ItemOverview> searchRegions,
            List<ItemOverview> searchDepartments,
            List<ItemOverview> searchDivisions,
            NotifiersFilter predefinedFilters,
            NotifierStatus statusDefaultValue,
            int recordsOnPageDefaultValue,
            SearchResult searchResult,
            List<ItemOverview> languages,
            int selectedLanguageId)
        {
            var notifiersModel = this.notifiersModelFactory.Create(
                settings,
                searchDomains,
                searchRegions,
                searchDepartments,
                searchDivisions,
                predefinedFilters,
                statusDefaultValue,
                recordsOnPageDefaultValue,
                searchResult);

            var languageItems =
                languages.Select(
                    l => new DropDownItem(Translation.Get(l.Name, Enums.TranslationSource.TextTranslation), l.Value))
                         .ToList();

            var languageDropDownContent = new DropDownContent(
                languageItems, selectedLanguageId.ToString(CultureInfo.InvariantCulture));

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

            return new IndexModel(notifiersModel, settingsModel);
        }

        private static SettingModel CreateSettingModel(FieldSetting fieldSetting)
        {
            return new SettingModel(
                fieldSetting.Name,
                fieldSetting.ShowInDetails,
                fieldSetting.ShowInNotifiers,
                fieldSetting.Caption,
                fieldSetting.Required,
                fieldSetting.LdapAttribute);
        }
    }
}