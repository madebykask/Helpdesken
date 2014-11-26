namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Accounts
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Enums.Accounts;
    using DH.Helpdesk.BusinessData.Models.Accounts;
    using DH.Helpdesk.BusinessData.Models.Accounts.Read;
    using DH.Helpdesk.BusinessData.Models.Accounts.Read.Edit;
    using DH.Helpdesk.Common.Extensions.Integer;
    using DH.Helpdesk.Common.Types;
    using DH.Helpdesk.Dal.NewInfrastructure;

    public static class AccountForReadMapper
    {
        public static AccountForEdit ExtractAccountDto(this IQueryable<Domain.Accounts.Account> query, IQueryable<Domain.User> users)
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
                                ChangedByUserSurName = k.SurName
                            })
                    .IncludePath(x => x.Entity.Programs);

            AccountForEdit dto =
                anonymus.Select(
                    x =>
                    new AccountForEdit(
                        x.Entity.Id,
                        x.Entity.AccountType_Id,
                        new Orderer(
                        x.Entity.OrdererId,
                        x.Entity.OrdererFirstName,
                        x.Entity.OrdererLastName,
                        x.Entity.OrdererPhone,
                        x.Entity.OrdererEmail),
                        new User(
                        x.Entity.UserId != null ? x.Entity.UserId.Split(';').ToList() : new List<string>(),
                        x.Entity.UserFirstName,
                        x.Entity.UserInitials,
                        x.Entity.UserLastName,
                        x.Entity.UserPersonalIdentityNumber,
                        x.Entity.UserPhone,
                        x.Entity.UserExtension,
                        x.Entity.UserEMail,
                        x.Entity.UserTitle,
                        x.Entity.UserLocation,
                        x.Entity.UserRoomNumber,
                        x.Entity.UserPostalAddress,
                        x.Entity.EmploymentType,
                        x.Entity.Department_Id,
                        x.Entity.OU_Id,
                        x.Entity.Department_Id2,
                        x.Entity.InfoUser,
                        x.Entity.Responsibility,
                        x.Entity.Activity,
                        x.Entity.Manager,
                        x.Entity.ReferenceNumber),
                        new AccountInformation(
                        x.Entity.AccountStartDate,
                        x.Entity.AccountEndDate,
                        (EMailTypes)x.Entity.EMailType,
                        x.Entity.HomeDirectory.ToBool(),
                        x.Entity.Profile.ToBool(),
                        x.Entity.InventoryNumber,
                        x.Entity.AccountType_Id,
                        x.Entity.AccountType2,
                        x.Entity.AccountType3,
                        x.Entity.AccountType4,
                        x.Entity.AccountType5,
                        x.Entity.Info),
                        new Contact(
                        x.Entity.ContactId != null ? x.Entity.ContactId.Split(';').ToList() : new List<string>(),
                        x.Entity.ContactName,
                        x.Entity.ContactPhone,
                        x.Entity.ContactEMail),
                        new DeliveryInformation(
                        x.Entity.DeliveryName,
                        x.Entity.DeliveryPhone,
                        x.Entity.DeliveryAddress,
                        x.Entity.DeliveryPostalAddress),
                        new ProgramForRead(x.Entity.InfoProduct, x.Entity.Programs.Select(y => y.Id).ToList()),
                        new OtherForRead(x.Entity.CaseNumber, x.Entity.InfoOther, x.Entity.AccountFileName),
                        x.Entity.FinishingDate,
                        new UserName(x.ChangedByUserFirstName, x.ChangedByUserSurName),
                        x.Entity.ChangedDate,
                        x.Entity.CreatedDate)).Single();

            return dto;
        }
    }
}