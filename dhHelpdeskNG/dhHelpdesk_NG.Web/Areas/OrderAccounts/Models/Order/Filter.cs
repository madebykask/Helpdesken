namespace DH.Helpdesk.Web.Areas.OrderAccounts.Models.Order
{
    using DH.Helpdesk.BusinessData.Enums.Accounts;
    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.Services.Requests.Account;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;
    using DH.Helpdesk.Web.Models.Shared;

    public class Filter
    {
        public Filter()
        {
            this.SortField = new SortFieldModel();
            this.State = Enums.Show.Active;
        }

        public Filter(int? userId, string searchFor, Enums.Show state, SortFieldModel sortFieldModel)
        {
            this.UserId = userId;
            this.SearchFor = searchFor;
            this.State = state;
            this.SortField = sortFieldModel;
        }

        public int? UserId { get; set; }

        [LocalizedDisplay("Sök")]
        public string SearchFor { get; set; }

        public Enums.Show State { get; set; }

        public SortFieldModel SortField { get; set; }

        public static Filter CreateDefault()
        {
            return new Filter();
        }

        public AccountFilter CreateRequest(int? activityTypeId)
        {
            SortField sortField = null;

            if (!string.IsNullOrEmpty(this.SortField.Name))
            {
                sortField = new SortField(this.SortField.Name, this.SortField.SortBy.Value);
            }

            return new AccountFilter(activityTypeId, this.UserId, this.SearchFor, (AccountStates)this.State, sortField);
        }
    }
}