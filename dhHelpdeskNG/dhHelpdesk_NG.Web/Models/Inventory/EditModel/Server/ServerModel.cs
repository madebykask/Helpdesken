namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Server
{
    using System;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Shared;

    public class ServerModel
    {
        public ServerModel()
        {
        }

        public ServerModel(
            int id,
            int? customerId,
            ConfigurableFieldModel<DateTime> createdDate,
            ConfigurableFieldModel<DateTime> changedDate,
            GeneralFieldsModel generalFields,
            OtherFieldsModel otherFields,
            StateFieldsModel stateFields,
            StorageFieldsModel storageFields,
            ChassisFieldsModel chassisFields,
            InventoryFieldsModel inventoryFields,
            MemoryFieldsModel memoryFields,
            OperatingSystemFieldsModel operatingSystemFields,
            OrganizationFieldsModel organizationFields,
            ProccesorFieldsModel proccesorFields,
            PlaceFieldsModel placeFields)
        {
            this.Id = id;
            this.CustomerId = customerId;
            this.CreatedDate = createdDate;
            this.ChangedDate = changedDate;
            this.GeneralFields = generalFields;
            this.OtherFields = otherFields;
            this.StateFields = stateFields;
            this.StorageFields = storageFields;
            this.ChassisFields = chassisFields;
            this.InventoryFields = inventoryFields;
            this.MemoryFields = memoryFields;
            this.OperatingSystemFields = operatingSystemFields;
            this.OrganizationFields = organizationFields;
            this.ProccesorFields = proccesorFields;
            this.PlaceFields = placeFields;
        }

        [IsId]
        public int Id { get; private set; }

        [IsId]
        public int? CustomerId { get; private set; }

        public ConfigurableFieldModel<DateTime> CreatedDate { get; private set; }

        public ConfigurableFieldModel<DateTime> ChangedDate { get; private set; }

        [NotNull]
        public GeneralFieldsModel GeneralFields { get; private set; }

        [NotNull]
        public OtherFieldsModel OtherFields { get; private set; }

        [NotNull]
        public StateFieldsModel StateFields { get; private set; }

        [NotNull]
        public StorageFieldsModel StorageFields { get; private set; }

        [NotNull]
        public ChassisFieldsModel ChassisFields { get; private set; }

        [NotNull]
        public InventoryFieldsModel InventoryFields { get; private set; }

        [NotNull]
        public MemoryFieldsModel MemoryFields { get; private set; }

        [NotNull]
        public OperatingSystemFieldsModel OperatingSystemFields { get; private set; }

        [NotNull]
        public OrganizationFieldsModel OrganizationFields { get; private set; }

        [NotNull]
        public ProccesorFieldsModel ProccesorFields { get; private set; }

        [NotNull]
        public PlaceFieldsModel PlaceFields { get; private set; }
    }
}