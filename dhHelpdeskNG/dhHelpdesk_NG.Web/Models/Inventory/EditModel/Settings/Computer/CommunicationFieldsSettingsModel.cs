namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Settings.Computer
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class CommunicationFieldsSettingsModel
    {
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
        public FieldSettingModel NetworkAdapterFieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel IPAddressFieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel MacAddressFieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel RASFieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel NovellClientFieldSettingModel { get; set; }
    }
}