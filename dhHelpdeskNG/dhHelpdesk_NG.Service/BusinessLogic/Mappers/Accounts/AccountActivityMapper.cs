namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Accounts
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Enums.Accounts;
    using DH.Helpdesk.BusinessData.Models.Accounts.Read.Overview;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Accounts;

    public static class AccountActivityMapper
    {
        public static List<ItemOverview> MapAccountActivitiesToItemOverview(this IQueryable<AccountActivity> query)
        {
            List<ItemOverview> overviews =
                query.Select(x => new { x.Id, x.Name })
                    .ToList()
                    .Select(x => new ItemOverview(x.Name, x.Id.ToString(CultureInfo.InvariantCulture)))
                    .ToList();

            return overviews;
        }

        public static IdAndNameOverview ExtractIdAndNameOverview(
            this IQueryable<AccountActivity> query,
            int id)
        {
            var overview = query.Where(x => x.Id == id).Select(x => new { x.Id, x.Name }).Single();

            return new IdAndNameOverview(overview.Id, overview.Name);
        }

        public static List<ItemOverview> MapEmploymentTypesToItemOverview(this IQueryable<EmploymentType> query)
        {
            List<ItemOverview> overviews =
                query.Select(x => new { x.Id, x.Name })
                    .ToList()
                    .Select(x => new ItemOverview(x.Name, x.Id.ToString(CultureInfo.InvariantCulture)))
                    .ToList();

            return overviews;
        }

        public static List<ItemOverview> MapProgramsToItemOverview(this IQueryable<DH.Helpdesk.Domain.Program> query)
        {
            List<ItemOverview> overviews =
                query.Select(x => new { x.Id, x.Name })
                    .ToList()
                    .Select(x => new ItemOverview(x.Name, x.Id.ToString(CultureInfo.InvariantCulture)))
                    .ToList();

            return overviews;
        }

        public static IQueryable<AccountType> GetByAyccountActivityId(
            this IQueryable<AccountType> query,
            int accountActivityId)
        {
            query = query.Where(x => x.AccountActivity_Id == accountActivityId);

            return query;
        }

        public static List<AccountTypeOverview> MapAccountTypesToItemOverview(
            this IQueryable<AccountType> query,
            int accountActivityId)
        {
            var anonymus =
                query.GetByAyccountActivityId(accountActivityId)
                    .Select(x => new { x.Id, x.Name, x.AccountField })
                    .ToList()
                    .GroupBy(x => x.AccountField);

            return (from item in anonymus
                    let itemOverviews =
                        item.Select(x => new ItemOverview(x.Name, x.Id.ToString(CultureInfo.InvariantCulture))).ToList()
                    select new AccountTypeOverview((AccountTypes)item.Key, itemOverviews)).ToList();
        }
    }
}
