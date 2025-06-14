﻿namespace DH.Helpdesk.Services.Response.Account
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Accounts.Read.Overview;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class AccountOverviewResponse
    {
        public AccountOverviewResponse(List<AccountOverview> accountOverviews, List<ItemOverview> accountTypes)
        {
            this.AccountOverviews = accountOverviews;
            this.AccountTypes = accountTypes;
        }

        [NotNull]
        public List<AccountOverview> AccountOverviews { get; set; }

        [NotNull]
        public List<ItemOverview> AccountTypes { get; set; }
    }
}
