namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Accounts
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Dal.Enums.Accounts.Fields;
    using DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings;
    using DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Read.Edit;
    using DH.Helpdesk.Common.Collections;
    using DH.Helpdesk.Common.Extensions.Integer;
    using DH.Helpdesk.Domain.Accounts;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Accounts.MapperData;

    public static class AccountFieldSettingsForEditMapper
    {
        public static AccountFieldsSettingsForEdit ExtractOrdersFieldSettingsOverview(
            this IQueryable<AccountFieldSettings> query)
        {
            List<AccountSettingsMapperDataForEdit> mapperData =
                query.Select(
                    s =>
                    new AccountSettingsMapperDataForEdit
                        {
                            Caption = s.Label,
                            AccountName = s.AccountField,
                            ShowInList = s.ShowInList,
                            ShowInDetails = s.Show,
                            Required = s.Required,
                            Help = s.FieldHelp,
                            MultiValue = s.MultiValue
                        }).ToList();

            var settingCollection = new NamedObjectCollection<AccountSettingsMapperDataForEdit>(mapperData);
            return Map(settingCollection);
        }

        private static AccountFieldsSettingsForEdit Map(
            NamedObjectCollection<AccountSettingsMapperDataForEdit> settingCollection)
        {
            throw new System.NotImplementedException();
        }

        private static OrdererFieldSettings CreateOrdererFieldSettings(NamedObjectCollection<AccountSettingsMapperDataForEdit> entity)
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
            NamedObjectCollection<AccountSettingsMapperDataForEdit> entity)
        {
            var ids = CreateFieldSettingMultipleChoices(entity.FindByName(UserFields.Ids));
            var firstName = CreateFieldSetting(entity.FindByName(UserFields.FirstName));
            var initials = CreateFieldSetting(entity.FindByName(UserFields.Initials));
            var lastName = CreateFieldSetting(entity.FindByName(UserFields.LastName));
            var personalIdentityNumber = CreateFieldSettingMultipleChoices(entity.FindByName(UserFields.PersonalIdentityNumber));
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

        private static FieldSetting CreateFieldSetting(AccountSettingsMapperDataForEdit fieldSetting)
        {
            return new FieldSetting(
                fieldSetting.ShowInDetails.ToBool(),
                fieldSetting.ShowInList.ToBool(),
                fieldSetting.Caption,
                fieldSetting.Help,
                fieldSetting.Required.ToBool());
        }

        private static FieldSettingMultipleChoices CreateFieldSettingMultipleChoices(AccountSettingsMapperDataForEdit fieldSetting)
        {
            return new FieldSettingMultipleChoices(
                fieldSetting.ShowInDetails.ToBool(),
                fieldSetting.ShowInList.ToBool(),
                fieldSetting.Caption,
                fieldSetting.Help,
                fieldSetting.Required.ToBool(),
                fieldSetting.MultiValue.ToBool());
        }
    }
}
