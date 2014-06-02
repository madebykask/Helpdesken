namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Settings.Computer
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Settings.Shared;

    public class ComputerFieldsSettingsViewModel
    {
        public ComputerFieldsSettingsViewModel()
        {
        }

        public ComputerFieldsSettingsViewModel(
            DateFieldsSettingsModel dateFieldsSettingsModel,
            CommunicationFieldsSettingsModel communicationFieldsSettingsModel,
            ContactFieldsSettingsModel contactFieldsSettingsModel,
            ContactInformationFieldsSettingsModel contactInformationFieldsSettingsModel,
            ContractFieldsSettingsModel contractFieldsSettingsModel,
            GraphicsFieldsSettingsModel graphicsFieldsSettingsModel,
            OtherFieldsSettingsModel otherFieldsSettingsModel,
            PlaceFieldsSettingsModel placeFieldsSettingsModel,
            SoundFieldsSettingsModel soundFieldsSettingsModel,
            StateFieldsSettingsModel stateFieldsSettingsModel,
            ChassisFieldsSettingsModel chassisFieldsSettingsModel,
            InventoryFieldsSettingsModel inventoryFieldsSettingsModel,
            MemoryFieldsSettingsModel memoryFieldsSettingsModel,
            OperatingSystemFieldsSettingsModel operatingSystemFieldsSettingsModel,
            OrganizationFieldsSettingsModel organizationFieldsSettingsModel,
            ProccesorFieldsSettingsModel proccesorFieldsSettingsModel,
            WorkstationFieldsSettingsModel workstationFieldsSettingsModel)
        {
            this.DateFieldsSettingsModel = dateFieldsSettingsModel;
            this.CommunicationFieldsSettingsModel = communicationFieldsSettingsModel;
            this.ContactFieldsSettingsModel = contactFieldsSettingsModel;
            this.ContactInformationFieldsSettingsModel = contactInformationFieldsSettingsModel;
            this.ContractFieldsSettingsModel = contractFieldsSettingsModel;
            this.GraphicsFieldsSettingsModel = graphicsFieldsSettingsModel;
            this.OtherFieldsSettingsModel = otherFieldsSettingsModel;
            this.PlaceFieldsSettingsModel = placeFieldsSettingsModel;
            this.SoundFieldsSettingsModel = soundFieldsSettingsModel;
            this.StateFieldsSettingsModel = stateFieldsSettingsModel;
            this.ChassisFieldsSettingsModel = chassisFieldsSettingsModel;
            this.InventoryFieldsSettingsModel = inventoryFieldsSettingsModel;
            this.MemoryFieldsSettingsModel = memoryFieldsSettingsModel;
            this.OperatingSystemFieldsSettingsModel = operatingSystemFieldsSettingsModel;
            this.OrganizationFieldsSettingsModel = organizationFieldsSettingsModel;
            this.ProccesorFieldsSettingsModel = proccesorFieldsSettingsModel;
            this.WorkstationFieldsSettingsModel = workstationFieldsSettingsModel;
        }

        [NotNull]
        public DateFieldsSettingsModel DateFieldsSettingsModel { get; private set; }

        [NotNull]
        public CommunicationFieldsSettingsModel CommunicationFieldsSettingsModel { get; private set; }

        [NotNull]
        public ContactFieldsSettingsModel ContactFieldsSettingsModel { get; private set; }

        [NotNull]
        public ContactInformationFieldsSettingsModel ContactInformationFieldsSettingsModel { get; private set; }

        [NotNull]
        public ContractFieldsSettingsModel ContractFieldsSettingsModel { get; private set; }

        [NotNull]
        public GraphicsFieldsSettingsModel GraphicsFieldsSettingsModel { get; private set; }

        [NotNull]
        public OtherFieldsSettingsModel OtherFieldsSettingsModel { get; private set; }

        [NotNull]
        public PlaceFieldsSettingsModel PlaceFieldsSettingsModel { get; private set; }

        [NotNull]
        public SoundFieldsSettingsModel SoundFieldsSettingsModel { get; private set; }

        [NotNull]
        public StateFieldsSettingsModel StateFieldsSettingsModel { get; private set; }

        [NotNull]
        public ChassisFieldsSettingsModel ChassisFieldsSettingsModel { get; private set; }

        [NotNull]
        public InventoryFieldsSettingsModel InventoryFieldsSettingsModel { get; private set; }

        [NotNull]
        public MemoryFieldsSettingsModel MemoryFieldsSettingsModel { get; private set; }

        [NotNull]
        public OperatingSystemFieldsSettingsModel OperatingSystemFieldsSettingsModel { get; private set; }

        [NotNull]
        public OrganizationFieldsSettingsModel OrganizationFieldsSettingsModel { get; private set; }

        [NotNull]
        public ProccesorFieldsSettingsModel ProccesorFieldsSettingsModel { get; private set; }

        [NotNull]
        public WorkstationFieldsSettingsModel WorkstationFieldsSettingsModel { get; private set; }
    }
}