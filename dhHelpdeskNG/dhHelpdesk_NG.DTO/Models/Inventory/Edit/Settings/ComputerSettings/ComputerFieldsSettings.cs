namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.SharedSettings;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ComputerFieldsSettings
    {
        public ComputerFieldsSettings(DateFieldsSettings dateFieldsSettings, CommunicationFieldsSettings communicationFieldsSettings, ContactFieldsSettings contactFieldsSettings, ContactInformationFieldsSettings contactInformationFieldsSettings, ContractFieldsSettings contractFieldsSettings, GraphicsFieldsSettings graphicsFieldsSettings, OtherFieldsSettings otherFieldsSettings, PlaceFieldsSettings placeFieldsSettings, SoundFieldsSettings soundFieldsSettings, StateFieldsSettings stateFieldsSettings, ChassisFieldsSettings chassisFieldsSettings, InventoryFieldsSettings inventoryFieldsSettings, MemoryFieldsSettings memoryFieldsSettings, OperatingSystemFieldsSettings operatingSystemFieldsSettings, OrganizationFieldsSettings organizationFieldsSettings, ProccesorFieldsSettings proccesorFieldsSettings, WorkstationFieldsSettings workstationFieldsSettings)
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

        [IsId]
        public int? CustomerId { get; private set; }

        [IsId]
        public int? LanguageId { get; private set; }

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
        public ProccesorFieldsSettings ProccesorFieldsSettings { get; private set; }

        [NotNull]
        public WorkstationFieldsSettings WorkstationFieldsSettings { get; private set; }

        public static ComputerFieldsSettings CreateUpdated(int? customerId, int? langaugeId, DateFieldsSettings dateFieldsSettings, CommunicationFieldsSettings communicationFieldsSettings, ContactFieldsSettings contactFieldsSettings, ContactInformationFieldsSettings contactInformationFieldsSettings, ContractFieldsSettings contractFieldsSettings, GraphicsFieldsSettings graphicsFieldsSettings, OtherFieldsSettings otherFieldsSettings, PlaceFieldsSettings placeFieldsSettings, SoundFieldsSettings soundFieldsSettings, StateFieldsSettings stateFieldsSettings, ChassisFieldsSettings chassisFieldsSettings, InventoryFieldsSettings inventoryFieldsSettings, MemoryFieldsSettings memoryFieldsSettings, OperatingSystemFieldsSettings operatingSystemFieldsSettings, OrganizationFieldsSettings organizationFieldsSettings, ProccesorFieldsSettings proccesorFieldsSettings, WorkstationFieldsSettings workstationFieldsSettings)
        {
            var businessModel = new ComputerFieldsSettings(dateFieldsSettings, communicationFieldsSettings, contactFieldsSettings, contactInformationFieldsSettings, contractFieldsSettings, graphicsFieldsSettings, otherFieldsSettings, placeFieldsSettings, soundFieldsSettings, stateFieldsSettings, chassisFieldsSettings, inventoryFieldsSettings, memoryFieldsSettings, operatingSystemFieldsSettings, organizationFieldsSettings, proccesorFieldsSettings, workstationFieldsSettings) { CustomerId = customerId, LanguageId = langaugeId };

            return businessModel;
        }

        public static ComputerFieldsSettings CreateForEdit(DateFieldsSettings dateFieldsSettings, CommunicationFieldsSettings communicationFieldsSettings, ContactFieldsSettings contactFieldsSettings, ContactInformationFieldsSettings contactInformationFieldsSettings, ContractFieldsSettings contractFieldsSettings, GraphicsFieldsSettings graphicsFieldsSettings, OtherFieldsSettings otherFieldsSettings, PlaceFieldsSettings placeFieldsSettings, SoundFieldsSettings soundFieldsSettings, StateFieldsSettings stateFieldsSettings, ChassisFieldsSettings chassisFieldsSettings, InventoryFieldsSettings inventoryFieldsSettings, MemoryFieldsSettings memoryFieldsSettings, OperatingSystemFieldsSettings operatingSystemFieldsSettings, OrganizationFieldsSettings organizationFieldsSettings, ProccesorFieldsSettings proccesorFieldsSettings, WorkstationFieldsSettings workstationFieldsSettings)
        {
            var businessModel = new ComputerFieldsSettings(dateFieldsSettings, communicationFieldsSettings, contactFieldsSettings, contactInformationFieldsSettings, contractFieldsSettings, graphicsFieldsSettings, otherFieldsSettings, placeFieldsSettings, soundFieldsSettings, stateFieldsSettings, chassisFieldsSettings, inventoryFieldsSettings, memoryFieldsSettings, operatingSystemFieldsSettings, organizationFieldsSettings, proccesorFieldsSettings, workstationFieldsSettings);

            return businessModel;
        }
    }
}