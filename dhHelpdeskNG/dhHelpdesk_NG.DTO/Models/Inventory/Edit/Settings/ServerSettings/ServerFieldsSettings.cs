namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ServerSettings
{
    using System;

    using DH.Helpdesk.BusinessData.Attributes;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.SharedSettings;
    using DH.Helpdesk.Common.ValidationAttributes;

    using PlaceFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.SharedSettings.PlaceFieldsSettings;

    public class ServerFieldsSettings
    {
        private ServerFieldsSettings(
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
            this.CommunicationFieldsSettings = communicationFieldsSettings;
        }

        [IsId]
        [AllowRead(ModelStates.Updated)]
        public int CustomerId { get; private set; }

        [IsId]
        [AllowRead(ModelStates.Updated)]
        public int LanguageId { get; private set; }

        [AllowRead(ModelStates.Updated)]
        public DateTime ChangedDate { get; private set; }

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

        [NotNull]
        public CommunicationFieldsSettings CommunicationFieldsSettings { get; private set; }

        public static ServerFieldsSettings CreateUpdated(
            int customerId,
            int langaugeId,
            DateTime changedDate,
            GeneralFieldsSettings generalFieldsSettings,
            OtherFieldsSettings otherFieldsSettings,
            StateFieldsSettings stateFieldsSettings,
            StorageFieldsSettings storageFieldsSettings,
            ChassisFieldsSettings chassisFieldsSettings,
            InventoryFieldsSettings inventoryFieldsSettings,
            OperatingSystemFieldsSettings operatingSystemFieldsSettings,
            MemoryFieldsSettings memoryFieldsSettings,
            PlaceFieldsSettings placeFieldsSettings,
            ProcessorFieldsSettings proccesorFieldsSettings,
            CommunicationFieldsSettings communicationFieldsSettings)
        {
            var businessModel = new ServerFieldsSettings(
                generalFieldsSettings,
                otherFieldsSettings,
                stateFieldsSettings,
                storageFieldsSettings,
                chassisFieldsSettings,
                inventoryFieldsSettings,
                memoryFieldsSettings,
                operatingSystemFieldsSettings,
                proccesorFieldsSettings,
                placeFieldsSettings,
                communicationFieldsSettings)
                                    {
                                        ChangedDate = changedDate,
                                        CustomerId = customerId,
                                        LanguageId = langaugeId
                                    };

            return businessModel;
        }

        public static ServerFieldsSettings CreateForEdit(
            GeneralFieldsSettings generalFieldsSettings,
            OtherFieldsSettings otherFieldsSettings,
            StateFieldsSettings stateFieldsSettings,
            StorageFieldsSettings storageFieldsSettings,
            ChassisFieldsSettings chassisFieldsSettings,
            InventoryFieldsSettings inventoryFieldsSettings,
            OperatingSystemFieldsSettings operatingSystemFieldsSettings,
            MemoryFieldsSettings memoryFieldsSettings,
            PlaceFieldsSettings placeFieldsSettings,
            ProcessorFieldsSettings proccesorFieldsSettings,
            CommunicationFieldsSettings communicationFieldsSettings)
        {
            var businessModel = new ServerFieldsSettings(
                generalFieldsSettings,
                otherFieldsSettings,
                stateFieldsSettings,
                storageFieldsSettings,
                chassisFieldsSettings,
                inventoryFieldsSettings,
                memoryFieldsSettings,
                operatingSystemFieldsSettings,
                proccesorFieldsSettings,
                placeFieldsSettings,
                communicationFieldsSettings);

            return businessModel;
        }
    }
}