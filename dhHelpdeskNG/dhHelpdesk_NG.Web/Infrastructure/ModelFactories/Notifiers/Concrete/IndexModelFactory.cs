namespace dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Notifiers.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using dhHelpdesk_NG.DTO.DTOs;
    using dhHelpdesk_NG.DTO.DTOs.Common.Output;
    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Output;
    using dhHelpdesk_NG.Web.Infrastructure.Extensions.HtmlHelperExtensions.Content;
    using dhHelpdesk_NG.Web.Models.Notifiers.Output;

    public sealed class IndexModelFactory : IIndexModelFactory
    {
        private readonly INotifiersModelFactory notifiersModelFactory;

        public IndexModelFactory(INotifiersModelFactory notifiersModelFactory)
        {
            this.notifiersModelFactory = notifiersModelFactory;
        }

        public IndexModel Create(
            List<ItemOverviewDto> languages,
            int selectedLanguageId,
            FieldSettingsDto fieldSettings,
            List<ItemOverviewDto> searchDomains,
            List<ItemOverviewDto> searchRegions,
            List<ItemOverviewDto> searchDepartments,
            List<ItemOverviewDto> searchDivisions,
            Enums.Show show,
            int recordsOnPage,
            List<NotifierDetailedOverviewDto> notifiers)
        {
            var notifiersModel = this.notifiersModelFactory.Create(
                fieldSettings,
                searchDomains,
                searchRegions,
                searchDepartments,
                searchDivisions,
                show,
                recordsOnPage,
                notifiers);

            var items = languages.Select(l => new DropDownItem(l.Name, l.Value)).ToList();
            var language = new DropDownContent(items, selectedLanguageId.ToString(CultureInfo.InvariantCulture));

            var userIdSetting = FieldSettingDtoToModel(fieldSettings.UserId);
            var domainSetting = FieldSettingDtoToModel(fieldSettings.Domain);
            var loginNameSetting = FieldSettingDtoToModel(fieldSettings.LoginName);
            var firstNameSetting = FieldSettingDtoToModel(fieldSettings.FirstName);
            var initialsSetting = FieldSettingDtoToModel(fieldSettings.Initials);
            var lastNameSetting = FieldSettingDtoToModel(fieldSettings.LastName);
            var displayNameSetting = FieldSettingDtoToModel(fieldSettings.DisplayName);
            var placeSetting = FieldSettingDtoToModel(fieldSettings.Place);
            var phoneSetting = FieldSettingDtoToModel(fieldSettings.Phone);
            var cellPhoneSetting = FieldSettingDtoToModel(fieldSettings.CellPhone);
            var emailSetting = FieldSettingDtoToModel(fieldSettings.Email);
            var codeSetting = FieldSettingDtoToModel(fieldSettings.Code);
            var postalAddressSetting = FieldSettingDtoToModel(fieldSettings.PostalAddress);
            var postalCodeSetting = FieldSettingDtoToModel(fieldSettings.PostalCode);
            var citySetting = FieldSettingDtoToModel(fieldSettings.City);
            var titleSetting = FieldSettingDtoToModel(fieldSettings.Title);
            var departmentSetting = FieldSettingDtoToModel(fieldSettings.Department);
            var unitSetting = FieldSettingDtoToModel(fieldSettings.Unit);
            var organizationUnitSetting = FieldSettingDtoToModel(fieldSettings.OrganizationUnit);
            var divisionSetting = FieldSettingDtoToModel(fieldSettings.Division);
            var managerSetting = FieldSettingDtoToModel(fieldSettings.Manager);
            var groupSetting = FieldSettingDtoToModel(fieldSettings.Group);
            var passwordSetting = FieldSettingDtoToModel(fieldSettings.Password);
            var otherSetting = FieldSettingDtoToModel(fieldSettings.Other);
            var orderedSetting = FieldSettingDtoToModel(fieldSettings.Ordered);
            var createdDateSetting = FieldSettingDtoToModel(fieldSettings.CreatedDate);
            var changedDateSetting = FieldSettingDtoToModel(fieldSettings.ChangedDate);
            var synchronizationDateSetting = FieldSettingDtoToModel(fieldSettings.SynchronizationDate);

            var settingsModel = new SettingsModel(
                language,
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

            return new IndexModel(notifiersModel, settingsModel);
        }

        private static SettingModel FieldSettingDtoToModel(FieldSettingDto dto)
        {
            return new SettingModel(
                dto.Name, dto.ShowInDetails, dto.ShowInNotifiers, dto.Caption, dto.Required, dto.LdapAttribute);
        }
    }
}