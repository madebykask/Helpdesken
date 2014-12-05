namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings.Server
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class CommunicationFieldsSettingsModel
    {
        public CommunicationFieldsSettingsModel()
        {
        }

        public CommunicationFieldsSettingsModel(
            FieldSettingModel networkAdapterFieldSettingModel,
            FieldSettingModel ipAddressFieldSettingModel,
            FieldSettingModel macAddressFieldSettingModel)
        {
            this.NetworkAdapterFieldSettingModel = networkAdapterFieldSettingModel;
            this.IPAddressFieldSettingModel = ipAddressFieldSettingModel;
            this.MacAddressFieldSettingModel = macAddressFieldSettingModel;
        }

        [NotNull]
        [LocalizedDisplay("Network Adapter")]
        public FieldSettingModel NetworkAdapterFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("IP Address")]
        public FieldSettingModel IPAddressFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Mac Address")]
        public FieldSettingModel MacAddressFieldSettingModel { get; set; }
    }
}