namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings.Printer
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
        [LocalizedDisplay("Nätverkskort")]
        public FieldSettingModel NetworkAdapterFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("IP adress")]
        public FieldSettingModel IPAddressFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("MAC adress")]
        public FieldSettingModel MacAddressFieldSettingModel { get; set; }
    }
}