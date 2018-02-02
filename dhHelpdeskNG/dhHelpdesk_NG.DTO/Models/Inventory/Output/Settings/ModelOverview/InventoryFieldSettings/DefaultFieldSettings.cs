namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.InventoryFieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class DefaultFieldSettings
    {
        public DefaultFieldSettings(
            FieldSettingOverview departmentFieldSetting,
            FieldSettingOverview nameFieldSetting,
            FieldSettingOverview modelFieldSetting,
            FieldSettingOverview manufacturerFieldSetting,
            FieldSettingOverview serialNumberFieldSetting,
            FieldSettingOverview theftMarkFieldSetting,
            FieldSettingOverview barCodeFieldSetting,
            FieldSettingOverview purchaseDateFieldSetting,
            FieldSettingOverview placeFieldSetting,
            FieldSettingOverview workstationFieldSetting,
            FieldSettingOverview infoFieldSetting,
            FieldSettingOverview createdDateFieldSetting,
            FieldSettingOverview changedDateFieldSetting)
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
            this.CreatedDateFieldSetting = createdDateFieldSetting;
            this.ChangedDateFieldSetting = changedDateFieldSetting;
        }

        [NotNull]
        public FieldSettingOverview DepartmentFieldSetting { get; private set; }

        [NotNull]
        public FieldSettingOverview NameFieldSetting { get; private set; }

        [NotNull]
        public FieldSettingOverview ModelFieldSetting { get; private set; }

        [NotNull]
        public FieldSettingOverview ManufacturerFieldSetting { get; private set; }

        [NotNull]
        public FieldSettingOverview SerialNumberFieldSetting { get; private set; }

        [NotNull]
        public FieldSettingOverview TheftMarkFieldSetting { get; private set; }

        [NotNull]
        public FieldSettingOverview BarCodeFieldSetting { get; private set; }

        [NotNull]
        public FieldSettingOverview PurchaseDateFieldSetting { get; private set; }

        [NotNull]
        public FieldSettingOverview PlaceFieldSetting { get; private set; }

        [NotNull]
        public FieldSettingOverview WorkstationFieldSetting { get; private set; }

        [NotNull]
        public FieldSettingOverview InfoFieldSetting { get; private set; }

        [NotNull]
        public FieldSettingOverview CreatedDateFieldSetting { get; private set; }

        [NotNull]
        public FieldSettingOverview ChangedDateFieldSetting { get; private set; }
    }
}
