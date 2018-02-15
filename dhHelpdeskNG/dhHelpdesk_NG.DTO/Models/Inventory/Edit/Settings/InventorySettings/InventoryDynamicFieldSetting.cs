namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.InventorySettings
{
    using System;

    using DH.Helpdesk.BusinessData.Attributes;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class InventoryDynamicFieldSetting : BusinessModel
    {
        private InventoryDynamicFieldSetting(
            ModelStates businessModelStates,
            int? inventoryTypeGroupId,
            string caption,
            int position,
            FieldTypes fieldType,
            int? propertySize,
            bool showInDetails,
            bool showInList,
            string xml,
            bool readOnly)
            : base(businessModelStates)
        {
            this.InventoryTypeGroupId = inventoryTypeGroupId;
            this.Caption = caption;
            this.Position = position;
            this.FieldType = fieldType;
            this.PropertySize = propertySize;
            this.ShowInDetails = showInDetails;
            this.ShowInList = showInList;
            this.XMLTag = xml;
            this.ReadOnly = readOnly;
        }

        [IsId]
        [AllowRead(ModelStates.Created)]
        public int InventoryTypeId { get; private set; }

        [IsId]
        public int? InventoryTypeGroupId { get; private set; }

        [NotNullAndEmpty]
        public string Caption { get; private set; }

        public int Position { get; private set; }

        public FieldTypes FieldType { get; private set; }

        public int? PropertySize { get; private set; }

        public bool ShowInDetails { get; private set; }

        public bool ShowInList { get; private set; }

        public string XMLTag { get; set; }

        public bool ReadOnly { get; set; }

        [AllowRead(ModelStates.Updated)]
        public DateTime ChangedDate { get; private set; }

        [AllowRead(ModelStates.Created)]
        public DateTime CreatedDate { get; private set; }

        public static InventoryDynamicFieldSetting CreateNew(
            int inventoryTypeId,
            int? inventoryTypeGroupId,
            string caption,
            int position,
            FieldTypes fieldType,
            int? propertySize,
            bool showInDetails,
            bool showInList,
            DateTime createdDate,
            string xml,
            bool readOnly)
        {
            var businessModel = new InventoryDynamicFieldSetting(
                ModelStates.Created,
                inventoryTypeGroupId,
                caption,
                position,
                fieldType,
                propertySize,
                showInDetails,
                showInList,
                xml,
                readOnly) { InventoryTypeId = inventoryTypeId, CreatedDate = createdDate };

            return businessModel;
        }

        public static InventoryDynamicFieldSetting CreateUpdated(
            int id,
            int? inventoryTypeGroupId,
            string caption,
            int position,
            FieldTypes fieldType,
            int? propertySize,
            bool showInDetails,
            bool showInList,
            DateTime changedDate,
            string xml,
            bool readOnly)
        {
            var businessModel = new InventoryDynamicFieldSetting(
                ModelStates.Updated,
                inventoryTypeGroupId,
                caption,
                position,
                fieldType,
                propertySize,
                showInDetails,
                showInList,
                xml,
                readOnly) { Id = id, ChangedDate = changedDate };

            return businessModel;
        }

        public static InventoryDynamicFieldSetting CreateForEdit(
            int id,
            int? inventoryTypeGroupId,
            string caption,
            int position,
            FieldTypes fieldType,
            int? propertySize,
            bool showInDetails,
            bool showInList,
            string xml,
            bool readOnly)
        {
            var businessModel = new InventoryDynamicFieldSetting(
                ModelStates.ForEdit,
                inventoryTypeGroupId,
                caption,
                position,
                fieldType,
                propertySize,
                showInDetails,
                showInList,
                xml,
                readOnly) { Id = id };

            return businessModel;
        }
    }
}