namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings
{
    using DataAnnotationsExtensions;

    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class InventoryDynamicFieldSettingModel
    {
        public InventoryDynamicFieldSettingModel()
        {
        }

        public InventoryDynamicFieldSettingModel(
            int id,
            int inventoryTypeId,
            int? inventoryTypeGroupId,
            string caption,
            int position,
            FieldTypes? fieldType,
            int? propertySize,
            bool showInDetails,
            bool showInList,
            string xml,
            bool readOnly)
        {
            this.Id = id;
            this.InventoryTypeId = inventoryTypeId;
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
        public int Id { get; set; }

        [IsId]
        public int InventoryTypeId { get; set; }

        [IsId]
        public int? InventoryTypeGroupId { get; set; }

        [NotNull]
        [LocalizedRequired]
        public string Caption { get; set; }

        [Min(0)]
        public int Position { get; set; }

        public FieldTypes? FieldType { get; set; }

        [Min(0)]
        public int? PropertySize { get; set; }

        public bool ShowInDetails { get; set; }

        public bool ShowInList { get; set; }

        public string XMLTag { get; set; }

        public bool ReadOnly { get; set; }
    }
}