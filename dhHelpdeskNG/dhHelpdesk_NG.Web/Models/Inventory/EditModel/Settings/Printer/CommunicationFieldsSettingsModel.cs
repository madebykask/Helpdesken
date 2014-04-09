namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Settings.Printer
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class CommunicationFieldsSettingsModel
    {
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
        public FieldSettingModel NetworkAdapterFieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel IPAddressFieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel MacAddressFieldSettingModel { get; set; }
    }
}