namespace DH.Helpdesk.BusinessData.Models.Inventory
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class InventoryValue
    {
        public InventoryValue(int inventoryId, int inventoryTypePropertyId, FieldTypes fieldType, string value)
        {
            this.InventoryId = inventoryId;
            this.InventoryTypePropertyId = inventoryTypePropertyId;
            this.FieldType = fieldType;
            this.Value = value;
        }

        [IsId]
        public int InventoryId { get; private set; }

        [IsId]
        public int InventoryTypePropertyId { get; private set; }

        public FieldTypes FieldType { get; private set; }

        public string Value { get; private set; }
    }
}