namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Computer
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ComputerOverview
    {
        public ComputerOverview(
            int id,
            int? customerId,
            DateTime createdDate,
            DateTime changedDate,
            WorkstationFields workstationFields,
            ProcessorFields proccesorFields,
            OrganizationFields organizationFields,
            OperatingSystemFields operatingSystemFields,
            MemoryFields memoryFields,
            InventoryFields inventoryFields,
            ChassisFields chassisFields,
            StateFields stateFields,
            SoundFields soundFields,
            PlaceFields placeFields,
            OtherFields otherFields,
            GraphicsFields graphicsFields,
            ContractFields contractFields,
            ContactInformationFields contactInformationFields,
            ContactFields contactFields,
            CommunicationFields communicationFields,
            DateFields dateFields)
        {
            this.Id = id;
            this.CustomerId = customerId;
            this.CreatedDate = createdDate;
            this.ChangedDate = changedDate;
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
        public ProcessorFields ProccesorFields { get; private set; }

        [NotNull]
        public WorkstationFields WorkstationFields { get; private set; }
    }
}