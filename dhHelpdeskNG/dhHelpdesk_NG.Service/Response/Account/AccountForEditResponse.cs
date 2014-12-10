namespace DH.Helpdesk.Services.Response.Account
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Accounts.Read.Edit;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class AccountForEditResponse
    {
        public AccountForEditResponse(AccountForEdit accountForEdit, List<ItemOverview> accountTypes)
        {
            this.AccountForEdit = accountForEdit;
            this.AccountTypes = accountTypes;
        }

        [NotNull]
        public AccountForEdit AccountForEdit { get; set; }

        [NotNull]
        public List<ItemOverview> AccountTypes { get; set; }
    }
}