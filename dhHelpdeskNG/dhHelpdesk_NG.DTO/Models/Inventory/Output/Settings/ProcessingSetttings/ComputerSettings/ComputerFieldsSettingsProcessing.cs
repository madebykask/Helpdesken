﻿namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.ComputerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ComputerFieldsSettingsProcessing
    {
        public ComputerFieldsSettingsProcessing(
            DateFieldsSettings dateFieldsSettings,
            CommunicationFieldsSettings communicationFieldsSettings,
            ContactFieldsSettings contactFieldsSettings,
            ContactInformationFieldsSettings contactInformationFieldsSettings,
            ContractFieldsSettings contractFieldsSettings,
            GraphicsFieldsSettings graphicsFieldsSettings,
            OtherFieldsSettings otherFieldsSettings,
            PlaceFieldsSettings placeFieldsSettings,
            SoundFieldsSettings soundFieldsSettings,
            StateFieldsSettings stateFieldsSettings,
            ChassisFieldsSettings chassisFieldsSettings,
            InventoryFieldsSettings inventoryFieldsSettings,
            MemoryFieldsSettings memoryFieldsSettings,
            OperatingSystemFieldsSettings operatingSystemFieldsSettings,
            OrganizationFieldsSettings organizationFieldsSettings,
            ProcessorFieldsSettings proccesorFieldsSettings,
            WorkstationFieldsSettings workstationFieldsSettings)
        {
            this.DateFieldsSettings = dateFieldsSettings;
            this.CommunicationFieldsSettings = communicationFieldsSettings;
            this.ContactFieldsSettings = contactFieldsSettings;
            this.ContactInformationFieldsSettings = contactInformationFieldsSettings;
            this.ContractFieldsSettings = contractFieldsSettings;
            this.GraphicsFieldsSettings = graphicsFieldsSettings;
            this.OtherFieldsSettings = otherFieldsSettings;
            this.PlaceFieldsSettings = placeFieldsSettings;
            this.SoundFieldsSettings = soundFieldsSettings;
            this.StateFieldsSettings = stateFieldsSettings;
            this.ChassisFieldsSettings = chassisFieldsSettings;
            this.InventoryFieldsSettings = inventoryFieldsSettings;
            this.MemoryFieldsSettings = memoryFieldsSettings;
            this.OperatingSystemFieldsSettings = operatingSystemFieldsSettings;
            this.OrganizationFieldsSettings = organizationFieldsSettings;
            this.ProccesorFieldsSettings = proccesorFieldsSettings;
            this.WorkstationFieldsSettings = workstationFieldsSettings;
        }

        [NotNull]
        public DateFieldsSettings DateFieldsSettings { get; private set; }

        [NotNull]
        public CommunicationFieldsSettings CommunicationFieldsSettings { get; private set; }

        [NotNull]
        public ContactFieldsSettings ContactFieldsSettings { get; private set; }

        [NotNull]
        public ContactInformationFieldsSettings ContactInformationFieldsSettings { get; private set; }

        [NotNull]
        public ContractFieldsSettings ContractFieldsSettings { get; private set; }

        [NotNull]
        public GraphicsFieldsSettings GraphicsFieldsSettings { get; private set; }

        [NotNull]
        public OtherFieldsSettings OtherFieldsSettings { get; private set; }

        [NotNull]
        public PlaceFieldsSettings PlaceFieldsSettings { get; private set; }

        [NotNull]
        public SoundFieldsSettings SoundFieldsSettings { get; private set; }

        [NotNull]
        public StateFieldsSettings StateFieldsSettings { get; private set; }

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
        public WorkstationFieldsSettings WorkstationFieldsSettings { get; private set; }
    }
}