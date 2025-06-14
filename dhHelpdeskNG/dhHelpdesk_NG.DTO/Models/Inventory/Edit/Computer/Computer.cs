﻿namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Computer
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.Common.ValidationAttributes;

    public abstract class Computer : INewBusinessModel
    {
        protected Computer(
            CommunicationFields communicationFields,
            ContactFields contactFields,
            ContactInformationFields contactInformationFields,
            ContractFields contractFields,
            GraphicsFields graphicsFields,
            OtherFields otherFields,
            PlaceFields placeFields,
            SoundFields soundFields,
            StateFields stateFields,
            ChassisFields chassisFields,
            InventoryFields inventoryFields,
            MemoryFields memoryFields,
            OperatingSystemFields operatingSystemFields,
            OrganizationFields organizationFields,
            ProcessorFields proccesorFields,
            WorkstationFields workstationFields)
        {
            this.CommunicationFields = communicationFields;
            this.ContactFields = contactFields;
            this.ContactInformationFields = contactInformationFields;
            this.ContractFields = contractFields;
            this.GraphicsFields = graphicsFields;
            this.OtherFields = otherFields;
            this.PlaceFields = placeFields;
            this.SoundFields = soundFields;
            this.StateFields = stateFields;
            this.ChassisFields = chassisFields;
            this.InventoryFields = inventoryFields;
            this.MemoryFields = memoryFields;
            this.OperatingSystemFields = operatingSystemFields;
            this.OrganizationFields = organizationFields;
            this.ProccesorFields = proccesorFields;
            this.WorkstationFields = workstationFields;
        }

        [IsId]
        public int Id { get; set; }

        [NotNull]
        public CommunicationFields CommunicationFields { get; private set; }

        [NotNull]
        public ContactFields ContactFields { get; private set; }

        [NotNull]
        public ContactInformationFields ContactInformationFields { get; private set; }

        [NotNull]
        public ContractFields ContractFields { get; private set; }

        [NotNull]
        public GraphicsFields GraphicsFields { get; private set; }

        [NotNull]
        public OtherFields OtherFields { get; private set; }

        [NotNull]
        public PlaceFields PlaceFields { get; private set; }

        [NotNull]
        public SoundFields SoundFields { get; private set; }

        [NotNull]
        public StateFields StateFields { get; private set; }

        [NotNull]
        public ChassisFields ChassisFields { get; private set; }

        [NotNull]
        public InventoryFields InventoryFields { get; private set; }

        [NotNull]
        public MemoryFields MemoryFields { get; private set; }

        [NotNull]
        public OperatingSystemFields OperatingSystemFields { get; private set; }

        [NotNull]
        public OrganizationFields OrganizationFields { get; private set; }

        [NotNull]
        public ProcessorFields ProccesorFields { get; private set; }

        [NotNull]
        public WorkstationFields WorkstationFields { get; private set; }
    }
}