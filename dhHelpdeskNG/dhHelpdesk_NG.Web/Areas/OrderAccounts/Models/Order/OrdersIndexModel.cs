namespace DH.Helpdesk.Web.Areas.OrderAccounts.Models.Order
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Areas.OrderAccounts.Models.Index;

    public class OrdersIndexModel : BaseIndexModel
    {
        private OrdersIndexModel(
            int activityType,
            List<ItemOverview> activityTypes,
            List<ItemOverview> users,
            Filter filter)
            : base(activityType, activityTypes)
        {
            this.ActivityTypeForEdit = 0;
            this.Filter = filter;
            this.Users = new SelectList(users, "Value", "Name");
        }

        public int ActivityTypeForEdit { get; private set; }

        [NotNull]
        public Filter Filter { get; private set; }

        public SelectList Users { get; private set; }

        public override sealed IndexModelTypes IndexModelType
        {
            get
            {
                return IndexModelTypes.Orders;
            }
        }

        public static OrdersIndexModel BuildViewModel(
            int activityType,
            List<ItemOverview> activityTypes,
            List<ItemOverview> users,
            Filter filter)
        {
            return new OrdersIndexModel(activityType, activityTypes, users, filter);
        }
    }
}