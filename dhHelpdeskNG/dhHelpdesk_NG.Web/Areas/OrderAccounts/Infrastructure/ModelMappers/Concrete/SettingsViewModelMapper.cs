namespace DH.Helpdesk.Web.Areas.OrderAccounts.Infrastructure.ModelMappers.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Read.Edit;
    using DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Read.ModelEdit;
    using DH.Helpdesk.Web.Areas.OrderAccounts.Models.Settings.FieldSettings;

    using AccountInformationFieldSettings = DH.Helpdesk.Web.Areas.OrderAccounts.Models.Settings.FieldSettings.AccountInformationFieldSettings;
    using ContactFieldSettings = DH.Helpdesk.Web.Areas.OrderAccounts.Models.Settings.FieldSettings.ContactFieldSettings;
    using DeliveryInformationFieldSettings = DH.Helpdesk.Web.Areas.OrderAccounts.Models.Settings.FieldSettings.DeliveryInformationFieldSettings;
    using FieldSetting = DH.Helpdesk.Web.Areas.OrderAccounts.Models.Settings.FieldSettings.FieldSetting;
    using FieldSettingMultipleChoices = DH.Helpdesk.Web.Areas.OrderAccounts.Models.Settings.FieldSettings.FieldSettingMultipleChoices;
    using OrdererFieldSettings = DH.Helpdesk.Web.Areas.OrderAccounts.Models.Settings.FieldSettings.OrdererFieldSettings;
    using OtherFieldSettings = DH.Helpdesk.Web.Areas.OrderAccounts.Models.Settings.FieldSettings.OtherFieldSettings;
    using ProgramFieldSettings = DH.Helpdesk.Web.Areas.OrderAccounts.Models.Settings.FieldSettings.ProgramFieldSettings;
    using UserFieldSettings = DH.Helpdesk.Web.Areas.OrderAccounts.Models.Settings.FieldSettings.UserFieldSettings;

    public class SettingsViewModelMapper : ISettingsViewModelMapper
    {
        public AccountFieldsSettingsModel BuildModel(AccountFieldsSettingsForEdit settings, HeadersFieldSettings headers)
        {
            var orderer = MapOrdererFieldSettings(settings.Orderer);
            var user = MapUserFieldSettings(settings.User);
            var accountInformation = MapAccountInformationFieldSettings(settings.AccountInformation);
            var program = MapProgramFieldSettings(settings.Program);
            var other = MapOtherFieldSettings(settings.Other);
            var deliveryInformation = MapDeliveryInformationFieldSettings(settings.DeliveryInformation);
            var contact = MapContactFieldSettings(settings.Contact);

            return new AccountFieldsSettingsModel(
                orderer,
                user,
                accountInformation,
                contact,
                deliveryInformation,
                program,
                other,
                new HeadersFieldSettingsModel(
                    headers.OrderLabel,
                    headers.UserLabel,
                    headers.AccountLabel,
                    headers.ContactLabel,
                    headers.DeliveryLabel,
                    headers.ProgramLabel));
        }

        private static OrdererFieldSettings MapOrdererFieldSettings(
            BusinessData.Models.Accounts.AccountSettings.Read.Edit.OrdererFieldSettings settings)
        {
            var id = MapFieldSettings(settings.Id);
            var firstName = MapFieldSettings(settings.FirstName);
            var lastName = MapFieldSettings(settings.LastName);
            var phone = MapFieldSettings(settings.Phone);
            var email = MapFieldSettings(settings.Email);

            return new OrdererFieldSettings(id, firstName, lastName, phone, email);
        }

        private static UserFieldSettings MapUserFieldSettings(
            BusinessData.Models.Accounts.AccountSettings.Read.Edit.UserFieldSettings settings)
        {
            var ids = MapFieldSettingsMultipleChoices(settings.Ids);
            var firstName = MapFieldSettings(settings.FirstName);
            var initials = MapFieldSettings(settings.Initials);
            var lastName = MapFieldSettings(settings.LastName);
            var personalIdentityNumber = MapFieldSettingsMultipleChoices(settings.PersonalIdentityNumber);
            var phone = MapFieldSettings(settings.Phone);
            var extension = MapFieldSettings(settings.Extension);
            var eMail = MapFieldSettings(settings.EMail);
            var title = MapFieldSettings(settings.Title);
            var location = MapFieldSettings(settings.Location);
            var roomNumber = MapFieldSettings(settings.RoomNumber);
            var postalAddress = MapFieldSettings(settings.PostalAddress);
            var employmentType = MapFieldSettings(settings.EmploymentType);
            var departmentId = MapFieldSettings(settings.DepartmentId);
            var unitId = MapFieldSettings(settings.UnitId);
            var departmentId2 = MapFieldSettings(settings.DepartmentId2);
            var info = MapFieldSettings(settings.Info);
            var responsibility = MapFieldSettings(settings.Responsibility);
            var activity = MapFieldSettings(settings.Activity);
            var manager = MapFieldSettings(settings.Manager);
            var referenceNumber = MapFieldSettings(settings.ReferenceNumber);

            return new UserFieldSettings(
                ids,
                firstName,
                initials,
                lastName,
                personalIdentityNumber,
                phone,
                extension,
                eMail,
                title,
                location,
                roomNumber,
                postalAddress,
                employmentType,
                departmentId,
                unitId,
                departmentId2,
                info,
                responsibility,
                activity,
                manager,
                referenceNumber);
        }

        private static AccountInformationFieldSettings MapAccountInformationFieldSettings(
            BusinessData.Models.Accounts.AccountSettings.Read.Edit.AccountInformationFieldSettings settings)
        {
            var startedDate = MapFieldSettings(settings.StartedDate);
            var finishDate = MapFieldSettings(settings.FinishDate);
            var eMailTypeId = MapFieldSettings(settings.EMailTypeId);
            var homeDirectory = MapFieldSettings(settings.HomeDirectory);
            var profile = MapFieldSettings(settings.Profile);
            var inventoryNumber = MapFieldSettings(settings.InventoryNumber);
            var accountTypeId = MapFieldSettings(settings.AccountTypeId);
            var accountType2 = MapFieldSettings(settings.AccountType2);
            var accountType3 = MapFieldSettings(settings.AccountType3);
            var accountType4 = MapFieldSettings(settings.AccountType4);
            var accountType5 = MapFieldSettings(settings.AccountType5);
            var info = MapFieldSettings(settings.Info);

            return new AccountInformationFieldSettings(
                startedDate,
                finishDate,
                eMailTypeId,
                homeDirectory,
                profile,
                inventoryNumber,
                accountTypeId,
                accountType2,
                accountType3,
                accountType4,
                accountType5,
                info);
        }

        private static ProgramFieldSettings MapProgramFieldSettings(
            BusinessData.Models.Accounts.AccountSettings.Read.Edit.ProgramFieldSettings settings)
        {
            var programs = MapFieldSettings(settings.Programs);
            var infoProduct = MapFieldSettings(settings.InfoProduct);

            return new ProgramFieldSettings(programs, infoProduct);
        }

        private static OtherFieldSettings MapOtherFieldSettings(
            BusinessData.Models.Accounts.AccountSettings.Read.Edit.OtherFieldSettings settings)
        {
            var caseNumber = MapFieldSettings(settings.CaseNumber);
            var fileName = MapFieldSettings(settings.FileName);
            var info = MapFieldSettings(settings.Info);

            return new OtherFieldSettings(caseNumber, fileName, info);
        }

        private static DeliveryInformationFieldSettings MapDeliveryInformationFieldSettings(
            BusinessData.Models.Accounts.AccountSettings.Read.Edit.DeliveryInformationFieldSettings settings)
        {
            var name = MapFieldSettings(settings.Name);
            var phone = MapFieldSettings(settings.Phone);
            var address = MapFieldSettings(settings.Address);
            var postalAddress = MapFieldSettings(settings.PostalAddress);

            return new DeliveryInformationFieldSettings(name, phone, address, postalAddress);
        }

        private static ContactFieldSettings MapContactFieldSettings(
            BusinessData.Models.Accounts.AccountSettings.Read.Edit.ContactFieldSettings settings)
        {
            var id = MapFieldSettingsMultipleChoices(settings.Ids);
            var name = MapFieldSettings(settings.Name);
            var phone = MapFieldSettings(settings.Phone);
            var email = MapFieldSettings(settings.Email);

            return new ContactFieldSettings(id, name, phone, email);
        }

        private static FieldSetting MapFieldSettings(BusinessData.Models.Accounts.AccountSettings.FieldSetting setting)
        {
            var settingModel = new FieldSetting(
                setting.IsShowInDetails,
                setting.IsShowInList,
                setting.IsShowExternal,
                setting.Caption,
                setting.Help,
                setting.IsRequired);

            return settingModel;
        }

        private static FieldSettingMultipleChoices MapFieldSettingsMultipleChoices(
            BusinessData.Models.Accounts.AccountSettings.FieldSettingMultipleChoices setting)
        {
            var settingModel = new FieldSettingMultipleChoices(
                setting.IsShowInDetails,
                setting.IsShowInList,
                setting.IsShowExternal,
                setting.Caption,
                setting.Help,
                setting.IsRequired,
                setting.IsMultiple);

            return settingModel;
        }
    }
}