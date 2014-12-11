namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Accounts
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Accounts;
    using DH.Helpdesk.BusinessData.Models.Accounts.Read.Edit;
    using DH.Helpdesk.Common.Extensions.Integer;
    using DH.Helpdesk.Common.Types;

    public static class AccountForReadMapper
    {
        public static AccountForEdit ExtractAccountDto(
            this IQueryable<Domain.Accounts.Account> query,
            IQueryable<Domain.User> users)
        {
            var anonymus =
                query.GroupJoin(users, s => s.ChangedByUser_Id, u => u.Id, (s, res) => new { s, res })
                    .SelectMany(
                        t => t.res.DefaultIfEmpty(),
                        (t, k) =>
                        new
                            {
                                Entity = t.s,
                                ChangedByUserFirstName = k.FirstName,
                                ChangedByUserSurName = k.SurName,
                                RegionId = t.s.Department.Region_Id,
                                ActivityName = t.s.AccountActivity.Name
                            })
                    .Single();

            var dto = new AccountForEdit(
                anonymus.Entity.Id,
                anonymus.Entity.AccountActivity_Id,
                anonymus.ActivityName,
                new Orderer(
                    anonymus.Entity.OrdererId,
                    anonymus.Entity.OrdererFirstName,
                    anonymus.Entity.OrdererLastName,
                    anonymus.Entity.OrdererPhone,
                    anonymus.Entity.OrdererEmail),
                new UserForEdit(
                    anonymus.Entity.UserId != null ? anonymus.Entity.UserId.Split(';').ToList() : new List<string>(),
                    anonymus.Entity.UserFirstName,
                    anonymus.Entity.UserInitials,
                    anonymus.Entity.UserLastName,
                    anonymus.Entity.UserPersonalIdentityNumber != null
                        ? anonymus.Entity.UserPersonalIdentityNumber.Split(';').ToList()
                        : new List<string>(),
                    anonymus.Entity.UserPhone,
                    anonymus.Entity.UserExtension,
                    anonymus.Entity.UserEMail,
                    anonymus.Entity.UserTitle,
                    anonymus.Entity.UserLocation,
                    anonymus.Entity.UserRoomNumber,
                    anonymus.Entity.UserPostalAddress,
                    anonymus.Entity.EmploymentType,
                    anonymus.Entity.Department_Id,
                    anonymus.Entity.OU_Id,
                    anonymus.Entity.Department_Id2,
                    anonymus.Entity.InfoUser,
                    anonymus.Entity.Responsibility,
                    anonymus.Entity.Activity,
                    anonymus.Entity.Manager,
                    anonymus.Entity.ReferenceNumber,
                    anonymus.RegionId),
                new AccountInformation(
                    anonymus.Entity.AccountStartDate,
                    anonymus.Entity.AccountEndDate,
                    anonymus.Entity.EMailType,
                    anonymus.Entity.HomeDirectory.ToBool(),
                    anonymus.Entity.Profile.ToBool(),
                    anonymus.Entity.InventoryNumber,
                    anonymus.Entity.AccountType_Id,
                    anonymus.Entity.AccountType2 != null
                        ? anonymus.Entity.AccountType2.Split(',').Select(int.Parse).ToList()
                        : new List<int>(),
                    anonymus.Entity.AccountType3,
                    anonymus.Entity.AccountType4,
                    anonymus.Entity.AccountType5,
                    anonymus.Entity.Info),
                new Contact(
                    anonymus.Entity.ContactId != null
                        ? anonymus.Entity.ContactId.Split(';').ToList()
                        : new List<string>(),
                    anonymus.Entity.ContactName,
                    anonymus.Entity.ContactPhone,
                    anonymus.Entity.ContactEMail),
                new DeliveryInformation(
                    anonymus.Entity.DeliveryName,
                    anonymus.Entity.DeliveryPhone,
                    anonymus.Entity.DeliveryAddress,
                    anonymus.Entity.DeliveryPostalAddress),
                new Program(anonymus.Entity.InfoProduct, anonymus.Entity.Programs.Select(y => y.Id).ToList()),
                new Other(
                    anonymus.Entity.CaseNumber,
                    anonymus.Entity.InfoOther,
                    anonymus.Entity.AccountFileName,
                    anonymus.Entity.AccountFile),
                anonymus.Entity.FinishingDate,
                new UserName(anonymus.ChangedByUserFirstName, anonymus.ChangedByUserSurName),
                anonymus.Entity.ChangedDate,
                anonymus.Entity.CreatedDate);

            return dto;
        }
    }
}