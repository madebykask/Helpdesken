namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Accounts
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Enums.Accounts.Fields;
    using DH.Helpdesk.BusinessData.Models.Accounts.Read.Overview;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Common.Extensions.Integer;
    using DH.Helpdesk.Dal.NewInfrastructure;

    public static class AccountOverviewMapper
    {
        public static List<AccountOverview> MapToAccountOverview(
            this IQueryable<Domain.Accounts.Account> query,
            IQueryable<Domain.EmploymentType> employmentTypes,
            IQueryable<Domain.Accounts.AccountType> accountTypes)
        {
            var anonymus =
                query.Select(
                    x =>
                    new
                        {
                            Entity = x,
                            DepartmentName1 = x.Department.DepartmentName,
                            DepartmentName2 = x.Department2.DepartmentName,
                            UnitName = x.OU.Name,
                            AccountTypeName1 = x.AccountType.Name
                        })
                    .GroupJoin(employmentTypes, s => s.Entity.EmploymentType, u => u.Id, (s, res) => new { s, res })
                    .SelectMany(
                        t => t.res.DefaultIfEmpty(),
                        (t, k) =>
                        new
                            {
                                t.s.Entity,
                                t.s.DepartmentName1,
                                t.s.DepartmentName2,
                                t.s.UnitName,
                                t.s.AccountTypeName1,
                                EmploymentTypeName = k.Name
                            })
                    .IncludePath(x => x.Entity.Programs)
                    .ToList();

            List<IdAndNameOverview> accountOverviews =
                accountTypes.Select(x => new IdAndNameOverview(x.Id, x.Name)).ToList();

            List<AccountOverview> overviews =
                anonymus.Select(
                    x =>
                    new AccountOverview(
                        x.Entity.Id,
                        x.Entity.AccountActivity_Id,
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
                        x.EmploymentTypeName,
                        x.DepartmentName1,
                        x.UnitName,
                        x.DepartmentName2,
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
                        x.AccountTypeName1,
                        ExtractAccountNames(accountOverviews, x.Entity.AccountType2),
                        ExtractAccountName(accountOverviews, x.Entity.AccountType3),
                        ExtractAccountName(accountOverviews, x.Entity.AccountType4),
                        ExtractAccountName(accountOverviews, x.Entity.AccountType5),
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
                        new Program(x.Entity.InfoProduct, x.Entity.Programs.Select(y => y.Name).ToList()),
                        new Other(x.Entity.CaseNumber, x.Entity.InfoOther, x.Entity.AccountFileName))).ToList();

            return overviews;
        }

        private static string ExtractAccountName(IEnumerable<IdAndNameOverview> list, int? id)
        {
            if (!id.HasValue)
            {
                return null;
            }

            IdAndNameOverview overview = list.FirstOrDefault(y => y.Id == id);

            return overview == null ? null : overview.Name;
        }

        private static List<string> ExtractAccountNames(IEnumerable<IdAndNameOverview> list, string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
            {
                return new List<string>();
            }

            string[] idList = ids.Split(',');

            List<string> overviews =
                list.Where(y => idList.Contains(y.Id.ToString(CultureInfo.InvariantCulture)))
                    .Select(x => x.Name)
                    .ToList();

            return overviews;
        }
    }
}
