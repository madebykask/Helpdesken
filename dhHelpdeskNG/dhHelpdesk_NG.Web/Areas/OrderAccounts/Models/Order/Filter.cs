namespace DH.Helpdesk.Web.Areas.OrderAccounts.Models.Order
{
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;
    using DH.Helpdesk.Web.Models.Shared;

    public class Filter
    {
        public Filter()
        {
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
    }
}