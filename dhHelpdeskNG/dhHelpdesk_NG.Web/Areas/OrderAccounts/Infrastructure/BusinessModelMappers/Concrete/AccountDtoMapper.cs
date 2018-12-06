using DH.Helpdesk.Web.Common.Tools.Files;

namespace DH.Helpdesk.Web.Areas.OrderAccounts.Infrastructure.BusinessModelMappers.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Accounts.Write;
    using DH.Helpdesk.Web.Areas.OrderAccounts.Models.Order.Edit;
    using DH.Helpdesk.Web.Areas.OrderAccounts.Models.Order.FieldModels;
    using DH.Helpdesk.Web.Infrastructure.Tools;

    public class AccountDtoMapper : IAccountDtoMapper
    {
        public AccountForUpdate BuidForUpdate(
            AccountModel model,
            WebTemporaryFile file,
            OperationContext operationContext)
        {
            var order = MapOrderer(model.Orderer);
            var user = MapUser(model.User);
            var account = MapAccountInformation(model.AccountInformation);
            var contact = MapContact(model.Contact);
            var delivery = MapDeliveryInformation(model.DeliveryInformation);
            var program = MapProgram(model.Program);
            var other = MapOther(model.Other, file);

            return new AccountForUpdate(
                model.Id,
                model.ActivityTypeId,
                order,
                user,
                account,
                contact,
                delivery,
                program,
                other,
                operationContext.DateAndTime,
                model.FinishDate,
                operationContext.UserId);
        }

        public AccountForInsert BuidForInsert(
            AccountModel model,
            WebTemporaryFile file,
            OperationContext operationContext)
        {
            var order = MapOrderer(model.Orderer);
            var user = MapUser(model.User);
            var account = MapAccountInformation(model.AccountInformation);
            var contact = MapContact(model.Contact);
            var delivery = MapDeliveryInformation(model.DeliveryInformation);
            var program = MapProgram(model.Program);
            var other = MapOther(model.Other, file);

            return new AccountForInsert(
                order,
                user,
                account,
                contact,
                delivery,
                program,
                other,
                model.ActivityTypeId,
                operationContext.DateAndTime,
                operationContext.UserId);
        }

        private static BusinessData.Models.Accounts.Orderer MapOrderer(Orderer model)
        {
            if (model == null)
            {
                return BusinessData.Models.Accounts.Orderer.CreateDefault();
            }

            var id = ConfigurableFieldModel<string>.GetValueOrDefault(model.Id);
            var firstName = ConfigurableFieldModel<string>.GetValueOrDefault(model.FirstName);
            var lastName = ConfigurableFieldModel<string>.GetValueOrDefault(model.LastName);
            var phone = ConfigurableFieldModel<string>.GetValueOrDefault(model.Phone);
            var email = ConfigurableFieldModel<string>.GetValueOrDefault(model.Email);

            var fields = new BusinessData.Models.Accounts.Orderer(id, firstName, lastName, phone, email);

            return fields;
        }

        private static BusinessData.Models.Accounts.User MapUser(User model)
        {
            if (model == null)
            {
                return BusinessData.Models.Accounts.User.CreateDefault();
            }

            var id = ConfigurableFieldModel<string>.GetValueOrDefault(model.Ids);
            var personnelNumber = ConfigurableFieldModel<string>.GetValueOrDefault(model.PersonalIdentityNumber);
            var firstName = ConfigurableFieldModel<string>.GetValueOrDefault(model.FirstName);
            var initials = ConfigurableFieldModel<string>.GetValueOrDefault(model.Initials);
            var lastName = ConfigurableFieldModel<string>.GetValueOrDefault(model.LastName);
            var phone = ConfigurableFieldModel<string>.GetValueOrDefault(model.Phone);
            var extension = ConfigurableFieldModel<string>.GetValueOrDefault(model.Extension);
            var eMail = ConfigurableFieldModel<string>.GetValueOrDefault(model.EMail);
            var title = ConfigurableFieldModel<string>.GetValueOrDefault(model.Title);
            var location = ConfigurableFieldModel<string>.GetValueOrDefault(model.Location);
            var roomNumber = ConfigurableFieldModel<string>.GetValueOrDefault(model.RoomNumber);
            var postalAddress = ConfigurableFieldModel<string>.GetValueOrDefault(model.PostalAddress);

            var employmentType = ConfigurableFieldModel<int?>.GetValueOrDefault(model.EmploymentType);
            var employmentTypeInt = employmentType.HasValue ? employmentType.Value : 0;
            var departmentId = ConfigurableFieldModel<int?>.GetValueOrDefault(model.DepartmentId);
            var unitId = ConfigurableFieldModel<int?>.GetValueOrDefault(model.UnitId);
            var departmentId2 = ConfigurableFieldModel<int?>.GetValueOrDefault(model.DepartmentId2);

            var info = ConfigurableFieldModel<string>.GetValueOrDefault(model.Info);
            var resp = ConfigurableFieldModel<string>.GetValueOrDefault(model.Responsibility);
            var activity = ConfigurableFieldModel<string>.GetValueOrDefault(model.Activity);
            var manager = ConfigurableFieldModel<string>.GetValueOrDefault(model.Manager);
            var referenceNumber = ConfigurableFieldModel<string>.GetValueOrDefault(model.ReferenceNumber);

            return new BusinessData.Models.Accounts.User(
                GetIds(id),
                firstName,
                initials,
                lastName,
                GetIds(personnelNumber),
                phone,
                extension,
                eMail,
                title,
                location,
                roomNumber,
                postalAddress,
                employmentTypeInt,
                departmentId,
                unitId,
                departmentId2,
                info,
                resp,
                activity,
                manager,
                referenceNumber);
        }

        private static BusinessData.Models.Accounts.AccountInformation MapAccountInformation(AccountInformation model)
        {
            if (model == null)
            {
                return BusinessData.Models.Accounts.AccountInformation.CreateDefault();
            }

            var startedDate = ConfigurableFieldModel<DateTime?>.GetValueOrDefault(model.StartedDate);
            var finishDate = ConfigurableFieldModel<DateTime?>.GetValueOrDefault(model.FinishDate);

            var postTypeId = ConfigurableFieldModel<int?>.GetValueOrDefault(model.EMailTypeId);
            var postTypeInt = postTypeId.HasValue ? postTypeId.Value : 0;

            var home = ConfigurableFieldModel<bool>.GetValueOrDefault(model.HomeDirectory);
            var profil = ConfigurableFieldModel<bool>.GetValueOrDefault(model.Profile);

            var number = ConfigurableFieldModel<string>.GetValueOrDefault(model.InventoryNumber);
            var accountTypeId = ConfigurableFieldModel<int?>.GetValueOrDefault(model.AccountTypeId);
            var accountTypeId3 = ConfigurableFieldModel<int?>.GetValueOrDefault(model.AccountType3);
            var accountTypeId4 = ConfigurableFieldModel<int?>.GetValueOrDefault(model.AccountType4);
            var accountTypeId5 = ConfigurableFieldModel<int?>.GetValueOrDefault(model.AccountType5);
            var accountTypeId2 = ConfigurableFieldModel<List<int>>.GetValueOrDefault(model.AccountType2);

            var info = ConfigurableFieldModel<string>.GetValueOrDefault(model.Info);

            return new BusinessData.Models.Accounts.AccountInformation(
                startedDate,
                finishDate,
                postTypeInt,
                home,
                profil,
                number,
                accountTypeId,
                accountTypeId2,
                accountTypeId3,
                accountTypeId4,
                accountTypeId5,
                info);
        }

        private static BusinessData.Models.Accounts.Contact MapContact(Contact model)
        {
            if (model == null)
            {
                return BusinessData.Models.Accounts.Contact.CreateDefault();
            }

            var id = ConfigurableFieldModel<string>.GetValueOrDefault(model.Ids);
            var name = ConfigurableFieldModel<string>.GetValueOrDefault(model.Name);
            var phone = ConfigurableFieldModel<string>.GetValueOrDefault(model.Phone);
            var email = ConfigurableFieldModel<string>.GetValueOrDefault(model.Email);

            return new BusinessData.Models.Accounts.Contact(GetIds(id), name, phone, email);
        }

        private static BusinessData.Models.Accounts.DeliveryInformation MapDeliveryInformation(
            DeliveryInformation model)
        {
            if (model == null)
            {
                return BusinessData.Models.Accounts.DeliveryInformation.CreateDefault();
            }

            var name = ConfigurableFieldModel<string>.GetValueOrDefault(model.Name);
            var phone = ConfigurableFieldModel<string>.GetValueOrDefault(model.Phone);
            var address = ConfigurableFieldModel<string>.GetValueOrDefault(model.Address);
            var postalAddress = ConfigurableFieldModel<string>.GetValueOrDefault(model.PostalAddress);

            return new BusinessData.Models.Accounts.DeliveryInformation(name, phone, address, postalAddress);
        }

        private static BusinessData.Models.Accounts.Program MapProgram(Program model)
        {
            if (model == null)
            {
                return BusinessData.Models.Accounts.Program.CreateDefault();
            }

            var programs = ConfigurableFieldModel<List<int>>.GetValueOrDefault(model.Programs);
            var info = ConfigurableFieldModel<string>.GetValueOrDefault(model.InfoProduct);

            return new BusinessData.Models.Accounts.Program(info, programs);
        }

        private static BusinessData.Models.Accounts.Other MapOther(Other model, WebTemporaryFile tempFile)
        {
            if (model == null)
            {
                return BusinessData.Models.Accounts.Other.CreateDefault();
            }

            var caseNumber = ConfigurableFieldModel<decimal?>.GetValueOrDefault(model.CaseNumber);
            var caseNumberDecimal = caseNumber.HasValue ? caseNumber.Value : 0;

            var info = ConfigurableFieldModel<string>.GetValueOrDefault(model.Info);

            FilesModel file = ConfigurableFieldModel<FilesModel>.GetValueOrDefault(model.FileName);

            return new BusinessData.Models.Accounts.Other(
                caseNumberDecimal,
                info,
                tempFile == null ? null : tempFile.Name,
                tempFile == null ? null : tempFile.Content);
        }

        private static List<string> GetIds(string id)
        {
            return id == null ? new List<string>() : id.Split(',').ToList();
        }
    }
}