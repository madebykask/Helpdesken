namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Computer
{
    using System;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Shared;

    public class ComputerViewModel
    {
        public ComputerViewModel()
        {
        }

        public ComputerViewModel(
            int id,
            int? customerId,
            ConfigurableFieldModel<DateTime> createdDate,
            ConfigurableFieldModel<DateTime> changedDate,
            DateFieldsModel dateFields,
            CommunicationFieldsViewModel communicationFieldsViewModel,
            ContactFieldsModel contactFields,
            ContactInformationFieldsModel contactInformationFields,
            ContractFieldsViewModel contractFieldsViewModel,
            GraphicsFieldsModel graphicsFields,
            OtherFieldsModel otherFields,
            PlaceFieldsViewModel placeFieldsViewModel,
            SoundFieldsModel soundFields,
            StateFieldsViewModel stateFieldsViewModel,
            ChassisFieldsModel chassisFields,
            InventoryFieldsModel inventoryFields,
            MemoryFieldsViewModel memoryFieldsViewModel,
            OperatingSystemFieldsViewModel operatingSystemFieldsViewModel,
            OrganizationFieldsViewModel organizationFieldViewModel,
            ProccesorFieldsViewModel proccesorFieldsViewModel,
            WorkstationFieldsViewModel workstationFieldsViewModel)
        {
            this.Id = id;
            this.CustomerId = customerId;
            this.CreatedDate = createdDate;
            this.ChangedDate = changedDate;
            this.DateFieldsModel = dateFields;
            this.CommunicationFieldsViewModel = communicationFieldsViewModel;
            this.ContactFieldsModel = contactFields;
            this.ContactInformationFieldsModel = contactInformationFields;
            this.ContractFieldsViewModel = contractFieldsViewModel;
            this.GraphicsFieldsModel = graphicsFields;
            this.OtherFieldsModel = otherFields;
            this.PlaceFieldsViewModel = placeFieldsViewModel;
            this.SoundFieldsModel = soundFields;
            this.StateFieldsViewModel = stateFieldsViewModel;
            this.ChassisFieldsModel = chassisFields;
            this.InventoryFieldsModel = inventoryFields;
            this.MemoryFieldsViewModel = memoryFieldsViewModel;
            this.OperatingSystemFieldsViewModel = operatingSystemFieldsViewModel;
            this.OrganizationFieldViewModel = organizationFieldViewModel;
            this.ProccesorFieldsViewModel = proccesorFieldsViewModel;
            this.WorkstationFieldsViewModel = workstationFieldsViewModel;
        }

        [IsId]
        public int Id { get; private set; }

        [IsId]
        public int? CustomerId { get; private set; }

        public ConfigurableFieldModel<DateTime> CreatedDate { get; private set; }

        public ConfigurableFieldModel<DateTime> ChangedDate { get; private set; }

        [NotNull]
        public DateFieldsModel DateFieldsModel { get; private set; }

        [NotNull]
        public CommunicationFieldsViewModel CommunicationFieldsViewModel { get; private set; }

        [NotNull]
        public ContactFieldsModel ContactFieldsModel { get; private set; }

        [NotNull]
        public ContactInformationFieldsModel ContactInformationFieldsModel { get; private set; }

        [NotNull]
        public ContractFieldsViewModel ContractFieldsViewModel { get; private set; }

        [NotNull]
        public GraphicsFieldsModel GraphicsFieldsModel { get; private set; }

        [NotNull]
        public OtherFieldsModel OtherFieldsModel { get; private set; }

        [NotNull]
        public PlaceFieldsViewModel PlaceFieldsViewModel { get; private set; }

        [NotNull]
        public SoundFieldsModel SoundFieldsModel { get; private set; }

        [NotNull]
        public StateFieldsViewModel StateFieldsViewModel { get; private set; }

        [NotNull]
        public ChassisFieldsModel ChassisFieldsModel { get; private set; }

        [NotNull]
        public InventoryFieldsModel InventoryFieldsModel { get; private set; }

        [NotNull]
        public MemoryFieldsViewModel MemoryFieldsViewModel { get; private set; }

        [NotNull]
        public OperatingSystemFieldsViewModel OperatingSystemFieldsViewModel { get; private set; }

        [NotNull]
        public OrganizationFieldsViewModel OrganizationFieldViewModel { get; private set; }

        [NotNull]
        public ProccesorFieldsViewModel ProccesorFieldsViewModel { get; private set; }

        [NotNull]
        public WorkstationFieldsViewModel WorkstationFieldsViewModel { get; private set; }
    }
}