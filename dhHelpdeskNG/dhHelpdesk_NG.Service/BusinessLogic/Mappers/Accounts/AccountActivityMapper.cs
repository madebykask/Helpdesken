namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Accounts
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Shared;
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
    }
}
