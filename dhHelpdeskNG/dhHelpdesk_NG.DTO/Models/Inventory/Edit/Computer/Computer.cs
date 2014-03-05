namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Computer
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class Computer
    {
        private Computer(DateFields dateFields, CommunicationFields communicationFields, ContactFields contactFields, ContactInformationFields contactInformationFields, ContractFields contractFields, GraphicsFields graphicsFields, OtherFields otherFields, PlaceFields placeFields, SoundFields soundFields, StateFields stateFields, ChassisFields chassisFields, InventoryFields inventoryFields, MemoryFields memoryFields, OperatingSystemFields operatingSystemFields, OrganizationFields organizationFields, ProccesorFields proccesorFields, WorkstationFields workstationFields)
        {
            this.DateFields = dateFields;
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
        public int Id { get; private set; }

        [IsId]
        public int? CustomerId { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public DateTime ChangedDate { get; private set; }

        [NotNull]
        public DateFields DateFields { get; private set; }

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
        public ProccesorFields ProccesorFields { get; private set; }

        [NotNull]
        public WorkstationFields WorkstationFields { get; private set; }

        public static Computer CreateNew(int? customerId, DateFields dateFields, CommunicationFields communicationFields, ContactFields contactFields, ContactInformationFields contactInformationFields, ContractFields contractFields, GraphicsFields graphicsFields, OtherFields otherFields, PlaceFields placeFields, SoundFields soundFields, StateFields stateFields, ChassisFields chassisFields, InventoryFields inventoryFields, MemoryFields memoryFields, OperatingSystemFields operatingSystemFields, OrganizationFields organizationFields, ProccesorFields proccesorFields, WorkstationFields workstationFields, DateTime createdDate)
        {
            var businessModel = new Computer(dateFields, communicationFields, contactFields, contactInformationFields, contractFields, graphicsFields, otherFields, placeFields, soundFields, stateFields, chassisFields, inventoryFields, memoryFields, operatingSystemFields, organizationFields, proccesorFields, workstationFields) { CustomerId = customerId, CreatedDate = createdDate };

            return businessModel;
        }

        public static Computer CreateUpdated(int id, DateFields dateFields, CommunicationFields communicationFields, ContactFields contactFields, ContactInformationFields contactInformationFields, ContractFields contractFields, GraphicsFields graphicsFields, OtherFields otherFields, PlaceFields placeFields, SoundFields soundFields, StateFields stateFields, ChassisFields chassisFields, InventoryFields inventoryFields, MemoryFields memoryFields, OperatingSystemFields operatingSystemFields, OrganizationFields organizationFields, ProccesorFields proccesorFields, WorkstationFields workstationFields, DateTime changedDate)
        {
            var businessModel = new Computer(dateFields, communicationFields, contactFields, contactInformationFields, contractFields, graphicsFields, otherFields, placeFields, soundFields, stateFields, chassisFields, inventoryFields, memoryFields, operatingSystemFields, organizationFields, proccesorFields, workstationFields) { Id = id, ChangedDate = changedDate };

            return businessModel;
        }

        public static Computer CreateForEdit(int id, DateFields dateFields, CommunicationFields communicationFields, ContactFields contactFields, ContactInformationFields contactInformationFields, ContractFields contractFields, GraphicsFields graphicsFields, OtherFields otherFields, PlaceFields placeFields, SoundFields soundFields, StateFields stateFields, ChassisFields chassisFields, InventoryFields inventoryFields, MemoryFields memoryFields, OperatingSystemFields operatingSystemFields, OrganizationFields organizationFields, ProccesorFields proccesorFields, WorkstationFields workstationFields, DateTime createdDate, DateTime changedDate)
        {
            var businessModel = new Computer(dateFields, communicationFields, contactFields, contactInformationFields, contractFields, graphicsFields, otherFields, placeFields, soundFields, stateFields, chassisFields, inventoryFields, memoryFields, operatingSystemFields, organizationFields, proccesorFields, workstationFields) { Id = id, CreatedDate = createdDate, ChangedDate = changedDate };

            return businessModel;
        }
    }
}