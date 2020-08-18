namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.ServerSettings
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.SharedSettings;
    using DH.Helpdesk.Common.ValidationAttributes;

    using PlaceFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.SharedSettings.PlaceFieldsSettings;

    public class ServerFieldsSettingsForModelEdit
    {
        public ServerFieldsSettingsForModelEdit(
            GeneralFieldsSettings generalFieldsSettings,
            OtherFieldsSettings otherFieldsSettings,
            StateFieldsSettings stateFieldsSettings,
            StorageFieldsSettings storageFieldsSettings,
            ChassisFieldsSettings chassisFieldsSettings,
            InventoryFieldsSettings inventoryFieldsSettings,
            MemoryFieldsSettings memoryFieldsSettings,
            OperatingSystemFieldsSettings operatingSystemFieldsSettings,
            ProcessorFieldsSettings proccesorFieldsSettings,
            PlaceFieldsSettings placeFieldsSettings, 
            DocumentFieldsSettings documentFieldsSettings,
            CommunicationFieldsSettings communicationFieldsSettings)
        {
            this.GeneralFieldsSettings = generalFieldsSettings;
            this.OtherFieldsSettings = otherFieldsSettings;
            this.StateFieldsSettings = stateFieldsSettings;
            this.StorageFieldsSettings = storageFieldsSettings;
            this.ChassisFieldsSettings = chassisFieldsSettings;
            this.InventoryFieldsSettings = inventoryFieldsSettings;
            this.MemoryFieldsSettings = memoryFieldsSettings;
            this.OperatingSystemFieldsSettings = operatingSystemFieldsSettings;
            this.ProccesorFieldsSettings = proccesorFieldsSettings;
            this.PlaceFieldsSettings = placeFieldsSettings;
            this.DocumentFieldsSettings = documentFieldsSettings;
            this.CommunicationFieldsSettings = communicationFieldsSettings;
        }

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
        public ProcessorFieldsSettings ProccesorFieldsSettings { get; private set; }

        [NotNull]
        public PlaceFieldsSettings PlaceFieldsSettings { get; private set; }

        public DocumentFieldsSettings DocumentFieldsSettings { get; private set; }

        [NotNull]
        public CommunicationFieldsSettings CommunicationFieldsSettings { get; private set; }
    }
}