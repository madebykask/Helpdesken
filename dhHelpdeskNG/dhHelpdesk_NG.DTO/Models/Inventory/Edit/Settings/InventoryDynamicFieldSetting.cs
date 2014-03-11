namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class InventoryDynamicFieldSetting
    {
        private InventoryDynamicFieldSetting(int inventoryTypeId, string caption, int position, FieldTypes? fieldType, int propertySize, bool showInDetails, bool showInList)
        {
            this.InventoryTypeId = inventoryTypeId;
            this.Caption = caption;
            this.Position = position;
            this.FieldType = fieldType;
            this.PropertySize = propertySize;
            this.ShowInDetails = showInDetails;
            this.ShowInList = showInList;
        }

        [IsId]
        public int Id { get; private set; }

        [IsId]
        public int InventoryTypeId { get; private set; }

        [NotNullAndEmpty]
        public string Caption { get; private set; }

        public int Position { get; private set; }

        public FieldTypes? FieldType { get; private set; }

        public int PropertySize { get; private set; }

        public bool ShowInDetails { get; private set; }

        public bool ShowInList { get; private set; }

        public static InventoryDynamicFieldSetting CreateNew(int inventoryTypeId, string caption, int position, FieldTypes? fieldType, int propertySize, bool showInDetails, bool showInList)
        {
            var businessModel = new InventoryDynamicFieldSetting(inventoryTypeId, caption, position, fieldType, propertySize, showInDetails, showInList);

            return businessModel;
        }

        public static InventoryDynamicFieldSetting CreateUpdated(int id, int inventoryTypeId, string caption, int position, FieldTypes? fieldType, int propertySize, bool showInDetails, bool showInList)
        {
            var businessModel = new InventoryDynamicFieldSetting(inventoryTypeId, caption, position, fieldType, propertySize, showInDetails, showInList) { Id = id };

            return businessModel;
        }

        public static InventoryDynamicFieldSetting CreateForEdit(int id, int inventoryTypeId, string caption, int position, FieldTypes? fieldType, int propertySize, bool showInDetails, bool showInList)
        {
            var businessModel = new InventoryDynamicFieldSetting(inventoryTypeId, caption, position, fieldType, propertySize, showInDetails, showInList) { Id = id };

            return businessModel;
        }
    }
}