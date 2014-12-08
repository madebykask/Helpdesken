namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Accounts
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Read.Overview;
    using DH.Helpdesk.Common.Collections;
    using DH.Helpdesk.Common.Extensions.Integer;
    using DH.Helpdesk.Dal.Enums.Accounts.Fields;
    using DH.Helpdesk.Domain.Accounts;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Accounts.MapperData;

    using FieldSetting = DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Read.Overview.FieldSetting;

    public static class AccountFieldSettingsOverviewMapper
    {
        public static AccountFieldsSettingsOverview ExtractOrdersFieldSettingsOverview(
            this IQueryable<AccountFieldSettings> query)
        {
            List<AccountSettingsMapperDataOverview> mapperData =
                query.Select(
                    s =>
                    new AccountSettingsMapperDataOverview
                        {
                            Caption = s.Label,
                            FieldName = s.AccountField,
                            ShowInList = s.ShowInList,
                        }).ToList();

            var settingCollection = new NamedObjectCollection<AccountSettingsMapperDataOverview>(mapperData);
            return Map(settingCollection);
        }

        private static AccountFieldsSettingsOverview Map(
            NamedObjectCollection<AccountSettingsMapperDataOverview> settingCollection)
        {
            OrdererFieldSettings ordererSettings = CreateOrdererFieldSettings(settingCollection);
            UserFieldSettings userSettings = CreateUserFieldSettings(settingCollection);
            AccountInformationFieldSettings accountInformationSettings =
                CreateAccountInformationFieldSettings(settingCollection);
            ProgramFieldSettings programSettings = CreateProgramFieldSettings(settingCollection);
            OtherFieldSettings otherSettings = CreateOtherFieldSettings(settingCollection);
            DeliveryInformationFieldSettings delveryInformationSettings =
                CreateDeliveryInformationFieldSettings(settingCollection);

            return new AccountFieldsSettingsOverview(
                ordererSettings,
                userSettings,
                accountInformationSettings,
                delveryInformationSettings,
                programSettings,
                otherSettings);
        }

        private static OrdererFieldSettings CreateOrdererFieldSettings(
            NamedObjectCollection<AccountSettingsMapperDataOverview> entity)
        {
            var id = CreateFieldSetting(entity.FindByName(OrdererFields.Id));
            var name = CreateFieldSetting(entity.FindByName(OrdererFields.FirstName));
            var lastName = CreateFieldSetting(entity.FindByName(OrdererFields.LastName));
            var phone = CreateFieldSetting(entity.FindByName(OrdererFields.Phone));
            var email = CreateFieldSetting(entity.FindByName(OrdererFields.Email));

            var settings = new OrdererFieldSettings(id, name, lastName, phone, email);

            return settings;
        }

        private static UserFieldSettings CreateUserFieldSettings(
            NamedObjectCollection<AccountSettingsMapperDataOverview> entity)
        {
            var ids = CreateFieldSetting(entity.FindByName(UserFields.Ids));
            var firstName = CreateFieldSetting(entity.FindByName(UserFields.FirstName));
            var initials = CreateFieldSetting(entity.FindByName(UserFields.Initials));
            var lastName = CreateFieldSetting(entity.FindByName(UserFields.LastName));
            var personalIdentityNumber =
                CreateFieldSetting(entity.FindByName(UserFields.PersonalIdentityNumber));
            var phone = CreateFieldSetting(entity.FindByName(UserFields.Phone));
            var extension = CreateFieldSetting(entity.FindByName(UserFields.Extension));
            var eMail = CreateFieldSetting(entity.FindByName(UserFields.EMail));
            var title = CreateFieldSetting(entity.FindByName(UserFields.Title));
            var location = CreateFieldSetting(entity.FindByName(UserFields.Location));
            var roomNumber = CreateFieldSetting(entity.FindByName(UserFields.RoomNumber));
            var postalAddress = CreateFieldSetting(entity.FindByName(UserFields.PostalAddress));
            var employmentType = CreateFieldSetting(entity.FindByName(UserFields.EmploymentType));
            var departmentId = CreateFieldSetting(entity.FindByName(UserFields.DepartmentId));
            var unitId = CreateFieldSetting(entity.FindByName(UserFields.UnitId));
            var departmentId2 = CreateFieldSetting(entity.FindByName(UserFields.DepartmentId2));
            var info = CreateFieldSetting(entity.FindByName(UserFields.Info));
            var responsibility = CreateFieldSetting(entity.FindByName(UserFields.Responsibility));
            var activity = CreateFieldSetting(entity.FindByName(UserFields.Activity));
            var manager = CreateFieldSetting(entity.FindByName(UserFields.Manager));
            var referenceNumber = CreateFieldSetting(entity.FindByName(UserFields.ReferenceNumber));

            var settings = new UserFieldSettings(
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

            return settings;
        }

        private static AccountInformationFieldSettings CreateAccountInformationFieldSettings(
            NamedObjectCollection<AccountSettingsMapperDataOverview> entity)
        {
            var startedDate = CreateFieldSetting(entity.FindByName(AccountInformationFields.StartedDate));
            var finishDate = CreateFieldSetting(entity.FindByName(AccountInformationFields.FinishDate));
            var eMailTypeId = CreateFieldSetting(entity.FindByName(AccountInformationFields.EMailTypeId));
            var homeDirectory =
                CreateFieldSetting(entity.FindByName(AccountInformationFields.HomeDirectory));
            var profile = CreateFieldSetting(entity.FindByName(AccountInformationFields.Profile));
            var inventoryNumber =
                CreateFieldSetting(entity.FindByName(AccountInformationFields.InventoryNumber));
            var accountTypeId =
                CreateFieldSetting(entity.FindByName(AccountInformationFields.AccountTypeId));
            var accountType2 =
                CreateFieldSetting(entity.FindByName(AccountInformationFields.AccountType2));
            var accountType3 =
                CreateFieldSetting(entity.FindByName(AccountInformationFields.AccountType3));
            var accountType4 =
                CreateFieldSetting(entity.FindByName(AccountInformationFields.AccountType4));
            var accountType5 =
                CreateFieldSetting(entity.FindByName(AccountInformationFields.AccountType5));
            var info = CreateFieldSetting(entity.FindByName(AccountInformationFields.Info));

            var settings = new AccountInformationFieldSettings(
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

            return settings;
        }

        private static ProgramFieldSettings CreateProgramFieldSettings(
            NamedObjectCollection<AccountSettingsMapperDataOverview> entity)
        {
            var programs = CreateFieldSetting(entity.FindByName(ProgramFields.Programs));
            var infoProduct = CreateFieldSetting(entity.FindByName(ProgramFields.InfoProduct));

            var settings = new ProgramFieldSettings(programs, infoProduct);

            return settings;
        }

        private static OtherFieldSettings CreateOtherFieldSettings(
            NamedObjectCollection<AccountSettingsMapperDataOverview> entity)
        {
            var caseNumber = CreateFieldSetting(entity.FindByName(OtherFields.CaseNumber));
            var fileName = CreateFieldSetting(entity.FindByName(OtherFields.FileName));
            var info = CreateFieldSetting(entity.FindByName(OtherFields.Info));

            var settings = new OtherFieldSettings(caseNumber, fileName, info);

            return settings;
        }

        private static DeliveryInformationFieldSettings CreateDeliveryInformationFieldSettings(
            NamedObjectCollection<AccountSettingsMapperDataOverview> entity)
        {
            var name = CreateFieldSetting(entity.FindByName(DeliveryInformationFields.Name));
            var phone = CreateFieldSetting(entity.FindByName(DeliveryInformationFields.Phone));
            var address = CreateFieldSetting(entity.FindByName(DeliveryInformationFields.Address));
            var postalAddress =
                CreateFieldSetting(entity.FindByName(DeliveryInformationFields.PostalAddress));

            var settings = new DeliveryInformationFieldSettings(name, phone, address, postalAddress);

            return settings;
        }

        private static FieldSetting CreateFieldSetting(AccountSettingsMapperDataOverview fieldSetting)
        {
            return new FieldSetting(
                fieldSetting.ShowInList.ToBool(),
                fieldSetting.Caption);
        }
    }
}
