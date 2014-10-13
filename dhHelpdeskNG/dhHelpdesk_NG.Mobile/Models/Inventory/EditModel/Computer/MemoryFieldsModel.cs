namespace DH.Helpdesk.Mobile.Models.Inventory.EditModel.Computer
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class MemoryFieldsModel
    {
        public MemoryFieldsModel()
        {
        }

        public MemoryFieldsModel(ConfigurableFieldModel<int?> ramId)
        {
            this.RAMId = ramId;
        }

        [NotNull]
        public ConfigurableFieldModel<int?> RAMId { get; set; }
    }
}