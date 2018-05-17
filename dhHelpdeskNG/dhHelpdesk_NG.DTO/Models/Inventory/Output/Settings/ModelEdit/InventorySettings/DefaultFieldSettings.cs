namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.InventorySettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class DefaultFieldSettings
    {
        public DefaultFieldSettings(
            InventoryFieldSettingForModelEdit departmentFieldSetting,
            InventoryFieldSettingForModelEdit nameFieldSetting,
            InventoryFieldSettingForModelEdit modelFieldSetting,
            InventoryFieldSettingForModelEdit manufacturerFieldSetting,
            InventoryFieldSettingForModelEdit serialNumberFieldSetting,
            InventoryFieldSettingForModelEdit theftMarkFieldSetting,
            InventoryFieldSettingForModelEdit barCodeFieldSetting,
            InventoryFieldSettingForModelEdit purchaseDateFieldSetting,
            InventoryFieldSettingForModelEdit placeFieldSetting,
            InventoryFieldSettingForModelEdit workstationFieldSetting,
            InventoryFieldSettingForModelEdit infoFieldSetting,
            InventoryFieldSettingForModelEdit createdDate,
            InventoryFieldSettingForModelEdit changedDate,
            InventoryFieldSettingForModelEdit syncDate)
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
            this.CreatedDate = createdDate;
            this.ChangedDate = changedDate;
            this.SyncDate = syncDate;
        }

        [NotNull]
        public InventoryFieldSettingForModelEdit DepartmentFieldSetting { get; private set; }

        [NotNull]
        public InventoryFieldSettingForModelEdit NameFieldSetting { get; private set; }

        [NotNull]
        public InventoryFieldSettingForModelEdit ModelFieldSetting { get; private set; }

        [NotNull]
        public InventoryFieldSettingForModelEdit ManufacturerFieldSetting { get; private set; }

        [NotNull]
        public InventoryFieldSettingForModelEdit SerialNumberFieldSetting { get; private set; }

        [NotNull]
        public InventoryFieldSettingForModelEdit TheftMarkFieldSetting { get; private set; }

        [NotNull]
        public InventoryFieldSettingForModelEdit BarCodeFieldSetting { get; private set; }

        [NotNull]
        public InventoryFieldSettingForModelEdit PurchaseDateFieldSetting { get; private set; }

        [NotNull]
        public InventoryFieldSettingForModelEdit PlaceFieldSetting { get; private set; }

        [NotNull]
        public InventoryFieldSettingForModelEdit WorkstationFieldSetting { get; private set; }

        [NotNull]
        public InventoryFieldSettingForModelEdit InfoFieldSetting { get; private set; }

        [NotNull]
        public InventoryFieldSettingForModelEdit CreatedDate { get; private set; }

        [NotNull]
        public InventoryFieldSettingForModelEdit ChangedDate { get; private set; }

        [NotNull]
        public InventoryFieldSettingForModelEdit SyncDate { get; private set; }
    }
}
