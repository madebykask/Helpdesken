namespace DH.Helpdesk.BusinessData.Models.Accounts.Read.Overview
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Enums.Accounts;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class AccountTypeOverview
    {
        public AccountTypeOverview(AccountTypes accountType, List<ItemOverview> itemOverviews)
        {
            this.AccountType = accountType;
            this.ItemOverviews = itemOverviews;
        }

        public AccountTypes AccountType { get; set; }

        [NotNull]
        public List<ItemOverview> ItemOverviews { get; set; } 
    }
}
