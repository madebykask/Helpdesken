using DH.Helpdesk.Domain.Orders;

namespace DH.Helpdesk.Web.Areas.OrderAccounts.Infrastructure.ModelMappers.Concrete
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
    using DH.Helpdesk.BusinessData.Models.User.Input;
    using DH.Helpdesk.Services.Response.Account;
    using DH.Helpdesk.Web.Areas.OrderAccounts.Models.Order.Edit;
    using DH.Helpdesk.Web.Areas.OrderAccounts.Models.Order.FieldModels;
    using DH.Helpdesk.Web.Infrastructure.Extensions;

    using AccountInformation = DH.Helpdesk.Web.Areas.OrderAccounts.Models.Order.Edit.AccountInformation;
    using Contact = DH.Helpdesk.Web.Areas.OrderAccounts.Models.Order.Edit.Contact;
    using DeliveryInformation = DH.Helpdesk.Web.Areas.OrderAccounts.Models.Order.Edit.DeliveryInformation;
    using Orderer = DH.Helpdesk.Web.Areas.OrderAccounts.Models.Order.Edit.Orderer;
    using Other = DH.Helpdesk.Web.Areas.OrderAccounts.Models.Order.Edit.Other;
    using Program = DH.Helpdesk.Web.Areas.OrderAccounts.Models.Order.Edit.Program;
    using User = DH.Helpdesk.Web.Areas.OrderAccounts.Models.Order.Edit.User;

    public class OrderModelMapper : IOrderModelMapper
    {
        public AccountModel BuildViewModel(
            AccountForEdit model,
            AccountOptionsResponse options,
            AccountFieldsSettingsForModelEdit settings,
            HeadersFieldSettings headers)
        {
            var order = MapOrderer(model, options, settings);
            var user = MapUser(model, options, settings);
            var account = MapAccountInformation(model, options, settings);
            var contact = MapContact(model, options, settings);
            var delivery = MapDeliveryInformation(model, options, settings);
            var program = MapProgram(model, options, settings);
            var other = MapOther(model, options, settings);

            return new AccountModel(order, user, account, contact, delivery, program, other)
                       {
                           Id = model.Id,
                           FinishDate =
                               model.FinishingDate,
                           ActivityTypeId =
                               model.ActivityId,
                           ActivityName =
                               model.ActivityName,
                           CreatedDate =
                               model.CreatedDate,
                           ChangedDate =
                               model.ChangedDate,
                           ChangedByUserName =
                               model.ChangedByUser,
                           Headers = headers,
                           Guid =
                               model.Id.ToString(
                                   CultureInfo
                               .InvariantCulture)
                       };
        }

        public AccountModel BuildViewModel(
            int activityId,
            AccountOptionsResponse options,
            AccountFieldsSettingsForModelEdit settings,
            UserOverview userOverview,
            HeadersFieldSettings headers)
        {
            var order = MapOrderer(options, settings, userOverview);
            var user = MapUser(options, settings);
            var account = MapAccountInformation(options, settings);
            var contact = MapContact(options, settings);
            var delivery = MapDeliveryInformation(options, settings);
            var program = MapProgram(options, settings);

            var guid = Guid.NewGuid().ToString();
            var other = MapOther(options, settings, guid);

            return new AccountModel(order, user, account, contact, delivery, program, other)
            {
                ActivityTypeId = activityId,
                Headers = headers,
                Guid = guid
            };
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

            var id = CreateStringField(settings.User.Ids, GetIds(model.User.Ids));

            var idMultiple = CreateMultipleStringField(id, settings.User.Ids.IsMultiple);

            var personnelNumber = CreateStringField(
                settings.User.PersonalIdentityNumber,
                GetIds(model.User.PersonalIdentityNumber));

            var personnelNumberMultiple = CreateMultipleStringField(
                personnelNumber,
                settings.User.PersonalIdentityNumber.IsMultiple);

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
                idMultiple,
                firstName,
                initials,
                lastName,
                personnelNumberMultiple,
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

        private static Contact MapContact(
            AccountForEdit model,
            AccountOptionsResponse options,
            AccountFieldsSettingsForModelEdit settings)
        {
            var id = CreateStringField(settings.Contact.Ids, GetIds(model.Contact.Ids));

            var idMultiple = CreateMultipleStringField(id, settings.Contact.Ids.IsMultiple);

            var name = CreateStringField(settings.Contact.Name, model.Contact.Name);
            var phone = CreateStringField(settings.Contact.Phone, model.Contact.Phone);
            var email = CreateStringField(settings.Contact.Email, model.Contact.Email);

            return new Contact(idMultiple, name, phone, email);
        }

        private static DeliveryInformation MapDeliveryInformation(
            AccountForEdit model,
            AccountOptionsResponse options,
            AccountFieldsSettingsForModelEdit settings)
        {
            var name = CreateStringField(settings.DeliveryInformation.Name, model.DeliveryInformation.Name);
            var phone = CreateStringField(settings.DeliveryInformation.Phone, model.DeliveryInformation.Phone);
            var addres = CreateStringField(settings.DeliveryInformation.Address, model.DeliveryInformation.Address);
            var postalAddres = CreateStringField(
                settings.DeliveryInformation.PostalAddress,
                model.DeliveryInformation.PostalAddress);

            return new DeliveryInformation(name, phone, addres, postalAddres);
        }

        private static Program MapProgram(
            AccountForEdit model,
            AccountOptionsResponse options,
            AccountFieldsSettingsForModelEdit settings)
        {
            var programsSelectList = CreateMultiSelectListField(
                settings.Program.Programs,
                options.Programs,
                model.Program.ProgramIds);

            var programId = CreateListIntField(settings.Program.Programs, model.Program.ProgramIds);

            var info = CreateStringField(settings.Program.InfoProduct, model.Program.InfoProduct);

            return new Program(info, programId, programsSelectList);
        }

        private static Other MapOther(
            AccountForEdit model,
            AccountOptionsResponse options,
            AccountFieldsSettingsForModelEdit settings)
        {
            var caseNumber = CreateNullableDecimalField(settings.Other.CaseNumber, model.Other.CaseNumber);
            var info = CreateStringField(settings.Other.Info, model.Other.Info);

            ConfigurableFieldModel<FilesModel> fileName;
            if (settings.Other.FileName.IsShowInDetails)
            {
                var fileNameModel = new FilesModel(
                    model.Id.ToString(CultureInfo.InvariantCulture),
                    model.Other.FileName);
                fileName = new ConfigurableFieldModel<FilesModel>(
                    settings.Other.FileName.Caption,
                    fileNameModel,
                    settings.Other.FileName.IsRequired);
            }
            else
            {
                fileName = ConfigurableFieldModel<FilesModel>.CreateUnshowable();
            }

            return new Other(caseNumber, info, fileName);
        }

        private static Orderer MapOrderer(
            AccountOptionsResponse options,
            AccountFieldsSettingsForModelEdit settings,
            UserOverview user)
        {
            var id = CreateStringField(settings.Orderer.Id, user.UserId);
            var firstName = CreateStringField(settings.Orderer.FirstName, user.FirstName);
            var lastName = CreateStringField(settings.Orderer.LastName, user.SurName);
            var phone = CreateStringField(settings.Orderer.Phone, user.Phone);
            var email = CreateStringField(settings.Orderer.Email, user.Email);

            return new Orderer(id, firstName, lastName, phone, email);
        }

        private static User MapUser(AccountOptionsResponse options, AccountFieldsSettingsForModelEdit settings)
        {
            var id = CreateStringField(settings.User.Ids, null);

            var idMultiple = CreateMultipleStringField(id, settings.User.Ids.IsMultiple);

            var personnelNumber = CreateStringField(settings.User.PersonalIdentityNumber, null);

            var personnelNumberMultiple = CreateMultipleStringField(
                personnelNumber,
                settings.User.PersonalIdentityNumber.IsMultiple);

            var firstName = CreateStringField(settings.User.FirstName, null);
            var initials = CreateStringField(settings.User.Initials, null);
            var lastName = CreateStringField(settings.User.LastName, null);
            var phone = CreateStringField(settings.User.Phone, null);
            var extension = CreateStringField(settings.User.Extension, null);
            var eMail = CreateStringField(settings.User.EMail, null);
            var title = CreateStringField(settings.User.Title, null);
            var location = CreateStringField(settings.User.Location, null);
            var roomNumber = CreateStringField(settings.User.RoomNumber, null);
            var postalAddress = CreateStringField(settings.User.PostalAddress, null);

            var emplTypes = CreateSelectListField(settings.User.EmploymentType, options.EmploymentTypes, null);
            var regions = CreateSelectListField(settings.User.DepartmentId, options.Regions, null);
            var departments1 = CreateSelectListField(settings.User.DepartmentId, options.Departments, null);
            var units = CreateSelectListField(settings.User.UnitId, options.Units, null);
            var departments2 = CreateSelectListField(settings.User.DepartmentId2, options.Departments, null);

            var emplTypeId = CreateNullableIntegerField(settings.User.EmploymentType, null);
            var department1Id = CreateNullableIntegerField(settings.User.DepartmentId, null);
            var unitId = CreateNullableIntegerField(settings.User.UnitId, null);
            var department2Id = CreateNullableIntegerField(settings.User.DepartmentId2, null);

            var info = CreateStringField(settings.User.Info, null);
            var responsibility = CreateStringField(settings.User.Responsibility, null);
            var activity = CreateStringField(settings.User.Activity, null);
            var manager = CreateStringField(settings.User.Manager, null);
            var referenceNumber = CreateStringField(settings.User.ReferenceNumber, null);

            return new User(
                idMultiple,
                firstName,
                initials,
                lastName,
                personnelNumberMultiple,
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
                null,
                regions);
        }

        private static AccountInformation MapAccountInformation(
            AccountOptionsResponse options,
            AccountFieldsSettingsForModelEdit settings)
        {
            var startedDate = CreateNullableDateTimeField(settings.AccountInformation.StartedDate, null);
            var finishDate = CreateNullableDateTimeField(settings.AccountInformation.FinishDate, null);

            var postTypes = CreateSelectListField(settings.AccountInformation.EMailTypeId, new EMailTypes(), null);
            var postTypeId = CreateNullableIntegerField(settings.AccountInformation.EMailTypeId, null);

            var home = CreateBooleanField(settings.AccountInformation.HomeDirectory, false);
            var profil = CreateBooleanField(settings.AccountInformation.Profile, false);

            var number = CreateStringField(settings.AccountInformation.InventoryNumber, null);
            var info = CreateStringField(settings.AccountInformation.Info, null);

            var accountTypeSelectList = CreateSelectListField(
                settings.AccountInformation.AccountTypeId,
                ExtractOverviews(options.AccountTypes, AccountTypes.AccountType),
                null);
            var accountTypeId = CreateNullableIntegerField(settings.AccountInformation.AccountTypeId, null);

            var accountType3SelectList = CreateSelectListField(
                settings.AccountInformation.AccountType3,
                ExtractOverviews(options.AccountTypes, AccountTypes.AccountType3),
                null);
            var accountType3Id = CreateNullableIntegerField(settings.AccountInformation.AccountType3, null);

            var accountType4SelectList = CreateSelectListField(
                settings.AccountInformation.AccountType4,
                ExtractOverviews(options.AccountTypes, AccountTypes.AccountType4),
                null);
            var accountType4Id = CreateNullableIntegerField(settings.AccountInformation.AccountType4, null);

            var accountType5SelectList = CreateSelectListField(
                settings.AccountInformation.AccountType5,
                ExtractOverviews(options.AccountTypes, AccountTypes.AccountType5),
                null);
            var accountType5Id = CreateNullableIntegerField(settings.AccountInformation.AccountType5, null);

            var accountType2SelectList = CreateMultiSelectListField(
                settings.AccountInformation.AccountType2,
                ExtractOverviews(options.AccountTypes, AccountTypes.AccountType2),
                null);

            var accountType2Id = CreateListIntField(settings.AccountInformation.AccountType2, null);

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

        private static Contact MapContact(AccountOptionsResponse options, AccountFieldsSettingsForModelEdit settings)
        {
            var id = CreateStringField(settings.Contact.Ids, null);

            var idMultiple = CreateMultipleStringField(id, settings.Contact.Ids.IsMultiple);

            var name = CreateStringField(settings.Contact.Name, null);
            var phone = CreateStringField(settings.Contact.Phone, null);
            var email = CreateStringField(settings.Contact.Email, null);

            return new Contact(idMultiple, name, phone, email);
        }

        private static DeliveryInformation MapDeliveryInformation(
            AccountOptionsResponse options,
            AccountFieldsSettingsForModelEdit settings)
        {
            var name = CreateStringField(settings.DeliveryInformation.Name, null);
            var phone = CreateStringField(settings.DeliveryInformation.Phone, null);
            var addres = CreateStringField(settings.DeliveryInformation.Address, null);
            var postalAddres = CreateStringField(settings.DeliveryInformation.PostalAddress, null);

            return new DeliveryInformation(name, phone, addres, postalAddres);
        }

        private static Program MapProgram(AccountOptionsResponse options, AccountFieldsSettingsForModelEdit settings)
        {
            var programsSelectList = CreateMultiSelectListField(settings.Program.Programs, options.Programs, null);

            var programId = CreateListIntField(settings.Program.Programs, null);

            var info = CreateStringField(settings.Program.InfoProduct, null);

            return new Program(info, programId, programsSelectList);
        }

        private static Other MapOther(
            AccountOptionsResponse options,
            AccountFieldsSettingsForModelEdit settings,
            string guid)
        {
            var caseNumber = CreateNullableDecimalField(settings.Other.CaseNumber, null);
            var info = CreateStringField(settings.Other.Info, null);

            ConfigurableFieldModel<FilesModel> fileName;
            if (settings.Other.FileName.IsShowInDetails)
            {
                var fileNameModel = new FilesModel(guid, null);
                fileName = new ConfigurableFieldModel<FilesModel>(
                    settings.Other.FileName.Caption,
                    fileNameModel,
                    settings.Other.FileName.IsRequired);
            }
            else
            {
                fileName = ConfigurableFieldModel<FilesModel>.CreateUnshowable();
            }

            return new Other(caseNumber, info, fileName);
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

        private static ConfigurableFieldModelMultipleChoices<string> CreateMultipleStringField(
            ConfigurableFieldModel<string> setting,
            bool isMultiple)
        {
            return new ConfigurableFieldModelMultipleChoices<string>(setting, isMultiple);
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

        private static ConfigurableFieldModel<decimal?> CreateNullableDecimalField(FieldSetting setting, decimal? value)
        {
            return !setting.IsShowInDetails
                       ? ConfigurableFieldModel<decimal?>.CreateUnshowable()
                       : new ConfigurableFieldModel<decimal?>(setting.Caption, value, setting.IsRequired);
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

        private static ConfigurableFieldModel<List<int>> CreateListIntField(FieldSetting setting, List<int> value)
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

        private static string GetIds(IEnumerable<string> model)
        {
            if (model == null)
            {
                return null;
            }

            return string.Join(",", model);
        }

        #endregion
    }
}