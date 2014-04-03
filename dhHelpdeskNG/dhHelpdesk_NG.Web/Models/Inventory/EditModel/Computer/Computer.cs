namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Computer
{
    using System;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Shared;

    public class Computer
    {
        public Computer()
        {
        }

        public Computer(
            int id,
            int? customerId,
            ConfigurableFieldModel<DateTime> createdDate,
            ConfigurableFieldModel<DateTime> changedDate,
            DateFields dateFields,
            CommunicationFields communicationFields,
            ContactFields contactFields,
            ConfigurableFieldModel<ContactInformationFields> contactInformationFields,
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
            ProccesorFields proccesorFields,
            WorkstationFieldsModel workstationFields)
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

        public ConfigurableFieldModel<DateTime> CreatedDate { get; private set; }

        public ConfigurableFieldModel<DateTime> ChangedDate { get; private set; }

        [NotNull]
        public DateFields DateFields { get; private set; }

        [NotNull]
        public CommunicationFields CommunicationFields { get; private set; }

        [NotNull]
        public ContactFields ContactFields { get; private set; }

        [NotNull]
        public ConfigurableFieldModel<ContactInformationFields> ContactInformationFields { get; private set; }

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
        public WorkstationFieldsModel WorkstationFields { get; private set; }
    }
}