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
            FieldsSettingsDto fieldsSettings,
            List<ItemOverviewDto> searchDomains,
            List<ItemOverviewDto> searchRegions,
            List<ItemOverviewDto> searchDepartments,
            List<ItemOverviewDto> searchDivisions,
            Enums.Show show,
            int recordsOnPage,
            List<NotifierDetailedOverviewDto> notifiers)
        {
            var notifiersModel = this.notifiersModelFactory.Create(
                fieldsSettings,
                searchDomains,
                searchRegions,
                searchDepartments,
                searchDivisions,
                show,
                recordsOnPage,
                notifiers);

            var items = languages.Select(l => new DropDownItem(l.Name, l.Value)).ToList();
            var language = new DropDownContent(items, selectedLanguageId.ToString(CultureInfo.InvariantCulture));

            var userIdSetting = FieldSettingDtoToModel(fieldsSettings.UserId);
            var domainSetting = FieldSettingDtoToModel(fieldsSettings.Domain);
            var loginNameSetting = FieldSettingDtoToModel(fieldsSettings.LoginName);
            var firstNameSetting = FieldSettingDtoToModel(fieldsSettings.FirstName);
            var initialsSetting = FieldSettingDtoToModel(fieldsSettings.Initials);
            var lastNameSetting = FieldSettingDtoToModel(fieldsSettings.LastName);
            var displayNameSetting = FieldSettingDtoToModel(fieldsSettings.DisplayName);
            var placeSetting = FieldSettingDtoToModel(fieldsSettings.Place);
            var phoneSetting = FieldSettingDtoToModel(fieldsSettings.Phone);
            var cellPhoneSetting = FieldSettingDtoToModel(fieldsSettings.CellPhone);
            var emailSetting = FieldSettingDtoToModel(fieldsSettings.Email);
            var codeSetting = FieldSettingDtoToModel(fieldsSettings.Code);
            var postalAddressSetting = FieldSettingDtoToModel(fieldsSettings.PostalAddress);
            var postalCodeSetting = FieldSettingDtoToModel(fieldsSettings.PostalCode);
            var citySetting = FieldSettingDtoToModel(fieldsSettings.City);
            var titleSetting = FieldSettingDtoToModel(fieldsSettings.Title);
            var departmentSetting = FieldSettingDtoToModel(fieldsSettings.Department);
            var unitSetting = FieldSettingDtoToModel(fieldsSettings.Unit);
            var organizationUnitSetting = FieldSettingDtoToModel(fieldsSettings.OrganizationUnit);
            var divisionSetting = FieldSettingDtoToModel(fieldsSettings.Division);
            var managerSetting = FieldSettingDtoToModel(fieldsSettings.Manager);
            var groupSetting = FieldSettingDtoToModel(fieldsSettings.Group);
            var passwordSetting = FieldSettingDtoToModel(fieldsSettings.Password);
            var otherSetting = FieldSettingDtoToModel(fieldsSettings.Other);
            var orderedSetting = FieldSettingDtoToModel(fieldsSettings.Ordered);
            var createdDateSetting = FieldSettingDtoToModel(fieldsSettings.CreatedDate);
            var changedDateSetting = FieldSettingDtoToModel(fieldsSettings.ChangedDate);
            var synchronizationDateSetting = FieldSettingDtoToModel(fieldsSettings.SynchronizationDate);

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