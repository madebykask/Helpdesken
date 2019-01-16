namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings.Computer
{
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;

    public class ComputerFieldsSettingsViewModel
    {
        public ComputerFieldsSettingsViewModel()
        {
        }

        public ComputerFieldsSettingsViewModel(
            int languageId,
            SelectList langauges,
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
            WorkstationFieldsSettingsModel workstationFieldsSettingsModel,
            WorkstationTabsSettingsModel workstationTabsSettingsModel)
        {
            LanguageId = languageId;
            Languages = langauges;
            DateFieldsSettingsModel = dateFieldsSettingsModel;
            CommunicationFieldsSettingsModel = communicationFieldsSettingsModel;
            ContactFieldsSettingsModel = contactFieldsSettingsModel;
            ContactInformationFieldsSettingsModel = contactInformationFieldsSettingsModel;
            ContractFieldsSettingsModel = contractFieldsSettingsModel;
            GraphicsFieldsSettingsModel = graphicsFieldsSettingsModel;
            OtherFieldsSettingsModel = otherFieldsSettingsModel;
            PlaceFieldsSettingsModel = placeFieldsSettingsModel;
            SoundFieldsSettingsModel = soundFieldsSettingsModel;
            StateFieldsSettingsModel = stateFieldsSettingsModel;
            ChassisFieldsSettingsModel = chassisFieldsSettingsModel;
            InventoryFieldsSettingsModel = inventoryFieldsSettingsModel;
            MemoryFieldsSettingsModel = memoryFieldsSettingsModel;
            OperatingSystemFieldsSettingsModel = operatingSystemFieldsSettingsModel;
            OrganizationFieldsSettingsModel = organizationFieldsSettingsModel;
            ProccesorFieldsSettingsModel = proccesorFieldsSettingsModel;
            WorkstationFieldsSettingsModel = workstationFieldsSettingsModel;
            WorkstationTabsSettingsModel = workstationTabsSettingsModel;
        }

        [IsId]
        public int LanguageId { get; set; }

        [NotNull]
        public SelectList Languages { get; set; }

        [NotNull]
        public DateFieldsSettingsModel DateFieldsSettingsModel { get; set; }

        [NotNull]
        public CommunicationFieldsSettingsModel CommunicationFieldsSettingsModel { get; set; }

        [NotNull]
        public ContactFieldsSettingsModel ContactFieldsSettingsModel { get; set; }

        [NotNull]
        public ContactInformationFieldsSettingsModel ContactInformationFieldsSettingsModel { get; set; }

        [NotNull]
        public ContractFieldsSettingsModel ContractFieldsSettingsModel { get; set; }

        [NotNull]
        public GraphicsFieldsSettingsModel GraphicsFieldsSettingsModel { get; set; }

        [NotNull]
        public OtherFieldsSettingsModel OtherFieldsSettingsModel { get; set; }

        [NotNull]
        public PlaceFieldsSettingsModel PlaceFieldsSettingsModel { get; set; }

        [NotNull]
        public SoundFieldsSettingsModel SoundFieldsSettingsModel { get; set; }

        [NotNull]
        public StateFieldsSettingsModel StateFieldsSettingsModel { get; set; }

        [NotNull]
        public ChassisFieldsSettingsModel ChassisFieldsSettingsModel { get; set; }

        [NotNull]
        public InventoryFieldsSettingsModel InventoryFieldsSettingsModel { get; set; }

        [NotNull]
        public MemoryFieldsSettingsModel MemoryFieldsSettingsModel { get; set; }

        [NotNull]
        public OperatingSystemFieldsSettingsModel OperatingSystemFieldsSettingsModel { get; set; }

        [NotNull]
        public OrganizationFieldsSettingsModel OrganizationFieldsSettingsModel { get; set; }

        [NotNull]
        public ProccesorFieldsSettingsModel ProccesorFieldsSettingsModel { get; set; }

        [NotNull]
        public WorkstationFieldsSettingsModel WorkstationFieldsSettingsModel { get; set; }
        [NotNull]
        public WorkstationTabsSettingsModel WorkstationTabsSettingsModel { get; set; }
    }
}