namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings.Computer
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
        [LocalizedDisplay("Nätverkskort")]
        public FieldSettingModel NetworkAdapterFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("IP adress")]
        public FieldSettingModel IPAddressFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("MAC adress")]
        public FieldSettingModel MacAddressFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("RAS")]
        public FieldSettingModel RASFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Novellklient")]
        public FieldSettingModel NovellClientFieldSettingModel { get; set; }
    }
}