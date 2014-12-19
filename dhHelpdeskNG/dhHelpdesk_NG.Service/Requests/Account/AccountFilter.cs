namespace DH.Helpdesk.Services.Requests.Account
{
    using DH.Helpdesk.BusinessData.Enums.Accounts;
    using DH.Helpdesk.BusinessData.Models.Shared.Input;

    public class AccountFilter
    {
        public AccountFilter(
            int? activityTypeId,
            int? administratorTypeId,
            string searchString,
            AccountStates accountState,
            SortField sortField)
        {
            this.ActivityTypeId = activityTypeId;
            this.AdministratorTypeId = administratorTypeId;
            this.SearchString = searchString;
            this.AccountState = accountState;
            this.SortField = sortField;
        }

        public int? ActivityTypeId { get; private set; }

        public int? AdministratorTypeId { get; private set; }

        public string SearchString { get; private set; }

        public AccountStates AccountState { get; private set; }

        public SortField SortField { get; private set; }
    }
}
