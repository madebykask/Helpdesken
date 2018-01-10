namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.InventorySettings
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class InventoryDynamicFieldSettingForModelEdit
    {
        public InventoryDynamicFieldSettingForModelEdit(
            int id,
            int? inventoryTypeGroupId,
            string caption,
            int position,
            FieldTypes fieldType,
            int propertySize,
            bool isReadOnly = false)
        {
            this.Id = id;
            this.InventoryTypeGroupId = inventoryTypeGroupId;
            this.Caption = caption;
            this.Position = position;
            this.FieldType = fieldType;
            this.PropertySize = propertySize;
            this.IsReadOnly = isReadOnly;
        }

        [IsId]
        public int Id { get; set; }

        [IsId]
        public int? InventoryTypeGroupId { get; private set; }

        [NotNullAndEmpty]
        public string Caption { get; private set; }

        public int Position { get; private set; }

        public FieldTypes FieldType { get; private set; }

        public int PropertySize { get; private set; }

        public bool IsReadOnly { get; set; }
    }
}