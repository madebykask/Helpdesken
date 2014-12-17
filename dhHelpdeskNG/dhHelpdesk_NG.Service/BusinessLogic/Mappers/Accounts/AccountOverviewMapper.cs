namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Accounts
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;

    using DH.Helpdesk.BusinessData.Enums.Accounts.Fields;
    using DH.Helpdesk.BusinessData.Models.Accounts.Read.Overview;
    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.Common.Extensions.Integer;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Accounts;

    using User = DH.Helpdesk.BusinessData.Models.Accounts.Read.Overview.User;

    public static class AccountOverviewMapper
    {
        public static List<AccountOverview> MapToAccountOverview(
            this IQueryable<Account> query,
            IQueryable<EmploymentType> employmentTypes,
            IQueryable<AccountType> accountTypes)
        {
            var anonymus =
                query.SelectIncluding(
                    new List<Expression<Func<Account, object>>>
                        {
                            o => o.Department.DepartmentName,
                            o => o.Department2.DepartmentName,
                            o => o.OU.Name,
                            o => o.AccountType.Name,
                            o => o.AccountActivity.Name,
                            o => o.Programs.Select(p => p.Name),
                        }).ToList();

            List<IdAndNameOverview> accountOverviews =
                accountTypes.ToList().Select(x => new IdAndNameOverview(x.Id, x.Name)).ToList();

            List<IdAndNameOverview> typeOverviews =
                employmentTypes.ToList().Select(x => new IdAndNameOverview(x.Id, x.Name)).ToList();

            List<AccountOverview> overviews = anonymus.Select(
                x =>
                    {
                        var account = (Account)x.sourceObject;
                        string departmentName1 = x.f0;
                        string departmentName2 = x.f1;
                        string unitName = x.f2;
                        string accountTypeName1 = x.f3;
                        string activityName = x.f4;


                        return new AccountOverview(
                            account.Id,
                            account.AccountActivity_Id,
                            activityName,
                            new Orderer(
                                account.OrdererId,
                                account.OrdererFirstName,
                                account.OrdererLastName,
                                account.OrdererPhone,
                                account.OrdererEmail),
                            new User(
                                !string.IsNullOrWhiteSpace(account.UserId)
                                    ? account.UserId.Split(';').ToList()
                                    : new List<string>(),
                                account.UserFirstName,
                                account.UserInitials,
                                account.UserLastName,
                                !string.IsNullOrWhiteSpace(account.UserPersonalIdentityNumber)
                                    ? account.UserPersonalIdentityNumber.Split(';').ToList()
                                    : new List<string>(),
                                account.UserPhone,
                                account.UserExtension,
                                account.UserEMail,
                                account.UserTitle,
                                account.UserLocation,
                                account.UserRoomNumber,
                                account.UserPostalAddress,
                                ExtractEmploymentType(typeOverviews, account.EmploymentType),
                                departmentName1,
                                unitName,
                                departmentName2,
                                account.InfoUser,
                                account.Responsibility,
                                account.Activity,
                                account.Manager,
                                account.ReferenceNumber),
                            new AccountInformation(
                                account.AccountStartDate,
                                account.AccountEndDate,
                                (EMailTypes)account.EMailType,
                                account.HomeDirectory.ToBool(),
                                account.Profile.ToBool(),
                                account.InventoryNumber,
                                accountTypeName1,
                                ExtractAccountNames(accountOverviews, account.AccountType2),
                                ExtractAccountName(accountOverviews, account.AccountType3),
                                ExtractAccountName(accountOverviews, account.AccountType4),
                                ExtractAccountName(accountOverviews, account.AccountType5),
                                account.Info),
                            new Contact(
                                !string.IsNullOrWhiteSpace(account.ContactId)
                                    ? account.ContactId.Split(';').ToList()
                                    : new List<string>(),
                                account.ContactName,
                                account.ContactPhone,
                                account.ContactEMail),
                            new DeliveryInformation(
                                account.DeliveryName,
                                account.DeliveryPhone,
                                account.DeliveryAddress,
                                account.DeliveryPostalAddress),
                            new BusinessData.Models.Accounts.Read.Overview.Program(
                                account.InfoProduct,
                                account.Programs.Select(y => y.Name).ToList()),
                            new Other(account.CaseNumber, account.InfoOther, account.AccountFileName));
                    }).ToList();

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

        private static string ExtractEmploymentType(IEnumerable<IdAndNameOverview> list, int id)
        {
            if (id == 0)
            {
                return null;
            }

            IdAndNameOverview overview = list.SingleOrDefault(x => x.Id == id);

            if (overview == null)
            {
                return null;
            }

            return overview.Name;
        }
    }
}
