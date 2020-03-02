namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.InventorySettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class DefaultFieldSettings
    {
        public DefaultFieldSettings(
            InventoryFieldSettingForProcessing departmentFieldSetting,
            InventoryFieldSettingForProcessing nameFieldSetting,
            InventoryFieldSettingForProcessing modelFieldSetting,
            InventoryFieldSettingForProcessing manufacturerFieldSetting,
            InventoryFieldSettingForProcessing serialNumberFieldSetting,
            InventoryFieldSettingForProcessing theftMarkFieldSetting,
            InventoryFieldSettingForProcessing barCodeFieldSetting,
            InventoryFieldSettingForProcessing purchaseDateFieldSetting,
            InventoryFieldSettingForProcessing placeFieldSetting,
            InventoryFieldSettingForProcessing workstationFieldSetting,
            InventoryFieldSettingForProcessing infoFieldSetting,
            InventoryFieldSettingForProcessing typeFieldSetting)
        {
            this.DepartmentFieldSetting = departmentFieldSetting;
            this.NameFieldSetting = nameFieldSetting;
            this.ModelFieldSetting = modelFieldSetting;
            this.ManufacturerFieldSetting = manufacturerFieldSetting;
            this.SerialNumberFieldSetting = serialNumberFieldSetting;
            this.TheftMarkFieldSetting = theftMarkFieldSetting;
            this.BarCodeFieldSetting = barCodeFieldSetting;
            this.PurchaseDateFieldSetting = purchaseDateFieldSetting;
            this.PlaceFieldSetting = placeFieldSetting;
            this.WorkstationFieldSetting = workstationFieldSetting;
            this.InfoFieldSetting = infoFieldSetting;
            this.TypeFieldSetting = typeFieldSetting;
        }

        [NotNull]
        public InventoryFieldSettingForProcessing DepartmentFieldSetting { get; private set; }

        [NotNull]
        public InventoryFieldSettingForProcessing NameFieldSetting { get; private set; }

        [NotNull]
        public InventoryFieldSettingForProcessing ModelFieldSetting { get; private set; }

        [NotNull]
        public InventoryFieldSettingForProcessing ManufacturerFieldSetting { get; private set; }

        [NotNull]
        public InventoryFieldSettingForProcessing SerialNumberFieldSetting { get; private set; }

        [NotNull]
        public InventoryFieldSettingForProcessing TheftMarkFieldSetting { get; private set; }

        [NotNull]
        public InventoryFieldSettingForProcessing BarCodeFieldSetting { get; private set; }

        [NotNull]
        public InventoryFieldSettingForProcessing PurchaseDateFieldSetting { get; private set; }

        [NotNull]
        public InventoryFieldSettingForProcessing PlaceFieldSetting { get; private set; }

        [NotNull]
        public InventoryFieldSettingForProcessing WorkstationFieldSetting { get; private set; }

        [NotNull]
        public InventoryFieldSettingForProcessing InfoFieldSetting { get; private set; }

        [NotNull]
        public InventoryFieldSettingForProcessing TypeFieldSetting { get; private set; }
    }
}
