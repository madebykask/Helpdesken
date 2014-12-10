namespace DH.Helpdesk.Web.Areas.OrderAccounts.Models.Index
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Shared;

    public abstract class BaseIndexModel
    {
        protected BaseIndexModel(int? activityType, List<ItemOverview> activityTypes)
        {
            this.ActivityType = activityType;
            this.ActivityTypes = new SelectList(activityTypes, "Value", "Name");
        }

        public abstract IndexModelTypes IndexModelType { get; }

        public int? ActivityType { get; set; }

        public SelectList ActivityTypes { get; private set; }
    }
}