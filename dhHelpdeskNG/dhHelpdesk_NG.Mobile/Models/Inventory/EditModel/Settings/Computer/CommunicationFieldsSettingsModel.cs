namespace DH.Helpdesk.Mobile.Models.Inventory.EditModel.Settings.Computer
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Mobile.Infrastructure.LocalizedAttributes;

    public class CommunicationFieldsSettingsModel
    {
        public CommunicationFieldsSettingsModel()
        {
        }

        public CommunicationFieldsSettingsModel(
            FieldSettingModel networkAdapterFieldSettingModel,
            FieldSettingModel ipAddressFieldSettingModel,
            FieldSettingModel macAddressFieldSettingModel,
            FieldSettingModel rasFieldSettingModel,
            FieldSettingModel novellClientFieldSettingModel)
        {
            this.NetworkAdapterFieldSettingModel = networkAdapterFieldSettingModel;
            this.IPAddressFieldSettingModel = ipAddressFieldSettingModel;
            this.MacAddressFieldSettingModel = macAddressFieldSettingModel;
            this.RASFieldSettingModel = rasFieldSettingModel;
            this.NovellClientFieldSettingModel = novellClientFieldSettingModel;
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

        [NotNull]
        [LocalizedDisplay("RAS")]
        public FieldSettingModel RASFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Novell Client")]
        public FieldSettingModel NovellClientFieldSettingModel { get; set; }
    }
}