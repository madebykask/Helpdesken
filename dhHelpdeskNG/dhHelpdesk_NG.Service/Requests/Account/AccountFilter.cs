namespace DH.Helpdesk.Services.Requests.Account
{
    using DH.Helpdesk.BusinessData.Enums.Accounts;

    public class AccountFilter
    {
        public AccountFilter(
            int? activityTypeId,
            int? administratorTypeId,
            string searchString,
            AccountStates accountState)
        {
            this.ActivityTypeId = activityTypeId;
            this.AdministratorTypeId = administratorTypeId;
            this.SearchString = searchString;
            this.AccountState = accountState;
        }

        public int? ActivityTypeId { get; private set; }

        public int? AdministratorTypeId { get; private set; }

        public string SearchString { get; private set; }

        public AccountStates AccountState { get; private set; }
    }
}
