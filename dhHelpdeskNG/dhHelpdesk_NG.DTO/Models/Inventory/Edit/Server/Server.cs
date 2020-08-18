namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Server
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.Common.ValidationAttributes;

    public abstract class Server : INewBusinessModel
    {
        protected Server(
            bool isOperationObject,
            GeneralFields generalFields,
            OtherFields otherFields,
            StorageFields storageFields,
            ChassisFields chassisFields,
            InventoryFields inventoryFields,
            OperatingSystemFields operatingSystemFields,
            MemoryFields memoryFields,
            PlaceFields placeFields,
            DocumentFields documentFields,
            ProcessorFields proccesorFields,
            CommunicationFields communicationFields)
        {
            this.IsOperationObject = isOperationObject;
            this.GeneralFields = generalFields;
            this.OtherFields = otherFields;
            this.StorageFields = storageFields;
            this.ChassisFields = chassisFields;
            this.InventoryFields = inventoryFields;
            this.OperatingSystemFields = operatingSystemFields;
            this.MemoryFields = memoryFields;
            this.PlaceFields = placeFields;
            this.DocumentFields =  documentFields;
            this.ProccesorFields = proccesorFields;
            this.CommunicationFields = communicationFields;
        }

        [IsId]
        public int Id { get; set; }

        public bool IsOperationObject { get; private set; }

        [NotNull]
        public GeneralFields GeneralFields { get; private set; }

        [NotNull]
        public OtherFields OtherFields { get; private set; }

        [NotNull]
        public StorageFields StorageFields { get; private set; }

        [NotNull]
        public ChassisFields ChassisFields { get; private set; }

        [NotNull]
        public InventoryFields InventoryFields { get; private set; }

        [NotNull]
        public MemoryFields MemoryFields { get; private set; }

        [NotNull]
        public OperatingSystemFields OperatingSystemFields { get; private set; }

        [NotNull]
        public ProcessorFields ProccesorFields { get; private set; }

        [NotNull]
        public PlaceFields PlaceFields { get; private set; }

        public DocumentFields DocumentFields { get; private set; }

        [NotNull]
        public CommunicationFields CommunicationFields { get; private set; }
    }
}