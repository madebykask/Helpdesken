namespace DH.Helpdesk.Web.Areas.OrderAccounts.Models.Settings
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Web.Areas.OrderAccounts.Models.Index;
    using DH.Helpdesk.Web.Areas.OrderAccounts.Models.Settings.FieldSettings;

    using BaseIndexModel = DH.Helpdesk.Web.Areas.OrderAccounts.Models.Index.BaseIndexModel;

    public class SettingsIndexModel : BaseIndexModel
    {
        public SettingsIndexModel(
            int activityType,
            List<ItemOverview> activityTypes,
            AccountFieldsSettingsModel settingsModel)
            : base(activityType, activityTypes)
        {
            this.AccountFieldsSettingsModel = settingsModel;
        }

        public AccountFieldsSettingsModel AccountFieldsSettingsModel { get; set; }

        public override sealed IndexModelTypes IndexModelType
        {
            get
            {
                return IndexModelTypes.OrderFieldSettings;
            }
        }
    }
}