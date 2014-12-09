namespace DH.Helpdesk.Web.Areas.OrderAccounts.Models.Settings
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Web.Areas.OrderAccounts.Models.Index;

    using BaseIndexModel = DH.Helpdesk.Web.Areas.OrderAccounts.Models.Index.BaseIndexModel;

    public class SettingsIndexModel : BaseIndexModel
    {
        protected SettingsIndexModel(int activityType, List<ItemOverview> activityTypes)
        {
            this.ActivityType = activityType;
            this.ActivityTypes = new SelectList(activityTypes, "Value", "Name");
        }

        public int ActivityType { get; set; }

        public SelectList ActivityTypes { get; private set; }

        public override sealed IndexModelTypes IndexModelType
        {
            get
            {
                return IndexModelTypes.OrderFieldSettings;
            }
        }
    }
}