namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.ServerFieldSettings
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.ComputerFieldSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.SharedFieldSettings;
    using DH.Helpdesk.Common.ValidationAttributes;

    using PlaceFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.SharedFieldSettings.PlaceFieldsSettings;

    public class ServerFieldsSettingsOverview
    {
        public ServerFieldsSettingsOverview(int? customerId, int? languageId, GeneralFieldsSettings generalFieldsSettings, OtherFieldsSettings otherFieldsSettings, StateFieldsSettings stateFieldsSettings, StorageFieldsSettings storageFieldsSettings, ChassisFieldsSettings chassisFieldsSettings, InventoryFieldsSettings inventoryFieldsSettings, MemoryFieldsSettings memoryFieldsSettings, OperatingSystemFieldsSettings operatingSystemFieldsSettings, OrganizationFieldsSettings organizationFieldsSettings, ProcessorFieldsSettings proccesorFieldsSettings, PlaceFieldsSettings placeFieldsSettings)
        {
            this.CustomerId = customerId;
            this.LanguageId = languageId;
            this.GeneralFieldsSettings = generalFieldsSettings;
            this.OtherFieldsSettings = otherFieldsSettings;
            this.StateFieldsSettings = stateFieldsSettings;
            this.StorageFieldsSettings = storageFieldsSettings;
            this.ChassisFieldsSettings = chassisFieldsSettings;
            this.InventoryFieldsSettings = inventoryFieldsSettings;
            this.MemoryFieldsSettings = memoryFieldsSettings;
            this.OperatingSystemFieldsSettings = operatingSystemFieldsSettings;
            this.OrganizationFieldsSettings = organizationFieldsSettings;
            this.ProccesorFieldsSettings = proccesorFieldsSettings;
            this.PlaceFieldsSettings = placeFieldsSettings;
        }

        [IsId]
        public int? CustomerId { get; private set; }

        [IsId]
        public int? LanguageId { get; private set; }

        [NotNull]
        public GeneralFieldsSettings GeneralFieldsSettings { get; private set; }

        [NotNull]
        public OtherFieldsSettings OtherFieldsSettings { get; private set; }

        [NotNull]
        public StateFieldsSettings StateFieldsSettings { get; private set; }

        [NotNull]
        public StorageFieldsSettings StorageFieldsSettings { get; private set; }

        [NotNull]
        public ChassisFieldsSettings ChassisFieldsSettings { get; private set; }

        [NotNull]
        public InventoryFieldsSettings InventoryFieldsSettings { get; private set; }

        [NotNull]
        public MemoryFieldsSettings MemoryFieldsSettings { get; private set; }

        [NotNull]
        public OperatingSystemFieldsSettings OperatingSystemFieldsSettings { get; private set; }

        [NotNull]
        public OrganizationFieldsSettings OrganizationFieldsSettings { get; private set; }

        [NotNull]
        public ProcessorFieldsSettings ProccesorFieldsSettings { get; private set; }

        [NotNull]
        public PlaceFieldsSettings PlaceFieldsSettings { get; private set; }
    }
}