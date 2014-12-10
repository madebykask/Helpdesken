namespace DH.Helpdesk.Web.Areas.OrderAccounts.Infrastructure.ModelMappers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Enums.Accounts;
    using DH.Helpdesk.BusinessData.Enums.Accounts.Fields;
    using DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Read.ModelEdit;
    using DH.Helpdesk.BusinessData.Models.Accounts.Read.Edit;
    using DH.Helpdesk.BusinessData.Models.Accounts.Read.Overview;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Services.Response.Account;
    using DH.Helpdesk.Web.Areas.OrderAccounts.Models.Order.Edit;
    using DH.Helpdesk.Web.Areas.OrderAccounts.Models.Order.FieldModels;
    using DH.Helpdesk.Web.Infrastructure.Extensions;

    using AccountInformation = DH.Helpdesk.Web.Areas.OrderAccounts.Models.Order.Edit.AccountInformation;
    using Orderer = DH.Helpdesk.Web.Areas.OrderAccounts.Models.Order.Edit.Orderer;
    using User = DH.Helpdesk.Web.Areas.OrderAccounts.Models.Order.Edit.User;

    public interface IOrderModelMapper
    {
        AccountModel BuildViewModel(
            AccountForEdit model,
            AccountOptionsResponse options,
            AccountFieldsSettingsForModelEdit settings);

        AccountModel BuildViewModel(AccountOptionsResponse options, AccountFieldsSettingsForModelEdit settings);
    }

    public class OrderModelMapper : IOrderModelMapper
    {
        public AccountModel BuildViewModel(
            AccountForEdit model,
            AccountOptionsResponse options,
            AccountFieldsSettingsForModelEdit settings)
        {
            throw new NotImplementedException();
        }

        public AccountModel BuildViewModel(AccountOptionsResponse options, AccountFieldsSettingsForModelEdit settings)
        {
            throw new NotImplementedException();
        }

        private static Orderer MapOrderer(
            AccountForEdit model,
            AccountOptionsResponse options,
            AccountFieldsSettingsForModelEdit settings)
        {
            var id = CreateStringField(settings.Orderer.Id, model.Orderer.Id);
            var firstName = CreateStringField(settings.Orderer.FirstName, model.Orderer.FirstName);
            var lastName = CreateStringField(settings.Orderer.LastName, model.Orderer.LastName);
            var phone = CreateStringField(settings.Orderer.Phone, model.Orderer.Phone);
            var email = CreateStringField(settings.Orderer.Email, model.Orderer.Email);

            return new Orderer(id, firstName, lastName, phone, email);
        }

        private static User MapUser(
            AccountForEdit model,
            AccountOptionsResponse options,
            AccountFieldsSettingsForModelEdit settings)
        {
            var id = CreateListStringField(settings.User.Ids, model.User.Ids);
            var personnelNumber = CreateListStringField(
                settings.User.PersonalIdentityNumber,
                model.User.PersonalIdentityNumber);

            var firstName = CreateStringField(settings.User.FirstName, model.User.FirstName);
            var initials = CreateStringField(settings.User.Initials, model.User.Initials);
            var lastName = CreateStringField(settings.User.LastName, model.User.LastName);
            var phone = CreateStringField(settings.User.Phone, model.User.Phone);
            var extension = CreateStringField(settings.User.Extension, model.User.Extension);
            var eMail = CreateStringField(settings.User.EMail, model.User.EMail);
            var title = CreateStringField(settings.User.Title, model.User.Title);
            var location = CreateStringField(settings.User.Location, model.User.Location);
            var roomNumber = CreateStringField(settings.User.RoomNumber, model.User.RoomNumber);
            var postalAddress = CreateStringField(settings.User.PostalAddress, model.User.PostalAddress);

            var emplTypes = CreateSelectListField(
                settings.User.EmploymentType,
                options.EmploymentTypes,
                model.User.EmploymentType.ToString(CultureInfo.InvariantCulture));
            var regions = CreateSelectListField(
                settings.User.DepartmentId,
                options.Regions,
                model.User.RegionId.ToString());
            var departments1 = CreateSelectListField(
                settings.User.DepartmentId,
                options.Departments,
                model.User.DepartmentId.ToString());
            var units = CreateSelectListField(settings.User.UnitId, options.Units, model.User.UnitId.ToString());
            var departments2 = CreateSelectListField(
                settings.User.DepartmentId2,
                options.Departments,
                model.User.DepartmentId2.ToString());

            var emplTypeId = CreateNullableIntegerField(settings.User.EmploymentType, model.User.EmploymentType);
            var department1Id = CreateNullableIntegerField(settings.User.DepartmentId, model.User.DepartmentId);
            var unitId = CreateNullableIntegerField(settings.User.UnitId, model.User.UnitId);
            var department2Id = CreateNullableIntegerField(settings.User.DepartmentId2, model.User.DepartmentId2);

            var info = CreateStringField(settings.User.Info, model.User.Info);
            var responsibility = CreateStringField(settings.User.Responsibility, model.User.Responsibility);
            var activity = CreateStringField(settings.User.Activity, model.User.Activity);
            var manager = CreateStringField(settings.User.Manager, model.User.Manager);
            var referenceNumber = CreateStringField(settings.User.ReferenceNumber, model.User.ReferenceNumber);

            return new User(
                id,
                firstName,
                initials,
                lastName,
                personnelNumber,
                phone,
                extension,
                eMail,
                title,
                location,
                roomNumber,
                postalAddress,
                emplTypeId,
                department1Id,
                unitId,
                department2Id,
                info,
                responsibility,
                activity,
                manager,
                referenceNumber,
                emplTypes,
                departments1,
                units,
                departments2,
                model.User.RegionId,
                regions);
        }

        private static AccountInformation MapAccountInformation(
            AccountForEdit model,
            AccountOptionsResponse options,
            AccountFieldsSettingsForModelEdit settings)
        {
            var startedDate = CreateNullableDateTimeField(
                settings.AccountInformation.StartedDate,
                model.AccountInformation.StartedDate);
            var finishDate = CreateNullableDateTimeField(
                settings.AccountInformation.FinishDate,
                model.AccountInformation.FinishDate);

            var postTypes = CreateSelectListField(
                settings.AccountInformation.EMailTypeId,
                new EMailTypes(),
                model.AccountInformation.EMailTypeId.ToString(CultureInfo.InvariantCulture));
            var postTypeId = CreateNullableIntegerField(
                settings.AccountInformation.EMailTypeId,
                model.AccountInformation.EMailTypeId == 0 ? null : (int?)model.AccountInformation.EMailTypeId);

            var home = CreateBooleanField(
                settings.AccountInformation.HomeDirectory,
                model.AccountInformation.HomeDirectory);
            var profil = CreateBooleanField(settings.AccountInformation.Profile, model.AccountInformation.Profile);
            var number = CreateStringField(
                settings.AccountInformation.InventoryNumber,
                model.AccountInformation.InventoryNumber);
            var info = CreateStringField(settings.AccountInformation.Info, model.AccountInformation.Info);

            var accountTypeSelectList = CreateSelectListField(
                settings.AccountInformation.AccountTypeId,
                ExtractOverviews(options.AccountTypes, AccountTypes.AccountType),
                model.AccountInformation.AccountTypeId.ToString());
            var accountTypeId = CreateNullableIntegerField(
                settings.AccountInformation.AccountTypeId,
                model.AccountInformation.AccountTypeId);

            var accountType3SelectList = CreateSelectListField(
                settings.AccountInformation.AccountType3,
                ExtractOverviews(options.AccountTypes, AccountTypes.AccountType3),
                model.AccountInformation.AccountType3.ToString());
            var accountType3Id = CreateNullableIntegerField(
                settings.AccountInformation.AccountType3,
                model.AccountInformation.AccountType3);

            var accountType4SelectList = CreateSelectListField(
                settings.AccountInformation.AccountType4,
                ExtractOverviews(options.AccountTypes, AccountTypes.AccountType4),
                model.AccountInformation.AccountType4.ToString());
            var accountType4Id = CreateNullableIntegerField(
                settings.AccountInformation.AccountType4,
                model.AccountInformation.AccountType4);

            var accountType5SelectList = CreateSelectListField(
                settings.AccountInformation.AccountType5,
                ExtractOverviews(options.AccountTypes, AccountTypes.AccountType5),
                model.AccountInformation.AccountType5.ToString());
            var accountType5Id = CreateNullableIntegerField(
                settings.AccountInformation.AccountType5,
                model.AccountInformation.AccountType5);

            var accountType2SelectList = CreateMultiSelectListField(
                settings.AccountInformation.AccountType2,
                ExtractOverviews(options.AccountTypes, AccountTypes.AccountType2),
                model.AccountInformation.AccountType2);

            var accountType2Id = CreateListIntField(
                settings.AccountInformation.AccountType2,
                model.AccountInformation.AccountType2);

            return new AccountInformation(
                startedDate,
                finishDate,
                postTypeId,
                home,
                profil,
                number,
                accountTypeId,
                accountType2Id,
                accountType3Id,
                accountType4Id,
                accountType5Id,
                info,
                postTypes,
                accountTypeSelectList,
                accountType2SelectList,
                accountType3SelectList,
                accountType4SelectList,
                accountType5SelectList);
        }

        #region

        private static ConfigurableFieldModel<DateTime?> CreateNullableDateTimeField(
            FieldSetting setting,
            DateTime? value)
        {
            return !setting.IsShowInDetails
                       ? ConfigurableFieldModel<DateTime?>.CreateUnshowable()
                       : new ConfigurableFieldModel<DateTime?>(setting.Caption, value, setting.IsRequired);
        }

        private static ConfigurableFieldModel<DateTime> CreateDateTimeField(FieldSetting setting, DateTime value)
        {
            return !setting.IsShowInDetails
                       ? ConfigurableFieldModel<DateTime>.CreateUnshowable()
                       : new ConfigurableFieldModel<DateTime>(setting.Caption, value, setting.IsRequired);
        }

        private static SelectList CreateSelectListField(
            FieldSetting setting,
            List<ItemOverview> items,
            string selectedValue)
        {
            if (!setting.IsShowInDetails)
            {
                return new SelectList(Enumerable.Empty<SelectListItem>());
            }

            var list = new SelectList(items, "Value", "Name", selectedValue);
            return list;
        }

        private static MultiSelectList CreateMultiSelectListField(
           FieldSetting setting,
           List<ItemOverview> items,
           List<int> selectedValue)
        {
            if (!setting.IsShowInDetails)
            {
                return new MultiSelectList(Enumerable.Empty<SelectListItem>(), Enumerable.Empty<SelectListItem>());
            }


            var list = new MultiSelectList(items, "Value", "Name", selectedValue);
            return list;
        }

        private static SelectList CreateSelectListField(FieldSetting setting, Enum items, string selectedValue)
        {
            if (!setting.IsShowInDetails)
            {
                return new SelectList(Enumerable.Empty<SelectListItem>());
            }

            var list = items.ToSelectList(selectedValue);
            return list;
        }

        private static ConfigurableFieldModel<string> CreateStringField(FieldSetting setting, string value)
        {
            return !setting.IsShowInDetails
                       ? ConfigurableFieldModel<string>.CreateUnshowable()
                       : new ConfigurableFieldModel<string>(setting.Caption, value, setting.IsRequired);
        }

        private static ConfigurableFieldModel<bool> CreateBooleanField(FieldSetting setting, bool value)
        {
            return !setting.IsShowInDetails
                       ? ConfigurableFieldModel<bool>.CreateUnshowable()
                       : new ConfigurableFieldModel<bool>(setting.Caption, value, setting.IsRequired);
        }

        private static ConfigurableFieldModel<int> CreateIntegerField(FieldSetting setting, int value)
        {
            return !setting.IsShowInDetails
                       ? ConfigurableFieldModel<int>.CreateUnshowable()
                       : new ConfigurableFieldModel<int>(setting.Caption, value, setting.IsRequired);
        }

        private static ConfigurableFieldModel<int?> CreateNullableIntegerField(FieldSetting setting, int? value)
        {
            return !setting.IsShowInDetails
                       ? ConfigurableFieldModel<int?>.CreateUnshowable()
                       : new ConfigurableFieldModel<int?>(setting.Caption, value, setting.IsRequired);
        }

        private static ConfigurableFieldModel<List<string>> CreateListStringField(
            FieldSetting setting,
            List<string> value)
        {
            return !setting.IsShowInDetails
                       ? ConfigurableFieldModel<List<string>>.CreateUnshowable()
                       : new ConfigurableFieldModel<List<string>>(setting.Caption, value, setting.IsRequired);
        }

        private static ConfigurableFieldModel<List<int>> CreateListIntField(
            FieldSetting setting,
            List<int> value)
        {
            return !setting.IsShowInDetails
                       ? ConfigurableFieldModel<List<int>>.CreateUnshowable()
                       : new ConfigurableFieldModel<List<int>>(setting.Caption, value, setting.IsRequired);
        }

        private static List<ItemOverview> ExtractOverviews(
            List<AccountTypeOverview> overviews,
            AccountTypes accountType)
        {
            AccountTypeOverview accountTypeOverview = overviews.SingleOrDefault(x => x.AccountType == accountType);

            var accountTypes = accountTypeOverview != null && accountTypeOverview.ItemOverviews != null
                                   ? accountTypeOverview.ItemOverviews
                                   : new List<ItemOverview>();

            return accountTypes;
        }

        #endregion
    }
}